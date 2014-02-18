using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Net;
using System.Threading;

using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;
using RaumfeldNET.Renderer;

namespace RaumfeldNET
{
    public class MediaList : Base.BaseHelper
    {
        // list which holds the information of class "MediaItem" and all of its descendants
        public List<MediaItem> list;
        public String listId;

        // this list is used on updateing, so 'list' itself will be untouched until full read is done
        protected List<MediaItem> updateList;

        // data for current position of the list. This is needed for restoring the visible position
        // (e.g in a ListView)
        public object visualPosition;

        public delegate void delegate_onListDataReady(String _listId);
        public event delegate_onListDataReady listDataReady;

        public MediaList(String _listId = "")
        {
            listId = _listId;
            list = new List<MediaItem>();
            updateList = new List<MediaItem>();
        }

        ~MediaList()
        {
            list = null;
            updateList = null;
        }

        // should be called if the list data is ready!
        protected void eventListDataReady()
        {
            if (listDataReady != null) this.listDataReady(listId);
        }

        protected void clearList()
        {
            lock (list)
            {
                list.Clear();
                this.listUpdated();
            }
        }

        protected virtual void listUpdated()
        {
            this.eventListDataReady();
        }
    }

    public class UPNPMediaList : MediaList
    {
        public String containerId;
        public String containerInfoMetaData;
        protected CpContentDirectory contentDirectory;

        public delegate void delegate_onQueueCeatedHandler(String _listId);
        public event delegate_onQueueCeatedHandler queueCreated;

        public delegate void delegate_onItemAdded(String _listId, MediaItem _mediaItem, int _itemIdx, int itemCount);
        public event delegate_onItemAdded itemAdded;


        public UPNPMediaList(String _listId = "")
            : base(_listId)
        {
            contentDirectory = Global.getMediaServerManager().getContentDirectory();
        }

        public void retriveList()
        {
            if (String.IsNullOrWhiteSpace(containerId))
                return;
            this.retrieveListByContainerId(containerId);
        }

        // retrieve list for a containerId
        public void retrieveListByContainerId(String _containerId, String _searchCriteria = "", Boolean _syncRequest = false)
        {
            System.String result;
            uint numberReturned, totalMatches, updateID;

            containerId = _containerId;

            if (_syncRequest)
            {
                if (!String.IsNullOrWhiteSpace(_searchCriteria))
                {
                    if (_searchCriteria == "*") _searchCriteria = "";
                    contentDirectory.SearchSync(containerId, _searchCriteria, "*", 0, 0, String.Empty, out result, out numberReturned, out totalMatches, out updateID);
                    this.contentDirectory_SearchSink(contentDirectory, containerId, String.Empty, "*", 0, 0, String.Empty, result, numberReturned, totalMatches, updateID, null, null);
                }
                else
                {
                    contentDirectory.BrowseSync(_containerId, "*", 0, 0, String.Empty, out result, out numberReturned, out totalMatches, out updateID);
                    this.contentDirectory_BrowseSink(contentDirectory, _containerId, "*", 0, 0, String.Empty, result, numberReturned, totalMatches, updateID, null, null);   
                }
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(_searchCriteria))
                {
                    if (_searchCriteria == "*") _searchCriteria = "";
                    contentDirectory.Search(containerId, _searchCriteria, "*", 0, 0, String.Empty, containerId, contentDirectory_SearchSink);
                }
                else
                    contentDirectory.Browse(_containerId, "*", 0, 0, String.Empty, containerId, contentDirectory_BrowseSink);

            }                
        }

        protected void contentDirectory_SearchSink(CpContentDirectory sender, System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, System.String Result, System.UInt32 NumberReturned, System.UInt32 TotalMatches, System.UInt32 UpdateID, Object e, object _Tag)
        {
            this.createItemsFromMetaData(Result);
        }

        protected void contentDirectory_BrowseSink(CpContentDirectory sender, System.String ContainerID, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, System.String Result, System.UInt32 NumberReturned, System.UInt32 TotalMatches, System.UInt32 UpdateID, Object e, object _Tag)
        {
            this.createItemsFromMetaData(Result);
        }

        public void retrieveListByMetaData(String _metaData)
        {
            // There is only one item in the tracklist. Therefore its no container, its a direct link to 
            // the file, we have to get the infos from the renderer metadata            
            this.createItemsFromMetaData(_metaData);
        }

        public void createItemsFromMetaData(String _metaData)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);

            if (String.IsNullOrWhiteSpace(_metaData))
            {
                this.clearList();
                return;
            }

            try
            {
                updateList.Clear();

                xmlDoc.LoadXml(_metaData);

                manager.AddNamespace("ns", @"urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/");
                manager.AddNamespace("dc", @"http://purl.org/dc/elements/1.1/");
                manager.AddNamespace("upnp", @"urn:schemas-upnp-org:metadata-1-0/upnp/");
                manager.AddNamespace("dlna", @"urn:schemas-dlna-org:metadata-2-0/");
                manager.AddNamespace("raumfeld", @"urn:schemas-raumfeld-com:meta-data/raumfeld");

                this.createItemsFromMetaDataNode(xmlDoc, manager, "ns:DIDL-Lite/ns:item");
                this.createItemsFromMetaDataNode(xmlDoc, manager, "ns:DIDL-Lite/ns:container");

                lock (list)
                {
                    list = updateList.ToList();
                }
                this.listUpdated();

            }
            catch (Exception e)
            {
                this.writeLog(LogType.Error, "Fehler beim Parsen des Metadatenfiles für Media-Information", e, _metaData);
            }         
        }

        protected void createItemsFromMetaDataNode(XmlDocument _doc, XmlNamespaceManager _manager, string _nodeId)
        {
            XmlNodeList nodeList;
            MediaItem mediaItem;

            nodeList = _doc.SelectNodes(_nodeId, _manager);
            foreach (XmlNode nodeItem in nodeList)
            {
                mediaItem = this.createMediaItemForMetaDataNode(nodeItem, _manager);
                if (mediaItem != null)
                {
                    mediaItem.listIndex = updateList.Count;
                    updateList.Add(mediaItem);
                }
            }
        }

        protected virtual MediaItem createMediaItemForMetaDataNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            MediaItem mediaItem;
            String raumfeldSection;
            String upnpClass;

            // check type of item by info we get on result, it may be a track, a album, artist info... aso..
            // for this create own Classed derived from Media Information. In fact a list will consists only
            // of items with one type. But this class should be able to handle differnt types of items in a list
            upnpClass = this.xmlDocumentHelper.getChildNodeValue(_nodeItem, "upnp:class", _manager); 
            raumfeldSection = this.xmlDocumentHelper.getChildNodeValue(_nodeItem, "raumfeld:section", _manager);

            if (String.IsNullOrWhiteSpace(upnpClass))
                return null;

            mediaItem = MediaItem.newFromUpnpItemClass(upnpClass, raumfeldSection);
            mediaItem.initValuesFromXMLNode(_nodeItem, _manager);

            if (!isItemViewable(mediaItem))
                return null;

            return mediaItem;
        }

        protected Boolean isItemViewable(MediaItem _mediaItem)
        {
            if (_mediaItem.upnpItemClass.Contains(".search"))
                return false;
            if (_mediaItem.text.ToUpper() == "SEARCH")
                return false;
            if (_mediaItem.text.ToUpper() == "ALL TRACKS") // @@@ activate if visual style is ok
                return false;
            return true;
        }

        // returns true if current loaded list is a queue or not
        // a queue is defined by the 'name' for now. That means if there is some special text in the containerId, then its a queue
        public virtual Boolean isQueue()
        {
            if (!String.IsNullOrEmpty(containerId))
                return containerId.IndexOf("0/Zones/") != -1;
            return false;
        }

        public virtual void moveItem(Int32 _itemIndex, Int32 _itemIndexTo)
        {
            MediaItem mediaItemMoveing;
            Boolean isQueueCreated;

            //if there is no queue the create a queue (do this with sync method!)
            isQueueCreated = this.createQueue();

            mediaItemMoveing = list.ElementAt(_itemIndex);
            contentDirectory.MoveInQueue(mediaItemMoveing.objectId, Convert.ToUInt32(_itemIndexTo), null, moveItemSink);

            if (isQueueCreated)
            {
                if (queueCreated != null) queueCreated(listId);
            }

            // rebuild of internal list will be done when retrieven the new information from renderer.
            // this may lead to a little "slow" playlist, but it will always be accurate!
        }

        public virtual void addItems(List<MediaItem> _mediaItems, Int32 _itemIndexTo, Boolean _useThread = false)
        {
            if (!_useThread)
                this.addItems(_mediaItems, _itemIndexTo);

            Thread thread = new Thread(() => addItems(_mediaItems, _itemIndexTo));
            thread.Start();
        }

        public virtual void addItems(List<MediaItem> _mediaItems, Int32 _itemIndexTo)
        {
            if (_mediaItems == null || _mediaItems.Count == 0)
                return;

            Global.getZoneTitleListManager().containerUpdateIdEventDisabled = true;

            for (int listIdx = _mediaItems.Count - 1; listIdx >= 0; listIdx--)
            {
                if (listIdx == 0)
                    Global.getZoneTitleListManager().containerUpdateIdEventDisabled = false;
               this.addItem(_mediaItems[listIdx], _itemIndexTo, true, true);
               if(itemAdded!=null) itemAdded(this.listId, _mediaItems[listIdx], listIdx+1, _mediaItems.Count);
            }
            this.listActionDone(); // @@@
        }

        public virtual void addItem(MediaItem _mediaItem, Int32 _itemIndexTo, Boolean _sync = false, Boolean _fromList = false)
        {
            Boolean isQueueCreated;

            if (_mediaItem == null)
            {
                this.writeLog(LogType.Error, String.Format("Hinzufügen von MediaItem zu Liste {0} fehlgeshlagen!", listId));
                return;
            }

            try
            {

                //if there is no queue the create a queue (do this with sync method!)
                // we always create a queue if there isn't one. We avoid direct links to containers
                isQueueCreated = this.createQueue();
                
                if (_mediaItem.itemType == MediaItemItemType.Shuffle)
                {
                    // no implementation for 'unSync' action for shuffle!
                    this.addShuffle(_mediaItem, _itemIndexTo, _sync);
                }
                else if (_mediaItem.isContainer())
                {
                    if (_sync)
                        contentDirectory.AddContainerToQueueSync(containerId, _mediaItem.objectId, _mediaItem.objectId, "", "", 0, 294967295, Convert.ToUInt32(_itemIndexTo));
                    else
                        contentDirectory.AddContainerToQueue(containerId, _mediaItem.objectId, _mediaItem.objectId, "", "", 0, 294967295, Convert.ToUInt32(_itemIndexTo), _mediaItem, addContainerSink);
                }
                else
                {
                    if (_sync)
                        contentDirectory.AddItemToQueueSync(containerId, _mediaItem.objectId, Convert.ToUInt32(_itemIndexTo));
                    else
                        contentDirectory.AddItemToQueue(containerId, _mediaItem.objectId, Convert.ToUInt32(_itemIndexTo), _mediaItem, addItemSink);
                }

                if (isQueueCreated)
                {
                    if (queueCreated != null) queueCreated(listId);
                }

                if (_sync && !_fromList)
                {
                    if (itemAdded != null) itemAdded(this.listId, _mediaItem, 1, 1);
                }
                

                // rebuild of internal list will be done when retrieven the new information from renderer.
                // this may lead to a little "slow" playlist, but it will always be accurate!

            }
            catch (Exception e)
            {
                this.writeLog(LogType.Error, String.Format("Fehler beim hinzufügen von Song '{0}' zu Liste '{1}'", _mediaItem.text, this.listId), e);
            }
        }

        protected void addShuffle(MediaItem _mediaItem, Int32 _itemIndexTo, Boolean _sync)
        {
            String playlistId, playlistMetaData;
            contentDirectory.ShuffleSync(_mediaItem.objectId, "", out playlistId, out playlistMetaData);
            contentDirectory.AddContainerToQueueSync(containerId, playlistId, playlistId, "", "", 0, 294967295, Convert.ToUInt32(_itemIndexTo));
            this.listActionDone();
        }

        public virtual void removeItem(Int32 _itemIndexFrom, Int32 _itemIndexTo)
        {
            uint containerUpdateId;
            Boolean isQueueCreated;

            //if there is no queue the create a queue (do this with sync method!)
            isQueueCreated = this.createQueue();

            contentDirectory.RemoveFromQueueSync(containerId, Convert.ToUInt32(_itemIndexFrom), Convert.ToUInt32(_itemIndexTo), out containerUpdateId);

            if (isQueueCreated)
            {
                if (queueCreated != null) queueCreated(listId);
            }

            this.listActionDone();

            // INFO: rebuild of internal list will be done when retrieven the new information from renderer.
            // this may lead to a little "slow" playlist, but it will always be accurate!
        }

        protected void moveItemSink(CpContentDirectory _sender, uint _containerUpdateId, Object _tag)
        {
            // Bend AV Transport uri to renderer with new fii info!
            this.listActionDone();
        }

        protected void addItemSink(CpContentDirectory _sender, Object _tag)
        {
            if (itemAdded != null) itemAdded(this.listId, (MediaItem)_tag, 1, 1);
            // Bend AV Transport uri to renderer with new fii info!            
            if (_tag != null)
                this.listActionDone();
        }

        protected void addContainerSink(CpContentDirectory _sender, Object _tag)
        {

            if (itemAdded != null) itemAdded(this.listId, (MediaItem)_tag, 1, 1);
            // Bend AV Transport uri to renderer with new fii info!
            if (_tag != null)
                this.listActionDone();
        }

        // is called if some list action like add, or remove was done
        protected virtual void listActionDone()
        {
            this.retrieveListByContainerId(containerId, "*", true);
        }

        // creates a queue from current list if it is not a queue
        protected virtual Boolean createQueue()
        {
            System.String givenName, queueIdCreated;
            uint cuid;            

            if (this.isQueue())
                return false;

            contentDirectory.CreateQueueSync(listId, "0/Zones", out givenName, out queueIdCreated, out containerInfoMetaData);

            if(!String.IsNullOrWhiteSpace(queueIdCreated))
                this.writeLog(LogType.Error, String.Format("Fehler beim erstellen einer queue aus containerID '{0}'", containerId));

            contentDirectory.RemoveFromQueueSync(queueIdCreated, 0, 4294967295, out cuid);

            if (this.list.Count > 0)
            {
                // move container or item to queue (Sync)
                if (!String.IsNullOrEmpty(containerId))
                    contentDirectory.AddContainerToQueueSync(queueIdCreated, containerId, containerId/*"urn:upnp-org:serviceId:ContentDirectory"*/, "", "", 0, 4294967295, 0);
                else
                    // there hast to be only one object in list! otherwise it would be a container ora a queue!
                    contentDirectory.AddItemToQueueSync(queueIdCreated, this.list[0].objectId, 0);
            }

            containerId = queueIdCreated;

            // reread list from new queue! (Sync!) we have to get the new list with the new ids!!!
            this.retrieveListByContainerId(containerId, "*", true);

            return true;
        }

        public int getListPositionOfObjectId(String _objectId)
        {
            if (String.IsNullOrEmpty(_objectId))
                return -1;
            var found = this.list.Find(item => item.objectId == _objectId);
            if (found != null)
            {
                return found.listIndex;
            }
            return -1;
        }

        protected virtual void removeAllItems()
        {
            this.removeItem(0, int.MaxValue);
        }

    }

    public class TrackMediaList : UPNPMediaList
    {
        public TrackMediaList(String _listId = "")
            : base(_listId)
        {
        }
    }

    public class ZoneTrackMediaList : TrackMediaList
    {
        public String zoneUDN;

        protected String objectIdTmp;
        protected String currentObjectIdPlaying;
        public Int32 currentTrackIndexPlaying;

        public ZoneTrackMediaList(String _listId = "")
            : base(_listId)
        {
        }

        protected override void listUpdated()
        {
            base.listUpdated();
            this.setListItemSelectedForPlaying();
        }

        override protected void listActionDone()
        {
            int trackIndex;
            RendererVirtual renderer = (RendererVirtual)Global.getRendererManager().getRendererByZoneUDN(zoneUDN);
            
            base.listActionDone();
                        
            // search track in list
            trackIndex = this.getListPositionOfObjectId(objectIdTmp);
            if (trackIndex == -1)
            {
                if (this.list.Count == 0)
                {
                    renderer.getRendererObject().Connections[0].SetAvTransportUri("", "");
                    currentTrackIndexPlaying = 0;
                }
                else
                {
                    if (this.list.Count < currentTrackIndexPlaying)
                        currentTrackIndexPlaying = this.list.Count;
                    else if (currentTrackIndexPlaying < 0)
                        currentTrackIndexPlaying = 0;
                    // Track was not found. Play next one
                    renderer.playTrack(currentTrackIndexPlaying);
                    //this.setTrackNrPlaying((int)trackNrPlaying);
                }
            }
            else
            {
                currentTrackIndexPlaying = trackIndex;
                renderer.getRendererObject().Connections[0].BendAvTransportUri(this.buildAvTransportUri(), containerInfoMetaData);
            }

            this.setListItemSelectedForPlaying();
        }

        public String buildAvTransportUri(int _trackIndex = -1, Boolean _setTrackInfo = true)
        {
            String uri;

            if (_trackIndex == -1)
                _trackIndex = currentTrackIndexPlaying;

            uri = "uuid:";
            uri += contentDirectory.getDeviceUdn();
            uri += "?sid=urn:upnp-org:serviceId:ContentDirectory&cid=";
            uri += containerId.Replace("%", "%25").Replace("=", "%3d").Replace(@"/", "%2F");
            uri += "&md=0";
            if (_trackIndex >= 0)
            {
                if (_setTrackInfo && this.list.Count > _trackIndex)
                    uri += "&fid=" + this.list[_trackIndex].objectId; // @@@ direct linkt to list with track id!!! ATTENTION!
                uri += "&fii=" + _trackIndex;
            }
            uri = "dlna-playcontainer://" + uri.Replace(":", "%3a");
            //uri = uri.Replace("&", "&amp;");
            // &amp;fid=0%2FNapster%2FImportedFavorites%2FAlbum%2FAlb.71029663%2FTra.71029668&amp;fii=4</CurrentURI>
            return uri;
        }

        protected override Boolean createQueue()
        {
            Boolean ret;
            ret = base.createQueue();
            this.setListItemSelectedForPlaying();
            objectIdTmp = currentObjectIdPlaying;
            return ret;
        }

        public override void moveItem(Int32 _itemIndex, Int32 _itemIndexTo)
        {
            objectIdTmp = currentObjectIdPlaying;
            base.moveItem(_itemIndex, _itemIndexTo);
        }

        public override void addItem(MediaItem mediaItem, Int32 _itemIndexTo, Boolean _sync = false, Boolean _fromList = false)
        {
            objectIdTmp = currentObjectIdPlaying;
            base.addItem(mediaItem, _itemIndexTo, _sync);
        }

        public override void removeItem(Int32 _itemIndexFrom, Int32 _itemIndexTo)
        {
            objectIdTmp = currentObjectIdPlaying;
            base.removeItem(_itemIndexFrom, _itemIndexTo);
        }

        public void addAtEnd(MediaItem _mediaItem)
        {
            this.addItem(_mediaItem, int.MaxValue);
        }
        public void addAtEnd(List<MediaItem> _mediaItems)
        {
            this.addItems(_mediaItems, /*int.MaxValue*/ this.list.Count, true);
        }

        public void addNext(MediaItem _mediaItem)
        {
            this.addItem(_mediaItem, currentTrackIndexPlaying);
        }

        public void addNext(List<MediaItem> _mediaItems)
        {
            this.addItems(_mediaItems, currentTrackIndexPlaying + 1, true);
        }

        public void playNow(MediaItem mediaItem)
        {
            this.removeAllItems();
            this.addItem(mediaItem, 0);
        }

        public void playNow(List<MediaItem> _mediaItems)
        {
            this.removeAllItems();
            this.addItems(_mediaItems, 0, true);
        }

        public void setListItemSelectedForPlaying()
        {
            MediaItem_Track trackInfo;

            if (this.list == null)
                return;

            //lock (list) @@@
            {
                for (int i = 0; i < this.list.Count; i++)
                {
                    trackInfo = (MediaItem_Track)this.list[i];
                    if (trackInfo.listIndex == currentTrackIndexPlaying)
                    {
                        trackInfo.isSelectedForPlaying = true;
                        currentObjectIdPlaying = trackInfo.objectId;
                    }
                    else
                    {
                        trackInfo.isSelectedForPlaying = false;
                        trackInfo.playState = TrackPlayState.Stopped;
                    }
                    this.list[i] = trackInfo;
                }
            }

            if (Global.getRendererManager().getRendererByZoneUDN(this.zoneUDN)!=null)
                this.updatePlayStateOnTrackItem(Global.rendererPlayStateToTrackPlayState(Global.getRendererManager().getRendererByZoneUDN(this.zoneUDN).playState));
        }

        public void updatePlayStateOnTrackItem(TrackPlayState _playState, Int32 _itemIndex = -1)
        {
            MediaItem_Track trackInfo;

            if (this.list == null)
                return;

            if (_itemIndex == -1)
                _itemIndex = currentTrackIndexPlaying;

            lock (list) 
            {
                for (int i = 0; i < this.list.Count; i++)
                {
                    trackInfo = (MediaItem_Track)this.list[i];
                    if (trackInfo.listIndex == _itemIndex)
                    {
                        trackInfo.playState = _playState;
                    }
                    else
                    {
                        trackInfo.playState = TrackPlayState.Stopped;
                    }
                    this.list[i] = trackInfo;
                }
            }
        }


    }

    public class PlaylistObjectMediaList : UPNPMediaListBrowse
    {
        public Boolean isEditable;
        public Boolean isTrackLevel;
        public Boolean isPlaylistLevel;

        public PlaylistObjectMediaList(String _listId = "")
            : base(_listId)
        {
            isEditable = false;
            isPlaylistLevel = false;
            isTrackLevel = false;
        }

        public void addAtEnd(MediaItem mediaItem)
        {
            this.addItem(mediaItem, int.MaxValue);
        }

        // Playlists are never a queue!
        protected override Boolean createQueue()
        {
            return false;
        }
    }

    public class UPNPMediaListBrowse : UPNPMediaList
    {
        public Boolean isSearchChild;
        public String parentListId;
        public String path;

        public UPNPMediaListBrowse(String _listId = "")
            : base(_listId)
        {
        }

    }

}
