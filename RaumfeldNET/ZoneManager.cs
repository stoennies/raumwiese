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

    public class ZoneManager : BaseManager
    {
        // identifier for the webReqest method for getting the zones
        private const string retrieveZonesUri = "getZones";
        // identifier for the webReqest method for connecting a room to a zone
        private const string connectRoomToZoneUri = "connectRoomToZone";
        // identifier for the webReqest method for connecting a room to a zone
        private const string dropRoomJobUri = "dropRoomJob";

        // dictionary which holds the zones
        protected Dictionary<string, Zone> zones;

        // dictionary which holds all rooms (assigned and unassigned)
        protected Dictionary<string, Room> rooms;

        public delegate void delegate_OnZonesRetrieved();
        public event delegate_OnZonesRetrieved zonesRetrieved;

        public delegate void delegate_OnAllRenderersLinked();
        public event delegate_OnAllRenderersLinked allRenderersLinked;

        public delegate void delegate_OnZonePlayStateChanged(String _zoneUDN, RendererPlayState _playState);
        public event delegate_OnZonePlayStateChanged zonePlayStateChanged;

        public delegate void delegate_OnZonePlayModeChanged(String _zoneUDN, AvTransportPlayMode _playMode);
        public event delegate_OnZonePlayModeChanged zonePlayModeChanged;

        public delegate void delegate_OnZoneTrackChanged(String _zoneUDN, uint _newTrackIdx);
        public event delegate_OnZoneTrackChanged zoneTrackChanged;
        
        public delegate void delegate_OnZoneTrackPositionChanged(String _zoneUDN, String _absTime);
        public event delegate_OnZoneTrackPositionChanged zoneTrackPositionChanged;


        public ZoneManager(UPNP.UNPN _upnpStack)
            : base(_upnpStack)
        {
            zones = new Dictionary<string, Zone>();
            rooms = new Dictionary<string, Room>();
        }

        public List<Zone> getZones()
        {
            return new List<Zone>(zones.Values);
        }

        public List<Room> getRooms()
        {
            return new List<Room>(rooms.Values);
        }

        public List<String> getLinkedRooms(String _zoneUdn)
        {
            List<String> list = new List<String>();
            Room room;
            lock(this.rooms)
            {
                foreach(var pair in rooms)  
                {
                    room = pair.Value;
                    if(!String.IsNullOrWhiteSpace(room.zoneUDN) && room.zoneUDN == _zoneUdn)
                        list.Add(room.udn);
                }
            }
            return list;
        }

        public Zone getZone(String _zoneUDN)
        {
            Zone zone;
            if(zones.TryGetValue(_zoneUDN, out zone))
                return zone;

            this.writeLog(LogType.Error, String.Format("Zone mit UDN '{0}' nicht gefunden", _zoneUDN));

            return null;
        }

        public Room getRoom(String _roomUDN)
        {
            Room room;
            if (rooms.TryGetValue(_roomUDN, out room))
                return room;

            this.writeLog(LogType.Error, String.Format("Raum mit UDN '{0}' nicht gefunden", _roomUDN));

            return null;
        }

        public Room getRoomByRendererUDN(String _rendererUDN)
        {
            Room room;

            room = rooms.FirstOrDefault(x => x.Value.rendererUDN == _rendererUDN).Value;
            if (room != null)
                return room;

            this.writeLog(LogType.Error, String.Format("Raum mit RendererUDN '{0}' nicht gefunden", _rendererUDN));

            return null;
        }

        public Zone getZoneUUID(String _zoneUUID)
        {
            return getZone("uuid:"+_zoneUUID);
        }

        public Room getRoomUUID(String _roomUUID)
        {
            return getRoom("uuid:" + _roomUUID);
        }      

        public void retrieveZones(String _updateId = "")
        {
            Dictionary<String,String> retrieveZonesParms = null;
            string retrieveZonesRequestUri = Global.getMediaServerManager().mediaServerRequestUriBase + retrieveZonesUri;
            this.writeLog(LogType.Info, String.Format("Starte Zonen Request mit updateId: '{0}'", _updateId));                     

            // set parameters for "long polling"
            if(!String.IsNullOrEmpty(_updateId))
            {
                retrieveZonesParms = new Dictionary<String, String>();
                retrieveZonesParms.Add("updateId", _updateId);
            }

            webRequestHelper.httpPostRequestAsync(retrieveZonesRequestUri, null, retrieveZonesParms, retrieveZonesResponseSink);
        }        

        protected void retrieveZonesResponseSink(HttpWebResponse _response)
        {
            Stream stream = _response.GetResponseStream();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);
            this.writeLog(LogType.Info, "Zonen Request XML erhalten. Parse...");
            this.handleZoneXml(xmlDoc);
            this.checkAllRenderersLinked();
            
            // the retrieveZones WebRequest is a "long poll" web request, so we may send the request with the
            // given updateId in the header and then we wil be evented all the time the zone management changes!
            String updateId = _response.Headers["updateId"];
            if(!String.IsNullOrEmpty(updateId))
                this.retrieveZones(updateId);
        }

        private void handleZoneXml(XmlDocument _zoneXmlDocument)
        {
            try
            {               
                if (_zoneXmlDocument == null)
                    return;     
           
                lock(zones)    
                {
                    lock(rooms)
                    {
                        zones.Clear();
                        rooms.Clear();

                        this.handleZoneXmlZone(_zoneXmlDocument);
                        this.handleZoneXmlUnassignedRooms(_zoneXmlDocument);
                    }
                }

                if (zonesRetrieved != null) zonesRetrieved();
            }
            catch (Exception _e)
            {
                this.writeLog(LogType.Error, "Fehler beim Parsen des Zonen-XML", _e, _zoneXmlDocument.OuterXml);
            }            
        }

        private void handleZoneXmlZone(XmlDocument _zoneXmlDocument)
        {
            XmlNodeList nodes;
            Zone newZone;           

            nodes = _zoneXmlDocument.SelectNodes("/zoneConfig/zones/zone");
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    newZone = this.createZoneForXMLNode(node);
                    zones.Add(newZone.udn, newZone);
                    //this.updateZonesValue(newZone);
                    this.linkZoneToVirtualRenderer(newZone);
                    newZone.playStateChanged += zonePlayStateChangedSink;
                    newZone.playModeChanged += zonePlayModeChangedSink;
                    newZone.trackChanged += zoneTrackChangedSink;
                    newZone.trackPositionChanged += zoneTrackPositionChangedSink;

                }
            }
        }

        private void zonePlayModeChangedSink(string _zoneUDN, AvTransportPlayMode _playMode)
        {
            if (zonePlayModeChanged != null) this.zonePlayModeChanged(_zoneUDN, _playMode);
        }

        private void zoneTrackChangedSink(string _zoneUDN, uint _newTrackIdx)
        {
            if (zoneTrackChanged != null) this.zoneTrackChanged(_zoneUDN, _newTrackIdx);
        }

        private void zoneTrackPositionChangedSink(string _zoneUDN, String _absTime)
        {
            if (zoneTrackPositionChanged != null) this.zoneTrackPositionChanged(_zoneUDN, _absTime);
        }

        private void zonePlayStateChangedSink(String _zoneUDN, RendererPlayState _playState)
        {
            if (zonePlayStateChanged != null) this.zonePlayStateChanged(_zoneUDN, _playState);
        }

        private void handleZoneXmlUnassignedRooms(XmlDocument _zoneXmlDocument)
        {
            XmlNodeList nodes;
            Room newRoom;           

            nodes = _zoneXmlDocument.SelectNodes("/zoneConfig/unassignedRooms/room");
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    newRoom = this.createRoomForXMLNode(node);
                    rooms.Add(newRoom.udn, newRoom);
                }
            }
        }

        private Zone createZoneForXMLNode(XmlNode _zoneNode)
        {
            if (_zoneNode == null)
                return null;

            try
            {
                Zone newZone = new Zone();
                ZoneTrackMediaList mediaList;
                XmlNodeList nodesAssignedRooms;
                Room newRoom;

                newZone.udn = xmlDocumentHelper.getNodeAttributeValue(_zoneNode, "udn");
                newZone.uuid = newZone.udn.Substring(5);

                mediaList = Global.getZoneTitleListManager().getListForZone(newZone.udn);
                if (mediaList != null)
                    newZone.trackListId = mediaList.listId;

                if (String.IsNullOrWhiteSpace(newZone.udn))
                    throw new Exception("Zone hat keinen UDN Identifier");

                nodesAssignedRooms = _zoneNode.SelectNodes("room");
                if (nodesAssignedRooms != null)
                {
                    foreach (XmlNode nodeRoom in nodesAssignedRooms)
                    {
                        newRoom = this.createRoomForXMLNode(nodeRoom);
                        newRoom.zoneUDN = newZone.udn;
                        rooms.Add(newRoom.udn, newRoom);
                        this.writeLog(LogType.Info, String.Format("Raum '{0}' in Zone '{1}' gefunden", newRoom.name, newZone.udn));
                        this.linkRoomToRoomRenderer(newRoom);
                        newZone.roomUDNs.Add(newRoom.udn);
                    }
                }

                newZone.createZoneNameFromRooms();

                this.writeLog(LogType.Info, String.Format("Zone UDN: '{0}' erstellt", newZone.udn));
                return newZone;
            }
            catch (Exception e)
            {
                this.writeLog(LogType.Error, "Fehler beim erstellen von Zone aus XML-Knoten", e, _zoneNode.InnerXml);
                return null;
            }
        }

        private Room createRoomForXMLNode(XmlNode _roomNode)
        {
            if (_roomNode == null)
                return null;

            try
            {
                Room  newRoom = new Room();

                newRoom.udn = xmlDocumentHelper.getNodeAttributeValue(_roomNode, "udn");
                newRoom.uuid = newRoom.udn.Substring(5);
                newRoom.name = xmlDocumentHelper.getNodeAttributeValue(_roomNode, "name");
                newRoom.color = xmlDocumentHelper.getNodeAttributeValue(_roomNode, "color");
                // we store real UDN of renderer and this is the same but without the uuid!
                newRoom.rendererUDN = xmlDocumentHelper.getChildNodeAttributeValue(_roomNode, "renderer", "udn");
                newRoom.rendererUDN = newRoom.rendererUDN.Substring(5);

                // if room has no color assigned, then give him some white painting
                if (String.IsNullOrWhiteSpace(newRoom.color))
                    newRoom.color = "#FFFFFF";

                if (String.IsNullOrWhiteSpace(newRoom.udn))
                    throw new Exception("Raum hat keinen UDN Identifier");

                this.writeLog(LogType.Info, String.Format("Raum UDN: '{0}' erstellt", newRoom.udn));
                return newRoom;
            }
            catch (Exception e)
            {
                this.writeLog(LogType.Error, "Fehler beim erstellen von Raum aus XML-Knoten", e, _roomNode.InnerXml);
                return null;
            }
        }       

        public void linkZoneToVirtualRenderer(Zone _zone, Boolean _unlink = false, Boolean _crossLink = true)
        {
            RendererManager rendererManager = Global.getRendererManager();
            RendererVirtual renderer;

            // renderer UDN's are stored without 'uuid:' so we have to search with the uuid value cause this is the udn of the renderer
            renderer = (RendererVirtual)rendererManager.getRenderer(_zone.uuid);
            if (renderer != null)
            {
                if (_unlink)
                    renderer.zoneUDN = String.Empty;
                else
                    renderer.zoneUDN = _zone.udn;
                renderer.zoneLinked();
                //rendererManager.updateRenderersValue(renderer);
                if(_crossLink)
                    rendererManager.linkVirtualRendererToZone(renderer, _unlink, false);

                this.checkAllRenderersLinked();

                if (_unlink == true)
                    this.writeLog(LogType.Info, String.Format("Zone '{0}' wurde von Renderer '{1}' entfernt", _zone.udn, renderer.udn));
                else
                    this.writeLog(LogType.Info, String.Format("Zone '{0}' wurde zu Renderer '{1}' zugeordnet", _zone.udn, renderer.udn));
            }
            else
                this.writeLog(LogType.Warning, String.Format("Kein Virtueller Renderer für Zone '{0}' für Zuordnung gefunden", _zone.udn));
            
        }

        public void linkRoomToRoomRenderer(Room _room, Boolean _unlink = false, Boolean _crossLink = true)
        {
            RendererManager rendererManager = Global.getRendererManager();
            RendererRoom renderer;
            
            renderer = (RendererRoom)rendererManager.getRenderer(_room.rendererUDN);
            if (renderer != null)
            {
                if (_unlink)
                    renderer.roomUDN = String.Empty;
                else
                    renderer.roomUDN = _room.udn;
                renderer.roomLinked();
                //rendererManager.updateRenderersValue(renderer);
                if (_crossLink)
                    rendererManager.linkRoomRendererToRoom(renderer, _unlink, false);

                this.checkAllRenderersLinked();

                if (_unlink == true)
                    this.writeLog(LogType.Info, String.Format("Raum '{0}' wurde von Renderer '{1}' entfernt", _room.udn, renderer.udn));
                else
                    this.writeLog(LogType.Info, String.Format("Raum '{0}' wurde zu Renderer '{1}' zugeordnet", _room.udn, renderer.udn));
            }
            else
                this.writeLog(LogType.Warning, String.Format("Kein Renderer für Raum '{0}' für Zuordnung gefunden", _room.udn));

        }

        protected Boolean areAllRenderersLinked()
        {            
            lock (zones)
            {
                lock (rooms)
                {

                    String[] keys = zones.Keys.ToArray<String>();
                    for (int zoneIdx = 0; zoneIdx < keys.Length; zoneIdx++)
                    {
                        if (String.IsNullOrEmpty(zones[keys[zoneIdx]].rendererUDN))
                            return false;
                    }

                    /*
                    keys = rooms.Keys.ToArray<String>();
                    for (int roomIdx = 0; roomIdx < keys.Length; roomIdx++)
                    {
                        if (String.IsNullOrEmpty(rooms[keys[roomIdx]].rendererUDN))
                            return false;
                    }
                     */
                }
            }

            return true;
        }

        protected void checkAllRenderersLinked()
        {
            this.writeLog(LogType.Info, "Prüfe ob alle Zonen/Räume einen Renderer zugewiesen haben");
            if (this.areAllRenderersLinked())
            {
                this.writeLog(LogType.Info, "Alle Zonen/Räume haben einen aktiven Renderer");
                if(allRenderersLinked != null) this.allRenderersLinked();
            }
            else
                this.writeLog(LogType.Info, "Es wurden nicht alle Renderer zu den Zonen/Räumen hinzugefügt");
        }

        public void connectRoomToZone(String _roomUDN, String _zoneUDN = "")
        {
            Dictionary<String, String> connectRoomToZoneParms = new Dictionary<String,String>();
            string connectRoomToZoneRequestUri = Global.getMediaServerManager().mediaServerRequestUriBase + connectRoomToZoneUri;

            if (!String.IsNullOrEmpty(_roomUDN))
                connectRoomToZoneParms.Add("roomUDN", _roomUDN);
            if (!String.IsNullOrEmpty(_zoneUDN))
                connectRoomToZoneParms.Add("zoneUDN", _zoneUDN);

            webRequestHelper.httpPostRequest(connectRoomToZoneRequestUri, connectRoomToZoneParms, null);
        }

        public void dropRoomFromZone(String _roomUDN)
        {
            Dictionary<String, String> dropRoomJobParms = new Dictionary<String,String>();
            string dropRoomJobRequestUri = Global.getMediaServerManager().mediaServerRequestUriBase + dropRoomJobUri;

            if (!String.IsNullOrEmpty(_roomUDN))
                dropRoomJobParms.Add("roomUDN", _roomUDN);

            webRequestHelper.httpPostRequest(dropRoomJobRequestUri, dropRoomJobParms, null);
        }
    }

}

/*
 *  XmlNodeList nodes;
            XmlNodeList nodesRoom;
            XmlNodeList nodesRenderer;

            if (_xmlDoc == null)
                return;

            zoneDict.Clear();

            nodes = _xmlDoc.SelectNodes("/zoneConfig/zones/zone");
            if (nodes != null)
            {
                // get zones and the rooms asigned to the zones
                foreach (XmlNode node in nodes)
                {
                    // create zone
                    Zone newZone = new Zone(this);
                    if (node.Attributes["udn"] == null)
                        throw new System.ArgumentException("Zone has no UDN!", "original");
                    newZone.udn = node.Attributes["udn"].InnerText;
                    newZone.uuid = newZone.udn.Substring(5);
                    nodesRoom = node.SelectNodes("room");
                    if (nodesRoom != null)
                    {
                        foreach (XmlNode nodeRoom in nodesRoom)
                        {
                            // add room to zone
                            RoomInformation roomInfo = new RoomInformation();
                            if (nodeRoom.Attributes["udn"] == null)
                                throw new System.ArgumentException("Room has no UDN!", "original");
                            roomInfo.udn = nodeRoom.Attributes["udn"].InnerText;
                            roomInfo.udn = roomInfo.udn.Substring(5);
                            if (nodeRoom.Attributes["color"] != null)
                                roomInfo.color = nodeRoom.Attributes["color"].InnerText;
                            else
                                roomInfo.color = "#FFFFFF";
                            if (nodeRoom.Attributes["name"]!=null)
                                roomInfo.name = nodeRoom.Attributes["name"].InnerText;
                            nodesRenderer = nodeRoom.SelectNodes("renderer");
                            if (nodesRenderer != null)
                            {
                                foreach (XmlNode nodeRenderer in nodesRenderer)
                                {
                                    if (nodeRenderer.Attributes["udn"] == null)
                                        throw new System.ArgumentException("Renderer has no UDN!", "original");
                                    roomInfo.rendererUdn = nodeRenderer.Attributes["udn"].InnerText;
                                    roomInfo.rendererUdn = roomInfo.rendererUdn.Substring(5);
                                    roomInfo.uuid = roomInfo.rendererUdn.Substring(5);
                                }
                            }
                            newZone.addRoomInfo(roomInfo);
                        }
                    }
                    newZone.playlist.setContentDirectoryObject(contentDirectory);
                    newZone.linkToRendererVirtual();

                    newZone.onPlaylistChanged += new Zone.VariableChangeHandler(ZonePlaylistUpdatedSink);
                    newZone.onPlayStateChanged += new Zone.VariableChangeHandler(ZonePlayStateChangedSink);
                    newZone.onTrackChanged += new Zone.VariableChangeHandler(ZoneTrackChangedSink);
                    newZone.onTrackPositionChanged += new Zone.VariableChangeHandler(ZoneTrackPositionChangedSink);
                    newZone.onPlaylistImageLoaded += new Zone.onPlaylistImageLoadedHandler(ZonePlaylistItemImageLoaded);
                    newZone.onRendererVolumeChanged += new Zone.onRendererVolumeChangedHandler(ZoneRendererVolumeChangedSink);
                    newZone.onRendererMuteStateChanged += new Zone.onRendererMuteStateChangedHandler(ZoneRendererMuteStateChangedSink);
                    newZone.onVirtualRendererLinked += new Zone.VariableChangeHandler(ZoneVirtualRendererLinked);
                    newZone.onPlaylistWillBeChanged += new Zone.VariableChangeHandler(ZonePlaylistWillBeChanged);

                    zoneDict.Add(newZone.udn, newZone);
                }
            }

            // get unassigned rooms
            nodes = _xmlDoc.SelectNodes("/zoneConfig/unassignedRooms");
            // get zones and the rooms asigned to the zones
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    //Console.WriteLine("{0}", "unassigned rooms");
                    nodesRoom = node.SelectNodes("room");
                    if (nodesRoom != null)
                    {
                        foreach (XmlNode nodeRoom in nodesRoom)
                        {
                            Zone newZone = new Zone(this);
                            newZone.udn = "";

                            // add room to zone
                            RoomInformation roomInfo = new RoomInformation(); 
                            if (nodeRoom.Attributes["udn"] == null)
                                throw new System.ArgumentException("Room has no UDN!", "original");
                            roomInfo.udn = nodeRoom.Attributes["udn"].InnerText;
                            roomInfo.udn = roomInfo.udn.Substring(5);
                            if (nodeRoom.Attributes["color"] != null)
                                roomInfo.color = nodeRoom.Attributes["color"].InnerText;
                            else
                                roomInfo.color = "#FFFFFF";
                            if (nodeRoom.Attributes["name"] != null)
                                roomInfo.name = nodeRoom.Attributes["name"].InnerText;
                            nodesRenderer = nodeRoom.SelectNodes("renderer");
                            if (nodesRenderer != null)
                            {
                                foreach (XmlNode nodeRenderer in nodesRenderer)
                                {
                                    if (nodeRenderer.Attributes["udn"] == null)
                                        throw new System.ArgumentException("Renderer has no UDN!", "original");
                                    roomInfo.rendererUdn = nodeRenderer.Attributes["udn"].InnerText;
                                    roomInfo.rendererUdn = roomInfo.rendererUdn.Substring(5);
                                    roomInfo.uuid = roomInfo.rendererUdn.Substring(5);
                                }
                            }
                            newZone.addRoomInfo(roomInfo);
                            newZone.playlist.setContentDirectoryObject(contentDirectory);
                            // use room udn as key becaus zonw udn is empty!
                            zoneDict.Add(roomInfo.udn, newZone);
                            //Console.WriteLine("    {0}", nodeRoom.Attributes["name"].InnerText);
                        }
                    }
                }
            }

            this.AttachRoomRenderersToZones();

            if (zonesUpdated != null) this.zonesUpdated();
 * 
 * */
