using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Timers;

using OpenHome;
using OpenHome.Net;
using OpenHome.Net.Core;
using OpenHome.Net.ControlPoint;
using OpenHome.Net.ControlPoint.Proxies;

namespace RaumfeldNET.UPNP
{
        public enum AvTransportPlayMode
        {
            Normal,
            Shuffle,
            RepeatOne,
            RepeatAll,
            Random
        };

        public struct NetworkConnectInfo
        {
            public int subNetIndex;
        }

        // Wrapper Basics
        public class UNPN
        {
            protected NetworkConnectInfo networkConnectInfo;

            public delegate void delegate_OnMediaRendererFound(CpAVRenderer _avRenderer);
            public event delegate_OnMediaRendererFound onMediaRendererFound;
            public delegate void delegate_OnMediaRendererRemoved(CpAVRenderer _avRenderer);
            public event delegate_OnMediaRendererRemoved onMediaRendererRemoved;

            public delegate void delegate_OnMediaServerFound(CpMediaServer _mediaServer);
            public event delegate_OnMediaServerFound onMediaServerFound;
            public delegate void delegate_OnMediaServerRemoved(CpMediaServer _mediaServer);
            public event delegate_OnMediaServerRemoved onMediaServerRemoved;

            public delegate void delegate_OnConfigServiceFound(CpConfigService _configService);
            public event delegate_OnConfigServiceFound onConfigServiceFound;
            public delegate void delegate_OnConfigServiceRemoved(CpConfigService _configService);
            public event delegate_OnConfigServiceRemoved onConfigServiceRemoved;

            public delegate void delegate_OnStartingNetwork(String _networkInfo);
            public event delegate_OnStartingNetwork onStartingNetwork;

            public void delegateStartingNetwork(String _networkInfo)
            {
                if (onStartingNetwork != null) onStartingNetwork(_networkInfo);
            }

            public virtual void init()
            {
            }

            public void setNetwork(NetworkConnectInfo _connectInfo)
            {
                networkConnectInfo = _connectInfo;
            }

            public virtual void findMediaRenderer()
            {
            }          

            public virtual void findMediaServer()
            {
            }

            public virtual void findConfigService()
            {
            }

            protected void MediaServerRemovedSink(CpMediaServer _mediaServer)
            {
                if (onMediaServerRemoved != null) this.onMediaServerRemoved(_mediaServer);
            }

            protected void MediaServerFoundSink(CpMediaServer _mediaServer)
            {    
                if (onMediaServerFound != null) this.onMediaServerFound(_mediaServer);
            }

            protected void RendererRemovedSink(CpAVRenderer _renderer)
            {
                if (onMediaRendererRemoved != null) this.onMediaRendererRemoved(_renderer);
            }

            protected void RendererFoundSink(CpAVRenderer _renderer)
            {
                if (onMediaRendererFound != null) this.onMediaRendererFound(_renderer);
            }

            protected void ConfigServiceRemovedSink(CpConfigService _configService)
            {
                if (onConfigServiceRemoved != null) this.onConfigServiceRemoved(_configService);
            }

            protected void ConfigServiceFoundSink(CpConfigService _configService)
            {    
                if (onConfigServiceFound != null) this.onConfigServiceFound(_configService);
            }
        }

        public class CpContentDirectory
        {
            public delegate void delegate_OnResult_Search(CpContentDirectory sender, System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, System.String Result, System.UInt32 NumberReturned, System.UInt32 TotalMatches, System.UInt32 UpdateID, Object e, object _Tag);
            public event delegate_OnResult_Search OnResultSearch;

            public delegate void delegate_OnResult_Browse(CpContentDirectory sender, System.String ContainerID, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, System.String Result, System.UInt32 NumberReturned, System.UInt32 TotalMatches, System.UInt32 UpdateID, Object e, object _Tag);
            public event delegate_OnResult_Browse OnResultBrowse;

            public delegate void delegate_OnResult_Shuffle(CpContentDirectory sender, System.String playlistId, System.String playlistMetaData, object _Tag);
            public event delegate_OnResult_Shuffle OnResultShuffle;

            public delegate void delegate_OnResult_MoveInQueue(CpContentDirectory sender, uint _containerUpdateId, object _Tag);
            public event delegate_OnResult_MoveInQueue OnResultMoveInQueue;

            public delegate void delegate_OnStateVariable_ContainerUpdateIds(CpContentDirectory _contentDirectory, string _value);
            public event delegate_OnStateVariable_ContainerUpdateIds onStateVariableContainerUpdateIds;

            public delegate void delegate_OnResult_AddItemToQueue(CpContentDirectory _contentDirectory, object _Tag);
            public event delegate_OnResult_AddItemToQueue onResultAddItemToQueue;
           
            public delegate void delegate_OnResult_RemoveFromQueue(CpContentDirectory _contentDirectory, object _Tag);
            public event delegate_OnResult_RemoveFromQueue onResultRemoveFromQueue;

            public delegate void delegate_OnResult_AddContainerToQueue(CpContentDirectory _contentDirectory, object _Tag);
            public event delegate_OnResult_AddContainerToQueue onResultAddContainerToQueue;
           

            public virtual void Search(System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, object _Tag, delegate_OnResult_Search _Sink)
            {
            }

            public virtual void Shuffle(System.String ContainerID, System.String SearchCriteria, object _Tag, delegate_OnResult_Search _Sink)
            {
            }

            public virtual void ShuffleSync(System.String ContainerID, System.String SearchCriteria, out System.String playlistId, out System.String playlistMetaData)
            {
                playlistId = "";
                playlistMetaData = "";
            }

            public virtual void Browse(System.String ContainerID, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, object _Tag, delegate_OnResult_Browse _Sink)
            {
            }

            public virtual void SearchSync(System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, out System.String result, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID)
            {
                result = "";
                aNumberReturned = 0;
                aTotalMatches = 0;
                aUpdateID = 0;
            }

            public virtual void BrowseSync(System.String ContainerID, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, out System.String result, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID)
            {
                result = "";
                aNumberReturned = 0;
                aTotalMatches = 0;
                aUpdateID = 0;
            }

            public virtual void BrowseSyncMeta(System.String ContainerID, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, out System.String result, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID)
            {
                result = "";
                aNumberReturned = 0;
                aTotalMatches = 0;
                aUpdateID = 0;
            }

            public virtual void DestroyObjectSync(System.String ObjectID)
            {
            }

            public virtual void RenameQueueSync(System.String queueId, String _desiredName, out String _givenName)
            {
                _givenName = "";
            }

            public virtual void MoveInQueue(System.String ObjectID, System.UInt32 NewPosition, object _Tag, delegate_OnResult_MoveInQueue _Sink)
            {
            }

            public virtual void AddItemToQueue(System.String QueueId, System.String ObjectId, System.UInt32 Position, object _Tag, delegate_OnResult_AddItemToQueue _Sink)
            {
            }

            public virtual void AddContainerToQueue(System.String QueueId, System.String ContainerId, System.String SourceId, System.String SearchCriteria, System.String SortCriteria, System.UInt32 StartIndex, System.UInt32 EndIndex, System.UInt32 Position, object _Tag, delegate_OnResult_AddContainerToQueue _Sink)
            {
            }

            protected void ResultSearchSink(System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, System.String Result, System.UInt32 NumberReturned, System.UInt32 TotalMatches, System.UInt32 UpdateID, Object e, object tag)
            {
                object[] objAry = (object[])tag;

                if (objAry[1] != null) ((delegate_OnResult_Search)objAry[1])(this, ContainerID, SearchCriteria, Filter, StartingIndex, RequestedCount, SortCriteria, Result, NumberReturned, TotalMatches, UpdateID, e, objAry[0]); ;
                if (OnResultSearch != null) this.OnResultSearch(this, ContainerID, SearchCriteria, Filter, StartingIndex, RequestedCount, SortCriteria, Result, NumberReturned, TotalMatches, UpdateID, e, objAry[0]);
            }

            protected void ResultShuffleSink(String _playlistId, String _playlistMetadata, object tag)
            {
                object[] objAry = (object[])tag;

                if (objAry[1] != null) ((delegate_OnResult_Shuffle)objAry[1])(this, _playlistId, _playlistMetadata, objAry[0]); ;
                if (OnResultShuffle != null) this.OnResultShuffle(this, _playlistId, _playlistMetadata, objAry[0]);

            }

            protected void ResultBrowseSink(System.String ContainerID, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, System.String Result, System.UInt32 NumberReturned, System.UInt32 TotalMatches, System.UInt32 UpdateID, Object e, object tag)
            {
                object[] objAry = (object[])tag;

                if (objAry[1] != null) ((delegate_OnResult_Browse)objAry[1])(this, ContainerID, Filter, StartingIndex, RequestedCount, SortCriteria, Result, NumberReturned, TotalMatches, UpdateID, e, objAry[0]); ;
                if (OnResultBrowse != null) this.OnResultBrowse(this, ContainerID, Filter, StartingIndex, RequestedCount, SortCriteria, Result, NumberReturned, TotalMatches, UpdateID, e, objAry[0]);

            }

            protected void ResultMoveInQueueSink(uint _containerUpdateId, object _tag)
            {
                object[] objAry = (object[])_tag;
                if (objAry[1] != null) ((delegate_OnResult_MoveInQueue)objAry[1])(this, _containerUpdateId, objAry[0]);
            }

            protected void ResultAddItemToQueueSink(object _tag)
            {
                object[] objAry = (object[])_tag;
                if (objAry[1] != null) ((delegate_OnResult_AddItemToQueue)objAry[1])(this, objAry[0]);

            }

            protected void ResultAddContainerToQueueSink(object _tag)
            {
                object[] objAry = (object[])_tag;
                if (objAry[1] != null) ((delegate_OnResult_AddContainerToQueue)objAry[1])(this, objAry[0]);

            }

            protected void ResultRemoveFromQueueSink(object _tag)
            {
                object[] objAry = (object[])_tag;
                if (objAry[1] != null) ((delegate_OnResult_RemoveFromQueue)objAry[1])(this, objAry[0]);

            }

            protected void ContainerUpdateIDsSink(String newValue)
            {
                if (onStateVariableContainerUpdateIds != null) this.onStateVariableContainerUpdateIds(this, newValue);
            }

            public virtual void CreateQueueSync(System.String _desiredName, System.String _containerId, out System.String _givenName, out System.String _queueId, out System.String _metaData)
            {
                _givenName = "";
                _queueId = "";
                _metaData = "";
            }

            public virtual void AddContainerToQueueSync(System.String QueueId, System.String ContainerId, System.String SourceId, System.String SearchCriteria, System.String SortCriteria, System.UInt32 StartIndex, System.UInt32 EndIndex, System.UInt32 Position)
            {
            }

            public virtual void AddItemToQueueSync(System.String QueueId, System.String ObjectId, System.UInt32 Position)
            {
            }

            public virtual String getDeviceUdn()
            {
                return "";
            }

            public virtual void RemoveFromQueue(System.String _queueId, uint _fromPos, uint _toPos, object _Tag, delegate_OnResult_RemoveFromQueue _Sink)
            {                
            }

            public virtual void RemoveFromQueueSync(System.String _queueId, uint _fromPos, uint _toPos, out uint _containerUpdatedId)
            {
                _containerUpdatedId = 0;
            }
        }

        public class CpAVRenderer
        {
            public string UniqueDeviceName;
            public string Manufacturer;
            public string ModelDescription;
            public string FriendlyName;
            public UInt16 currentVolume;
            public Boolean isMuted;

            public delegate void delegate_VolumeChangedHandler(CpAVRenderer _connection, UInt16 _volume);
            public event delegate_VolumeChangedHandler onVolumeChanged;

            public delegate void delegate_MuteStateChangedHandler(CpAVRenderer _connection, bool _newMuteState);
            public event delegate_MuteStateChangedHandler onMuteChanged;

            public delegate void delegate_OnResult_SetVolume(CpAVRenderer _avRenderer, object _Tag);
            public event delegate_OnResult_SetVolume onResultSetVolume;


            public List<CpAVConnection> Connections;

            public virtual Boolean isVirtualRenderer()
            {
                return false;
            }

            protected void VolumeChangedSink(UInt16 _volume)
            {
                if (this.onVolumeChanged != null) this.onVolumeChanged(this, _volume);
            }

            protected void MuteStateChangedSink(bool _mute)
            {
                if (this.onMuteChanged != null) this.onMuteChanged(this, _mute);
            }

            public virtual void SetVolume(uint _volume, object _Tag, delegate_OnResult_SetVolume _Sink)
            {
            }

            protected void ResultSetVolumeSink(object _tag)
            {
                object[] objAry = (object[])_tag;
                if (objAry[1] != null) ((delegate_OnResult_SetVolume)objAry[1])(this, objAry[0]);
            }

            public virtual void SetMute(bool _mute)
            {
            }
        }

        public class CpMediaServer
        {
            public CpContentDirectory contentDirectory;
            public string ServerFriendlyName;
            public string DeviceXml;
            public string Location;
        }

        public class CpAVConnection
        {
            public string AVTransportUri;
            public string AVTransportUriMetaData;
            public string currentPlayMode;
            public UInt32 CurrentTrack;
            public AvTransportPlayMode playMode;
           

            public delegate void delegate_TrackChangedHandler(CpAVConnection _connection, UInt32 _newTrack);
            public event delegate_TrackChangedHandler onTrackChanged;

            public delegate void delegate_AVTransportIURIChangedHandler(CpAVConnection _connection);
            public event delegate_AVTransportIURIChangedHandler onAVTransportURIChanged;

            public delegate void delegate_AVTransportIURIMetaDataChangedHandler(CpAVConnection _connection);
            public event delegate_AVTransportIURIMetaDataChangedHandler onAVTransportURIMetaDataChanged;

            public delegate void delegate_NumberOfTracksChangedHandler(CpAVConnection _connection, UInt32 _trackCount);
            public event delegate_NumberOfTracksChangedHandler onNumberOfTracksChanged;

            public delegate void delegate_MediaResourceChangedHandler(CpAVConnection _connection, System.Object _mediaResource);
            public event delegate_MediaResourceChangedHandler onMediaResourceChanged;

            public delegate void delegate_PlayStateChangedHandler(CpAVConnection _connection, Object _playState);
            public event delegate_PlayStateChangedHandler onPlayStateChanged;

            public delegate void delegate_PositionChangedHandler(CpAVConnection _connection, String duration, String trackMetadata, String absTime);
            public event delegate_PositionChangedHandler onPositionChanged;

            public delegate void delegate_CurrentMetaDataChangedHandler(CpAVConnection _connection);
            public event delegate_CurrentMetaDataChangedHandler onCurrentMetaDataChanged;

            public delegate void delegate_TrackURIChangedHandler(CpAVConnection _connection);
            public event delegate_TrackURIChangedHandler onTrackURIChanged;

            public delegate void delegate_CurrentPlayModeChanged(CpAVConnection _connection, AvTransportPlayMode _playMode);
            public event delegate_CurrentPlayModeChanged onPlayModeChanged;

            public virtual void initiateEvents()
            {
            }

            public AvTransportPlayMode playModeStringToPlayMode(String _playModeString)
            {
                switch (_playModeString)
                {
                    case "NORMAL":
                        return AvTransportPlayMode.Normal;
                    case "SHUFFLE":
                        return AvTransportPlayMode.Shuffle;
                    case "REPEAT_ONE":
                        return AvTransportPlayMode.RepeatOne;
                    case "REPEAT_ALL":
                        return AvTransportPlayMode.RepeatAll;
                    case "RANDOM":
                        return AvTransportPlayMode.Random;
                }
                return AvTransportPlayMode.Normal;
            }

            public String playModeToPlayModeString(AvTransportPlayMode _playModeString)
            {
                switch (_playModeString)
                {
                    case AvTransportPlayMode.Normal:
                        return "NORMAL";
                    case AvTransportPlayMode.Shuffle:
                        return "SHUFFLE";
                    case AvTransportPlayMode.RepeatOne:
                        return "REPEAT_ONE";
                    case AvTransportPlayMode.RepeatAll:
                        return "REPEAT_ALL";
                    case AvTransportPlayMode.Random:
                        return "RANDOM";                    
                }
                return "NORMAL";
            }
            
            
            protected void TrackChangedSink(UInt32 _newTrack)
            {
                if (this.onTrackChanged != null) this.onTrackChanged(this, _newTrack);
            }

            protected void AVTransportURIChangedSink()
            {
                if (this.onAVTransportURIChanged != null) this.onAVTransportURIChanged(this);
            }

            protected void CurrentPlayModeChangedSink()
            {
                if (this.onPlayModeChanged != null) this.onPlayModeChanged(this, playMode);
            }

            protected void AVTransportURIMetaDataChangedSink()
            {
                if (this.onAVTransportURIMetaDataChanged != null) this.onAVTransportURIMetaDataChanged(this);
            }

            protected void NumberOfTracksChangedSink(UInt32 _trackCount)
            {
                if (this.onNumberOfTracksChanged != null) this.onNumberOfTracksChanged(this, _trackCount);
            }

            protected void MediaResourceChangedSink(System.Object _mediaResource)
            {
                if (this.onMediaResourceChanged != null) this.onMediaResourceChanged(this, _mediaResource);
            }

            protected void PlayStateChangedSink(Object _playState)
            {
                if (this.onPlayStateChanged != null) this.onPlayStateChanged(this, _playState);
            }

            protected void PositionChangedSink(String duration, String trackMetadata, String absTime)
            {
                if (this.onPositionChanged != null) this.onPositionChanged(this, duration, trackMetadata, absTime);
            }

            protected void CurrentMetaDataChangedSink()
            {
                if (this.onCurrentMetaDataChanged != null) this.onCurrentMetaDataChanged(this);
            }

            protected void TrackURIChangedSink()
            {
                if (this.onTrackURIChanged != null) this.onTrackURIChanged(this);
            }

            public virtual void SetAvTransportUri(System.String _uri, System.String _uriMetadata)
            {               
            }

            public virtual void BendAvTransportUri(System.String _uri, System.String _uriMetadata)
            {
            }

            public virtual void Play()
            {
            }

            public virtual void Stop()
            {
            }

            public virtual void Seek(System.String _target)
            {
            }

            public virtual void Next()
            {
            }

            public virtual void Previous()
            {
            }

            public virtual void Pause()
            {
            }

            public virtual void SetPlayMode(AvTransportPlayMode _playMode)
            {
            }

            public virtual void initTimer()
            {
            }
        }

        public class CpConfigService
        {
            public string FriendlyName;
            public string DeviceXml;
            public string Location;

            public delegate void delegate_ZoneSetupChanged(CpConfigService _configService);
            public event delegate_ZoneSetupChanged onZoneSetupChanged;

            protected void ZoneSetupChangedSink()
            {
                if (this.onZoneSetupChanged != null) this.onZoneSetupChanged(this);
            }
        }

        // INTEL UPNP 

        /*
        public class UNPN_IntelUPNP : UNPN
        {
            AVRendererDiscovery avRendererDiscovery;
            MediaServerDiscovery mediaServerDiscovery;

            override public void findMediaRenderer()
            {
                avRendererDiscovery = new AVRendererDiscovery(new AVRendererDiscovery.DiscoveryHandler(RendererFoundSink));
                avRendererDiscovery.OnRendererRemoved += new AVRendererDiscovery.DiscoveryHandler(new AVRendererDiscovery.DiscoveryHandler(RendererRemovedSink));
            }          

            override public void findMediaServer()
            {
                mediaServerDiscovery = new MediaServerDiscovery(null,
                                                            null,
                                                            new MediaServerDiscovery.Delegate_OnGoodServersChange(MediaServerFoundSink),
                                                            new MediaServerDiscovery.Delegate_OnGoodServersChange(MediaServerRemovedSink));
            }

            protected void MediaServerRemovedSink(MediaServerDiscovery sender, OpenSource.UPnP.AV.MediaServer.CP.CpMediaServer device)
            {
                CpMediaServer mediaServer = new CpMediaServer_IntelUPNP(device);
                this.MediaServerRemovedSink(mediaServer);
            }

            protected void MediaServerFoundSink(MediaServerDiscovery sender, OpenSource.UPnP.AV.MediaServer.CP.CpMediaServer device)
            {
                CpMediaServer mediaServer = new CpMediaServer_IntelUPNP(device);
                this.MediaServerFoundSink(mediaServer);
            }

            protected void RendererRemovedSink(AVRendererDiscovery sender, AVRenderer renderer)
            {
                CpAVRenderer avRenderer = new CpAVRenderer_IntelUPNP(renderer);
                this.RendererRemovedSink(avRenderer);
            }

            protected void RendererFoundSink(AVRendererDiscovery sender, AVRenderer renderer)
		    {
                CpAVRenderer avRenderer = new CpAVRenderer_IntelUPNP(renderer);
                this.RendererFoundSink(avRenderer);
            }
        }
   
        public class CpContentDirectory_IntelUPNP_Raumfeld : CpContentDirectory
        {
            protected OpenSource.DeviceBuilder.CpContentDirectoryRaumfeld contentDirectory;

            public CpContentDirectory_IntelUPNP_Raumfeld(OpenSource.UPnP.AV.CpContentDirectory _contentDirectory)
            {
                contentDirectory = new OpenSource.DeviceBuilder.CpContentDirectoryRaumfeld(_contentDirectory.GetUPnPService());
                contentDirectory.OnStateVariable_ContainerUpdateIDs += new OpenSource.DeviceBuilder.CpContentDirectoryRaumfeld.StateVariableModifiedHandler_ContainerUpdateIDs(ContainerUpdateIDsSink);                    
            }

            protected void ContainerUpdateIDsSink(OpenSource.DeviceBuilder.CpContentDirectoryRaumfeld _contentDirectory, String _newValue)
            {
                this.ContainerUpdateIDsSink(_newValue);
            }

            override public void Search(System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, object _Tag, Delegate_OnResult_Search _Sink)
            {
                contentDirectory.Search(ContainerID, SearchCriteria, Filter, StartingIndex, RequestedCount, SortCriteria, new object[2] { _Tag, _Sink }, new OpenSource.DeviceBuilder.CpContentDirectoryRaumfeld.Delegate_OnResult_Search(ResultSearchSink));
            }

            override public void Browse(System.String ContainerID, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, object _Tag, Delegate_OnResult_Browse _Sink)
            {
                contentDirectory.Browse(ContainerID, OpenSource.DeviceBuilder.CpContentDirectoryRaumfeld.Enum_A_ARG_TYPE_BrowseFlag.BROWSEDIRECTCHILDREN, Filter, StartingIndex, RequestedCount, SortCriteria, new object[2] { _Tag, _Sink }, new OpenSource.DeviceBuilder.CpContentDirectoryRaumfeld.Delegate_OnResult_Browse(ResultBrowseSink));
            }

            protected void ResultSearchSink(Object sender, System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, System.String Result, System.UInt32 NumberReturned, System.UInt32 TotalMatches, System.UInt32 UpdateID, Object e, object tag)
            {
                this.ResultSearchSink(ContainerID, SearchCriteria, Filter, StartingIndex, RequestedCount, SortCriteria, Result, NumberReturned, TotalMatches, UpdateID, e, tag);
            }

            protected void ResultBrowseSink(Object sender, System.String ObjectID, OpenSource.DeviceBuilder.CpContentDirectoryRaumfeld.Enum_A_ARG_TYPE_BrowseFlag BrowseFlag, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, System.String Result, System.UInt32 NumberReturned, System.UInt32 TotalMatches, System.UInt32 UpdateID, UPnPInvokeException e, object tag)
            {
                this.ResultBrowseSink(ObjectID, Filter, StartingIndex, RequestedCount, SortCriteria, Result, NumberReturned, TotalMatches, UpdateID, e, tag);
            }
          

            override public void MoveInQueue(System.String ObjectID, System.UInt32 NewPosition, object _Tag, Delegate_OnResult_MoveInQueue _Sink)
            {
            }

            ~CpContentDirectory_IntelUPNP_Raumfeld()
            {
            }        
        }
       
        public class CpAVRenderer_IntelUPNP : CpAVRenderer
        {
            protected AVRenderer avRenderer;
            CpAVTransport cpAvTransport;

            public CpAVRenderer_IntelUPNP(AVRenderer renderer)
            {
                avRenderer = renderer;
                UniqueDeviceName = avRenderer.UniqueDeviceName;
                Manufacturer = avRenderer.device.Manufacturer;

                avRenderer.OnInitialized +=new AVRenderer.OnInitializedHandler(OnInitialized);

                this.addConnections();

                cpAvTransport = new CpAVTransport(avRenderer.device.GetService("urn:upnp-org:serviceId:AVTransport"));
                cpAvTransport.OnStateVariable_LastChange += new CpAVTransport.StateVariableModifiedHandler_LastChange(lastChangedSink);
            }

            protected void OnInitialized(AVRenderer sender)
            {
                //this.addConnections();
            }

            protected void lastChangedSink(CpAVTransport sender, System.String NewValue)
            {
                int pos1,pos2 = 0;
                string avTransportUriNew;

                pos1 = NewValue.IndexOf("<AVTransportURI val=\"");
                if (pos1 > 0)
                {
                    pos1 += 21; 
                    pos2 = NewValue.IndexOf("\"", pos1);
                }
                if (pos2 > 0)
                {
                    avTransportUriNew = NewValue.Substring(pos1, pos2 - pos1);

                }
                    
            }

            protected void addConnections()
            {
                Connections = new List<CpAVConnection>();

                foreach (AVConnection connection in avRenderer.Connections)
                {
                    CpAVConnection avCon = new CpAVConnection_IntelUPNP(connection);
                    Connections.Add(avCon);
                }   
            }

            ~CpAVRenderer_IntelUPNP()
            {
            }

            override public Boolean isVirtualRenderer()
            {
                if (avRenderer.device.ModelDescription == "Virtual Media Player")
                    return true;
                return false;
            }
            
        }
       
        public class CpMediaServer_IntelUPNP : CpMediaServer
        {
            protected OpenSource.UPnP.AV.MediaServer.CP.CpMediaServer mediaServer;

            public CpMediaServer_IntelUPNP(OpenSource.UPnP.AV.MediaServer.CP.CpMediaServer _mediaServer)
            {
                mediaServer = _mediaServer;
                contentDirectory = new CpContentDirectory_IntelUPNP_Raumfeld(_mediaServer.ContentDirectory);
                ServerFriendlyName = _mediaServer.Root.ServerFriendlyName;
            }

            ~CpMediaServer_IntelUPNP()
            {
            }  
        }       

        public class CpAVConnection_IntelUPNP : CpAVConnection
        {
            AVConnection avConnection;      

            public CpAVConnection_IntelUPNP(AVConnection _avConnection)
            {
                avConnection = _avConnection;
                this.registerEvents();
                
            }

            protected void registerEvents()
            {
                avConnection.OnVolume += new AVConnection.VolumeChangedHandler(VolumeChangedSink);
                avConnection.OnPlayStateChanged += new AVConnection.PlayStateChangedHandler(PlayStateChangedSink);
                avConnection.OnMute += new AVConnection.MuteStateChangedHandler(MuteStateChangedSink);
                avConnection.OnPositionChanged += new AVConnection.PositionChangedHandler(PositionChangedSink);
                avConnection.OnTrackChanged += new AVConnection.CurrentTrackChangedHandler(TrackChangedSink);
                avConnection.OnCurrentMetaDataChanged += new AVConnection.CurrentMetaDataChangedHandler(CurrentMetaDataChangedSink);
                avConnection.OnNumberOfTracksChanged += new AVConnection.NumberOfTracksChangedHandler(NumberOfTracksChangedSink);
                avConnection.OnTrackURIChanged += new AVConnection.TrackURIChangedHandler(TrackURIChangedSink);
                avConnection.OnMediaResourceChanged += new AVConnection.MediaResourceChangedHandler(MediaResourceChangedSink);
                avConnection.OnAVTransportURIChanged += new AVConnection.VariableChangeHandler(AVTransportURIChangedSink);
            }          

            ~CpAVConnection_IntelUPNP()
            {
            }

            protected void VolumeChangedSink(AVConnection _connection, UInt16 _volume)
            {
                this.VolumeChangedSink(_volume);
            }

            protected void MuteStateChangedSink(AVConnection _connection, bool _newMuteStatus)
            {
                this.MuteStateChangedSink(_newMuteStatus);
            }

            protected void TrackURIChangedSink(AVConnection _connection)
            {
                CurrentTrack = _connection.CurrentTrack;
                this.TrackURIChangedSink();
            }

            protected void CurrentMetaDataChangedSink(AVConnection _connection)
            {
                CurrentTrack = _connection.CurrentTrack;
                this.CurrentMetaDataChangedSink();
            }

            protected void AVTransportURIChangedSink(AVConnection _connection)
            {
                if (_connection.MediaResource != null)
                    AVTransportUri = _connection.MediaResource.ContentUri;
                else
                    AVTransportUri = _connection.AV_LastChange._AVTransportURI;
                CurrentTrack = _connection.CurrentTrack;
                this.AVTransportURIChangedSink();
            }

            protected void MediaResourceChangedSink(AVConnection _connection, System.Object _mediaResource)
            {
                this.MediaResourceChangedSink(_mediaResource);
            }

            protected void NumberOfTracksChangedSink(AVConnection _connection, UInt32 _trackCount)
            {
                CurrentTrack = _connection.CurrentTrack;
                this.NumberOfTracksChangedSink(_trackCount);
            }

            protected void PlayStateChangedSink(AVConnection _connection, AVConnection.PlayState NewState)
            {
                this.PlayStateChangedSink(NewState);
            }

            protected void TrackChangedSink(AVConnection _connection, UInt32 _newTrack)
            {
                CurrentTrack = _connection.CurrentTrack;
                this.TrackChangedSink(_newTrack);
            }

            protected void PositionChangedSink(AVConnection _connection, TimeSpan _time)
            {
                this.PositionChangedSink(_time);
            }
        }

        */


        // OHNET
        public class UNPN_OhNet : UNPN
        {
            OpenHome.Net.Core.Library lib;

            MessageListener ml;
            InitParams.MessageHandler mh;

            public UNPN_OhNet()
            {

                
            }

            public void mhCb(string _s)
            {

            }

            public override void init()
            {
                mh = new InitParams.MessageHandler(mhCb);
                ml = new MessageListener(mh);

                OpenHome.Net.Core.InitParams initParams = new OpenHome.Net.Core.InitParams();
                OpenHome.Net.Core.Library.SetDebugLevel(Library.DebugLevel.Error);
                initParams.LogOutput = ml;
                lib = OpenHome.Net.Core.Library.Create(initParams);
                OpenHome.Net.Core.SubnetList subnetList = new OpenHome.Net.Core.SubnetList();
                OpenHome.Net.Core.NetworkAdapter nif = subnetList.SubnetAt((uint)networkConnectInfo.subNetIndex);
                this.delegateStartingNetwork(nif.FullName());
                uint subnet = nif.Subnet();
                subnetList.Dispose();
                lib.StartCp(subnet);
            }

            ~UNPN_OhNet()
            {
                if (lib!=null)
                    lib.Dispose(); // TODO: Kills App?!?!
            }

            override public void findMediaRenderer()
            {
                OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler added = new OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler(RendererFoundSink);
                OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler removed = new OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler(RendererRemovedSink);
                OpenHome.Net.ControlPoint.CpDeviceListUpnpServiceType list = new OpenHome.Net.ControlPoint.CpDeviceListUpnpServiceType("upnp.org", "RenderingControl", 1, added, removed);                       
            }

            override public void findMediaServer()
            {
                OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler added = new OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler(MediaServerFoundSink);
                OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler removed = new OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler(MediaServerRemovedSink);                
                OpenHome.Net.ControlPoint.CpDeviceListUpnpServiceType list = new OpenHome.Net.ControlPoint.CpDeviceListUpnpServiceType("upnp.org", "ContentDirectory", 1, added, removed);                       
            }

            override public void findConfigService()
            {
                OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler added = new OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler(ConfigServiceFoundSink);
                OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler removed = new OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler(ConfigServiceRemovedSink);                
                //OpenHome.Net.ControlPoint.CpDeviceListUpnpServiceType list = new OpenHome.Net.ControlPoint.CpDeviceListUpnpServiceType("raumfeld.com", "ConfigService", 1, added, removed);
                OpenHome.Net.ControlPoint.CpDeviceListUpnpAll list = new OpenHome.Net.ControlPoint.CpDeviceListUpnpAll(added, removed);                       
            }

            protected void MediaServerRemovedSink(OpenHome.Net.ControlPoint.CpDeviceList aList, OpenHome.Net.ControlPoint.CpDevice aDevice)
            {
                CpMediaServer mediaServer = new CpMediaServer_OhNet(aDevice);
                this.MediaServerRemovedSink(mediaServer);
            }

            protected void MediaServerFoundSink(OpenHome.Net.ControlPoint.CpDeviceList aList, OpenHome.Net.ControlPoint.CpDevice aDevice)
            {
                CpMediaServer mediaServer = new CpMediaServer_OhNet(aDevice);
                this.MediaServerFoundSink(mediaServer);                
            }

            protected void RendererRemovedSink(OpenHome.Net.ControlPoint.CpDeviceList aList, OpenHome.Net.ControlPoint.CpDevice aDevice)
            {
                CpAVRenderer avRenderer = new CpAVRenderer_OhNet(aDevice);
                this.RendererRemovedSink(avRenderer);
            }

            protected void RendererFoundSink(OpenHome.Net.ControlPoint.CpDeviceList aList, OpenHome.Net.ControlPoint.CpDevice aDevice)
            {
                CpAVRenderer avRenderer = new CpAVRenderer_OhNet(aDevice);
                this.RendererFoundSink(avRenderer);
            }

            protected void ConfigServiceRemovedSink(OpenHome.Net.ControlPoint.CpDeviceList aList, OpenHome.Net.ControlPoint.CpDevice aDevice)
            {
                CpConfigService configService = new CpConfigService_OhNet(aDevice);
                this.ConfigServiceRemovedSink(configService);
            }

            protected void ConfigServiceFoundSink(OpenHome.Net.ControlPoint.CpDeviceList aList, OpenHome.Net.ControlPoint.CpDevice aDevice)
            {
                String name;
                aDevice.GetAttribute("Upnp.FriendlyName", out name);
                if (name == "Raumfeld ConfigDevice") // Only until find works correct!
                {
                    CpConfigService configService = new CpConfigService_OhNet(aDevice);
                    this.ConfigServiceFoundSink(configService);
                }
            }
        }

        public class CpMediaServer_OhNet : CpMediaServer
        {
            OpenHome.Net.ControlPoint.CpDevice mediaServerDevice;           

            public CpMediaServer_OhNet(OpenHome.Net.ControlPoint.CpDevice _device)
            {
                mediaServerDevice = _device;               
                mediaServerDevice.GetAttribute("Upnp.FriendlyName", out ServerFriendlyName);
                mediaServerDevice.GetAttribute("Upnp.DeviceXml", out DeviceXml);
                mediaServerDevice.GetAttribute("Upnp.Location", out Location);                
                contentDirectory = new CpContentDirectory_OhNet_Raumfeld(_device);
            }

            ~CpMediaServer_OhNet()
            {
            }
        }

        public class CpContentDirectory_OhNet_Raumfeld : CpContentDirectory
        {
            protected OpenHome.Net.ControlPoint.Proxies.CpProxyUpnpOrgContentDirectory1 contentDirectory;
            protected OpenHome.Net.ControlPoint.CpDevice device;

            public CpContentDirectory_OhNet_Raumfeld(OpenHome.Net.ControlPoint.CpDevice _device)
            {
                device = _device;
                contentDirectory = new OpenHome.Net.ControlPoint.Proxies.CpProxyUpnpOrgContentDirectory1(_device);            
                contentDirectory.SetPropertyContainerUpdateIDsChanged(ContainerUpdateIDsSink); 
                contentDirectory.Subscribe();                
            }

            protected void ContainerUpdateIDsSink()
            {
         
                //contentDirectory.PropertyContainerUpdateIDs();
                String updateIds = contentDirectory.PropertyContainerUpdateIDs();                
                this.ContainerUpdateIDsSink(updateIds);
            }

            override public void DestroyObjectSync(System.String _ObjectID)
            {
                contentDirectory.SyncDestroyObject(_ObjectID);
            }

            override public void RenameQueueSync(System.String queueId, String _desiredName, out String _givenName)
            {
                contentDirectory.SyncRenameQueue(queueId, _desiredName, out _givenName);
            }

            override public void RemoveFromQueue(System.String _queueId, uint _fromPos, uint _toPos, object _Tag, delegate_OnResult_RemoveFromQueue _Sink)
            {
                contentDirectory.BeginRemoveFromQueue(_queueId, _fromPos, _toPos, (asyncHandle) => ResultRemoveFromQueueSink(asyncHandle, new Object[2] { null, _Sink }));
            }

            override public void RemoveFromQueueSync(System.String _queueId, uint _fromPos, uint _toPos, out uint _containerUpdateID)
            {
                contentDirectory.SyncRemoveFromQueue(_queueId, _fromPos, _toPos, out _containerUpdateID);
            }

            override public void Search(System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, object _Tag, delegate_OnResult_Search _Sink)
            {               
                contentDirectory.BeginSearch(ContainerID, SearchCriteria, Filter, StartingIndex, RequestedCount, SortCriteria, (asyncHandle) => ResultSearchSink(asyncHandle, new Object[2] { null, _Sink }));                                           
            }

            override public void Shuffle(System.String ContainerID, System.String SearchCriteria, object _Tag, delegate_OnResult_Search _Sink)
            {
                contentDirectory.BeginShuffle(ContainerID, SearchCriteria, (asyncHandle) => ResultShuffleSink(asyncHandle, new Object[2] { null, _Sink }));
            }

            override public void ShuffleSync(System.String ContainerID, System.String SearchCriteria, out String playlistId, out String playlistMetaData)
            {
                contentDirectory.SyncShuffle(ContainerID, SearchCriteria, out playlistId, out playlistMetaData);
            }

            protected void ResultShuffleSink(IntPtr aPtr, Object _tag)
            {
                String playlistId, playlistMetaData;
               contentDirectory.EndShuffle(aPtr,out playlistId, out playlistMetaData);
               this.ResultShuffleSink(playlistId, playlistMetaData, _tag);
            }
           
            protected void ResultSearchSink(IntPtr aPtr, Object _tag)
            {               
                String result;
                uint totalMatches, NumberReturned, updateId;
                contentDirectory.EndSearch(aPtr, out result, out NumberReturned, out totalMatches, out updateId);
                //contentDirectory.EndBrowse(aPtr, out result, out NumberReturned, out totalMatches, out updateId);
                this.ResultSearchSink("", "", "", 0, 0, "", result, NumberReturned, totalMatches, updateId, null, _tag);  
            }

            protected void ResultRemoveFromQueueSink(IntPtr aPtr, Object _tag)
            {
                uint updateContainerId;
                contentDirectory.EndRemoveFromQueue(aPtr, out updateContainerId);
                this.ResultRemoveFromQueueSink(_tag);
            }

            override public void Browse(System.String ContainerID, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, object _Tag, delegate_OnResult_Browse _Sink)
            {               
                contentDirectory.BeginBrowse(ContainerID, "BrowseDirectChildren", Filter, StartingIndex, RequestedCount, SortCriteria, (asyncHandle) => ResultBrowseSink(asyncHandle, new Object[2] { null, _Sink }));
            }
            public void ResultBrowseSink(IntPtr aPtr, Object _tag)
            {
                String result;
                uint totalMatches, NumberReturned, updateId;
                contentDirectory.EndBrowse(aPtr, out result, out NumberReturned, out totalMatches, out updateId);
                this.ResultBrowseSink("", "", 0, 0, "", result, NumberReturned, totalMatches, updateId, null, _tag);           
            }

            override public void MoveInQueue(System.String ObjectID, System.UInt32 NewPosition, object _Tag, delegate_OnResult_MoveInQueue _Sink)
            {
                contentDirectory.BeginMoveInQueue(ObjectID, NewPosition, (asyncHandle) => ResultMoveInQueueSink(asyncHandle, new Object[2] { null, _Sink }));
            }
            public void ResultMoveInQueueSink(IntPtr aAsyncHandle, Object _tag)
            {
                uint containerUpdateId;
                contentDirectory.EndMoveInQueue(aAsyncHandle, out containerUpdateId);
                this.ResultMoveInQueueSink(containerUpdateId, _tag);                
            }

            override public void AddItemToQueue(System.String QueueId, System.String ObjectId, System.UInt32 Position, object _Tag, delegate_OnResult_AddItemToQueue _Sink)
            {
                contentDirectory.BeginAddItemToQueue(QueueId, ObjectId, Position, (asyncHandle) => ResultAddItemToQueueSink(asyncHandle, new Object[2] { _Tag, _Sink })); 
            }
            public void ResultAddItemToQueueSink(IntPtr aAsyncHandle, Object _tag)
            {
                contentDirectory.EndAddItemToQueue(aAsyncHandle);
                this.ResultAddItemToQueueSink(_tag);
            }

            override public void AddContainerToQueue(System.String QueueId, System.String ContainerId, System.String SourceId, System.String SearchCriteria, System.String SortCriteria, System.UInt32 StartIndex, System.UInt32 EndIndex, System.UInt32 Position, object _Tag, delegate_OnResult_AddContainerToQueue _Sink)
            {
                contentDirectory.BeginAddContainerToQueue(QueueId, ContainerId, SourceId, SearchCriteria, SortCriteria, StartIndex, EndIndex, Position, (asyncHandle) => ResultAddContainerToQueueSink(asyncHandle, new Object[2] { _Tag, _Sink })); 
            }
            public void ResultAddContainerToQueueSink(IntPtr aAsyncHandle, Object _tag)
            {
                contentDirectory.EndAddContainerToQueue(aAsyncHandle);
                this.ResultAddContainerToQueueSink(_tag);
            }

            override public void CreateQueueSync(System.String _desiredName, System.String _containerId, out System.String _givenName, out System.String _queueId, out System.String _metaData)
            {
                contentDirectory.SyncCreateQueue(_desiredName, _containerId, out _givenName, out _queueId, out _metaData);
            }

            override public void AddContainerToQueueSync(System.String QueueId, System.String ContainerId, System.String SourceId, System.String SearchCriteria, System.String SortCriteria, System.UInt32 StartIndex, System.UInt32 EndIndex, System.UInt32 Position)
            {
                contentDirectory.SyncAddContainerToQueue(QueueId, ContainerId, SourceId, SearchCriteria, SortCriteria, StartIndex, EndIndex, Position);
            }

            override public void AddItemToQueueSync(System.String QueueId, System.String ObjectId, System.UInt32 Position)
            {
                contentDirectory.SyncAddItemToQueue(QueueId, ObjectId, Position);
            }

            override public void SearchSync(System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, out System.String result, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID)
            {
                contentDirectory.SyncSearch(ContainerID, SearchCriteria, Filter, StartingIndex, RequestedCount, SortCriteria, out result, out aNumberReturned, out aTotalMatches, out aUpdateID);
            }

            override public void BrowseSync(System.String ContainerID, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, out System.String result, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID)
            {
                contentDirectory.SyncBrowse(ContainerID, "BrowseDirectChildren", Filter, StartingIndex, RequestedCount, SortCriteria, out result, out aNumberReturned, out aTotalMatches, out aUpdateID);
            }

            override public void BrowseSyncMeta(System.String ContainerID, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, out System.String result, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID)
            {
                contentDirectory.SyncBrowse(ContainerID, "BrowseMetadata", Filter, StartingIndex, RequestedCount, SortCriteria, out result, out aNumberReturned, out aTotalMatches, out aUpdateID);
            }

            override public String getDeviceUdn()
            {
                return device.Udn();
            }

            ~CpContentDirectory_OhNet_Raumfeld()
            {
            }
        }

        public class CpAVRenderer_OhNet : CpAVRenderer
        {
            OpenHome.Net.ControlPoint.CpDevice avRendererDevice;
            OpenHome.Net.ControlPoint.Proxies.CpProxyUpnpOrgRenderingControl1 renderingControl;
            string DeviceXml;

            public CpAVRenderer_OhNet(OpenHome.Net.ControlPoint.CpDevice _rendererDevice)
            {
                XMLParser xmlParser = new XMLParser();

                renderingControl = new CpProxyUpnpOrgRenderingControl1(_rendererDevice);
                renderingControl.SetPropertyInitialEvent(propertyChanged);
                renderingControl.SetPropertyChanged(propertyChanged);
                renderingControl.SetPropertyLastChangeChanged(propertyChanged);
                renderingControl.Subscribe();
                
                avRendererDevice = _rendererDevice;
                avRendererDevice.GetAttribute("Upnp.DeviceXml", out DeviceXml);                

                ModelDescription = xmlParser.getNodeValue(DeviceXml, "modelDescription");
                FriendlyName = xmlParser.getNodeValue(DeviceXml, "friendlyName");                
                Manufacturer = xmlParser.getNodeValue(DeviceXml, "manufacturer");
                UniqueDeviceName = avRendererDevice.Udn();

                Connections = new List<CpAVConnection>();
                Connections.Add(new CpAVConnection_OhNet(avRendererDevice));
                
            }


            ~CpAVRenderer_OhNet()
            {

            }

            override public void SetVolume(uint _volume, object _Tag, delegate_OnResult_SetVolume _Sink)
            {                
                renderingControl.BeginSetVolume(0, "Master", _volume, (asyncHandle) => ResultSetVolume(asyncHandle, new Object[2] { null, _Sink }));
            }

            protected void ResultSetVolume(IntPtr asyncHandle, object _tag)
            {
                this.ResultSetVolumeSink(_tag);
            }

            override public void SetMute(bool _mute)
            {
                renderingControl.BeginSetMute(0, "Master", _mute, null);
            }

            protected void propertyChanged()
            {
                string propXml = renderingControl.PropertyLastChange();
                string result;
                XMLParser xmlParser = new XMLParser(); 
                UInt16 currentVolumeNew;
                Boolean isMutedNew;

                result = xmlParser.getNodeAttributeValue(propXml, "Volume", "val");
                if (result != null)
                {
                    currentVolumeNew = (UInt16)Convert.ToInt16(result);
                    if (currentVolumeNew != null && currentVolume != currentVolumeNew)
                    {
                        currentVolume = currentVolumeNew;
                        this.VolumeChangedSink(currentVolumeNew);
                    }
                }

                result = xmlParser.getNodeAttributeValue(propXml, "Mute", "val");
                if (result != null)
                {
                    if (result == "0")
                        isMutedNew = false;
                    else
                        isMutedNew = true;
                    if (isMutedNew != null /*&& isMuted != isMutedNew*/)
                    {
                        isMuted = isMutedNew;
                        this.MuteStateChangedSink(isMutedNew);
                    }
                }

            }

            override public Boolean isVirtualRenderer()
            {
                if (ModelDescription == "Virtual Media Player")
                    return true;
                return false;
            }

        }

        public class CpAVConnection_OhNet : CpAVConnection
        {
            OpenHome.Net.ControlPoint.Proxies.CpProxyUpnpOrgAVTransport1 avTransport;

            Timer progressTimer;

            public CpAVConnection_OhNet(OpenHome.Net.ControlPoint.CpDevice _rendererDevice)
            {
                avTransport = new OpenHome.Net.ControlPoint.Proxies.CpProxyUpnpOrgAVTransport1(_rendererDevice);
                avTransport.SetPropertyInitialEvent(propertyChanged);
                avTransport.SetPropertyChanged(propertyChanged);
                avTransport.SetPropertyLastChangeChanged(propertyChanged);                

                avTransport.Subscribe();              
            }

            public override void initTimer()
            {
                progressTimer = new Timer(1000);
                progressTimer.Elapsed += progressTimer_Elapsed;
                progressTimer.AutoReset = true;
            }

            protected void progressTimer_Elapsed(object sender, ElapsedEventArgs e)
            {
                avTransport.BeginGetPositionInfo(0, ResultGetPositionInfoSink);    
            }

            public void ResultGetPositionInfoSink(IntPtr _iHandle)
            {
                uint trackNr;
                int relCount, absCount;
                String trackDuration, trackMetaData, trackURI, relTime, absTime;
                try
                {
                    avTransport.EndGetPositionInfo(_iHandle, out trackNr, out trackDuration, out trackMetaData,
                        out trackURI, out relTime, out absTime, out relCount, out absCount);
                    this.PositionChangedSink(trackDuration, trackMetaData, absTime);
                }
                catch (Exception e)
                {
                   
                    // TODO: @@@
                    // If not implemeneted.. no matter
                    //progressTimer.AutoReset = false;
                    //progressTimer.Stop();
                    //@@@throw e;
                }
                 
            }
             
            ~CpAVConnection_OhNet()
            {
            }

            public override void initiateEvents()
            {
                //this.propertyChanged();
                this.AVTransportURIChangedSink();
            }

            protected void propertyChanged()
            {
                string avTransportUriNew;
                string avTransportUriMetaDataNew;
                string tmp;
                UInt32 CurrentTrackNew;
                string propXml = avTransport.PropertyLastChange();
                XMLParser xmlParser = new XMLParser();                

                avTransportUriMetaDataNew = xmlParser.getNodeAttributeValue(propXml, "AVTransportURIMetaData", "val");
                if (avTransportUriMetaDataNew != null && AVTransportUriMetaData != avTransportUriMetaDataNew)
                {
                    AVTransportUriMetaData = avTransportUriMetaDataNew;
                    this.AVTransportURIMetaDataChangedSink();
                }

                avTransportUriNew = xmlParser.getNodeAttributeValue(propXml, "AVTransportURI", "val");
                if (avTransportUriNew != null && AVTransportUri != avTransportUriNew)
                {
                    AVTransportUri = avTransportUriNew;
                    this.AVTransportURIChangedSink();
                }

                tmp = xmlParser.getNodeAttributeValue(propXml, "CurrentTrack", "val");
                if (tmp != null && tmp != "")
                {
                    CurrentTrackNew = Convert.ToUInt32(tmp);
                    if (CurrentTrack != CurrentTrackNew)
                    {
                        CurrentTrack = CurrentTrackNew;
                        this.TrackChangedSink(CurrentTrack); 
                    }
                }

                tmp = xmlParser.getNodeAttributeValue(propXml, "CurrentPlayMode", "val");
                if (tmp != null && tmp != currentPlayMode)
                {
                    currentPlayMode = tmp;
                    playMode = playModeStringToPlayMode(currentPlayMode);
                    this.CurrentPlayModeChangedSink();
                }

                tmp = xmlParser.getNodeAttributeValue(propXml, "TransportState", "val");
                if (tmp != null && tmp != "")
                {
                    this.PlayStateChangedSink(tmp);

                    if (progressTimer != null)
                    {
                        if (tmp == "PLAYING")
                            progressTimer.Start();
                        else
                            progressTimer.Stop();
                    }

                }
            }

            public override void SetAvTransportUri(System.String _uri, System.String _uriMetadata)
            {
                //avTransport.SyncSetPlayMode(1, "TRANSITIONING");                
                avTransport.SyncSetAVTransportURI(0, _uri, _uriMetadata);                
            }

            public override void BendAvTransportUri(System.String _uri, System.String _uriMetadata)
            {
                //avTransport.SyncSetPlayMode(1, "TRANSITIONING");                
                //avTransport.BeginBendAVTransportURI(0, _uri, _uriMetadata, null);
                avTransport.SyncBendAVTransportURI(0, _uri, _uriMetadata);
            }

            public override void Play()
            {
 
                avTransport.SyncPlay(0, "1");
            }

            public override void Stop()
            {
                avTransport.SyncStop(0);
            }

            public override void Seek(System.String _target)
            {
                avTransport.SyncSeek(0, "ABS_TIME", _target);
            }

            public override void Next()
            {
                avTransport.SyncNext(0);
            }

            public override void Previous()
            {
                avTransport.SyncPrevious(0);
            }

            public override void Pause()
            {
                avTransport.SyncPause(0);
            }

            public override void SetPlayMode(AvTransportPlayMode _playMode)
            {
                avTransport.SyncSetPlayMode(0, this.playModeToPlayModeString(_playMode));
            }
        }

        public class CpConfigService_OhNet : CpConfigService
        {
            OpenHome.Net.ControlPoint.Proxies.CpProxyRaumfeldComConfigService1 configService;
            public CpConfigService_OhNet(OpenHome.Net.ControlPoint.CpDevice _configDevice)
            {                               
                _configDevice.GetAttribute("Upnp.FriendlyName", out FriendlyName);
                _configDevice.GetAttribute("Upnp.DeviceXml", out DeviceXml);
                _configDevice.GetAttribute("Upnp.Location", out Location);
       
                configService = new OpenHome.Net.ControlPoint.Proxies.CpProxyRaumfeldComConfigService1(_configDevice);
                //configService.SetPropertyInitialEvent(propertyChanged);
                configService.SetPropertyChanged(propertyChanged);                
                configService.SetPropertyLastChangeChanged(propertyChanged); 

                configService.Subscribe();              
            }

            protected void propertyChanged()
            {                              
                //string propXml = configService.PropertyLastChange();
                //XMLParser xmlParser = new XMLParser();                
                this.ZoneSetupChangedSink();
                // TODO: @@@
              
            }
        }
    
}
