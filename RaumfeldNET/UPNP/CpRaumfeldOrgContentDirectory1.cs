using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenHome.Net.Core;
using OpenHome.Net.ControlPoint;

namespace OpenHome.Net.ControlPoint.Proxies
{
    public interface ICpProxyUpnpOrgContentDirectory1 : ICpProxy, IDisposable
    {
        void SyncBrowse(String aObjectID, String aBrowseFlag, String aFilter, uint aStartingIndex, uint aRequestedCount, String aSortCriteria, out String aResult, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID);
        void BeginBrowse(String aObjectID, String aBrowseFlag, String aFilter, uint aStartingIndex, uint aRequestedCount, String aSortCriteria, CpProxy.CallbackAsyncComplete aCallback);
        void EndBrowse(IntPtr aAsyncHandle, out String aResult, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID);
        void SyncSearch(String aContainerID, String aSearchCriteria, String aFilter, uint aStartingIndex, uint aRequestedCount, String aSortCriteria, out String aResult, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID);
        void BeginSearch(String aContainerID, String aSearchCriteria, String aFilter, uint aStartingIndex, uint aRequestedCount, String aSortCriteria, CpProxy.CallbackAsyncComplete aCallback);
        void EndSearch(IntPtr aAsyncHandle, out String aResult, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID);
        void SyncShuffle(String aContainerID, String aSelection, out String aPlaylistID, out String aPlaylistMetadata);
        void BeginShuffle(String aContainerID, String aSelection, CpProxy.CallbackAsyncComplete aCallback);
        void EndShuffle(IntPtr aAsyncHandle, out String aPlaylistID, out String aPlaylistMetadata);
        void SyncGetSearchCapabilities(out String aSearchCaps);
        void BeginGetSearchCapabilities(CpProxy.CallbackAsyncComplete aCallback);
        void EndGetSearchCapabilities(IntPtr aAsyncHandle, out String aSearchCaps);
        void SyncGetSortCapabilities(out String aSortCaps);
        void BeginGetSortCapabilities(CpProxy.CallbackAsyncComplete aCallback);
        void EndGetSortCapabilities(IntPtr aAsyncHandle, out String aSortCaps);
        void SyncCreateReference(String aContainerID, String aObjectID, out String aNewID);
        void BeginCreateReference(String aContainerID, String aObjectID, CpProxy.CallbackAsyncComplete aCallback);
        void EndCreateReference(IntPtr aAsyncHandle, out String aNewID);
        void SyncAddContainerToQueue(String aQueueID, String aContainerID, String aSourceID, String aSearchCriteria, String aSortCriteria, uint aStartIndex, uint aEndIndex, uint aPosition);
        void BeginAddContainerToQueue(String aQueueID, String aContainerID, String aSourceID, String aSearchCriteria, String aSortCriteria, uint aStartIndex, uint aEndIndex, uint aPosition, CpProxy.CallbackAsyncComplete aCallback);
        void EndAddContainerToQueue(IntPtr aAsyncHandle);
        void SyncAddItemToQueue(String aQueueID, String aObjectID, uint aPosition);
        void BeginAddItemToQueue(String aQueueID, String aObjectID, uint aPosition, CpProxy.CallbackAsyncComplete aCallback);
        void EndAddItemToQueue(IntPtr aAsyncHandle);
        void SyncRemoveFromQueue(String aQueueID, uint aFromPosition, uint aToPosition, out uint aContainerUpdateID);
        void BeginRemoveFromQueue(String aQueueID, uint aFromPosition, uint aToPosition, CpProxy.CallbackAsyncComplete aCallback);
        void EndRemoveFromQueue(IntPtr aAsyncHandle, out uint aContainerUpdateID);
        void SyncCreateQueue(String aDesiredName, String aContainerID, out String aGivenName, out String aQueueID, out String aMetaData);
        void BeginCreateQueue(String aDesiredName, String aContainerID, CpProxy.CallbackAsyncComplete aCallback);
        void EndCreateQueue(IntPtr aAsyncHandle, out String aGivenName, out String aQueueID, out String aMetaData);
        void SyncRenameQueue(String aQueueID, String aDesiredName, out String aGivenName);
        void BeginRenameQueue(String aQueueID, String aDesiredName, CpProxy.CallbackAsyncComplete aCallback);
        void EndRenameQueue(IntPtr aAsyncHandle, out String aGivenName);
        void SyncMoveInQueue(String aObjectID, uint aNewPosition, out uint aContainerUpdateID);
        void BeginMoveInQueue(String aObjectID, uint aNewPosition, CpProxy.CallbackAsyncComplete aCallback);
        void EndMoveInQueue(IntPtr aAsyncHandle, out uint aContainerUpdateID);
        void SyncDestroyObject(String aObjectID);
        void BeginDestroyObject(String aObjectID, CpProxy.CallbackAsyncComplete aCallback);
        void EndDestroyObject(IntPtr aAsyncHandle);
        void SyncResetDatabase(String aScope);
        void BeginResetDatabase(String aScope, CpProxy.CallbackAsyncComplete aCallback);
        void EndResetDatabase(IntPtr aAsyncHandle);
        void SyncGetSystemUpdateID(out uint aId);
        void BeginGetSystemUpdateID(CpProxy.CallbackAsyncComplete aCallback);
        void EndGetSystemUpdateID(IntPtr aAsyncHandle, out uint aId);
        void SyncGetIndexerStatus(out String aStatus);
        void BeginGetIndexerStatus(CpProxy.CallbackAsyncComplete aCallback);
        void EndGetIndexerStatus(IntPtr aAsyncHandle, out String aStatus);
        void SyncGetSourceInfo(String aSourceID, out uint aNumTracks, out uint aTotalSize, out uint aTotalDuration, out uint aScanProgress, out String aIndexerResult);
        void BeginGetSourceInfo(String aSourceID, CpProxy.CallbackAsyncComplete aCallback);
        void EndGetSourceInfo(IntPtr aAsyncHandle, out uint aNumTracks, out uint aTotalSize, out uint aTotalDuration, out uint aScanProgress, out String aIndexerResult);
        void SyncRescanSource(String aSourceID, String aRescanMode);
        void BeginRescanSource(String aSourceID, String aRescanMode, CpProxy.CallbackAsyncComplete aCallback);
        void EndRescanSource(IntPtr aAsyncHandle);
        void SyncQueryDatabaseState(out uint aCurrentNumResources, out uint aCriticalNumResources, out uint aCurrentDiskUsage, out uint aMaxDiskUsage);
        void BeginQueryDatabaseState(CpProxy.CallbackAsyncComplete aCallback);
        void EndQueryDatabaseState(IntPtr aAsyncHandle, out uint aCurrentNumResources, out uint aCriticalNumResources, out uint aCurrentDiskUsage, out uint aMaxDiskUsage);
        void SetPropertySystemUpdateIDChanged(System.Action aSystemUpdateIDChanged);
        uint PropertySystemUpdateID();
        void SetPropertyContainerUpdateIDsChanged(System.Action aContainerUpdateIDsChanged);
        String PropertyContainerUpdateIDs();
        void SetPropertyIndexerStatusChanged(System.Action aIndexerStatusChanged);
        String PropertyIndexerStatus();
        void SetPropertyA_ARG_TYPE_ScopeChanged(System.Action aA_ARG_TYPE_ScopeChanged);
        String PropertyA_ARG_TYPE_Scope();
    }

    internal class SyncBrowseUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private String iResult;
        private uint iNumberReturned;
        private uint iTotalMatches;
        private uint iUpdateID;

        public SyncBrowseUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public String Result()
        {
            return iResult;
        }
        public uint NumberReturned()
        {
            return iNumberReturned;
        }
        public uint TotalMatches()
        {
            return iTotalMatches;
        }
        public uint UpdateID()
        {
            return iUpdateID;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndBrowse(aAsyncHandle, out iResult, out iNumberReturned, out iTotalMatches, out iUpdateID);
        }
    };

    internal class SyncSearchUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private String iResult;
        private uint iNumberReturned;
        private uint iTotalMatches;
        private uint iUpdateID;

        public SyncSearchUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public String Result()
        {
            return iResult;
        }
        public uint NumberReturned()
        {
            return iNumberReturned;
        }
        public uint TotalMatches()
        {
            return iTotalMatches;
        }
        public uint UpdateID()
        {
            return iUpdateID;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndSearch(aAsyncHandle, out iResult, out iNumberReturned, out iTotalMatches, out iUpdateID);
        }
    };

    internal class SyncShuffleUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private String iPlaylistID;
        private String iPlaylistMetadata;

        public SyncShuffleUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public String PlaylistID()
        {
            return iPlaylistID;
        }
        public String PlaylistMetadata()
        {
            return iPlaylistMetadata;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndShuffle(aAsyncHandle, out iPlaylistID, out iPlaylistMetadata);
        }
    };

    internal class SyncGetSearchCapabilitiesUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private String iSearchCaps;

        public SyncGetSearchCapabilitiesUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public String SearchCaps()
        {
            return iSearchCaps;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetSearchCapabilities(aAsyncHandle, out iSearchCaps);
        }
    };

    internal class SyncGetSortCapabilitiesUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private String iSortCaps;

        public SyncGetSortCapabilitiesUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public String SortCaps()
        {
            return iSortCaps;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetSortCapabilities(aAsyncHandle, out iSortCaps);
        }
    };

    internal class SyncCreateReferenceUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private String iNewID;

        public SyncCreateReferenceUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public String NewID()
        {
            return iNewID;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndCreateReference(aAsyncHandle, out iNewID);
        }
    };

    internal class SyncAddContainerToQueueUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;

        public SyncAddContainerToQueueUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndAddContainerToQueue(aAsyncHandle);
        }
    };

    internal class SyncAddItemToQueueUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;

        public SyncAddItemToQueueUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndAddItemToQueue(aAsyncHandle);
        }
    };

    internal class SyncRemoveFromQueueUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private uint iContainerUpdateID;

        public SyncRemoveFromQueueUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public uint ContainerUpdateID()
        {
            return iContainerUpdateID;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndRemoveFromQueue(aAsyncHandle, out iContainerUpdateID);
        }
    };

    internal class SyncCreateQueueUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private String iGivenName;
        private String iQueueID;
        private String iMetaData;

        public SyncCreateQueueUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public String GivenName()
        {
            return iGivenName;
        }
        public String QueueID()
        {
            return iQueueID;
        }
        public String MetaData()
        {
            return iMetaData;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndCreateQueue(aAsyncHandle, out iGivenName, out iQueueID, out iMetaData);
        }
    };

    internal class SyncRenameQueueUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private String iGivenName;

        public SyncRenameQueueUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public String GivenName()
        {
            return iGivenName;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndRenameQueue(aAsyncHandle, out iGivenName);
        }
    };

    internal class SyncMoveInQueueUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private uint iContainerUpdateID;

        public SyncMoveInQueueUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public uint ContainerUpdateID()
        {
            return iContainerUpdateID;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndMoveInQueue(aAsyncHandle, out iContainerUpdateID);
        }
    };

    internal class SyncDestroyObjectUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;

        public SyncDestroyObjectUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndDestroyObject(aAsyncHandle);
        }
    };

    internal class SyncResetDatabaseUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;

        public SyncResetDatabaseUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndResetDatabase(aAsyncHandle);
        }
    };

    internal class SyncGetSystemUpdateIDUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private uint iId;

        public SyncGetSystemUpdateIDUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public uint Id()
        {
            return iId;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetSystemUpdateID(aAsyncHandle, out iId);
        }
    };

    internal class SyncGetIndexerStatusUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private String iStatus;

        public SyncGetIndexerStatusUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public String Status()
        {
            return iStatus;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetIndexerStatus(aAsyncHandle, out iStatus);
        }
    };

    internal class SyncGetSourceInfoUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private uint iNumTracks;
        private uint iTotalSize;
        private uint iTotalDuration;
        private uint iScanProgress;
        private String iIndexerResult;

        public SyncGetSourceInfoUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public uint NumTracks()
        {
            return iNumTracks;
        }
        public uint TotalSize()
        {
            return iTotalSize;
        }
        public uint TotalDuration()
        {
            return iTotalDuration;
        }
        public uint ScanProgress()
        {
            return iScanProgress;
        }
        public String IndexerResult()
        {
            return iIndexerResult;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndGetSourceInfo(aAsyncHandle, out iNumTracks, out iTotalSize, out iTotalDuration, out iScanProgress, out iIndexerResult);
        }
    };

    internal class SyncRescanSourceUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;

        public SyncRescanSourceUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndRescanSource(aAsyncHandle);
        }
    };

    internal class SyncQueryDatabaseStateUpnpOrgContentDirectory1 : SyncProxyAction
    {
        private CpProxyUpnpOrgContentDirectory1 iService;
        private uint iCurrentNumResources;
        private uint iCriticalNumResources;
        private uint iCurrentDiskUsage;
        private uint iMaxDiskUsage;

        public SyncQueryDatabaseStateUpnpOrgContentDirectory1(CpProxyUpnpOrgContentDirectory1 aProxy)
        {
            iService = aProxy;
        }
        public uint CurrentNumResources()
        {
            return iCurrentNumResources;
        }
        public uint CriticalNumResources()
        {
            return iCriticalNumResources;
        }
        public uint CurrentDiskUsage()
        {
            return iCurrentDiskUsage;
        }
        public uint MaxDiskUsage()
        {
            return iMaxDiskUsage;
        }
        protected override void CompleteRequest(IntPtr aAsyncHandle)
        {
            iService.EndQueryDatabaseState(aAsyncHandle, out iCurrentNumResources, out iCriticalNumResources, out iCurrentDiskUsage, out iMaxDiskUsage);
        }
    };

    /// <summary>
    /// Proxy for the upnp.org:ContentDirectory:1 UPnP service
    /// </summary>
    public class CpProxyUpnpOrgContentDirectory1 : CpProxy, IDisposable, ICpProxyUpnpOrgContentDirectory1
    {
        private OpenHome.Net.Core.Action iActionBrowse;
        private OpenHome.Net.Core.Action iActionSearch;
        private OpenHome.Net.Core.Action iActionShuffle;
        private OpenHome.Net.Core.Action iActionGetSearchCapabilities;
        private OpenHome.Net.Core.Action iActionGetSortCapabilities;
        private OpenHome.Net.Core.Action iActionCreateReference;
        private OpenHome.Net.Core.Action iActionAddContainerToQueue;
        private OpenHome.Net.Core.Action iActionAddItemToQueue;
        private OpenHome.Net.Core.Action iActionRemoveFromQueue;
        private OpenHome.Net.Core.Action iActionCreateQueue;
        private OpenHome.Net.Core.Action iActionRenameQueue;
        private OpenHome.Net.Core.Action iActionMoveInQueue;
        private OpenHome.Net.Core.Action iActionDestroyObject;
        private OpenHome.Net.Core.Action iActionResetDatabase;
        private OpenHome.Net.Core.Action iActionGetSystemUpdateID;
        private OpenHome.Net.Core.Action iActionGetIndexerStatus;
        private OpenHome.Net.Core.Action iActionGetSourceInfo;
        private OpenHome.Net.Core.Action iActionRescanSource;
        private OpenHome.Net.Core.Action iActionQueryDatabaseState;
        private PropertyUint iSystemUpdateID;
        private PropertyString iContainerUpdateIDs;
        private PropertyString iIndexerStatus;
        private PropertyString iA_ARG_TYPE_Scope;
        private System.Action iSystemUpdateIDChanged;
        private System.Action iContainerUpdateIDsChanged;
        private System.Action iIndexerStatusChanged;
        private System.Action iA_ARG_TYPE_ScopeChanged;
        private Mutex iPropertyLock;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>Use CpProxy::[Un]Subscribe() to enable/disable querying of state variable and reporting of their changes.</remarks>
        /// <param name="aDevice">The device to use</param>
        public CpProxyUpnpOrgContentDirectory1(CpDevice aDevice)
            : base("schemas-upnp-org", "ContentDirectory", 1, aDevice)
        {
            OpenHome.Net.Core.Parameter param;
            List<String> allowedValues = new List<String>();

            iActionBrowse = new OpenHome.Net.Core.Action("Browse");
            param = new ParameterString("ObjectID", allowedValues);
            iActionBrowse.AddInputParameter(param);
            allowedValues.Add("BrowseMetadata");
            allowedValues.Add("BrowseDirectChildren");
            param = new ParameterString("BrowseFlag", allowedValues);
            iActionBrowse.AddInputParameter(param);
            allowedValues.Clear();
            param = new ParameterString("Filter", allowedValues);
            iActionBrowse.AddInputParameter(param);
            param = new ParameterUint("StartingIndex");
            iActionBrowse.AddInputParameter(param);
            param = new ParameterUint("RequestedCount");
            iActionBrowse.AddInputParameter(param);
            param = new ParameterString("SortCriteria", allowedValues);
            iActionBrowse.AddInputParameter(param);
            param = new ParameterString("Result", allowedValues);
            iActionBrowse.AddOutputParameter(param);
            param = new ParameterUint("NumberReturned");
            iActionBrowse.AddOutputParameter(param);
            param = new ParameterUint("TotalMatches");
            iActionBrowse.AddOutputParameter(param);
            param = new ParameterUint("UpdateID");
            iActionBrowse.AddOutputParameter(param);

            iActionSearch = new OpenHome.Net.Core.Action("Search");
            param = new ParameterString("ContainerID", allowedValues);
            iActionSearch.AddInputParameter(param);
            param = new ParameterString("SearchCriteria", allowedValues);
            iActionSearch.AddInputParameter(param);
            param = new ParameterString("Filter", allowedValues);
            iActionSearch.AddInputParameter(param);
            param = new ParameterUint("StartingIndex");
            iActionSearch.AddInputParameter(param);
            param = new ParameterUint("RequestedCount");
            iActionSearch.AddInputParameter(param);
            param = new ParameterString("SortCriteria", allowedValues);
            iActionSearch.AddInputParameter(param);
            param = new ParameterString("Result", allowedValues);
            iActionSearch.AddOutputParameter(param);
            param = new ParameterUint("NumberReturned");
            iActionSearch.AddOutputParameter(param);
            param = new ParameterUint("TotalMatches");
            iActionSearch.AddOutputParameter(param);
            param = new ParameterUint("UpdateID");
            iActionSearch.AddOutputParameter(param);

            iActionShuffle = new OpenHome.Net.Core.Action("Shuffle");
            param = new ParameterString("ContainerID", allowedValues);
            iActionShuffle.AddInputParameter(param);
            param = new ParameterString("Selection", allowedValues);
            iActionShuffle.AddInputParameter(param);
            param = new ParameterString("PlaylistID", allowedValues);
            iActionShuffle.AddOutputParameter(param);
            param = new ParameterString("PlaylistMetadata", allowedValues);
            iActionShuffle.AddOutputParameter(param);

            iActionGetSearchCapabilities = new OpenHome.Net.Core.Action("GetSearchCapabilities");
            param = new ParameterString("SearchCaps", allowedValues);
            iActionGetSearchCapabilities.AddOutputParameter(param);

            iActionGetSortCapabilities = new OpenHome.Net.Core.Action("GetSortCapabilities");
            param = new ParameterString("SortCaps", allowedValues);
            iActionGetSortCapabilities.AddOutputParameter(param);

            iActionCreateReference = new OpenHome.Net.Core.Action("CreateReference");
            param = new ParameterString("ContainerID", allowedValues);
            iActionCreateReference.AddInputParameter(param);
            param = new ParameterString("ObjectID", allowedValues);
            iActionCreateReference.AddInputParameter(param);
            param = new ParameterString("NewID", allowedValues);
            iActionCreateReference.AddOutputParameter(param);

            iActionAddContainerToQueue = new OpenHome.Net.Core.Action("AddContainerToQueue");
            param = new ParameterString("QueueID", allowedValues);
            iActionAddContainerToQueue.AddInputParameter(param);
            param = new ParameterString("ContainerID", allowedValues);
            iActionAddContainerToQueue.AddInputParameter(param);
            param = new ParameterString("SourceID", allowedValues);
            iActionAddContainerToQueue.AddInputParameter(param);
            param = new ParameterString("SearchCriteria", allowedValues);
            iActionAddContainerToQueue.AddInputParameter(param);
            param = new ParameterString("SortCriteria", allowedValues);
            iActionAddContainerToQueue.AddInputParameter(param);
            param = new ParameterUint("StartIndex");
            iActionAddContainerToQueue.AddInputParameter(param);
            param = new ParameterUint("EndIndex");
            iActionAddContainerToQueue.AddInputParameter(param);
            param = new ParameterUint("Position");
            iActionAddContainerToQueue.AddInputParameter(param);

            iActionAddItemToQueue = new OpenHome.Net.Core.Action("AddItemToQueue");
            param = new ParameterString("QueueID", allowedValues);
            iActionAddItemToQueue.AddInputParameter(param);
            param = new ParameterString("ObjectID", allowedValues);
            iActionAddItemToQueue.AddInputParameter(param);
            param = new ParameterUint("Position");
            iActionAddItemToQueue.AddInputParameter(param);

            iActionRemoveFromQueue = new OpenHome.Net.Core.Action("RemoveFromQueue");
            param = new ParameterString("QueueID", allowedValues);
            iActionRemoveFromQueue.AddInputParameter(param);
            param = new ParameterUint("FromPosition");
            iActionRemoveFromQueue.AddInputParameter(param);
            param = new ParameterUint("ToPosition");
            iActionRemoveFromQueue.AddInputParameter(param);
            param = new ParameterUint("ContainerUpdateID");
            iActionRemoveFromQueue.AddOutputParameter(param);

            iActionCreateQueue = new OpenHome.Net.Core.Action("CreateQueue");
            param = new ParameterString("DesiredName", allowedValues);
            iActionCreateQueue.AddInputParameter(param);
            param = new ParameterString("ContainerID", allowedValues);
            iActionCreateQueue.AddInputParameter(param);
            param = new ParameterString("GivenName", allowedValues);
            iActionCreateQueue.AddOutputParameter(param);
            param = new ParameterString("QueueID", allowedValues);
            iActionCreateQueue.AddOutputParameter(param);
            param = new ParameterString("MetaData", allowedValues);
            iActionCreateQueue.AddOutputParameter(param);

            iActionRenameQueue = new OpenHome.Net.Core.Action("RenameQueue");
            param = new ParameterString("QueueID", allowedValues);
            iActionRenameQueue.AddInputParameter(param);
            param = new ParameterString("DesiredName", allowedValues);
            iActionRenameQueue.AddInputParameter(param);
            param = new ParameterString("GivenName", allowedValues);
            iActionRenameQueue.AddOutputParameter(param);

            iActionMoveInQueue = new OpenHome.Net.Core.Action("MoveInQueue");
            param = new ParameterString("ObjectID", allowedValues);
            iActionMoveInQueue.AddInputParameter(param);
            param = new ParameterUint("NewPosition");
            iActionMoveInQueue.AddInputParameter(param);
            param = new ParameterUint("ContainerUpdateID");
            iActionMoveInQueue.AddOutputParameter(param);

            iActionDestroyObject = new OpenHome.Net.Core.Action("DestroyObject");
            param = new ParameterString("ObjectID", allowedValues);
            iActionDestroyObject.AddInputParameter(param);

            iActionResetDatabase = new OpenHome.Net.Core.Action("ResetDatabase");
            allowedValues.Add("RESET_ALL");
            allowedValues.Add("RESET_USERDATA");
            allowedValues.Add("RESET_INDEX");
            param = new ParameterString("Scope", allowedValues);
            iActionResetDatabase.AddInputParameter(param);
            allowedValues.Clear();

            iActionGetSystemUpdateID = new OpenHome.Net.Core.Action("GetSystemUpdateID");
            param = new ParameterUint("Id");
            iActionGetSystemUpdateID.AddOutputParameter(param);

            iActionGetIndexerStatus = new OpenHome.Net.Core.Action("GetIndexerStatus");
            param = new ParameterString("Status", allowedValues);
            iActionGetIndexerStatus.AddOutputParameter(param);

            iActionGetSourceInfo = new OpenHome.Net.Core.Action("GetSourceInfo");
            param = new ParameterString("SourceID", allowedValues);
            iActionGetSourceInfo.AddInputParameter(param);
            param = new ParameterUint("NumTracks");
            iActionGetSourceInfo.AddOutputParameter(param);
            param = new ParameterUint("TotalSize");
            iActionGetSourceInfo.AddOutputParameter(param);
            param = new ParameterUint("TotalDuration");
            iActionGetSourceInfo.AddOutputParameter(param);
            param = new ParameterUint("ScanProgress");
            iActionGetSourceInfo.AddOutputParameter(param);
            allowedValues.Add("NONE");
            allowedValues.Add("SUCCESS");
            allowedValues.Add("MOUNTING_FAILED");
            allowedValues.Add("UNREACHABLE");
            param = new ParameterString("IndexerResult", allowedValues);
            iActionGetSourceInfo.AddOutputParameter(param);
            allowedValues.Clear();

            iActionRescanSource = new OpenHome.Net.Core.Action("RescanSource");
            param = new ParameterString("SourceID", allowedValues);
            iActionRescanSource.AddInputParameter(param);
            allowedValues.Add("FindNewTracks");
            allowedValues.Add("DoFullRescan");
            param = new ParameterString("RescanMode", allowedValues);
            iActionRescanSource.AddInputParameter(param);
            allowedValues.Clear();

            iActionQueryDatabaseState = new OpenHome.Net.Core.Action("QueryDatabaseState");
            param = new ParameterUint("CurrentNumResources");
            iActionQueryDatabaseState.AddOutputParameter(param);
            param = new ParameterUint("CriticalNumResources");
            iActionQueryDatabaseState.AddOutputParameter(param);
            param = new ParameterUint("CurrentDiskUsage");
            iActionQueryDatabaseState.AddOutputParameter(param);
            param = new ParameterUint("MaxDiskUsage");
            iActionQueryDatabaseState.AddOutputParameter(param);

            iSystemUpdateID = new PropertyUint("SystemUpdateID", SystemUpdateIDPropertyChanged);
            AddProperty(iSystemUpdateID);
            iContainerUpdateIDs = new PropertyString("ContainerUpdateIDs", ContainerUpdateIDsPropertyChanged);
            AddProperty(iContainerUpdateIDs);
            iIndexerStatus = new PropertyString("IndexerStatus", IndexerStatusPropertyChanged);
            AddProperty(iIndexerStatus);
            iA_ARG_TYPE_Scope = new PropertyString("A_ARG_TYPE_Scope", A_ARG_TYPE_ScopePropertyChanged);
            AddProperty(iA_ARG_TYPE_Scope);
            
            iPropertyLock = new Mutex();
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aObjectID"></param>
        /// <param name="aBrowseFlag"></param>
        /// <param name="aFilter"></param>
        /// <param name="aStartingIndex"></param>
        /// <param name="aRequestedCount"></param>
        /// <param name="aSortCriteria"></param>
        /// <param name="aResult"></param>
        /// <param name="aNumberReturned"></param>
        /// <param name="aTotalMatches"></param>
        /// <param name="aUpdateID"></param>
        public void SyncBrowse(String aObjectID, String aBrowseFlag, String aFilter, uint aStartingIndex, uint aRequestedCount, String aSortCriteria, out String aResult, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID)
        {
            SyncBrowseUpnpOrgContentDirectory1 sync = new SyncBrowseUpnpOrgContentDirectory1(this);
            BeginBrowse(aObjectID, aBrowseFlag, aFilter, aStartingIndex, aRequestedCount, aSortCriteria, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aResult = sync.Result();
            aNumberReturned = sync.NumberReturned();
            aTotalMatches = sync.TotalMatches();
            aUpdateID = sync.UpdateID();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndBrowse().</remarks>
        /// <param name="aObjectID"></param>
        /// <param name="aBrowseFlag"></param>
        /// <param name="aFilter"></param>
        /// <param name="aStartingIndex"></param>
        /// <param name="aRequestedCount"></param>
        /// <param name="aSortCriteria"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginBrowse(String aObjectID, String aBrowseFlag, String aFilter, uint aStartingIndex, uint aRequestedCount, String aSortCriteria, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionBrowse, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionBrowse.InputParameter(inIndex++), aObjectID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionBrowse.InputParameter(inIndex++), aBrowseFlag));
            invocation.AddInput(new ArgumentString((ParameterString)iActionBrowse.InputParameter(inIndex++), aFilter));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionBrowse.InputParameter(inIndex++), aStartingIndex));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionBrowse.InputParameter(inIndex++), aRequestedCount));
            invocation.AddInput(new ArgumentString((ParameterString)iActionBrowse.InputParameter(inIndex++), aSortCriteria));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionBrowse.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionBrowse.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionBrowse.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionBrowse.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aResult"></param>
        /// <param name="aNumberReturned"></param>
        /// <param name="aTotalMatches"></param>
        /// <param name="aUpdateID"></param>
        public void EndBrowse(IntPtr aAsyncHandle, out String aResult, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aResult = Invocation.OutputString(aAsyncHandle, index++);
            aNumberReturned = Invocation.OutputUint(aAsyncHandle, index++);
            aTotalMatches = Invocation.OutputUint(aAsyncHandle, index++);
            aUpdateID = Invocation.OutputUint(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aContainerID"></param>
        /// <param name="aSearchCriteria"></param>
        /// <param name="aFilter"></param>
        /// <param name="aStartingIndex"></param>
        /// <param name="aRequestedCount"></param>
        /// <param name="aSortCriteria"></param>
        /// <param name="aResult"></param>
        /// <param name="aNumberReturned"></param>
        /// <param name="aTotalMatches"></param>
        /// <param name="aUpdateID"></param>
        public void SyncSearch(String aContainerID, String aSearchCriteria, String aFilter, uint aStartingIndex, uint aRequestedCount, String aSortCriteria, out String aResult, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID)
        {
            SyncSearchUpnpOrgContentDirectory1 sync = new SyncSearchUpnpOrgContentDirectory1(this);
            BeginSearch(aContainerID, aSearchCriteria, aFilter, aStartingIndex, aRequestedCount, aSortCriteria, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aResult = sync.Result();
            aNumberReturned = sync.NumberReturned();
            aTotalMatches = sync.TotalMatches();
            aUpdateID = sync.UpdateID();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndSearch().</remarks>
        /// <param name="aContainerID"></param>
        /// <param name="aSearchCriteria"></param>
        /// <param name="aFilter"></param>
        /// <param name="aStartingIndex"></param>
        /// <param name="aRequestedCount"></param>
        /// <param name="aSortCriteria"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginSearch(String aContainerID, String aSearchCriteria, String aFilter, uint aStartingIndex, uint aRequestedCount, String aSortCriteria, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionSearch, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionSearch.InputParameter(inIndex++), aContainerID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSearch.InputParameter(inIndex++), aSearchCriteria));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSearch.InputParameter(inIndex++), aFilter));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionSearch.InputParameter(inIndex++), aStartingIndex));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionSearch.InputParameter(inIndex++), aRequestedCount));
            invocation.AddInput(new ArgumentString((ParameterString)iActionSearch.InputParameter(inIndex++), aSortCriteria));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionSearch.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionSearch.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionSearch.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionSearch.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aResult"></param>
        /// <param name="aNumberReturned"></param>
        /// <param name="aTotalMatches"></param>
        /// <param name="aUpdateID"></param>
        public void EndSearch(IntPtr aAsyncHandle, out String aResult, out uint aNumberReturned, out uint aTotalMatches, out uint aUpdateID)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aResult = Invocation.OutputString(aAsyncHandle, index++);
            aNumberReturned = Invocation.OutputUint(aAsyncHandle, index++);
            aTotalMatches = Invocation.OutputUint(aAsyncHandle, index++);
            aUpdateID = Invocation.OutputUint(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aContainerID"></param>
        /// <param name="aSelection"></param>
        /// <param name="aPlaylistID"></param>
        /// <param name="aPlaylistMetadata"></param>
        public void SyncShuffle(String aContainerID, String aSelection, out String aPlaylistID, out String aPlaylistMetadata)
        {
            SyncShuffleUpnpOrgContentDirectory1 sync = new SyncShuffleUpnpOrgContentDirectory1(this);
            BeginShuffle(aContainerID, aSelection, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aPlaylistID = sync.PlaylistID();
            aPlaylistMetadata = sync.PlaylistMetadata();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndShuffle().</remarks>
        /// <param name="aContainerID"></param>
        /// <param name="aSelection"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginShuffle(String aContainerID, String aSelection, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionShuffle, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionShuffle.InputParameter(inIndex++), aContainerID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionShuffle.InputParameter(inIndex++), aSelection));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionShuffle.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionShuffle.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aPlaylistID"></param>
        /// <param name="aPlaylistMetadata"></param>
        public void EndShuffle(IntPtr aAsyncHandle, out String aPlaylistID, out String aPlaylistMetadata)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aPlaylistID = Invocation.OutputString(aAsyncHandle, index++);
            aPlaylistMetadata = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aSearchCaps"></param>
        public void SyncGetSearchCapabilities(out String aSearchCaps)
        {
            SyncGetSearchCapabilitiesUpnpOrgContentDirectory1 sync = new SyncGetSearchCapabilitiesUpnpOrgContentDirectory1(this);
            BeginGetSearchCapabilities(sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aSearchCaps = sync.SearchCaps();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetSearchCapabilities().</remarks>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetSearchCapabilities(CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetSearchCapabilities, aCallback);
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetSearchCapabilities.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aSearchCaps"></param>
        public void EndGetSearchCapabilities(IntPtr aAsyncHandle, out String aSearchCaps)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aSearchCaps = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aSortCaps"></param>
        public void SyncGetSortCapabilities(out String aSortCaps)
        {
            SyncGetSortCapabilitiesUpnpOrgContentDirectory1 sync = new SyncGetSortCapabilitiesUpnpOrgContentDirectory1(this);
            BeginGetSortCapabilities(sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aSortCaps = sync.SortCaps();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetSortCapabilities().</remarks>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetSortCapabilities(CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetSortCapabilities, aCallback);
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetSortCapabilities.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aSortCaps"></param>
        public void EndGetSortCapabilities(IntPtr aAsyncHandle, out String aSortCaps)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aSortCaps = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aContainerID"></param>
        /// <param name="aObjectID"></param>
        /// <param name="aNewID"></param>
        public void SyncCreateReference(String aContainerID, String aObjectID, out String aNewID)
        {
            SyncCreateReferenceUpnpOrgContentDirectory1 sync = new SyncCreateReferenceUpnpOrgContentDirectory1(this);
            BeginCreateReference(aContainerID, aObjectID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aNewID = sync.NewID();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndCreateReference().</remarks>
        /// <param name="aContainerID"></param>
        /// <param name="aObjectID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginCreateReference(String aContainerID, String aObjectID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionCreateReference, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionCreateReference.InputParameter(inIndex++), aContainerID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionCreateReference.InputParameter(inIndex++), aObjectID));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionCreateReference.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aNewID"></param>
        public void EndCreateReference(IntPtr aAsyncHandle, out String aNewID)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aNewID = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aQueueID"></param>
        /// <param name="aContainerID"></param>
        /// <param name="aSourceID"></param>
        /// <param name="aSearchCriteria"></param>
        /// <param name="aSortCriteria"></param>
        /// <param name="aStartIndex"></param>
        /// <param name="aEndIndex"></param>
        /// <param name="aPosition"></param>
        public void SyncAddContainerToQueue(String aQueueID, String aContainerID, String aSourceID, String aSearchCriteria, String aSortCriteria, uint aStartIndex, uint aEndIndex, uint aPosition)
        {
            SyncAddContainerToQueueUpnpOrgContentDirectory1 sync = new SyncAddContainerToQueueUpnpOrgContentDirectory1(this);
            BeginAddContainerToQueue(aQueueID, aContainerID, aSourceID, aSearchCriteria, aSortCriteria, aStartIndex, aEndIndex, aPosition, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndAddContainerToQueue().</remarks>
        /// <param name="aQueueID"></param>
        /// <param name="aContainerID"></param>
        /// <param name="aSourceID"></param>
        /// <param name="aSearchCriteria"></param>
        /// <param name="aSortCriteria"></param>
        /// <param name="aStartIndex"></param>
        /// <param name="aEndIndex"></param>
        /// <param name="aPosition"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginAddContainerToQueue(String aQueueID, String aContainerID, String aSourceID, String aSearchCriteria, String aSortCriteria, uint aStartIndex, uint aEndIndex, uint aPosition, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionAddContainerToQueue, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionAddContainerToQueue.InputParameter(inIndex++), aQueueID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionAddContainerToQueue.InputParameter(inIndex++), aContainerID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionAddContainerToQueue.InputParameter(inIndex++), aSourceID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionAddContainerToQueue.InputParameter(inIndex++), aSearchCriteria));
            invocation.AddInput(new ArgumentString((ParameterString)iActionAddContainerToQueue.InputParameter(inIndex++), aSortCriteria));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionAddContainerToQueue.InputParameter(inIndex++), aStartIndex));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionAddContainerToQueue.InputParameter(inIndex++), aEndIndex));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionAddContainerToQueue.InputParameter(inIndex++), aPosition));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndAddContainerToQueue(IntPtr aAsyncHandle)
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
        /// <param name="aQueueID"></param>
        /// <param name="aObjectID"></param>
        /// <param name="aPosition"></param>
        public void SyncAddItemToQueue(String aQueueID, String aObjectID, uint aPosition)
        {
            SyncAddItemToQueueUpnpOrgContentDirectory1 sync = new SyncAddItemToQueueUpnpOrgContentDirectory1(this);
            BeginAddItemToQueue(aQueueID, aObjectID, aPosition, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndAddItemToQueue().</remarks>
        /// <param name="aQueueID"></param>
        /// <param name="aObjectID"></param>
        /// <param name="aPosition"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginAddItemToQueue(String aQueueID, String aObjectID, uint aPosition, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionAddItemToQueue, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionAddItemToQueue.InputParameter(inIndex++), aQueueID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionAddItemToQueue.InputParameter(inIndex++), aObjectID));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionAddItemToQueue.InputParameter(inIndex++), aPosition));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndAddItemToQueue(IntPtr aAsyncHandle)
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
        /// <param name="aQueueID"></param>
        /// <param name="aFromPosition"></param>
        /// <param name="aToPosition"></param>
        /// <param name="aContainerUpdateID"></param>
        public void SyncRemoveFromQueue(String aQueueID, uint aFromPosition, uint aToPosition, out uint aContainerUpdateID)
        {
            SyncRemoveFromQueueUpnpOrgContentDirectory1 sync = new SyncRemoveFromQueueUpnpOrgContentDirectory1(this);
            BeginRemoveFromQueue(aQueueID, aFromPosition, aToPosition, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aContainerUpdateID = sync.ContainerUpdateID();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndRemoveFromQueue().</remarks>
        /// <param name="aQueueID"></param>
        /// <param name="aFromPosition"></param>
        /// <param name="aToPosition"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginRemoveFromQueue(String aQueueID, uint aFromPosition, uint aToPosition, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionRemoveFromQueue, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionRemoveFromQueue.InputParameter(inIndex++), aQueueID));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionRemoveFromQueue.InputParameter(inIndex++), aFromPosition));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionRemoveFromQueue.InputParameter(inIndex++), aToPosition));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionRemoveFromQueue.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aContainerUpdateID"></param>
        public void EndRemoveFromQueue(IntPtr aAsyncHandle, out uint aContainerUpdateID)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aContainerUpdateID = Invocation.OutputUint(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aDesiredName"></param>
        /// <param name="aContainerID"></param>
        /// <param name="aGivenName"></param>
        /// <param name="aQueueID"></param>
        /// <param name="aMetaData"></param>
        public void SyncCreateQueue(String aDesiredName, String aContainerID, out String aGivenName, out String aQueueID, out String aMetaData)
        {
            SyncCreateQueueUpnpOrgContentDirectory1 sync = new SyncCreateQueueUpnpOrgContentDirectory1(this);
            BeginCreateQueue(aDesiredName, aContainerID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aGivenName = sync.GivenName();
            aQueueID = sync.QueueID();
            aMetaData = sync.MetaData();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndCreateQueue().</remarks>
        /// <param name="aDesiredName"></param>
        /// <param name="aContainerID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginCreateQueue(String aDesiredName, String aContainerID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionCreateQueue, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionCreateQueue.InputParameter(inIndex++), aDesiredName));
            invocation.AddInput(new ArgumentString((ParameterString)iActionCreateQueue.InputParameter(inIndex++), aContainerID));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionCreateQueue.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionCreateQueue.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionCreateQueue.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aGivenName"></param>
        /// <param name="aQueueID"></param>
        /// <param name="aMetaData"></param>
        public void EndCreateQueue(IntPtr aAsyncHandle, out String aGivenName, out String aQueueID, out String aMetaData)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aGivenName = Invocation.OutputString(aAsyncHandle, index++);
            aQueueID = Invocation.OutputString(aAsyncHandle, index++);
            aMetaData = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aQueueID"></param>
        /// <param name="aDesiredName"></param>
        /// <param name="aGivenName"></param>
        public void SyncRenameQueue(String aQueueID, String aDesiredName, out String aGivenName)
        {
            SyncRenameQueueUpnpOrgContentDirectory1 sync = new SyncRenameQueueUpnpOrgContentDirectory1(this);
            BeginRenameQueue(aQueueID, aDesiredName, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aGivenName = sync.GivenName();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndRenameQueue().</remarks>
        /// <param name="aQueueID"></param>
        /// <param name="aDesiredName"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginRenameQueue(String aQueueID, String aDesiredName, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionRenameQueue, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionRenameQueue.InputParameter(inIndex++), aQueueID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionRenameQueue.InputParameter(inIndex++), aDesiredName));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionRenameQueue.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aGivenName"></param>
        public void EndRenameQueue(IntPtr aAsyncHandle, out String aGivenName)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aGivenName = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aObjectID"></param>
        /// <param name="aNewPosition"></param>
        /// <param name="aContainerUpdateID"></param>
        public void SyncMoveInQueue(String aObjectID, uint aNewPosition, out uint aContainerUpdateID)
        {
            SyncMoveInQueueUpnpOrgContentDirectory1 sync = new SyncMoveInQueueUpnpOrgContentDirectory1(this);
            BeginMoveInQueue(aObjectID, aNewPosition, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aContainerUpdateID = sync.ContainerUpdateID();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndMoveInQueue().</remarks>
        /// <param name="aObjectID"></param>
        /// <param name="aNewPosition"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginMoveInQueue(String aObjectID, uint aNewPosition, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionMoveInQueue, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionMoveInQueue.InputParameter(inIndex++), aObjectID));
            invocation.AddInput(new ArgumentUint((ParameterUint)iActionMoveInQueue.InputParameter(inIndex++), aNewPosition));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionMoveInQueue.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aContainerUpdateID"></param>
        public void EndMoveInQueue(IntPtr aAsyncHandle, out uint aContainerUpdateID)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aContainerUpdateID = Invocation.OutputUint(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aObjectID"></param>
        public void SyncDestroyObject(String aObjectID)
        {
            SyncDestroyObjectUpnpOrgContentDirectory1 sync = new SyncDestroyObjectUpnpOrgContentDirectory1(this);
            BeginDestroyObject(aObjectID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndDestroyObject().</remarks>
        /// <param name="aObjectID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginDestroyObject(String aObjectID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionDestroyObject, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionDestroyObject.InputParameter(inIndex++), aObjectID));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndDestroyObject(IntPtr aAsyncHandle)
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
        /// <param name="aScope"></param>
        public void SyncResetDatabase(String aScope)
        {
            SyncResetDatabaseUpnpOrgContentDirectory1 sync = new SyncResetDatabaseUpnpOrgContentDirectory1(this);
            BeginResetDatabase(aScope, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndResetDatabase().</remarks>
        /// <param name="aScope"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginResetDatabase(String aScope, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionResetDatabase, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionResetDatabase.InputParameter(inIndex++), aScope));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndResetDatabase(IntPtr aAsyncHandle)
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
        /// <param name="aId"></param>
        public void SyncGetSystemUpdateID(out uint aId)
        {
            SyncGetSystemUpdateIDUpnpOrgContentDirectory1 sync = new SyncGetSystemUpdateIDUpnpOrgContentDirectory1(this);
            BeginGetSystemUpdateID(sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aId = sync.Id();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetSystemUpdateID().</remarks>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetSystemUpdateID(CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetSystemUpdateID, aCallback);
            int outIndex = 0;
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionGetSystemUpdateID.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aId"></param>
        public void EndGetSystemUpdateID(IntPtr aAsyncHandle, out uint aId)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aId = Invocation.OutputUint(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aStatus"></param>
        public void SyncGetIndexerStatus(out String aStatus)
        {
            SyncGetIndexerStatusUpnpOrgContentDirectory1 sync = new SyncGetIndexerStatusUpnpOrgContentDirectory1(this);
            BeginGetIndexerStatus(sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aStatus = sync.Status();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetIndexerStatus().</remarks>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetIndexerStatus(CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetIndexerStatus, aCallback);
            int outIndex = 0;
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetIndexerStatus.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aStatus"></param>
        public void EndGetIndexerStatus(IntPtr aAsyncHandle, out String aStatus)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aStatus = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aSourceID"></param>
        /// <param name="aNumTracks"></param>
        /// <param name="aTotalSize"></param>
        /// <param name="aTotalDuration"></param>
        /// <param name="aScanProgress"></param>
        /// <param name="aIndexerResult"></param>
        public void SyncGetSourceInfo(String aSourceID, out uint aNumTracks, out uint aTotalSize, out uint aTotalDuration, out uint aScanProgress, out String aIndexerResult)
        {
            SyncGetSourceInfoUpnpOrgContentDirectory1 sync = new SyncGetSourceInfoUpnpOrgContentDirectory1(this);
            BeginGetSourceInfo(aSourceID, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aNumTracks = sync.NumTracks();
            aTotalSize = sync.TotalSize();
            aTotalDuration = sync.TotalDuration();
            aScanProgress = sync.ScanProgress();
            aIndexerResult = sync.IndexerResult();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndGetSourceInfo().</remarks>
        /// <param name="aSourceID"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginGetSourceInfo(String aSourceID, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionGetSourceInfo, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionGetSourceInfo.InputParameter(inIndex++), aSourceID));
            int outIndex = 0;
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionGetSourceInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionGetSourceInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionGetSourceInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionGetSourceInfo.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentString((ParameterString)iActionGetSourceInfo.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aNumTracks"></param>
        /// <param name="aTotalSize"></param>
        /// <param name="aTotalDuration"></param>
        /// <param name="aScanProgress"></param>
        /// <param name="aIndexerResult"></param>
        public void EndGetSourceInfo(IntPtr aAsyncHandle, out uint aNumTracks, out uint aTotalSize, out uint aTotalDuration, out uint aScanProgress, out String aIndexerResult)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aNumTracks = Invocation.OutputUint(aAsyncHandle, index++);
            aTotalSize = Invocation.OutputUint(aAsyncHandle, index++);
            aTotalDuration = Invocation.OutputUint(aAsyncHandle, index++);
            aScanProgress = Invocation.OutputUint(aAsyncHandle, index++);
            aIndexerResult = Invocation.OutputString(aAsyncHandle, index++);
        }

        /// <summary>
        /// Invoke the action synchronously
        /// </summary>
        /// <remarks>Blocks until the action has been processed
        /// on the device and sets any output arguments</remarks>
        /// <param name="aSourceID"></param>
        /// <param name="aRescanMode"></param>
        public void SyncRescanSource(String aSourceID, String aRescanMode)
        {
            SyncRescanSourceUpnpOrgContentDirectory1 sync = new SyncRescanSourceUpnpOrgContentDirectory1(this);
            BeginRescanSource(aSourceID, aRescanMode, sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndRescanSource().</remarks>
        /// <param name="aSourceID"></param>
        /// <param name="aRescanMode"></param>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginRescanSource(String aSourceID, String aRescanMode, CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionRescanSource, aCallback);
            int inIndex = 0;
            invocation.AddInput(new ArgumentString((ParameterString)iActionRescanSource.InputParameter(inIndex++), aSourceID));
            invocation.AddInput(new ArgumentString((ParameterString)iActionRescanSource.InputParameter(inIndex++), aRescanMode));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        public void EndRescanSource(IntPtr aAsyncHandle)
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
        /// <param name="aCurrentNumResources"></param>
        /// <param name="aCriticalNumResources"></param>
        /// <param name="aCurrentDiskUsage"></param>
        /// <param name="aMaxDiskUsage"></param>
        public void SyncQueryDatabaseState(out uint aCurrentNumResources, out uint aCriticalNumResources, out uint aCurrentDiskUsage, out uint aMaxDiskUsage)
        {
            SyncQueryDatabaseStateUpnpOrgContentDirectory1 sync = new SyncQueryDatabaseStateUpnpOrgContentDirectory1(this);
            BeginQueryDatabaseState(sync.AsyncComplete());
            sync.Wait();
            sync.ReportError();
            aCurrentNumResources = sync.CurrentNumResources();
            aCriticalNumResources = sync.CriticalNumResources();
            aCurrentDiskUsage = sync.CurrentDiskUsage();
            aMaxDiskUsage = sync.MaxDiskUsage();
        }

        /// <summary>
        /// Invoke the action asynchronously
        /// </summary>
        /// <remarks>Returns immediately and will run the client-specified callback when the action
        /// later completes.  Any output arguments can then be retrieved by calling
        /// EndQueryDatabaseState().</remarks>
        /// <param name="aCallback">Delegate to run when the action completes.
        /// This is guaranteed to be run but may indicate an error</param>
        public void BeginQueryDatabaseState(CallbackAsyncComplete aCallback)
        {
            Invocation invocation = iService.Invocation(iActionQueryDatabaseState, aCallback);
            int outIndex = 0;
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionQueryDatabaseState.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionQueryDatabaseState.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionQueryDatabaseState.OutputParameter(outIndex++)));
            invocation.AddOutput(new ArgumentUint((ParameterUint)iActionQueryDatabaseState.OutputParameter(outIndex++)));
            iService.InvokeAction(invocation);
        }

        /// <summary>
        /// Retrieve the output arguments from an asynchronously invoked action.
        /// </summary>
        /// <remarks>This may only be called from the callback set in the above Begin function.</remarks>
        /// <param name="aAsyncHandle">Argument passed to the delegate set in the above Begin function</param>
        /// <param name="aCurrentNumResources"></param>
        /// <param name="aCriticalNumResources"></param>
        /// <param name="aCurrentDiskUsage"></param>
        /// <param name="aMaxDiskUsage"></param>
        public void EndQueryDatabaseState(IntPtr aAsyncHandle, out uint aCurrentNumResources, out uint aCriticalNumResources, out uint aCurrentDiskUsage, out uint aMaxDiskUsage)
        {
			uint code;
			string desc;
            if (Invocation.Error(aAsyncHandle, out code, out desc))
            {
                throw new ProxyError(code, desc);
            }
            uint index = 0;
            aCurrentNumResources = Invocation.OutputUint(aAsyncHandle, index++);
            aCriticalNumResources = Invocation.OutputUint(aAsyncHandle, index++);
            aCurrentDiskUsage = Invocation.OutputUint(aAsyncHandle, index++);
            aMaxDiskUsage = Invocation.OutputUint(aAsyncHandle, index++);
        }

        /// <summary>
        /// Set a delegate to be run when the SystemUpdateID state variable changes.
        /// </summary>
        /// <remarks>Callbacks may be run in different threads but callbacks for a
        /// CpProxyUpnpOrgContentDirectory1 instance will not overlap.</remarks>
        /// <param name="aSystemUpdateIDChanged">The delegate to run when the state variable changes</param>
        public void SetPropertySystemUpdateIDChanged(System.Action aSystemUpdateIDChanged)
        {
            lock (iPropertyLock)
            {
                iSystemUpdateIDChanged = aSystemUpdateIDChanged;
            }
        }

        private void SystemUpdateIDPropertyChanged()
        {
            lock (iPropertyLock)
            {
                ReportEvent(iSystemUpdateIDChanged);
            }
        }

        /// <summary>
        /// Set a delegate to be run when the ContainerUpdateIDs state variable changes.
        /// </summary>
        /// <remarks>Callbacks may be run in different threads but callbacks for a
        /// CpProxyUpnpOrgContentDirectory1 instance will not overlap.</remarks>
        /// <param name="aContainerUpdateIDsChanged">The delegate to run when the state variable changes</param>
        public void SetPropertyContainerUpdateIDsChanged(System.Action aContainerUpdateIDsChanged)
        {
            lock (iPropertyLock)
            {
                iContainerUpdateIDsChanged = aContainerUpdateIDsChanged;
            }
        }

        private void ContainerUpdateIDsPropertyChanged()
        {
            lock (iPropertyLock)
            {
                ReportEvent(iContainerUpdateIDsChanged);
            }
        }

        /// <summary>
        /// Set a delegate to be run when the IndexerStatus state variable changes.
        /// </summary>
        /// <remarks>Callbacks may be run in different threads but callbacks for a
        /// CpProxyUpnpOrgContentDirectory1 instance will not overlap.</remarks>
        /// <param name="aIndexerStatusChanged">The delegate to run when the state variable changes</param>
        public void SetPropertyIndexerStatusChanged(System.Action aIndexerStatusChanged)
        {
            lock (iPropertyLock)
            {
                iIndexerStatusChanged = aIndexerStatusChanged;
            }
        }

        private void IndexerStatusPropertyChanged()
        {
            lock (iPropertyLock)
            {
                ReportEvent(iIndexerStatusChanged);
            }
        }

        /// <summary>
        /// Set a delegate to be run when the A_ARG_TYPE_Scope state variable changes.
        /// </summary>
        /// <remarks>Callbacks may be run in different threads but callbacks for a
        /// CpProxyUpnpOrgContentDirectory1 instance will not overlap.</remarks>
        /// <param name="aA_ARG_TYPE_ScopeChanged">The delegate to run when the state variable changes</param>
        public void SetPropertyA_ARG_TYPE_ScopeChanged(System.Action aA_ARG_TYPE_ScopeChanged)
        {
            lock (iPropertyLock)
            {
                iA_ARG_TYPE_ScopeChanged = aA_ARG_TYPE_ScopeChanged;
            }
        }

        private void A_ARG_TYPE_ScopePropertyChanged()
        {
            lock (iPropertyLock)
            {
                ReportEvent(iA_ARG_TYPE_ScopeChanged);
            }
        }

        /// <summary>
        /// Query the value of the SystemUpdateID property.
        /// </summary>
        /// <remarks>This function is threadsafe and can only be called if Subscribe() has been
        /// called and a first eventing callback received more recently than any call
        /// to Unsubscribe().</remarks>
        /// <returns>Value of the SystemUpdateID property</returns>
        public uint PropertySystemUpdateID()
        {
            PropertyReadLock();
            uint val = iSystemUpdateID.Value();
            PropertyReadUnlock();
            return val;
        }

        /// <summary>
        /// Query the value of the ContainerUpdateIDs property.
        /// </summary>
        /// <remarks>This function is threadsafe and can only be called if Subscribe() has been
        /// called and a first eventing callback received more recently than any call
        /// to Unsubscribe().</remarks>
        /// <returns>Value of the ContainerUpdateIDs property</returns>
        public String PropertyContainerUpdateIDs()
        {
            PropertyReadLock();
            String val = iContainerUpdateIDs.Value();
            PropertyReadUnlock();
            return val;
        }

        /// <summary>
        /// Query the value of the IndexerStatus property.
        /// </summary>
        /// <remarks>This function is threadsafe and can only be called if Subscribe() has been
        /// called and a first eventing callback received more recently than any call
        /// to Unsubscribe().</remarks>
        /// <returns>Value of the IndexerStatus property</returns>
        public String PropertyIndexerStatus()
        {
            PropertyReadLock();
            String val = iIndexerStatus.Value();
            PropertyReadUnlock();
            return val;
        }

        /// <summary>
        /// Query the value of the A_ARG_TYPE_Scope property.
        /// </summary>
        /// <remarks>This function is threadsafe and can only be called if Subscribe() has been
        /// called and a first eventing callback received more recently than any call
        /// to Unsubscribe().</remarks>
        /// <returns>Value of the A_ARG_TYPE_Scope property</returns>
        public String PropertyA_ARG_TYPE_Scope()
        {
            PropertyReadLock();
            String val = iA_ARG_TYPE_Scope.Value();
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
            iActionBrowse.Dispose();
            iActionSearch.Dispose();
            iActionShuffle.Dispose();
            iActionGetSearchCapabilities.Dispose();
            iActionGetSortCapabilities.Dispose();
            iActionCreateReference.Dispose();
            iActionAddContainerToQueue.Dispose();
            iActionAddItemToQueue.Dispose();
            iActionRemoveFromQueue.Dispose();
            iActionCreateQueue.Dispose();
            iActionRenameQueue.Dispose();
            iActionMoveInQueue.Dispose();
            iActionDestroyObject.Dispose();
            iActionResetDatabase.Dispose();
            iActionGetSystemUpdateID.Dispose();
            iActionGetIndexerStatus.Dispose();
            iActionGetSourceInfo.Dispose();
            iActionRescanSource.Dispose();
            iActionQueryDatabaseState.Dispose();
            iSystemUpdateID.Dispose();
            iContainerUpdateIDs.Dispose();
            iIndexerStatus.Dispose();
            iA_ARG_TYPE_Scope.Dispose();
        }
    }
}

