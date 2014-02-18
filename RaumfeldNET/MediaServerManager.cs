using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;


using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;

namespace RaumfeldNET
{
    public class MediaServerManager : BaseManager
    {
        // we can only handle one media server (and this has to be a Raumfeld-MediaServer)
        protected CpMediaServer mediaServer;
        protected CpContentDirectory contentDirectory;

        // resolved ip address of media server
        public String mediaServerIpAddress;

        // resolved base request uri for web request
        public String mediaServerRequestUriBase;


        const String raumfeldMediaServerDeviceName = "Raumfeld MediaServer";
        const String raumfeldMediaServerRequestPort = "47365";


        public delegate void delegate_OnMediaServerFound();
        public event delegate_OnMediaServerFound mediaServerFound;

        public delegate void delegate_OnMediaServerRemoved();
        public event delegate_OnMediaServerRemoved mediaServerRemoved;


        public MediaServerManager(UPNP.UNPN _upnpStack)
            : base(_upnpStack)
        {
        }

        // start async search for Raumfeld-Mediaserver
        public void findMediaServer()
        {
            this.writeLog(LogType.Info, "Beginne 'Listen' nach Raumfeld MediaServer");

            upnpStack.onMediaServerFound += mediaServerFoundSink;
            upnpStack.onMediaServerRemoved += mediaServerRemovedSink;
            upnpStack.findMediaServer(); 
        }

        protected void mediaServerFoundSink(CpMediaServer _mediaServer)
        {
            this.writeLog(LogType.Info, String.Format("MediaServer '{0}' gefunden", _mediaServer.ServerFriendlyName));

            if (_mediaServer.ServerFriendlyName != raumfeldMediaServerDeviceName)
                return;
            
            mediaServer = _mediaServer;
            mediaServerIpAddress = this.getMediaServerIP(mediaServer);
            mediaServerRequestUriBase = this.getMediaServerRequestUri();

            contentDirectory = mediaServer.contentDirectory;
            contentDirectory.onStateVariableContainerUpdateIds += contentDirectory_onStateVariableContainerUpdateIds;

            this.writeLog(LogType.Info, String.Format("Medienserver IP: '{0}', RequestUrl: '{1}'", mediaServerIpAddress, mediaServerRequestUriBase));

            if (mediaServerFound != null) mediaServerFound();
            
        }      

        protected void mediaServerRemovedSink(CpMediaServer _mediaServer)
        {
            this.writeLog(LogType.Info, String.Format("MediaServer '{0}' verloren", _mediaServer.ServerFriendlyName));

            if (_mediaServer.ServerFriendlyName != raumfeldMediaServerDeviceName)
                return;

            mediaServer = null;
            contentDirectory = null;
            if (mediaServerRemoved != null) mediaServerRemoved();
        }

        public CpMediaServer getMediaServer()
        {
            return mediaServer;
        }

        protected String getMediaServerIP(CpMediaServer _mediaServer)
        {
            if (_mediaServer == null)
                return String.Empty;

            this.writeLog(LogType.Info, "Auflösen der Raumfeld-MedienServer IP");

            Uri url = new Uri(mediaServer.Location);           

            try
            {
                IPAddress address = IPAddress.Parse(url.Host);
                return address.ToString();
            }
            catch (Exception e)
            {
                this.writeLog(LogType.Warning, String.Format("IP-Adresse konnte nicht aufgelöst werden. Host: {0} Suche über DNS", url.Host), e);
            }

            try
            {                
                IPHostEntry hostEntry = Dns.GetHostEntry(url.Host);
                return hostEntry.AddressList[0].ToString();
            }
            catch (Exception e)
            {
                this.writeLog(LogType.Error, String.Format("IP-Adresse konnte nicht per DNS aufgelöst werden. Host: {0}", url.Host), e);
                // this is a fatal error! App hast to crash!
                throw new Exception(Global.getCrashInfo());
            }

        }

        protected String getMediaServerRequestUri()
        {
            String hostRequestUri;
            if (String.IsNullOrEmpty(mediaServerIpAddress))
            {
                this.writeLog(LogType.Error, "Keine gültige Host IP für erstellen der Request URL!");
                // this is a fatal error! App hast to be crashed!
                throw new Exception(Global.getCrashInfo());
            }
            hostRequestUri = String.Format("http://{0}:{1}/index/", mediaServerIpAddress, raumfeldMediaServerRequestPort);
            return hostRequestUri;
        }

        public CpContentDirectory getContentDirectory()
        {
            return contentDirectory;
        }

        void contentDirectory_onStateVariableContainerUpdateIds(CpContentDirectory _contentDirectory, string _value)
        {
            // zone title list may be update. So call some method on the manager to let him do his work
            Global.getZoneTitleListManager().retrieveListByContainerUpdateId(_value);
        }
    }
}
