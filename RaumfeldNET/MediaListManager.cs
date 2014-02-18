using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Net;


using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;
using RaumfeldNET.Renderer;

namespace RaumfeldNET
{
    public class MediaListManager : BaseManager
    {
        protected Dictionary<String, UPNPMediaList> lists;

        public delegate void delegate_onListDataReady(String _listId);
        public event delegate_onListDataReady listDataReady;

        public delegate void delegate_onItemAdded(String _listId, MediaItem _mediaItem, int _itemIdx, int itemCount);
        public event delegate_onItemAdded itemAdded;

        public Boolean containerUpdateIdEventDisabled;

        public MediaListManager(UPNP.UNPN _upnpStack) :
            base(_upnpStack)
        {
            lists = new Dictionary<String, UPNPMediaList>();
        }

        public virtual void retrieveListFromAvTransportUri(String _listId, String _avTransportUri, String _avTransportUriMetaData)
        {
            String cid;
            UPNPMediaList mediaList = this.getList(_listId);

            if (mediaList == null)
            {
                mediaList = this.createListObject(_listId);
                mediaList.listDataReady += mediaList_listDataReadySink;
                mediaList.itemAdded += mediaList_itemAddedSink;
                lists.Add(_listId, mediaList);
            }

            cid = this.getParameterFromAvTransportUri(_avTransportUri, "cid");
            if (String.IsNullOrWhiteSpace(cid))
                mediaList.retrieveListByMetaData(_avTransportUriMetaData);
            else
                mediaList.retrieveListByContainerId(cid);

            lists[_listId] = mediaList;
        }

        private void mediaList_itemAddedSink(String _listId, MediaItem _mediaItem, int _itemIdx, int itemCount)
        {
            this.eventItemAdded(_listId, _mediaItem, _itemIdx, itemCount);
        }

        private void mediaList_listDataReadySink(String _listId)
        {
            this.eventListDataReady(_listId);
        }

        protected virtual void eventListDataReady(String _listId)
        {
            if (listDataReady != null) listDataReady(_listId);
        }

        protected virtual void eventItemAdded(String _listId, MediaItem _mediaItem, int _itemIdx, int itemCount)
        {
            if (itemAdded != null) itemAdded(_listId, _mediaItem, _itemIdx, itemCount);
        }

        public void retrieveListByContainerUpdateId(String _containerUpdateIdValue)
        {
            String[] containerUpdateAry = _containerUpdateIdValue.Split(',');
            String containerId;

            if (containerUpdateIdEventDisabled)
                return;

            for (int aryIdx = 0; aryIdx < containerUpdateAry.Count(); aryIdx+=2)
            {
                containerId = containerUpdateAry[aryIdx];
                //containerId = System.Uri.UnescapeDataString(containerId);
                UPNPMediaList mediaList = this.getListByContainerId(containerId);
                if (mediaList != null)
                    mediaList.retriveList();
            }
        }

        public String getParameterFromAvTransportUri(String _avTransportUri, String _parmId)
        {
            Uri myUri = new Uri("http://www.dummy.com?" + _avTransportUri);
            return HttpUtility.ParseQueryString(myUri.Query).Get(_parmId);
        }

        public virtual UPNPMediaList getList(String _listId)
        {
            UPNPMediaList mediaList;            
            if (String.IsNullOrWhiteSpace(_listId))
                return null;
            if (lists.TryGetValue(_listId, out mediaList))
                return mediaList;
            return null;
        }

        // get a list by container id. There should not be several lists with the same container id
        public UPNPMediaList getListByContainerId(String _containerId)
        {
            UPNPMediaList mediaList;
            mediaList = lists.FirstOrDefault(x => x.Value.containerId == _containerId).Value;
            return mediaList;
        }

        protected virtual UPNPMediaList createListObject(String _listId)
        {
            return new UPNPMediaList(_listId);
        }
    }

    public class ZoneTitleListManager : MediaListManager
    {
        public delegate void delegate_onTrackListReady(String _zoneId, String _listId);
        public event delegate_onTrackListReady trackListReady;

        public ZoneTitleListManager(UPNP.UNPN _upnpStack)
            : base(_upnpStack)
        {
        }

        protected override UPNPMediaList createListObject(String _listId)
        {
            ZoneTrackMediaList mediaList =  new ZoneTrackMediaList(_listId);
            Zone zone;
            mediaList.zoneUDN = _listId;            
            zone = Global.getZoneManager().getZone(mediaList.zoneUDN);
            zone.trackListId = mediaList.zoneUDN;
            zone.trackListCreated();
            return mediaList;
        }

        public ZoneTrackMediaList getListForZone(String _zoneUdn)
        {
            ZoneTrackMediaList mediaList = (ZoneTrackMediaList)this.getList(_zoneUdn);
            return mediaList;
        }

        protected override void eventListDataReady(String _listId)
        {
            if (trackListReady != null) trackListReady(_listId, _listId);
        }

        public override void retrieveListFromAvTransportUri(String _listId, String _avTransportUri, String _avTransportUriMetaData)
        { 
            base.retrieveListFromAvTransportUri(_listId, _avTransportUri, _avTransportUriMetaData);
            ZoneTrackMediaList mediaList = (ZoneTrackMediaList)lists[_listId];
            String currentPlayingId = this.getParameterFromAvTransportUri(_avTransportUri, "fii");
            if (!String.IsNullOrWhiteSpace(currentPlayingId))
                mediaList.currentTrackIndexPlaying = Convert.ToInt32(currentPlayingId);
            mediaList.containerInfoMetaData = _avTransportUriMetaData;
            mediaList.setListItemSelectedForPlaying();
        }

        public void updatePlayStateOnTrackItem(String _listId, TrackPlayState _playState, Int32 _itemIndex = -1)
        {
            ZoneTrackMediaList mediaList = (ZoneTrackMediaList)this.getList(_listId);
            if (mediaList == null)
                return;

            mediaList.updatePlayStateOnTrackItem(_playState, _itemIndex);
           
        }
    }
}
