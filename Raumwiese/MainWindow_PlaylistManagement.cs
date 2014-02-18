using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Collections;
using System.Collections.ObjectModel;

using RaumfeldNET;
using RaumfeldControllerWPFControls;
using MahApps;
using MahApps.Metro.Controls;
using GongSolutions.Wpf.DragDrop;

namespace Raumwiese
{
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow, IDropTarget
    {
        protected void initPlaylistManagement()
        {
            ScrollViewer sv = (ScrollViewer)this.getScrollViewer(PlaylistManagementListControl);
            sv.ScrollChanged += playlistManagementList_ScrollChangedSink;
            PlaylistManagementListControl.MouseDoubleClick += playlistManagementListControl_MouseDoubleClickSink;
            PlaylistManagementListControl.KeyUp += playlistManagementList_KeyUpSink;
            rfController.playlistBrowserListReady += rfController_playlistManagementListReadySink;
            GongSolutions.Wpf.DragDrop.DragDrop.SetDropHandler(PlaylistManagementListControl, this);
        }

        public PlaylistObjectMediaList currentPlaylistManagementList()
        {
            return (PlaylistObjectMediaList)rfController.playlistBrowser.getCurrentList();
        }

        protected void playlistManagementListControlBrowseToParent_ClickSink(object sender, RoutedEventArgs e)
        {
            rfController.playlistBrowser.browseToParent();
        }

        protected void playlistManagementListControl_MouseDoubleClickSink(object sender, MouseButtonEventArgs e)
        {
            MediaItem mediaItem;
            if (PlaylistManagementListControl.SelectedItems != null)
            {
                mediaItem = (MediaItem)PlaylistManagementListControl.SelectedItem;
                if (mediaItem == null)
                    return;
                if (!mediaItem.isBrowsable())
                {
                    /*if (mediaItem.isAllowedToDropOnPlaylist())
                    {
                        ListBoxItem listBoxItem = (ListBoxItem)BrowseList.ItemContainerGenerator.ContainerFromItem(mediaInfo);
                        this.ShowBrowseListActionPopupMenu(mediaInfo, e.GetPosition(BrowseList), listBoxItem);
                    }*/
                    return;
                }

                this.setPlaylistManagementListOnUpdate(true);

                this.savePlaylistManagementListPosition();
                rfController.playlistBrowser.browseTo(mediaItem.objectId, mediaItem);
            }
        }

        delegate void invoke_rfController_playlistManagementListReadySink(String _listId);
        void rfController_playlistManagementListReadySink(String _listId)
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                this.setPlaylistManagementListData(_listId);
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_playlistManagementListReadySink(this.rfController_playlistManagementListReadySink), _listId);
        }

        protected void setPlaylistManagementListData(String _listId)
        {
            this.hidePlaylistManagementActionContextMenu();
            this.setPlaylistManagementListOnUpdate(true);
            if (String.IsNullOrWhiteSpace(_listId))
                return;
            UPNPMediaListBrowse mediaList = (UPNPMediaListBrowse)rfController.playlistBrowser.getList(_listId);
            this.setMediaItemListViewData(PlaylistManagementListControl, mediaList);
            PlaylistManagementSelectedPlaylistText.Text = mediaList.path;
            this.setPlaylistManagementListOnUpdate(false);
        }

        protected void playlistManagementList_ScrollChangedSink(object sender, ScrollChangedEventArgs e)
        {
            this.loadMediaItemListViewImages(PlaylistManagementListControl);
        }

        protected void setPlaylistManagementListOnUpdate(Boolean _isOnUpdate = true)
        {
            PlaylistManagementListControl.IsEnabled = !_isOnUpdate;
            PlaylistManagementProgressRingControl.Visibility = _isOnUpdate ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        public void savePlaylistManagementListPosition()
        {
            MediaList mediaList = rfController.playlistBrowser.getCurrentList();
            if (mediaList == null)
                return;

            this.saveMediaItemListPosition(PlaylistManagementListControl, mediaList);
        }

        private void playlistManagementList_KeyUpSink(object sender, KeyEventArgs e)
        {
            this.savePlaylistManagementListPosition();
            if (e.Key == Key.Delete)
            {
                this.setPlaylistManagementListOnUpdate(true);
                PlaylistObjectMediaList playlistList = (PlaylistObjectMediaList)rfController.playlistBrowser.getCurrentList();
                if (playlistList.isEditable)
                {
                    this.removeSelectedItemsFromPlaylistManagementList();
                }
                else
                {
                    if (MessageBox.Show("Markierte Playlisten wirklich löschen?", "Playlisten löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        this.removeSelectedPlaylists();
                    else
                        this.setPlaylistManagementListOnUpdate(false);
                }
            }            
        }

        protected void removeSelectedPlaylists()
        {
            int curPos = -1;
            SortedList<int, int> deleteList;
            deleteList = this.removeSelectedItemsFromMediaItemList(PlaylistManagementListControl);
            IEnumerator<KeyValuePair<int, int>> listEnum;

            PlaylistObjectMediaList playlistList = (PlaylistObjectMediaList)rfController.playlistBrowser.getCurrentList();

            // run through sorted list from the backwards    
            listEnum = deleteList.GetEnumerator();
            while (listEnum.MoveNext())
            {
                curPos = listEnum.Current.Value;
                rfController.playlistManagement.deletePlaylist(playlistList.list[curPos].objectId);
            }
            rfController.playlistBrowser.rereadCurrentList();
        }

        protected void removeSelectedItemsFromPlaylistManagementList()
        {
            SortedList<int, int> deleteList;
            int rowStartPos = -1, prevPos = -1, curPos = -1;
            IEnumerator selectedItemsEnum = PlaylistManagementListControl.SelectedItems.GetEnumerator();
            IEnumerator<KeyValuePair<int, int>> listEnum; 

            deleteList = this.removeSelectedItemsFromMediaItemList(PlaylistManagementListControl);

            PlaylistObjectMediaList playlistList = (PlaylistObjectMediaList)rfController.playlistBrowser.getCurrentList();

            // run through sorted list from the backwards    
            listEnum = deleteList.GetEnumerator();
            while (listEnum.MoveNext())
            {
                curPos = listEnum.Current.Value;
                if (rowStartPos == -1)
                    rowStartPos = curPos;
                if (prevPos != -1 && prevPos - 1 != curPos)
                {
                    playlistList.removeItem(prevPos, rowStartPos);
                    rowStartPos = curPos;
                }
                prevPos = curPos;
            }
            playlistList.removeItem(curPos, rowStartPos);
        }

        private void buttonCreatePlaylist_ClickSink(object sender, RoutedEventArgs e)
        {
            MediaItem_Playlist playlistItem;
            if(!mediaServerPresent || String.IsNullOrWhiteSpace(PlaylistNameCreateTextBox.Text))
                return;
            playlistItem = rfController.playlistManagement.createPlaylist(PlaylistNameCreateTextBox.Text);    
            rfController.playlistBrowser.browseTo(playlistItem.objectId, playlistItem);
            PlaylistNameCreateTextBox.Text = "";
        }
    }
}
