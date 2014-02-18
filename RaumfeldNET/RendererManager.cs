using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web;
using System.Net;
using System.Xml;
using System.IO;
using System.Collections;
using System.ComponentModel;

using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;
using RaumfeldNET.Renderer;

namespace RaumfeldNET
{
    public class RendererManager : BaseManager
    {
        // identifier for the webReqest method for getting the zones
        private const string retrieveDevicesUri = "listDevices";
        
        // dictionary which holds all the renderers found by UPNP and which may be handled by the Raumfeld System
        protected Dictionary<string, Renderer.Renderer> renderers;


        public delegate void delegate_OnMediaRendererFound();
        public event delegate_OnMediaRendererFound mediaRendererFound;

        public delegate void delegate_OnMediaRendererRemoved();
        public event delegate_OnMediaRendererRemoved mediaRendererRemoved;

        public delegate void delegate_OnMuteStateChanged(String _rendererUDN, Boolean _mute);
        public event delegate_OnMuteStateChanged rendererMuteStateChanged;

        public delegate void delegate_OnVolumeChanged(String _rendererUDN, uint _volume);
        public event delegate_OnVolumeChanged rendererVolumeChanged;


        public RendererManager(UPNP.UNPN _upnpStack)
            : base(_upnpStack)
        {
            renderers = new Dictionary<string, Renderer.Renderer>();
        }

        /*
        public void retrieveRenderers(String _updateId = "")
        {
            Dictionary<String, String> retrieveDevicesParms = null;
            string retrieveDevicesRequestUri = Global.getMediaServerManager().mediaServerRequestUriBase + retrieveDevicesUri;
            this.writeLog(LogType.Info, String.Format("Starte Device Request mit updateId: '{0}'", _updateId));

            // set parameters for "long polling"
            if (!String.IsNullOrEmpty(_updateId))
            {
                retrieveDevicesParms = new Dictionary<String, String>();
                retrieveDevicesParms.Add("updateId", _updateId);
            }

            this.httpPostRequestAsync(retrieveDevicesRequestUri, null, retrieveDevicesParms, retrieveRenderersResponseSink);
        }

        protected void retrieveRenderersResponseSink(HttpWebResponse _response)
        {
            Stream stream = _response.GetResponseStream();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);
            this.writeLog(LogType.Info, "Renderer Request XML erhalten. Parse...");

            // TODO: @@@

            // the listDevices WebRequest is a "long poll" web request, so we may send the request with the
            // given updateId in the header and then we wil be evented all the time the zone management changes!
            String updateId = _response.Headers["updateId"];
            if (!String.IsNullOrEmpty(updateId))
                this.retrieveRenderers(updateId);
        }
         */

        public void findMediaRenderer()
        {
            this.writeLog(LogType.Info, "Beginne 'Listen' nach Media Renderer");

            upnpStack.onMediaRendererFound += mediaRendererFoundSink;
            upnpStack.onMediaRendererRemoved += mediaRendererRemovedSink;
            upnpStack.findMediaRenderer();
        }

        protected void mediaRendererFoundSink(CpAVRenderer _avRenderer)
        {
            Renderer.Renderer   renderer;
            
            this.writeLog(LogType.Info, String.Format("MediaRenderer '{0}' gefunden (UDN: {1})", _avRenderer.FriendlyName, _avRenderer.UniqueDeviceName));            

            if (_avRenderer.isVirtualRenderer())
            {
                renderer = new RendererVirtual(_avRenderer);
                if (renderers.ContainsKey(renderer.udn))
                    renderers.Remove(renderer.udn);
                renderers.Add(renderer.udn, renderer);
                this.linkVirtualRendererToZone((RendererVirtual)renderer);
            }
            else
            {
                renderer = new RendererRoom(_avRenderer);
                if (renderers.ContainsKey(renderer.udn))
                    renderers.Remove(renderer.udn);
                renderers.Add(renderer.udn, renderer);
                this.linkRoomRendererToRoom((RendererRoom)renderer);
            }

            renderer.muteStateChanged += renderer_muteStateChangedSink;
            renderer.volumeChanged += renderer_volumeChangedSink;
            
            if (mediaRendererFound != null) mediaRendererFound();
        }

        protected void renderer_volumeChangedSink(string _rendererUDN, uint _volume)
        {
            if (rendererVolumeChanged != null) rendererVolumeChanged(_rendererUDN, _volume);
        }

        protected void renderer_muteStateChangedSink(string _rendererUDN, bool _mute)
        {
            if (rendererMuteStateChanged != null) rendererMuteStateChanged(_rendererUDN, _mute);
        }

        protected void mediaRendererRemovedSink(CpAVRenderer _avRenderer)
        {
            Renderer.Renderer renderer;

            this.writeLog(LogType.Info, String.Format("MediaRenderer '{0}' verloren (UDN: {1})", _avRenderer.FriendlyName, _avRenderer.UniqueDeviceName));

            if (!renderers.ContainsKey(_avRenderer.UniqueDeviceName))
                return;

            if (!renderers.TryGetValue(_avRenderer.UniqueDeviceName, out renderer))
                return;

            if (renderer.isVirtualRenderer())
                this.linkVirtualRendererToZone((RendererVirtual)renderer, true, false);

            renderers.Remove(renderer.udn);            

            if (mediaRendererFound != null) mediaRendererRemoved();
        }

        public List<Renderer.Renderer> getRenderers()
        {
            return new List<Renderer.Renderer>(renderers.Values);
        }

        public Renderer.Renderer getRenderer(String _rendererUDN)
        {
            Renderer.Renderer renderer;
            if (renderers.TryGetValue(_rendererUDN, out renderer))
                return renderer;

            this.writeLog(LogType.Error, String.Format("Renderer mit UDN '{0}' nicht gefunden", _rendererUDN));

            return null;
        }

        public RendererVirtual getRendererByZoneUDN(String _zoneUDN)
        {
            Renderer.Renderer renderer;
            Zone zone = Global.getZoneManager().getZone(_zoneUDN);
            if (String.IsNullOrWhiteSpace(zone.rendererUDN))
                return null;
            if (zone != null && renderers.TryGetValue(zone.rendererUDN, out renderer))
                return (RendererVirtual)renderer;

            this.writeLog(LogType.Error, String.Format("Renderer für Zonne mit UDN '{0}' nicht gefunden", _zoneUDN));

            return null;
        }    

        public void linkVirtualRendererToZone(RendererVirtual _virtualRenderer, Boolean _unlink = false, Boolean _crossLink = true)
        {
            Zone zone;
            ZoneManager zoneManager = Global.getZoneManager();
            zone = zoneManager.getZoneUUID(_virtualRenderer.getRendererObject().UniqueDeviceName);
            if (zone != null)
            {
                if (_unlink)
                    zone.rendererUDN = String.Empty;
                else
                    zone.rendererUDN = _virtualRenderer.udn;
                zone.rendererLinked();
                //zoneManager.updateZonesValue(zone);
                if (_crossLink)
                    zoneManager.linkZoneToVirtualRenderer(zone, _unlink, false);

                if (_unlink == true)
                    this.writeLog(LogType.Info, String.Format("Renderer '{1}' wurde von Zone '{0}' entfernt", zone.udn, _virtualRenderer.udn));
                else
                    this.writeLog(LogType.Info, String.Format("Renderer '{1}' wurde zu Zone '{0}' zugeordnet", zone.udn, _virtualRenderer.udn));
            }
            else
            {
                this.writeLog(LogType.Warning, String.Format("Keine Zone für virtuellen Renderer '{0}' für Zuordnung gefunden", _virtualRenderer.getRendererObject().UniqueDeviceName));
            }
        }

        public void linkRoomRendererToRoom(RendererRoom _roomRenderer, Boolean _unlink = false, Boolean _crossLink = true)
        {            
            Room room;
            ZoneManager zoneManager = Global.getZoneManager();

            // the renderer id is given by the xml we get from the zoneInformation
            room = zoneManager.getRoomByRendererUDN(_roomRenderer.getRendererObject().UniqueDeviceName);            
            if (room != null)
            {
                if (_unlink)
                    room.rendererUDN = String.Empty;
                else
                    room.rendererUDN = _roomRenderer.udn;
                room.rendererLinked();
                //zoneManager.updateRoomsValue(room);
                if (_crossLink)
                    zoneManager.linkRoomToRoomRenderer(room, _unlink, false);

                if (_unlink == true)
                    this.writeLog(LogType.Info, String.Format("Renderer '{1}' wurde von Raum '{0}' entfernt", room.name, _roomRenderer.udn));
                else
                    this.writeLog(LogType.Info, String.Format("Renderer '{1}' wurde zu Raum '{0}' zugeordnet", room.name, _roomRenderer.udn));
            }
            else
            {
                this.writeLog(LogType.Warning, String.Format("Kein Raum für Renderer '{0}' für Zuordnung gefunden", _roomRenderer.getRendererObject().UniqueDeviceName));
            }
        }

        /*
        public void updateRenderersValue(Renderer.Renderer _renderer)
        {
            Renderer.Renderer rendererDummy;
            if (renderers.TryGetValue(_renderer.udn, out rendererDummy))
                renderers[_renderer.udn] = _renderer;
            else
                renderers.Add(_renderer.udn, _renderer);
        }*/

        
    }
}
