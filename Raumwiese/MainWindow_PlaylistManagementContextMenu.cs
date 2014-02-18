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

using RaumfeldNET;
using RaumfeldControllerWPFControls;
using MahApps;
using MahApps.Metro.Controls;
using GongSolutions.Wpf.DragDrop;

namespace Raumwiese
{
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow, IDropTarget
    {

        private void initPlaylistManagementContextMenu()
        {
            PlaylistManagementListControl.PreviewMouseUp += playlistManagementControl_PreviewMouseUpSink;
            PlaylistManagementListControl.SelectionChanged += playlistManagementControl_SelectionChangedSink;

            PlaylistManagementContextMenu.actionCloseMenue += playlistManagementContextMenu_actionCloseMenueSink;
            PlaylistManagementContextMenu.actionRenamePlaylist += playlistManagementContextMenu_actionRenamePlaylistSink;
            PlaylistManagementContextMenu.actionDeletePlaylist += playlistManagementContextMenu_actionDeletePlaylistSink;
        }

        void playlistManagementContextMenu_actionDeletePlaylistSink(RaumfeldNET.WPFControls.PlaylistManagementContextMenuControl _control, string _playlistObjectId)
        {
            MediaItem mediaItem = (MediaItem)_control.DataContext;
            PlaylistObjectMediaList playlistList = (PlaylistObjectMediaList)rfController.playlistBrowser.getCurrentList();

            this.setPlaylistManagementListOnUpdate(true);
            this.hidePlaylistManagementActionContextMenu();            

            if (MessageBox.Show("Playliste wirklich löschen?", "Playliste löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                rfController.playlistManagement.deletePlaylist(mediaItem.objectId);
                rfController.playlistBrowser.rereadCurrentList();
            }
            else
                this.setPlaylistManagementListOnUpdate(false);
            
        }

        void playlistManagementContextMenu_actionRenamePlaylistSink(RaumfeldNET.WPFControls.PlaylistManagementContextMenuControl _control, string _playlistObjectId, string _newName)
        {
            MediaItem mediaItem = (MediaItem)_control.DataContext;
            PlaylistObjectMediaList playlistList = (PlaylistObjectMediaList)rfController.playlistBrowser.getCurrentList();

            this.setPlaylistManagementListOnUpdate(true);
            this.savePlaylistManagementListPosition();
            this.hidePlaylistManagementActionContextMenu();

            // TODO: doesn't work properly
            //rfController.playlistManagement.renamePlaylist(mediaItem.objectId, _newName);

            rfController.playlistBrowser.rereadCurrentList();
        }


        void playlistManagementControl_SelectionChangedSink(object sender, SelectionChangedEventArgs e)
        {
            this.hidePlaylistManagementActionContextMenu();
        }
     
        protected void hidePlaylistManagementActionContextMenu()
        {
            PlaylistManagementContextMenuPopup.IsOpen = false;
        }

        void playlistManagementContextMenu_actionCloseMenueSink(RaumfeldNET.WPFControls.PlaylistManagementContextMenuControl _control, RoutedEventArgs e)
        {
            this.hidePlaylistManagementActionContextMenu();
        }

        private void playlistManagementControl_PreviewMouseUpSink(object sender, MouseButtonEventArgs e)
        {
            MediaItem mediaInfo;
            HitTestResult result = VisualTreeHelper.HitTest(PlaylistManagementListControl, e.GetPosition(PlaylistManagementListControl));
            if (result == null)
                return;
            ListBoxItem lbi = findVisualParent<ListBoxItem>(result.VisualHit);

            if (lbi == null)
                return;            

            mediaInfo = (MediaItem)lbi.Content;
            if (mediaInfo == null)
                return;

            PlaylistObjectMediaList mediaList = (PlaylistObjectMediaList)rfController.playlistBrowser.getCurrentList();

            if (mediaList.isPlaylistLevel)
            {
                ListBoxItem listBoxItem = (ListBoxItem)PlaylistManagementListControl.ItemContainerGenerator.ContainerFromItem(mediaInfo);
                if (e.GetPosition(PlaylistManagementListControl).X <= 55)
                    this.showPlaylistManagementListActionContextMenu(mediaInfo, e.GetPosition(PlaylistManagementListControl), listBoxItem);
            }
        }


        protected void showPlaylistManagementListActionContextMenu(MediaItem _mediaItem, Point _point, ListBoxItem lbi)
        {
            if (PlaylistManagementListControl.SelectedItem == null)
                return;

            if (!_mediaItem.isAllowedToDropOnTrackList())
                return;
            
            // TODO: Disabled due not finished ýet!
            return;

            Point relativeLocation = PlaylistManagementListControl.TranslatePoint(new Point(0, 0), lbi);

            PlaylistManagementContextMenuPopup.Width = PlaylistManagementListControl.ActualWidth;
            PlaylistManagementContextMenuPopup.DataContext = _mediaItem;
            PlaylistManagementContextMenuPopup.PlacementRectangle = new Rect(0, _point.Y - (_point.Y - (relativeLocation.Y * -1) - 1), PlaylistManagementListControl.ActualWidth + 1, 50);
            PlaylistManagementContextMenuPopup.IsOpen = true;
            PlaylistManagementContextMenuPopup.StaysOpen = false;
        }
    
    }
}
