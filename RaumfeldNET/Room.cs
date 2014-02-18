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
    public class Room : Base.Base
    {
        public String udn;        
        public String uuid;
        public String name;
        public String color;

        // a room has at least one renderer. There may be more than one, but we can only handle one for now!
        // the renderer in the room is needed for volumeControl, so only Rooms in zones need to have this one linked
        public String rendererUDN;

        public String zoneUDN;


        public RendererRoom getRenderer()
        {
            return (RendererRoom)Global.getRendererManager().getRenderer(rendererUDN);
        }

        public void rendererLinked()
        {
        }

        public Zone getZone()
        {
            return (Zone)Global.getZoneManager().getZone(zoneUDN);
        }
    }
}
