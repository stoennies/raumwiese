using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenHome.Net.Core;
using OpenHome.Net.ControlPoint;

namespace OpenHome.Net.ControlPoint.Proxies
{
    public interface ICpProxyUpnpOrgAVTransport1 : ICpProxy, IDisposable
    {
        void SyncSetAVTransportURI(uint aInstanceID, String aCurrentURI, String aCurrentURIMetaData);
        void BeginSetAVTransportURI(uint aInstanceID, String aCurrentURI, String aCurrentURIMetaData, CpProxy.CallbackAsyncComplete aCallback);
        void EndSetAVTransportURI(IntPtr aAsyncHandle);
        void SyncBendAVTransportURI(uint aInstanceID, String aCurrentURI, String aCurrentURIMetaData);
        void BeginBendAVTransportURI(uint aInstanceID, String aCurrentURI, String aCurrentURIMetaData, CpProxy.CallbackAsyncComplete aCallback);
        void EndBendAVTransportURI(IntPtr aAsyncHandle);
        void SyncLoveCurrentTrack(uint aInstanceID);
        void BeginLoveCurrentTrack(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndLoveCurrentTrack(IntPtr aAsyncHandle);
        void SyncBanCurrentTrack(uint aInstanceID);
        void BeginBanCurrentTrack(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndBanCurrentTrack(IntPtr aAsyncHandle);
        void SyncSetResourceForCurrentStream(String aResourceURI);
        void BeginSetResourceForCurrentStream(String aResourceURI, CpProxy.CallbackAsyncComplete aCallback);
        void EndSetResourceForCurrentStream(IntPtr aAsyncHandle);
        void SyncGetStreamProperties(out String aCurrentContentType, out uint aCurrentBitrate);
        void BeginGetStreamProperties(CpProxy.CallbackAsyncComplete aCallback);
        void EndGetStreamProperties(IntPtr aAsyncHandle, out String aCurrentContentType, out uint aCurrentBitrate);
        void SyncPause(uint aInstanceID);
        void BeginPause(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndPause(IntPtr aAsyncHandle);
        void SyncGetMediaInfo(uint aInstanceID, out uint aNrTracks, out String aMediaDuration, out String aCurrentURI, out String aCurrentURIMetaData, out String aNextURI, out String aNextURIMetaData, out String aPlayMedium, out String aRecordMedium, out String aWriteStatus);
        void BeginGetMediaInfo(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndGetMediaInfo(IntPtr aAsyncHandle, out uint aNrTracks, out String aMediaDuration, out String aCurrentURI, out String aCurrentURIMetaData, out String aNextURI, out String aNextURIMetaData, out String aPlayMedium, out String aRecordMedium, out String aWriteStatus);
        void SyncGetTransportInfo(uint aInstanceID, out String aCurrentTransportState, out String aCurrentTransportStatus, out String aCurrentSpeed);
        void BeginGetTransportInfo(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndGetTransportInfo(IntPtr aAsyncHandle, out String aCurrentTransportState, out String aCurrentTransportStatus, out String aCurrentSpeed);
        void SyncGetPositionInfo(uint aInstanceID, out uint aTrack, out String aTrackDuration, out String aTrackMetaData, out String aTrackURI, out String aRelTime, out String aAbsTime, out int aRelCount, out int aAbsCount);
        void BeginGetPositionInfo(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndGetPositionInfo(IntPtr aAsyncHandle, out uint aTrack, out String aTrackDuration, out String aTrackMetaData, out String aTrackURI, out String aRelTime, out String aAbsTime, out int aRelCount, out int aAbsCount);
        void SyncGetDeviceCapabilities(uint aInstanceID, out String aPlayMedia, out String aRecMedia, out String aRecQualityModes);
        void BeginGetDeviceCapabilities(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndGetDeviceCapabilities(IntPtr aAsyncHandle, out String aPlayMedia, out String aRecMedia, out String aRecQualityModes);
        void SyncGetTransportSettings(uint aInstanceID, out String aPlayMode, out String aRecQualityMode);
        void BeginGetTransportSettings(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndGetTransportSettings(IntPtr aAsyncHandle, out String aPlayMode, out String aRecQualityMode);
        void SyncStop(uint aInstanceID);
        void BeginStop(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndStop(IntPtr aAsyncHandle);
        void SyncPlay(uint aInstanceID, String aSpeed);
        void BeginPlay(uint aInstanceID, String aSpeed, CpProxy.CallbackAsyncComplete aCallback);
        void EndPlay(IntPtr aAsyncHandle);
        void SyncSeek(uint aInstanceID, String aUnit, String aTarget);
        void BeginSeek(uint aInstanceID, String aUnit, String aTarget, CpProxy.CallbackAsyncComplete aCallback);
        void EndSeek(IntPtr aAsyncHandle);
        void SyncNext(uint aInstanceID);
        void BeginNext(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndNext(IntPtr aAsyncHandle);
        void SyncPrevious(uint aInstanceID);
        void BeginPrevious(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndPrevious(IntPtr aAsyncHandle);
        void SyncSetPlayMode(uint aInstanceID, String aNewPlayMode);
        void BeginSetPlayMode(uint aInstanceID, String aNewPlayMode, CpProxy.CallbackAsyncComplete aCallback);
        void EndSetPlayMode(IntPtr aAsyncHandle);
        void SyncGetCurrentTransportActions(uint aInstanceID, out String aActions);
        void BeginGetCurrentTransportActions(uint aInstanceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndGetCurrentTransportActions(IntPtr aAsyncHandle, out String aActions);
        void SyncSetNextStartTriggerTime(uint aInstanceID, String aTimeService, String aStartTime);
        void BeginSetNextStartTriggerTime(uint aInstanceID, String aTimeService, String aStartTime, CpProxy.CallbackAsyncComplete aCallback);
        void EndSetNextStartTriggerTime(IntPtr aAsyncHandle);
        void SetPropertyBufferFilledChanged(System.Action aBufferFilledChanged);
        uint PropertyBufferFilled();
        void SetPropertyLastChangeChanged(System.Action aLastChangeChanged);
        String PropertyLastChange();
    }

    internal class SyncSetAVTransportURIUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncSetAVTransportURIUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndSetAVTransportURI(aAsyncHandle);
        }
    };

    internal class SyncBendAVTransportURIUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncBendAVTransportURIUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndBendAVTransportURI(aAsyncHandle);
        }
    };

    internal class SyncLoveCurrentTrackUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncLoveCurrentTrackUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndLoveCurrentTrack(aAsyncHandle);
        }
    };

    internal class SyncBanCurrentTrackUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncBanCurrentTrackUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndBanCurrentTrack(aAsyncHandle);
        }
    };

    internal class SyncSetResourceForCurrentStreamUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncSetResourceForCurrentStreamUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndSetResourceForCurrentStream(aAsyncHandle);
        }
    };

    internal class SyncGetStreamPropertiesUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;
        private String iCurrentContentType;
        private uint iCurrentBitrate;

        public SyncGetStreamPropertiesUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        public String CurrentContentType()
        {
            return iCurrentContentType;
        }
        public uint CurrentBitrate()
        {
            return iCurrentBitrate;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetStreamProperties(aAsyncHandle, out iCurrentContentType, out iCurrentBitrate);
        }
    };

    internal class SyncPauseUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncPauseUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndPause(aAsyncHandle);
        }
    };

    internal class SyncGetMediaInfoUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;
        private uint iNrTracks;
        private String iMediaDuration;
        private String iCurrentURI;
        private String iCurrentURIMetaData;
        private String iNextURI;
        private String iNextURIMetaData;
        private String iPlayMedium;
        private String iRecordMedium;
        private String iWriteStatus;

        public SyncGetMediaInfoUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        public uint NrTracks()
        {
            return iNrTracks;
        }
        public String MediaDuration()
        {
            return iMediaDuration;
        }
        public String CurrentURI()
        {
            return iCurrentURI;
        }
        public String CurrentURIMetaData()
        {
            return iCurrentURIMetaData;
        }
        public String NextURI()
        {
            return iNextURI;
        }
        public String NextURIMetaData()
        {
            return iNextURIMetaData;
        }
        public String PlayMedium()
        {
            return iPlayMedium;
        }
        public String RecordMedium()
        {
            return iRecordMedium;
        }
        public String WriteStatus()
        {
            return iWriteStatus;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetMediaInfo(aAsyncHandle, out iNrTracks, out iMediaDuration, out iCurrentURI, out iCurrentURIMetaData, out iNextURI, out iNextURIMetaData, out iPlayMedium, out iRecordMedium, out iWriteStatus);
        }
    };

    internal class SyncGetTransportInfoUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;
        private String iCurrentTransportState;
        private String iCurrentTransportStatus;
        private String iCurrentSpeed;

        public SyncGetTransportInfoUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        public String CurrentTransportState()
        {
            return iCurrentTransportState;
        }
        public String CurrentTransportStatus()
        {
            return iCurrentTransportStatus;
        }
        public String CurrentSpeed()
        {
            return iCurrentSpeed;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetTransportInfo(aAsyncHandle, out iCurrentTransportState, out iCurrentTransportStatus, out iCurrentSpeed);
        }
    };

    internal class SyncGetPositionInfoUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;
        private uint iTrack;
        private String iTrackDuration;
        private String iTrackMetaData;
        private String iTrackURI;
        private String iRelTime;
        private String iAbsTime;
        private int iRelCount;
        private int iAbsCount;

        public SyncGetPositionInfoUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        public uint Track()
        {
            return iTrack;
        }
        public String TrackDuration()
        {
            return iTrackDuration;
        }
        public String TrackMetaData()
        {
            return iTrackMetaData;
        }
        public String TrackURI()
        {
            return iTrackURI;
        }
        public String RelTime()
        {
            return iRelTime;
        }
        public String AbsTime()
        {
            return iAbsTime;
        }
        public int RelCount()
        {
            return iRelCount;
        }
        public int AbsCount()
        {
            return iAbsCount;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetPositionInfo(aAsyncHandle, out iTrack, out iTrackDuration, out iTrackMetaData, out iTrackURI, out iRelTime, out iAbsTime, out iRelCount, out iAbsCount);
        }
    };

    internal class SyncGetDeviceCapabilitiesUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;
        private String iPlayMedia;
        private String iRecMedia;
        private String iRecQualityModes;

        public SyncGetDeviceCapabilitiesUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        public String PlayMedia()
        {
            return iPlayMedia;
        }
        public String RecMedia()
        {
            return iRecMedia;
        }
        public String RecQualityModes()
        {
            return iRecQualityModes;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetDeviceCapabilities(aAsyncHandle, out iPlayMedia, out iRecMedia, out iRecQualityModes);
        }
    };

    internal class SyncGetTransportSettingsUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;
        private String iPlayMode;
        private String iRecQualityMode;

        public SyncGetTransportSettingsUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        public String PlayMode()
        {
            return iPlayMode;
        }
        public String RecQualityMode()
        {
            return iRecQualityMode;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetTransportSettings(aAsyncHandle, out iPlayMode, out iRecQualityMode);
        }
    };

    internal class SyncStopUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncStopUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndStop(aAsyncHandle);
        }
    };

    internal class SyncPlayUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncPlayUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndPlay(aAsyncHandle);
        }
    };

    internal class SyncSeekUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncSeekUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndSeek(aAsyncHandle);
        }
    };

    internal class SyncNextUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncNextUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndNext(aAsyncHandle);
        }
    };

    internal class SyncPreviousUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncPreviousUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndPrevious(aAsyncHandle);
        }
    };

    internal class SyncSetPlayModeUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncSetPlayModeUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndSetPlayMode(aAsyncHandle);
        }
    };

    internal class SyncGetCurrentTransportActionsUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;
        private String iActions;

        public SyncGetCurrentTransportActionsUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        public String Actions()
        {
            return iActions;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetCurrentTransportActions(aAsyncHandle, out iActions);
        }
    };

    internal class SyncSetNextStartTriggerTimeUpnpOrgAVTransport1 : SyncProxyAction
    {
        private CpProxyUpnpOrgAVTransport1 iService;

        public SyncSetNextStartTriggerTimeUpnpOrgAVTransport1(CpProxyUpnpOrgAVTransport1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndSetNextStartTriggerTime(aAsyncHandle);
        }
    };

    /// <summary>
    /// Proxy for the upnp.org:AVTransport:1 UPnP service
    /// </summary>
    public class CpProxyUpnpOrgAVTransport1 : CpProxy, IDisposable, ICpProxyUpnpOrgAVTransport1
    {
        private OpenHome.Net.Core.Action iActionSetAVTransportURI;
        private OpenHome.Net.Core.Action iActionBendAVTransportURI;
        private OpenHome.Net.Core.Action iActionLoveCurrentTrack;
        private OpenHome.Net.Core.Action iActionBanCurrentTrack;
        private OpenHome.Net.Core.Action iActionSetResourceForCurrentStream;
        private OpenHome.Net.Core.Action iActionGetStreamProperties;
        private OpenHome.Net.Core.Action iActionPause;
        private OpenHome.Net.Core.Action iActionGetMediaInfo;
        private OpenHome.Net.Core.Action iActionGetTransportInfo;
        private OpenHome.Net.Core.Action iActionGetPositionInfo;
        private OpenHome.Net.Core.Action iActionGetDeviceCapabilities;
        private OpenHome.Net.Core.Action iActionGetTransportSettings;
        private OpenHome.Net.Core.Action iActionStop;
        private OpenHome.Net.Core.Action iActionPlay;
        private OpenHome.Net.Core.Action iActionSeek;
        private OpenHome.Net.Core.Action iActionNext;
        private OpenHome.Net.Core.Action iActionPrevious;
        private OpenHome.Net.Core.Action iActionSetPlayMode;
        private OpenHome.Net.Core.Action iActionGetCurrentTransportActions;
        private OpenHome.Net.Core.Action iActionSetNextStartTriggerTime;
        private PropertyUint iBufferFilled;
        private PropertyString iLastChange;
        private System.Action iBufferFilledChanged;
        private System.Action iLastChangeChanged;
        private Mutex iPropertyLock;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>Use CpProxy::[Un]Subscribe() to enable/disable querying of state variable and reporting of their changes.</remarks>
        /// <param name="aDevice">The device to use</param>
        public CpProxyUpnpOrgAVTransport1(CpDevice aDevice)
            : base("schemas-upnp-org", "AVTransport", 1, aDevice)
        {
            OpenHome.Net.Core.Parameter param;
            List<String> allowedValues = new List<String>();

            iActionSetAVTransportURI = new OpenHome.Net.Core.Action("SetAVTransportURI");
            param = new ParameterUint("InstanceID");
            iActionSetAVTransportURI.AddInputParameter(param);
            param = new ParameterString("CurrentURI", allowedValues);
            iActionSetAVTransportURI.AddInputParameter(param);
            param = new ParameterString("CurrentURIMetaData", allowedValues);
            iActionSetAVTransportURI.AddInputParameter(param);

            iActionBendAVTransportURI = new OpenHome.Net.Core.Action("BendAVTransportURI");
            param = new ParameterUint("InstanceID");
            iActionBendAVTransportURI.AddInputParameter(param);
            param = new ParameterString("CurrentURI", allowedValues);
            iActionBendAVTransportURI.AddInputParameter(param);
            param = new ParameterString("CurrentURIMetaData", allowedValues);
            iActionBendAVTransportURI.AddInputParameter(param);

            iActionLoveCurrentTrack = new OpenHome.Net.Core.Action("LoveCurrentTrack");
            param = new ParameterUint("InstanceID");
            iActionLoveCurrentTrack.AddInputParameter(param);

            iActionBanCurrentTrack = new OpenHome.Net.Core.Action("BanCurrentTrack");
            param = new ParameterUint("InstanceID");
            iActionBanCurrentTrack.AddInputParameter(param);

            iActionSetResourceForCurrentStream = new OpenHome.Net.Core.Action("SetResourceForCurrentStream");
            param = new ParameterString("ResourceURI", allowedValues);
            iActionSetResourceForCurrentStream.AddInputParameter(param);

            iActionGetStreamProperties = new OpenHome.Net.Core.Action("GetStreamProperties");
            param = new ParameterString("CurrentContentType", allowedValues);
            iActionGetStreamProperties.AddOutputParameter(param);
            param = new ParameterUint("CurrentBitrate");
            iActionGetStreamProperties.AddOutputParameter(param);

            iActionPause = new OpenHome.Net.Core.Action("Pause");
            param = new ParameterUint("InstanceID");
            iActionPause.AddInputParameter(param);

            iActionGetMediaInfo = new OpenHome.Net.Core.Action("GetMediaInfo");
            param = new ParameterUint("InstanceID");
            iActionGetMediaInfo.AddInputParameter(param);
            param = new ParameterUint("NrTracks");
            iActionGetMediaInfo.AddOutputParameter(param);
            param = new ParameterString("MediaDuration", allowedValues);
            iActionGetMediaInfo.AddOutputParameter(param);
            param = new ParameterString("CurrentURI", allowedValues);
            iActionGetMediaInfo.AddOutputParameter(param);
            param = new ParameterString("CurrentURIMetaData", allowedValues);
            iActionGetMediaInfo.AddOutputParameter(param);
            param = new ParameterString("NextURI", allowedValues);
            iActionGetMediaInfo.AddOutputParameter(param);
            param = new ParameterString("NextURIMetaData", allowedValues);
            iActionGetMediaInfo.AddOutputParameter(param);
            allowedValues.Add("NETWORK");
            param = new ParameterString("PlayMedium", allowedValues);
            iActionGetMediaInfo.AddOutputParameter(param);
            allowedValues.Clear();
            allowedValues.Add("NOT_IMPLEMENTED");
            param = new ParameterString("RecordMedium", allowedValues);
            iActionGetMediaInfo.AddOutputParameter(param);
            allowedValues.Clear();
            allowedValues.Add("NOT_IMPLEMENTED");
            param = new ParameterString("WriteStatus", allowedValues);
            iActionGetMediaInfo.AddOutputParameter(param);
            allowedValues.Clear();

            iActionGetTransportInfo = new OpenHome.Net.Core.Action("GetTransportInfo");
            param = new ParameterUint("InstanceID");
            iActionGetTransportInfo.AddInputParameter(param);
            allowedValues.Add("STOPPED");
            allowedValues.Add("PLAYING");
            allowedValues.Add("TRANSITIONING");
            allowedValues.Add("NO_MEDIA_PRESENT");
            param = new ParameterString("CurrentTransportState", allowedValues);
            iActionGetTransportInfo.AddOutputParameter(param);
            allowedValues.Clear();
            allowedValues.Add("OK");
            allowedValues.Add("ERROR_OCCURRED");
            param = new ParameterString("CurrentTransportStatus", allowedValues);
            iActionGetTransportInfo.AddOutputParameter(param);
            allowedValues.Clear();
            allowedValues.Add("1");
            param = new ParameterString("CurrentSpeed", allowedValues);
            iActionGetTransportInfo.AddOutputParameter(param);
            allowedValues.Clear();

            iActionGetPositionInfo = new OpenHome.Net.Core.Action("GetPositionInfo");
            param = new ParameterUint("InstanceID");
            iActionGetPositionInfo.AddInputParameter(param);
            param = new ParameterUint("Track");
            iActionGetPositionInfo.AddOutputParameter(param);
            param = new ParameterString("TrackDuration", allowedValues);
            iActionGetPositionInfo.AddOutputParameter(param);
            param = new ParameterString("TrackMetaData", allowedValues);
            iActionGetPositionInfo.AddOutputParameter(param);
            param = new ParameterString("TrackURI", allowedValues);
            iActionGetPositionInfo.AddOutputParameter(param);
            param = new ParameterString("RelTime", allowedValues);
            iActionGetPositionInfo.AddOutputParameter(param);
            param = new ParameterString("AbsTime", allowedValues);
            iActionGetPositionInfo.AddOutputParameter(param);
            param = new ParameterInt("RelCount");
            iActionGetPositionInfo.AddOutputParameter(param);
            param = new ParameterInt("AbsCount");
            iActionGetPositionInfo.AddOutputParameter(param);

            iActionGetDeviceCapabilities = new OpenHome.Net.Core.Action("GetDeviceCapabilities");
            param = new ParameterUint("InstanceID");
            iActionGetDeviceCapabilities.AddInputParameter(param);
            param = new ParameterString("PlayMedia", allowedValues);
            iActionGetDeviceCapabilities.AddOutputParameter(param);
            param = new ParameterString("RecMedia", allowedValues);
            iActionGetDeviceCapabilities.AddOutputParameter(param);
            param = new ParameterString("RecQualityModes", allowedValues);
            iActionGetDeviceCapabilities.AddOutputParameter(param);

            iActionGetTransportSettings = new OpenHome.Net.Core.Action("GetTransportSettings");
            param = new ParameterUint("InstanceID");
            iActionGetTransportSettings.AddInputParameter(param);
            allowedValues.Add("NORMAL");
            param = new ParameterString("PlayMode", allowedValues);
            iActionGetTransportSettings.AddOutputParameter(param);
            allowedValues.Clear();
            allowedValues.Add("NOT_IMPLEMENTED");
            param = new ParameterString("RecQualityMode", allowedValues);
            iActionGetTransportSettings.AddOutputParameter(param);
            allowedValues.Clear();

            iActionStop = new OpenHome.Net.Core.Action("Stop");
            param = new ParameterUint("InstanceID");
            iActionStop.AddInputParameter(param);

            iActionPlay = new OpenHome.Net.Core.Action("Play");
            param = new ParameterUint("InstanceID");
            iActionPlay.AddInputParameter(param);
            allowedValues.Add("1");
            param = new ParameterString("Speed", allowedValues);
            iActionPlay.AddInputParameter(param);
            allowedValues.Clear();

            iActionSeek = new OpenHome.Net.Core.Action("Seek");
            param = new ParameterUint("InstanceID");
            iActionSeek.AddInputParameter(param);
            allowedValues.Add("TRACK_NR");
            allowedValues.Add("ABS_TIME");
            param = new ParameterString("Unit", allowedValues);
            iActionSeek.AddInputParameter(param);
            allowedValues.Clear();
            param = new ParameterString("Target", allowedValues);
            iActionSeek.AddInputParameter(param);

            iActionNext = new OpenHome.Net.Core.Action("Next");
            param = new ParameterUint("InstanceID");
            iActionNext.AddInputParameter(param);

            iActionPrevious = new OpenHome.Net.Core.Action("Previous");
            param = new ParameterUint("InstanceID");
            iActionPrevious.AddInputParameter(param);

            iActionSetPlayMode = new OpenHome.Net.Core.Action("SetPlayMode");
            param = new ParameterUint("InstanceID");
            iActionSetPlayMode.AddInputParameter(param);
            allowedValues.Add("NORMAL");
            allowedValues.Add("SHUFFLE");
            allowedValues.Add("RANDOM");
            allowedValues.Add("REPEAT_ONE");
            allowedValues.Add("REPEAT_ALL");
            param = new ParameterString("NewPlayMode", allowedValues);
            iActionSetPlayMode.AddInputParameter(param);
            allowedValues.Clear();

            iActionGetCurrentTransportActions = new OpenHome.Net.Core.Action("GetCurrentTransportActions");
            param = new ParameterUint("InstanceID");
            iActionGetCurrentTransportActions.AddInputParameter(param);
            param = new ParameterString("Actions", allowedValues);
            iActionGetCurrentTransportActions.AddOutputParameter(param);

            iActionSetNextStartTriggerTime = new OpenHome.Net.Core.Action("SetNextStartTriggerTime");
            param = new ParameterUint("InstanceID");
            iActionSetNextStartTriggerTime.AddInputParameter(param);
            param = new ParameterString("TimeService", allowedValues);
            iActionSetNextStartTriggerTime.AddInputParameter(param);
            param = new ParameterString("StartTime", allowedValues);
            iActionSetNextStartTriggerTime.AddInputParameter(param);

            iBufferFilled = new PropertyUint("BufferFilled", BufferFilledPropertyChanged);
            AddProperty(iBufferFilled);
            iLastChange = new PropertyString("LastChange", LastChangePropertyChanged);
            AddProperty(iLastChange);
            
            iPropertyLock = new Mutex();
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCurrentURI"></param>
        /// <param name="aCurrentURIMetaData"></param>
        public void SyncSetAVTransportURI(uint aInstanceID, String aCurrentURI, String aCurrentURIMetaData)
        {
            SyncSetAVTransportURIUpnpOrgAVTransport1 sync = new SyncSetAVTransportURIUpnpOrgAVTransport1(this);
            BeginSetAVTransportURI(aInstanceID, aCurrentURI, aCurrentURIMetaData, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndSetAVTransportURI().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCurrentURI"></param>
        /// <param name="aCurrentURIMetaData"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginSetAVTransportURI(uint aInstanceID, String aCurrentURI, String aCurrentURIMetaData, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionSetAVTransportURI, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionSetAVTransportURI.InputParameter(inIndex++), aInstanceID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSetAVTransportURI.InputParameter(inIndex++), aCurrentURI));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSetAVTransportURI.InputParameter(inIndex++), aCurrentURIMetaData));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndSetAVTransportURI(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCurrentURI"></param>
        /// <param name="aCurrentURIMetaData"></param>
        public void SyncBendAVTransportURI(uint aInstanceID, String aCurrentURI, String aCurrentURIMetaData)
        {
            SyncBendAVTransportURIUpnpOrgAVTransport1 sync = new SyncBendAVTransportURIUpnpOrgAVTransport1(this);
            BeginBendAVTransportURI(aInstanceID, aCurrentURI, aCurrentURIMetaData, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndBendAVTransportURI().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCurrentURI"></param>
        /// <param name="aCurrentURIMetaData"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginBendAVTransportURI(uint aInstanceID, String aCurrentURI, String aCurrentURIMetaData, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionBendAVTransportURI, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionBendAVTransportURI.InputParameter(inIndex++), aInstanceID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionBendAVTransportURI.InputParameter(inIndex++), aCurrentURI));
            invocation.AddInput(new ArgumentString((ParameterString)iActionBendAVTransportURI.InputParameter(inIndex++), aCurrentURIMetaData));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndBendAVTransportURI(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        public void SyncLoveCurrentTrack(uint aInstanceID)
        {
            SyncLoveCurrentTrackUpnpOrgAVTransport1 sync = new SyncLoveCurrentTrackUpnpOrgAVTransport1(this);
            BeginLoveCurrentTrack(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndLoveCurrentTrack().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginLoveCurrentTrack(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionLoveCurrentTrack, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionLoveCurrentTrack.InputParameter(inIndex++), aInstanceID));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndLoveCurrentTrack(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        public void SyncBanCurrentTrack(uint aInstanceID)
        {
            SyncBanCurrentTrackUpnpOrgAVTransport1 sync = new SyncBanCurrentTrackUpnpOrgAVTransport1(this);
            BeginBanCurrentTrack(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndBanCurrentTrack().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginBanCurrentTrack(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionBanCurrentTrack, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionBanCurrentTrack.InputParameter(inIndex++), aInstanceID));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndBanCurrentTrack(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aResourceURI"></param>
        public void SyncSetResourceForCurrentStream(String aResourceURI)
        {
            SyncSetResourceForCurrentStreamUpnpOrgAVTransport1 sync = new SyncSetResourceForCurrentStreamUpnpOrgAVTransport1(this);
            BeginSetResourceForCurrentStream(aResourceURI, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndSetResourceForCurrentStream().</remarks>
        /// <param name="aResourceURI"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginSetResourceForCurrentStream(String aResourceURI, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionSetResourceForCurrentStream, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionSetResourceForCurrentStream.InputParameter(inIndex++), aResourceURI));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndSetResourceForCurrentStream(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aCurrentContentType"></param>
        /// <param name="aCurrentBitrate"></param>
        public void SyncGetStreamProperties(out String aCurrentContentType, out uint aCurrentBitrate)
        {
            SyncGetStreamPropertiesUpnpOrgAVTransport1 sync = new SyncGetStreamPropertiesUpnpOrgAVTransport1(this);
            BeginGetStreamProperties(sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aCurrentContentType = sync.CurrentContentType();
            aCurrentBitrate = sync.CurrentBitrate();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetStreamProperties().</remarks>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetStreamProperties(CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetStreamProperties, aCallback);
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetStreamProperties.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionGetStreamProperties.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aCurrentContentType"></param>
        /// <param name="aCurrentBitrate"></param>
        public void EndGetStreamProperties(IntPtr aAsyncHandle, out String aCurrentContentType, out uint aCurrentBitrate)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aCurrentContentType = Invocation.OutputString(aAsyncHandle, index++);
            aCurrentBitrate = Invocation.OutputUint(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        public void SyncPause(uint aInstanceID)
        {
            SyncPauseUpnpOrgAVTransport1 sync = new SyncPauseUpnpOrgAVTransport1(this);
            BeginPause(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndPause().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginPause(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionPause, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionPause.InputParameter(inIndex++), aInstanceID));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndPause(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aNrTracks"></param>
        /// <param name="aMediaDuration"></param>
        /// <param name="aCurrentURI"></param>
        /// <param name="aCurrentURIMetaData"></param>
        /// <param name="aNextURI"></param>
        /// <param name="aNextURIMetaData"></param>
        /// <param name="aPlayMedium"></param>
        /// <param name="aRecordMedium"></param>
        /// <param name="aWriteStatus"></param>
        public void SyncGetMediaInfo(uint aInstanceID, out uint aNrTracks, out String aMediaDuration, out String aCurrentURI, out String aCurrentURIMetaData, out String aNextURI, out String aNextURIMetaData, out String aPlayMedium, out String aRecordMedium, out String aWriteStatus)
        {
            SyncGetMediaInfoUpnpOrgAVTransport1 sync = new SyncGetMediaInfoUpnpOrgAVTransport1(this);
            BeginGetMediaInfo(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aNrTracks = sync.NrTracks();
            aMediaDuration = sync.MediaDuration();
            aCurrentURI = sync.CurrentURI();
            aCurrentURIMetaData = sync.CurrentURIMetaData();
            aNextURI = sync.NextURI();
            aNextURIMetaData = sync.NextURIMetaData();
            aPlayMedium = sync.PlayMedium();
            aRecordMedium = sync.RecordMedium();
            aWriteStatus = sync.WriteStatus();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetMediaInfo().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetMediaInfo(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetMediaInfo, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionGetMediaInfo.InputParameter(inIndex++), aInstanceID));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionGetMediaInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetMediaInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetMediaInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetMediaInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetMediaInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetMediaInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetMediaInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetMediaInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetMediaInfo.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aNrTracks"></param>
        /// <param name="aMediaDuration"></param>
        /// <param name="aCurrentURI"></param>
        /// <param name="aCurrentURIMetaData"></param>
        /// <param name="aNextURI"></param>
        /// <param name="aNextURIMetaData"></param>
        /// <param name="aPlayMedium"></param>
        /// <param name="aRecordMedium"></param>
        /// <param name="aWriteStatus"></param>
        public void EndGetMediaInfo(IntPtr aAsyncHandle, out uint aNrTracks, out String aMediaDuration, out String aCurrentURI, out String aCurrentURIMetaData, out String aNextURI, out String aNextURIMetaData, out String aPlayMedium, out String aRecordMedium, out String aWriteStatus)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aNrTracks = Invocation.OutputUint(aAsyncHandle, index++);
            aMediaDuration = Invocation.OutputString(aAsyncHandle, index++);
            aCurrentURI = Invocation.OutputString(aAsyncHandle, index++);
            aCurrentURIMetaData = Invocation.OutputString(aAsyncHandle, index++);
            aNextURI = Invocation.OutputString(aAsyncHandle, index++);
            aNextURIMetaData = Invocation.OutputString(aAsyncHandle, index++);
            aPlayMedium = Invocation.OutputString(aAsyncHandle, index++);
            aRecordMedium = Invocation.OutputString(aAsyncHandle, index++);
            aWriteStatus = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCurrentTransportState"></param>
        /// <param name="aCurrentTransportStatus"></param>
        /// <param name="aCurrentSpeed"></param>
        public void SyncGetTransportInfo(uint aInstanceID, out String aCurrentTransportState, out String aCurrentTransportStatus, out String aCurrentSpeed)
        {
            SyncGetTransportInfoUpnpOrgAVTransport1 sync = new SyncGetTransportInfoUpnpOrgAVTransport1(this);
            BeginGetTransportInfo(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aCurrentTransportState = sync.CurrentTransportState();
            aCurrentTransportStatus = sync.CurrentTransportStatus();
            aCurrentSpeed = sync.CurrentSpeed();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetTransportInfo().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetTransportInfo(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetTransportInfo, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionGetTransportInfo.InputParameter(inIndex++), aInstanceID));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetTransportInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetTransportInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetTransportInfo.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aCurrentTransportState"></param>
        /// <param name="aCurrentTransportStatus"></param>
        /// <param name="aCurrentSpeed"></param>
        public void EndGetTransportInfo(IntPtr aAsyncHandle, out String aCurrentTransportState, out String aCurrentTransportStatus, out String aCurrentSpeed)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aCurrentTransportState = Invocation.OutputString(aAsyncHandle, index++);
            aCurrentTransportStatus = Invocation.OutputString(aAsyncHandle, index++);
            aCurrentSpeed = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aTrack"></param>
        /// <param name="aTrackDuration"></param>
        /// <param name="aTrackMetaData"></param>
        /// <param name="aTrackURI"></param>
        /// <param name="aRelTime"></param>
        /// <param name="aAbsTime"></param>
        /// <param name="aRelCount"></param>
        /// <param name="aAbsCount"></param>
        public void SyncGetPositionInfo(uint aInstanceID, out uint aTrack, out String aTrackDuration, out String aTrackMetaData, out String aTrackURI, out String aRelTime, out String aAbsTime, out int aRelCount, out int aAbsCount)
        {
            SyncGetPositionInfoUpnpOrgAVTransport1 sync = new SyncGetPositionInfoUpnpOrgAVTransport1(this);
            BeginGetPositionInfo(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aTrack = sync.Track();
            aTrackDuration = sync.TrackDuration();
            aTrackMetaData = sync.TrackMetaData();
            aTrackURI = sync.TrackURI();
            aRelTime = sync.RelTime();
            aAbsTime = sync.AbsTime();
            aRelCount = sync.RelCount();
            aAbsCount = sync.AbsCount();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetPositionInfo().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetPositionInfo(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetPositionInfo, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionGetPositionInfo.InputParameter(inIndex++), aInstanceID));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionGetPositionInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetPositionInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetPositionInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetPositionInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetPositionInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetPositionInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentInt((ParameterInt)iActionGetPositionInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentInt((ParameterInt)iActionGetPositionInfo.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aTrack"></param>
        /// <param name="aTrackDuration"></param>
        /// <param name="aTrackMetaData"></param>
        /// <param name="aTrackURI"></param>
        /// <param name="aRelTime"></param>
        /// <param name="aAbsTime"></param>
        /// <param name="aRelCount"></param>
        /// <param name="aAbsCount"></param>
        public void EndGetPositionInfo(IntPtr aAsyncHandle, out uint aTrack, out String aTrackDuration, out String aTrackMetaData, out String aTrackURI, out String aRelTime, out String aAbsTime, out int aRelCount, out int aAbsCount)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aTrack = Invocation.OutputUint(aAsyncHandle, index++);
            aTrackDuration = Invocation.OutputString(aAsyncHandle, index++);
            aTrackMetaData = Invocation.OutputString(aAsyncHandle, index++);
            aTrackURI = Invocation.OutputString(aAsyncHandle, index++);
            aRelTime = Invocation.OutputString(aAsyncHandle, index++);
            aAbsTime = Invocation.OutputString(aAsyncHandle, index++);
            aRelCount = Invocation.OutputInt(aAsyncHandle, index++);
            aAbsCount = Invocation.OutputInt(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aPlayMedia"></param>
        /// <param name="aRecMedia"></param>
        /// <param name="aRecQualityModes"></param>
        public void SyncGetDeviceCapabilities(uint aInstanceID, out String aPlayMedia, out String aRecMedia, out String aRecQualityModes)
        {
            SyncGetDeviceCapabilitiesUpnpOrgAVTransport1 sync = new SyncGetDeviceCapabilitiesUpnpOrgAVTransport1(this);
            BeginGetDeviceCapabilities(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aPlayMedia = sync.PlayMedia();
            aRecMedia = sync.RecMedia();
            aRecQualityModes = sync.RecQualityModes();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetDeviceCapabilities().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetDeviceCapabilities(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetDeviceCapabilities, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionGetDeviceCapabilities.InputParameter(inIndex++), aInstanceID));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetDeviceCapabilities.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetDeviceCapabilities.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetDeviceCapabilities.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aPlayMedia"></param>
        /// <param name="aRecMedia"></param>
        /// <param name="aRecQualityModes"></param>
        public void EndGetDeviceCapabilities(IntPtr aAsyncHandle, out String aPlayMedia, out String aRecMedia, out String aRecQualityModes)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aPlayMedia = Invocation.OutputString(aAsyncHandle, index++);
            aRecMedia = Invocation.OutputString(aAsyncHandle, index++);
            aRecQualityModes = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aPlayMode"></param>
        /// <param name="aRecQualityMode"></param>
        public void SyncGetTransportSettings(uint aInstanceID, out String aPlayMode, out String aRecQualityMode)
        {
            SyncGetTransportSettingsUpnpOrgAVTransport1 sync = new SyncGetTransportSettingsUpnpOrgAVTransport1(this);
            BeginGetTransportSettings(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aPlayMode = sync.PlayMode();
            aRecQualityMode = sync.RecQualityMode();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetTransportSettings().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetTransportSettings(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetTransportSettings, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionGetTransportSettings.InputParameter(inIndex++), aInstanceID));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetTransportSettings.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetTransportSettings.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aPlayMode"></param>
        /// <param name="aRecQualityMode"></param>
        public void EndGetTransportSettings(IntPtr aAsyncHandle, out String aPlayMode, out String aRecQualityMode)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aPlayMode = Invocation.OutputString(aAsyncHandle, index++);
            aRecQualityMode = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        public void SyncStop(uint aInstanceID)
        {
            SyncStopUpnpOrgAVTransport1 sync = new SyncStopUpnpOrgAVTransport1(this);
            BeginStop(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndStop().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginStop(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionStop, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionStop.InputParameter(inIndex++), aInstanceID));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndStop(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aSpeed"></param>
        public void SyncPlay(uint aInstanceID, String aSpeed)
        {
            SyncPlayUpnpOrgAVTransport1 sync = new SyncPlayUpnpOrgAVTransport1(this);
            BeginPlay(aInstanceID, aSpeed, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndPlay().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aSpeed"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginPlay(uint aInstanceID, String aSpeed, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionPlay, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionPlay.InputParameter(inIndex++), aInstanceID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionPlay.InputParameter(inIndex++), aSpeed));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndPlay(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aUnit"></param>
        /// <param name="aTarget"></param>
        public void SyncSeek(uint aInstanceID, String aUnit, String aTarget)
        {
            SyncSeekUpnpOrgAVTransport1 sync = new SyncSeekUpnpOrgAVTransport1(this);
            BeginSeek(aInstanceID, aUnit, aTarget, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndSeek().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aUnit"></param>
        /// <param name="aTarget"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginSeek(uint aInstanceID, String aUnit, String aTarget, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionSeek, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionSeek.InputParameter(inIndex++), aInstanceID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSeek.InputParameter(inIndex++), aUnit));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSeek.InputParameter(inIndex++), aTarget));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndSeek(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        public void SyncNext(uint aInstanceID)
        {
            SyncNextUpnpOrgAVTransport1 sync = new SyncNextUpnpOrgAVTransport1(this);
            BeginNext(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndNext().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginNext(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionNext, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionNext.InputParameter(inIndex++), aInstanceID));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndNext(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        public void SyncPrevious(uint aInstanceID)
        {
            SyncPreviousUpnpOrgAVTransport1 sync = new SyncPreviousUpnpOrgAVTransport1(this);
            BeginPrevious(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndPrevious().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginPrevious(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionPrevious, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionPrevious.InputParameter(inIndex++), aInstanceID));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndPrevious(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aNewPlayMode"></param>
        public void SyncSetPlayMode(uint aInstanceID, String aNewPlayMode)
        {
            SyncSetPlayModeUpnpOrgAVTransport1 sync = new SyncSetPlayModeUpnpOrgAVTransport1(this);
            BeginSetPlayMode(aInstanceID, aNewPlayMode, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndSetPlayMode().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aNewPlayMode"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginSetPlayMode(uint aInstanceID, String aNewPlayMode, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionSetPlayMode, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionSetPlayMode.InputParameter(inIndex++), aInstanceID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSetPlayMode.InputParameter(inIndex++), aNewPlayMode));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndSetPlayMode(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aActions"></param>
        public void SyncGetCurrentTransportActions(uint aInstanceID, out String aActions)
        {
            SyncGetCurrentTransportActionsUpnpOrgAVTransport1 sync = new SyncGetCurrentTransportActionsUpnpOrgAVTransport1(this);
            BeginGetCurrentTransportActions(aInstanceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aActions = sync.Actions();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetCurrentTransportActions().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetCurrentTransportActions(uint aInstanceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetCurrentTransportActions, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionGetCurrentTransportActions.InputParameter(inIndex++), aInstanceID));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetCurrentTransportActions.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aActions"></param>
        public void EndGetCurrentTransportActions(IntPtr aAsyncHandle, out String aActions)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aActions = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aTimeService"></param>
        /// <param name="aStartTime"></param>
        public void SyncSetNextStartTriggerTime(uint aInstanceID, String aTimeService, String aStartTime)
        {
            SyncSetNextStartTriggerTimeUpnpOrgAVTransport1 sync = new SyncSetNextStartTriggerTimeUpnpOrgAVTransport1(this);
            BeginSetNextStartTriggerTime(aInstanceID, aTimeService, aStartTime, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndSetNextStartTriggerTime().</remarks>
        /// <param name="aInstanceID"></param>
        /// <param name="aTimeService"></param>
        /// <param name="aStartTime"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginSetNextStartTriggerTime(uint aInstanceID, String aTimeService, String aStartTime, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionSetNextStartTriggerTime, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionSetNextStartTriggerTime.InputParameter(inIndex++), aInstanceID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSetNextStartTriggerTime.InputParameter(inIndex++), aTimeService));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSetNextStartTriggerTime.InputParameter(inIndex++), aStartTime));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndSetNextStartTriggerTime(IntPtr aAsyncHandle)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
        }

        /// <summary>
        /// Set a delegate to be run when the BufferFilled state variable changes.
        /// </summary>
        /// <remarks>Callbacks may be run in different threads but callbacks for a
        /// CpProxyUpnpOrgAVTransport1 instance will not overlap.</remarks>
        /// <param name="aBufferFilledChanged">The delegate to run when the state variable changes</param>
        public void SetPropertyBufferFilledChanged(System.Action aBufferFilledChanged)
        {
            lock (iPropertyLock)
            {
                iBufferFilledChanged = aBufferFilledChanged;
            }
        }

        private void BufferFilledPropertyChanged()
        {
            lock (iPropertyLock)
            {
                ReportEvent(iBufferFilledChanged);
            }
        }

        /// <summary>
        /// Set a delegate to be run when the LastChange state variable changes.
        /// </summary>
        /// <remarks>Callbacks may be run in different threads but callbacks for a
        /// CpProxyUpnpOrgAVTransport1 instance will not overlap.</remarks>
        /// <param name="aLastChangeChanged">The delegate to run when the state variable changes</param>
        public void SetPropertyLastChangeChanged(System.Action aLastChangeChanged)
        {
            lock (iPropertyLock)
            {
                iLastChangeChanged = aLastChangeChanged;
            }
        }

        private void LastChangePropertyChanged()
        {
            lock (iPropertyLock)
            {
                ReportEvent(iLastChangeChanged);
            }
        }

        /// <summary>
        /// Query the value of the BufferFilled property.
        /// </summary>
        /// <remarks>This function is threadsafe and can only be called if Subscribe() has been
        /// called and a first eventing callback received more recently than any call
        /// to Unsubscribe().</remarks>
        /// <returns>Value of the BufferFilled property</returns>
        public uint PropertyBufferFilled()
        {
            PropertyReadLock();
            uint val = iBufferFilled.Value();
            PropertyReadUnlock();
            return val;
        }

        /// <summary>
        /// Query the value of the LastChange property.
        /// </summary>
        /// <remarks>This function is threadsafe and can only be called if Subscribe() has been
        /// called and a first eventing callback received more recently than any call
        /// to Unsubscribe().</remarks>
        /// <returns>Value of the LastChange property</returns>
        public String PropertyLastChange()
        {
            PropertyReadLock();
            String val = iLastChange.Value();
            PropertyReadUnlock();
            return val;
        }

        /// <summary>
        /// Must be called for each class instance.  Must be called before Core.Library.Close().
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (iHandle == IntPtr.Zero)
                    return;
                DisposeProxy();
                iHandle = IntPtr.Zero;
            }
            iActionSetAVTransportURI.Dispose();
            iActionBendAVTransportURI.Dispose();
            iActionLoveCurrentTrack.Dispose();
            iActionBanCurrentTrack.Dispose();
            iActionSetResourceForCurrentStream.Dispose();
            iActionGetStreamProperties.Dispose();
            iActionPause.Dispose();
            iActionGetMediaInfo.Dispose();
            iActionGetTransportInfo.Dispose();
            iActionGetPositionInfo.Dispose();
            iActionGetDeviceCapabilities.Dispose();
            iActionGetTransportSettings.Dispose();
            iActionStop.Dispose();
            iActionPlay.Dispose();
            iActionSeek.Dispose();
            iActionNext.Dispose();
            iActionPrevious.Dispose();
            iActionSetPlayMode.Dispose();
            iActionGetCurrentTransportActions.Dispose();
            iActionSetNextStartTriggerTime.Dispose();
            iBufferFilled.Dispose();
            iLastChange.Dispose();
        }
    }
}

