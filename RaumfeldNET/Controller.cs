using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RaumfeldNET.Renderer;
using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;

namespace RaumfeldNET
{
    public class Controller : Base.Base
    {
        private UPNP.UNPN upnpStack;

        public ZoneManager zoneManager;
        public MediaServerManager mediaServerManager;
        public RendererManager rendererManager;
        public ConfigManager configManager;
        public ImageDataCache imageDataCache;
        public ZoneTitleListManager zoneTitleListManager;
        public ContentDirectoryBrowserMulti contentBrowser;
        public PlaylistBrowser playlistBrowser;
        public PlaylistManagement playlistManagement;
        public LogWriter logWriter;

        
        public delegate void delegate_OnRaumfeldMediaServerFound();
        public event delegate_OnRaumfeldMediaServerFound mediaServerFound;

        public delegate void delegate_OnRaumfeldMediaServerRemoved();
        public event delegate_OnRaumfeldMediaServerRemoved mediaServerRemoved;

        public delegate void delegate_OnZonesRetrieved();
        public event delegate_OnZonesRetrieved zonesRetrieved;

        public delegate void delegate_OnAllRenderersLinked();
        public event delegate_OnAllRenderersLinked allRenderersLinked;

        public delegate void delegate_OnZoneTrackListReady(String _zoneUDN, String _trackListId);
        public event delegate_OnZoneTrackListReady zoneTrackListReady;

        public delegate void delegate_OnZoneTrackChanged(String _zoneUDN, uint _newTrackIdx);
        public event delegate_OnZoneTrackChanged zoneTrackChanged;

        public delegate void delegate_OnZoneTrackPositionChanged(String _zoneUDN, String _absPos);
        public event delegate_OnZoneTrackPositionChanged zoneTrackPositionChanged;

        public delegate void delegate_OnContentBrowserListListReady(String _trackListId);
        public event delegate_OnContentBrowserListListReady contentBrowserListReady;

        public delegate void delegate_OnPlaylistBrowserListListReady(String _trackListId);
        public event delegate_OnPlaylistBrowserListListReady playlistBrowserListReady;

        public delegate void delegate_OnZonePlayStateChanged(String _zoneUDN, RendererPlayState _playState);
        public event delegate_OnZonePlayStateChanged zonePlayStateChanged;

        public delegate void delegate_OnZonePlayModeChanged(String _zoneUDN, AvTransportPlayMode _playMode);
        public event delegate_OnZonePlayModeChanged zonePlayModeChanged;

        public delegate void delegate_OnRendererMuteStateChanged(String _rendererUDN, Boolean _mute);
        public event delegate_OnRendererMuteStateChanged rendererMuteStateChanged;

        public delegate void delegate_OnRendererVolumeChanged(String _rendererUDN, uint _volume);
        public event delegate_OnRendererVolumeChanged rendererVolumeChanged;


        public Controller(NetworkConnectInfo _networkConnectionInfo)
        {           
            logWriter = Global.getLogWriter();
            logWriter.setLogLevel(LogType.Error);

            this.writeLog(LogType.Info, "Starte Applikation Raumwiese");

            // use OhNet-Upnp Stack. This is the only one we may use for now which contains most methods we need.
            // the intelUPNP device Stack is not developed in final realease for the wrapper. You may debelop other Stacks
            upnpStack = new UNPN_OhNet();
            upnpStack.setNetwork(_networkConnectionInfo);
            upnpStack.onStartingNetwork += upnpStack_onStartingNetworkSink;
            upnpStack.init();

            // create zone, mediaServer and renderer managers
            zoneManager = new ZoneManager(upnpStack);
            mediaServerManager = new MediaServerManager(upnpStack);
            rendererManager = new RendererManager(upnpStack);
            configManager = new ConfigManager(upnpStack);
            zoneTitleListManager = new ZoneTitleListManager(upnpStack);
            contentBrowser = new ContentDirectoryBrowserMulti("", upnpStack);
            playlistBrowser = new PlaylistBrowser(upnpStack);

            // create data cache for images
            imageDataCache = new ImageDataCache();

            // set Managers global.
            Global.setMediaServerManager(mediaServerManager);
            Global.setRendererManager(rendererManager);
            Global.setZoneManager(zoneManager);
            Global.setConfigManager(configManager);
            Global.setImageDataCache(imageDataCache);
            Global.setZoneTitleListManager(zoneTitleListManager);
            Global.setContentBrowser(contentBrowser);
            Global.setPlaylistBrowser(playlistBrowser);
        }    

        ~Controller()
        {
        }

        public void init()
        {
            configManager.zoneSetupChanged += configManager_zoneSetupChangedSink;

            zoneManager.zonesRetrieved +=zoneManager_zonesRetrievedSink;
            zoneManager.allRenderersLinked += zoneManager_allRenderersLinkedSink;
            zoneManager.zonePlayStateChanged += zoneManager_zonePlayStateChangedSink;
            zoneManager.zoneTrackChanged += zoneManager_zoneTrackChangedSink;
            zoneManager.zoneTrackPositionChanged += zoneManager_zoneTrackPositionChangedSink;
            zoneManager.zonePlayModeChanged += zoneManager_zonePlayModeChangedSink;

            zoneTitleListManager.trackListReady += zoneTitleListManager_trackListReadySink;

            contentBrowser.listDataReady += contentBrowser_listDataReadySink;

            playlistBrowser.listDataReady += playlistBrowser_listDataReadySink;

            mediaServerManager.mediaServerFound += mediaServerManager_mediaServerFoundSink;
            mediaServerManager.mediaServerRemoved += mediaServerManager_mediaServerRemovedSink;
            mediaServerManager.findMediaServer();
           
            rendererManager.mediaRendererFound += rendererManager_mediaRendererFoundSink;
            rendererManager.mediaRendererRemoved += rendererManager_mediaRendererRemovedSink;
            rendererManager.rendererMuteStateChanged += rendererManager_rendererMuteStateChangedSink;
            rendererManager.rendererVolumeChanged += rendererManager_rendererVolumeChangedSink;

            imageDataCache.initDatabase();
            imageDataCache.loadFromDB();
        }

        protected void upnpStack_onStartingNetworkSink(string _networkInfo)
        {
            Global.getLogWriter().writeLog(LogType.Always, String.Format("Benutze NW-Controller: {0}", _networkInfo));
        }

        protected void zoneManager_zonePlayModeChangedSink(string _zoneUDN, AvTransportPlayMode _playMode)
        {
            if (zonePlayModeChanged != null) zonePlayModeChanged(_zoneUDN, _playMode);
        }

        protected void rendererManager_rendererVolumeChangedSink(string _rendererUDN, uint _volume)
        {
            if (rendererVolumeChanged != null) rendererVolumeChanged(_rendererUDN, _volume);
        }

        protected void rendererManager_rendererMuteStateChangedSink(string _rendererUDN, bool _mute)
        {
            if (rendererMuteStateChanged != null) rendererMuteStateChanged(_rendererUDN, _mute);
        }

        protected void zoneManager_zoneTrackPositionChangedSink(string _zoneUDN, string _absTime)
        {
            if (zoneTrackPositionChanged != null) zoneTrackPositionChanged(_zoneUDN, _absTime);
        }

        protected void zoneManager_zoneTrackChangedSink(string _zoneUDN, uint _newTrackIdx)
        {
            if (zoneTrackChanged != null) zoneTrackChanged(_zoneUDN, _newTrackIdx);
        }

        protected void zoneManager_zonePlayStateChangedSink(string _zoneUDN, RendererPlayState _playState)
        {
            if (zonePlayStateChanged != null) zonePlayStateChanged(_zoneUDN, _playState);
        }

        protected void contentBrowser_listDataReadySink(string _listId)
        {
            if (contentBrowserListReady != null) contentBrowserListReady(_listId);
        }

        protected void playlistBrowser_listDataReadySink(string _listId)
        {
            if (playlistBrowserListReady != null) playlistBrowserListReady(_listId);
        }

        protected void zoneTitleListManager_trackListReadySink(string _zoneId, string _listId)
        {
            if (zoneTrackListReady != null) zoneTrackListReady(_zoneId, _listId);
        }

        protected void rendererManager_mediaRendererRemovedSink()
        {
            //throw new NotImplementedException();
        }

        protected void rendererManager_mediaRendererFoundSink()
        {
            //throw new NotImplementedException();
        }

        protected void mediaServerManager_mediaServerRemovedSink()
        {
            if (mediaServerRemoved != null) mediaServerRemoved();
        }

        protected void mediaServerManager_mediaServerFoundSink()
        {
            // after media server was found we can retrieve the zones because then we got the ip for web request
            // this will be done via eventing from the config service
            rendererManager.findMediaRenderer();
            configManager.findConfigService();
            zoneManager.retrieveZones();

            playlistManagement = new PlaylistManagement(upnpStack);

            if (mediaServerFound != null) mediaServerFound();
        }

        protected void configManager_zoneSetupChangedSink()
        { 
            // setup change of zones will be evented by long poll in ZoneManager class!
            // no need to handle zone updates here!
            //zoneManager.retrieveZones();
        }

        protected void zoneManager_zonesRetrievedSink()
        {
 	        if(zonesRetrieved != null) zonesRetrieved();
        }

        protected void zoneManager_allRenderersLinkedSink()
        {
            if (allRenderersLinked != null) allRenderersLinked();
        }

        public void shutDown()
        {
            imageDataCache.saveToDB();
        }
    }
}
