using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;
using RaumfeldNET.Renderer;

namespace RaumfeldNET
{
    public enum ContentDirectorySearchType
    {
        Track,
        Album,
        Artist,
        Any
    };

    public enum ContentDirectoryMainContentType
    {
        MyMusic,
        Simfy,
        Napster,
        TuneIn,
        Playlists,
        LastFm,
        Root
    };

    public class ContentDirectoryBrowser : MediaListManager
    {
        protected String rootContainerId;
        protected String rootListId;
        protected String currentContainerId;
        protected String currentListId;

        protected ContentDirectoryMainContentType mainContentType;     

        protected const String ContentType_MyMusic = "0/My Music";
        protected const String ContentType_Napster = "0/Napster";
        protected const String ContentType_Simfy = "0/Simfy";
        protected const String ContentType_TuneIn = "0/RadioTime";
        protected const String ContentType_Playlists = "0/Playlists";
        protected const String ContentType_LastFm = "0/Last.fm";
        protected const String ContentType_Root = "0";
          
        public ContentDirectoryBrowser(string _rootContainerId = "", UPNP.UNPN _upnpStack = null) :
            base(_upnpStack)
        {
            rootContainerId = _rootContainerId;            
        }      

        ~ContentDirectoryBrowser()
        {
        }

        public MediaList getCurrentList()
        {
            return this.getList(containerIdToListId(currentContainerId, mainContentType));
        }

        protected String containerIdToListId(String _containerId, ContentDirectoryMainContentType _contentType)
        {
            return _contentType.ToString() + "|" + _containerId;
        }

        public ContentDirectoryMainContentType getMainContentType()
        {
            return mainContentType;
        }

        public Boolean isSearchAvailable(ContentDirectoryMainContentType _contentType)
        {
            switch (_contentType)
            {

                case ContentDirectoryMainContentType.MyMusic:
                case ContentDirectoryMainContentType.Napster:
                case ContentDirectoryMainContentType.Simfy:
                    return true;
                case ContentDirectoryMainContentType.LastFm:
                case ContentDirectoryMainContentType.Playlists:
                case ContentDirectoryMainContentType.Root:
                case ContentDirectoryMainContentType.TuneIn:
                    return false;
            }
            return false;
        }

        public void browseTo(String _containerId = "", MediaItem _mediaItem = null)
        {
            if (_mediaItem!=null && !_mediaItem.isBrowsable())
                return;
            if (String.IsNullOrWhiteSpace(_containerId))
                _containerId = rootContainerId;
            this.retrieveList(_containerId, currentListId, false, _mediaItem);
        }

        protected void retrieveList(String _containerId, String _parentListId = "", Boolean _useCache = true, MediaItem _parentMediaItem = null)
        {
            UPNPMediaListBrowse mediaList;
            UPNPMediaListBrowse parentList = (UPNPMediaListBrowse)this.getList(_parentListId);
            String listId = this.containerIdToListId(_containerId, mainContentType);

            // try to get list from cache, we do not cache search results (isSearchListChild)!
            if (_useCache)
            {
                mediaList = (UPNPMediaListBrowse)this.getList(listId);
                if (mediaList != null && !mediaList.isSearchChild)
                {
                    currentContainerId = _containerId;
                    currentListId = mediaList.listId;
                    this.eventListDataReady(mediaList.listId);
                }
            }
            else
            {
                if (lists.ContainsKey(listId))
                    lists.Remove(listId);
            }

            string tempListPath = "";
            if (!String.IsNullOrWhiteSpace(_parentListId))
                tempListPath = parentList.path;
            if (_parentMediaItem != null)
            {
                if (!String.IsNullOrEmpty(tempListPath))
                    tempListPath += "/";
                tempListPath += _parentMediaItem.text;
            }

            // if there is no list in cache, then retrieve list
            mediaList = (UPNPMediaListBrowse)this.createListObject(listId);
            mediaList.listDataReady += listDataReadySink;
            mediaList.parentListId = _parentListId;
            mediaList.path = tempListPath;
            if (parentList!=null)
                mediaList.isSearchChild = parentList.isSearchChild; 
            lists.Add(mediaList.listId, mediaList);
            mediaList.retrieveListByContainerId(_containerId);
        }

        protected override UPNPMediaList createListObject(String _listId)
        {
            return (UPNPMediaList)new UPNPMediaListBrowse(_listId);
        }

        protected virtual void listDataReadySink(string _listId)
        {
            currentContainerId = this.getList(_listId).containerId;
            currentListId = _listId;
            this.eventListDataReady(_listId);
        }     

        public void browseToMainContentType(ContentDirectoryMainContentType _mainContentType)
        {
            MediaItem dummyInfo = new MediaItem();
            mainContentType = _mainContentType;
            switch (_mainContentType)
            {
                case ContentDirectoryMainContentType.MyMusic:
                    dummyInfo.text = "MyMusic";
                    this.browseTo(ContentType_MyMusic, dummyInfo);
                    break;
                case ContentDirectoryMainContentType.Napster:
                    dummyInfo.text = "Rhapsody";
                    this.browseTo(ContentType_Napster, dummyInfo);
                    break;
                case ContentDirectoryMainContentType.Simfy:
                    dummyInfo.text = "Simfy";
                    this.browseTo(ContentType_Simfy, dummyInfo);
                    break;
                case ContentDirectoryMainContentType.TuneIn:
                    dummyInfo.text = "TuneIn";
                    this.browseTo(ContentType_TuneIn, dummyInfo);
                    break;
                case ContentDirectoryMainContentType.Playlists:
                    dummyInfo.text = "Playlists";
                    this.browseTo(ContentType_Playlists, dummyInfo);
                    break;
                case ContentDirectoryMainContentType.LastFm:
                    dummyInfo.text = "Last.fm";
                    this.browseTo(ContentType_LastFm, dummyInfo);
                    break;
                case ContentDirectoryMainContentType.Root:
                    dummyInfo.text = "Root";
                    this.browseTo(ContentType_Root, dummyInfo);
                    break;
            }
        }
        
        public virtual void browseToParent(Boolean _rereadParentList = false)
        {
            UPNPMediaListBrowse mediaList;
            UPNPMediaListBrowse mediaListParent;

            mediaList = (UPNPMediaListBrowse)this.getList(this.currentListId);

            // parent should always be in cache!
            if (mediaList != null && !String.IsNullOrWhiteSpace(mediaList.parentListId))
            {
                mediaListParent = (UPNPMediaListBrowse)this.getList(mediaList.parentListId);
                if (mediaListParent != null)
                {
                    if (_rereadParentList)
                        mediaListParent.retriveList();
                    else
                        this.listDataReadySink(mediaList.parentListId);
                }
                else
                    this.writeLog(LogType.Error, String.Format("Parent Liste '{0}' für Liste '{1}' nicht in Cache gefunden!", mediaList.parentListId, mediaList.listId));
            }
        }

        public String getMetaDataForObjectId(String _objectId)
        {
            uint dummy;
            String metaData;
            Global.getMediaServerManager().getContentDirectory().BrowseSyncMeta(_objectId, "*", 0, 0, "", out metaData, out dummy, out dummy, out dummy);
            return metaData;
        }
    }

    public class ContentDirectoryBrowserMulti : ContentDirectoryBrowser
    {
        Dictionary<ContentDirectoryMainContentType, String> dictCurListIdContent;
        Dictionary<ContentDirectoryMainContentType, String> dictCurListIdSearchParent;

        public ContentDirectoryBrowserMulti(String _rootListId = "", UPNP.UNPN _upnpStack = null)
            : base(_rootListId, _upnpStack)
        {
            dictCurListIdContent = new Dictionary<ContentDirectoryMainContentType, String>();
            dictCurListIdSearchParent = new Dictionary<ContentDirectoryMainContentType, String>();
        }

        ~ContentDirectoryBrowserMulti()
        {
        }


        public void switchMainContent(ContentDirectoryMainContentType _mainContentType)
        {
            this.switchList(_mainContentType);
            mainContentType = _mainContentType;
        }

        protected void switchList(ContentDirectoryMainContentType _mainContentType)
        {
            String listId;

            this.setListIdFromMainContentType(mainContentType, currentListId);
            listId = this.getListIdFromMainContentType(_mainContentType);
            if (this.getList(listId) != null)
            {
                currentContainerId = this.getList(listId).containerId;
                currentListId = listId;
            }
            else
            {
                currentContainerId = "";
                currentListId = "";
            }

            if (String.IsNullOrWhiteSpace(currentContainerId))
                this.browseToMainContentType(_mainContentType);
            else
                this.eventListDataReady(currentListId);
        }

        protected String getListIdFromMainContentType(ContentDirectoryMainContentType _mainContentType)
        {
            String listId;
            dictCurListIdContent.TryGetValue(_mainContentType, out listId);
            return listId;
        }

        protected String getSearchListParentIdFromMainContentType(ContentDirectoryMainContentType _mainContentType)
        {
            String listId;
            dictCurListIdSearchParent.TryGetValue(_mainContentType, out listId);
            return listId;
        }

        protected void setListIdFromMainContentType(ContentDirectoryMainContentType _mainContentType, String _listId)
        {
            if (dictCurListIdContent.ContainsKey(_mainContentType))
                dictCurListIdContent.Remove(_mainContentType);
            dictCurListIdContent.Add(_mainContentType, _listId);
        }

        protected void setSearchListParentIdFromMainContentType(ContentDirectoryMainContentType _mainContentType, String _listId)
        {
            if (dictCurListIdSearchParent.ContainsKey(_mainContentType))
                dictCurListIdSearchParent.Remove(_mainContentType);
            dictCurListIdSearchParent.Add(_mainContentType,_listId);
        }

        public void search(String _searchString, ContentDirectorySearchType _searchType = ContentDirectorySearchType.Artist)
        {
            UPNPMediaListBrowse currentList, searchList;
            String containerId = this.buildContainerIdForSearch(mainContentType, _searchType);
            String raumfeldSearchString;

            raumfeldSearchString = "raumfeld:any contains \"" + _searchString + "\"";
            raumfeldSearchString = "dc:title contains \"" + _searchString + "\"";

            currentList = (UPNPMediaListBrowse)this.getList(this.containerIdToListId(currentContainerId, mainContentType));

            if (currentList != null && !currentList.isSearchChild)
                this.setSearchListParentIdFromMainContentType(mainContentType, this.containerIdToListId(currentContainerId, mainContentType));

            // always create new search List
            searchList = (UPNPMediaListBrowse)this.createListObject("Search=" + this.getMainContentType().ToString() + "Type=" + _searchType.ToString() + "ID=" + containerId);
            //searchList = new UPNPMediaListBrowse("Search=" + this.getMainContentType().ToString() + "Type=" + _searchType.ToString() + "ID=" + containerId);
            searchList.listDataReady += listDataReadySink;
            searchList.parentListId = this.getSearchListParentIdFromMainContentType(mainContentType);
            searchList.isSearchChild = true;
            searchList.path = mainContentType.ToString() + " Suchergebnis für '" + _searchString + "' ";
            if (this.getList(searchList.listId) != null)
                lists[searchList.listId] = searchList;
            else
                lists.Add(searchList.listId, searchList);
            searchList.retrieveListByContainerId(containerId, raumfeldSearchString);
        }

        protected String buildContainerIdForSearch(ContentDirectoryMainContentType _mainContentType, ContentDirectorySearchType _searchType = ContentDirectorySearchType.Artist)
        {
            String container = "";

            switch (_mainContentType)
            {
                case ContentDirectoryMainContentType.MyMusic:
                    container = "0/My Music/Search/";
                    break;
                case ContentDirectoryMainContentType.Napster:
                    container = "0/Napster/Search/";
                    break;
                case ContentDirectoryMainContentType.Simfy:
                    container = "0/Simfy/Search/";
                    break;
                case ContentDirectoryMainContentType.TuneIn:
                    container = "0/RadioTime/Search";
                    break;
            }

            switch (_searchType)
            {
                case ContentDirectorySearchType.Album:
                    container += "Album";
                    break;
                case ContentDirectorySearchType.Artist:
                    container += "Artist";
                    break;
                case ContentDirectorySearchType.Track:
                    // Damn Raumfeld ;), needs to have special for tracks too!
                    if (_mainContentType == ContentDirectoryMainContentType.MyMusic)
                        container += "AllTrack";
                    else
                        container += "Track";
                    break;
            }

            // Damn Raumfeld ;), needs to have an "s" on one special type 
            if (_mainContentType == ContentDirectoryMainContentType.MyMusic)
                container += "s";            

            return container;
        }

    }


}
