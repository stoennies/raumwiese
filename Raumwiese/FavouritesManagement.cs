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
       /* protected void initFavouritesManagement()
        {
            ScrollViewer sv = (ScrollViewer)this.getScrollViewer(PlaylistManagementListControl);
            sv.ScrollChanged += favouritesManagementList_ScrollChangedSink;
            PlaylistManagementListControl.KeyUp += favouritesManagementList_KeyUpSink;
            rfController.playlistBrowserListReady += rfController_playlistManagementListReadySink;
            GongSolutions.Wpf.DragDrop.DragDrop.SetDropHandler(PlaylistManagementListControl, this);
        }

        public PlaylistObjectMediaList currentPlaylistManagementList()
        {
            return (PlaylistObjectMediaList)rfController.playlistBrowser.getCurrentList();
        }

        delegate void invoke_rfController_fafvouritesManagementListReadySink(String _listId);
        void rfController_fafvouritesManagementListReadySink(String _listId)
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                this.setFavouritesManagementListData(_listId);
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_fafvouritesManagementListReadySink(this.rfController_fafvouritesManagementListReadySink), _listId);
        }

        protected void setPlaylistManagementListData(String _listId)
        {
            this.setFavouritesManagementListOnUpdate(true);
            if (String.IsNullOrWhiteSpace(_listId))
                return;
            UPNPMediaListBrowse mediaList = (UPNPMediaListBrowse)rfController.playlistBrowser.getList(_listId);
            this.setMediaItemListViewData(PlaylistManagementListControl, mediaList);
            PlaylistManagementSelectedPlaylistText.Text = mediaList.path;
            this.setFavouritesManagementListOnUpdate(false);
        }

        protected void playlistManagementList_ScrollChangedSink(object sender, ScrollChangedEventArgs e)
        {
            this.loadMediaItemListViewImages(PlaylistManagementListControl);
        }

        protected void setFavouritesManagementListOnUpdate(Boolean _isOnUpdate = true)
        {
            PlaylistManagementListControl.IsEnabled = !_isOnUpdate;
            PlaylistManagementProgressRingControl.Visibility = _isOnUpdate ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        public void saveFavouritesManagementListPosition()
        {
            MediaList mediaList = rfController.playlistBrowser.getCurrentList();
            if (mediaList == null)
                return;

            this.saveMediaItemListPosition(PlaylistManagementListControl, mediaList);
        }

        private void favouritesManagementList_KeyUpSink(object sender, KeyEventArgs e)
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

        protected void removeSelectedItemsFromFavouritesManagementList()
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
        * */

    }
}
