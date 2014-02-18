using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;
using RaumfeldNET.Renderer;

namespace RaumfeldNET
{
    public class Zone : Base.Base
    {
        public String udn;
        public String uuid;

        public String name;

        // a zone has at virtual renderer. this is built by the host and is noi hardware renderere
        // it's there to combine more rendereres to one Zone. So a virtualRenderer is a Zone
        public String rendererUDN;

        // id for the current TrackList in this zone. The List itself can be retrieved from the MediaList-Manager
        public String trackListId;

        // a zone has some rooms attached
        public List<String> roomUDNs;

        public delegate void delegate_OnPlayStateChanged(String _zoneUDN, RendererPlayState _playState);
        public event delegate_OnPlayStateChanged playStateChanged;

        public delegate void delegate_OnPlayModeChanged(String _zoneUDN, AvTransportPlayMode _playMode);
        public event delegate_OnPlayModeChanged playModeChanged;

        public delegate void delegate_OnTrackChanged(String _zoneUDN, uint _newTrackIdx);
        public event delegate_OnTrackChanged trackChanged;

        public delegate void delegate_OnTrackPositionChanged(String _zoneUDN, String _absTime);
        public event delegate_OnTrackPositionChanged trackPositionChanged;

        public Zone()
            :base()
        {
            roomUDNs = new List<string>();
        }


        public void rendererLinked()
        {
            //this.setPlayStateChanged(this.getRenderer().playState);
        }

        public void trackListCreated()
        {
            //if (this.getRenderer()!=null)
            //    this.setPlayStateChanged(this.getRenderer().playState);
        }

        public RendererVirtual getRenderer()
        {
            if (String.IsNullOrWhiteSpace(rendererUDN))
                return null;
            return (RendererVirtual)Global.getRendererManager().getRenderer(rendererUDN);
        }

        public void createZoneNameFromRooms()
        {
            Room room;

            name = "";

            foreach (var roomId in roomUDNs)
            {
                room = Global.getZoneManager().getRoom(roomId);
                if (!String.IsNullOrEmpty(name))
                    name += "\n";
                name += room.name;
            }
        }

        public void setPlayStateChanged(Renderer.RendererPlayState _playState)
        {
            Global.getZoneTitleListManager().updatePlayStateOnTrackItem(trackListId, Global.rendererPlayStateToTrackPlayState(_playState));
            if (playStateChanged != null) this.playStateChanged(udn, _playState);
        }

        public void setPlayModeChanged(AvTransportPlayMode _playMode)
        {
            if (playModeChanged != null) this.playModeChanged(udn, _playMode);
        }

        public void setTrackChanged(uint _newTrackIdx)
        {
            if (trackChanged != null) this.trackChanged(udn, _newTrackIdx);
        }

        public void setTrackPositionChanged(String _absTime)
        {
            if (trackChanged != null) this.trackPositionChanged(udn, _absTime);
        }

        public void updatePlayStateFromRenderer()
        {
            if (this.getRenderer() != null)
                this.setPlayStateChanged(this.getRenderer().playState);
        }
        
    }
}
