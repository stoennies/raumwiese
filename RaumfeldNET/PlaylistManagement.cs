using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;
using RaumfeldNET.Renderer;

namespace RaumfeldNET
{
    public class PlaylistBrowser : ContentDirectoryBrowser
    {
        const String PlaylistRootContainerId = ContentType_Playlists + @"/MyPlaylists";

        public PlaylistBrowser(UPNP.UNPN _upnpStack = null)
            : base(PlaylistRootContainerId, _upnpStack)
        {

        }

        public void rereadCurrentList()
        {
            UPNPMediaListBrowse mediaList;

            mediaList = (UPNPMediaListBrowse)this.getList(this.currentListId);
            if (mediaList!=null)
                mediaList.retriveList();
        }

        public void browseToPlaylistRoot()
        {
            this.browseTo(rootContainerId);
        }

        protected override UPNPMediaList createListObject(String _listId)
        {
            PlaylistObjectMediaList list = new PlaylistObjectMediaList(_listId);
            list.isPlaylistLevel = true;
            if (this.containerIdToListId(PlaylistRootContainerId, mainContentType) != _listId)
            {
                list.isEditable = true;
                list.isPlaylistLevel = false;
                list.isTrackLevel = true;
            }
            return list;
        }

        public override void browseToParent(Boolean _rereadParentList = false)
        {
            base.browseToParent(true);
        }

    }

    public class PlaylistManagement : BaseManager
    {
        const String PlaylistRootContainerId = @"0/Playlists/MyPlaylists";

        public delegate void delegate_OnPlaylistCreated(String _playlistName);
        public event delegate_OnPlaylistCreated playlistCreated;

        public delegate void delegate_OnPlaylistDeleted(String _playlistObjectId);
        public event delegate_OnPlaylistDeleted playlistDeleted;

        public delegate void delegate_OnPlaylistRenamed(String _playlistObjectId, String _playlistName);
        public event delegate_OnPlaylistRenamed playlistRenamed;

        public PlaylistManagement(UPNP.UNPN _upnpStack = null)
            : base(_upnpStack)
        {
            
        }


        public MediaItem_Playlist createPlaylist(String _name)
        {
            return this.createPlaylistQueue(_name);
        }

        public void deletePlaylist(String _playlistObjectId)
        {
            CpContentDirectory contentDirectory = Global.getMediaServerManager().getContentDirectory();
            contentDirectory.DestroyObjectSync(_playlistObjectId);
            if (playlistDeleted != null) playlistDeleted(_playlistObjectId);
        }

        public void renamePlaylist(String _playlistObjectId, String _desiredName)
        {
            String givenName;
            CpContentDirectory contentDirectory = Global.getMediaServerManager().getContentDirectory();
            contentDirectory.RenameQueueSync(_playlistObjectId, _desiredName, out givenName);
            if (playlistRenamed != null) playlistRenamed(_playlistObjectId, givenName);
        }

        protected virtual MediaItem_Playlist createPlaylistQueue(String _playlistName)
        {
            System.String givenName, queueIdCreated, containerInfoMetaData;
            CpContentDirectory contentDirectory = Global.getMediaServerManager().getContentDirectory();

            contentDirectory.CreateQueueSync(_playlistName, PlaylistRootContainerId, out givenName, out queueIdCreated, out containerInfoMetaData);
            
            UPNPMediaList dummyList = new UPNPMediaList();
            dummyList.createItemsFromMetaData(containerInfoMetaData);
            MediaItem_Playlist mediaItem = (MediaItem_Playlist)dummyList.list[0];           
            /*Regex regex = new Regex(@"<dc:title>(?<command>.+)</dc:title>");
            Match match = regex.Match(containerInfoMetaData);
            if (match.Success)
            {
                givenName = match.Groups["command"].Value;
            }*/

            if(playlistCreated!=null) playlistCreated(givenName);

            return mediaItem;
        }
    }
}
