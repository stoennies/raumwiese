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
        protected void initContentBrowserListControl()
        {
            ScrollViewer sv = (ScrollViewer)this.getScrollViewer(ContentBrowseListControl);
            sv.ScrollChanged += contentBrowseList_ScrollChangedSink;            
            rfController.contentBrowserListReady += rfController_contentBrowserListReadySink;
            ContentBrowseListControl.MouseDoubleClick += ContentBrowseListControl_MouseDoubleClickSink;            
            ContentBrowserBackControl.Click += ContentBrowserBackControl_ClickSink;
        }

        void ContentBrowserBackControl_ClickSink(object sender, RoutedEventArgs e)
        {
            rfController.contentBrowser.browseToParent();
        }

        void ContentBrowseListControl_MouseDoubleClickSink(object sender, MouseButtonEventArgs e)
        {
            MediaItem mediaItem;
            if (ContentBrowseListControl.SelectedItems != null)
            {
                mediaItem = (MediaItem)ContentBrowseListControl.SelectedItem;
                if (mediaItem == null)
                    return;
                if (!mediaItem.isBrowsable())
                {
                    if (mediaItem.isAllowedToDropOnTrackList())
                    {
                        ListBoxItem listBoxItem = (ListBoxItem)ContentBrowseListControl.ItemContainerGenerator.ContainerFromItem(mediaItem);
                        this.showBrowseListActionContextMenu(mediaItem, e.GetPosition(ContentBrowseListControl), listBoxItem);
                    }
                    return;
                }

                this.setContentBrowserListOnUpdate(true);

                //BrowseList.ItemsSource = null;
                this.saveContentBrowseListPosition();
                rfController.contentBrowser.browseTo(mediaItem.objectId, mediaItem);                
            }
        }

        delegate void invoke_rfController_contentBrowserListReadySink(String _listId);
        void rfController_contentBrowserListReadySink(String _listId)
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                this.setContentBrowserListData(_listId);
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_contentBrowserListReadySink(this.rfController_contentBrowserListReadySink), _listId);
        }

        protected void setContentBrowserListData(String _listId)
        {
            this.hideBrowseListActionContextMenu();
            this.setContentBrowserListOnUpdate(true);
            if (String.IsNullOrWhiteSpace(_listId))
                return;
            UPNPMediaListBrowse mediaList = (UPNPMediaListBrowse)rfController.contentBrowser.getList(_listId);
            this.setMediaItemListViewData(ContentBrowseListControl, mediaList);
            BrowsePath.Text = mediaList.path; 
            this.setContentBrowserListOnUpdate(false);
        }

        void contentBrowseList_ScrollChangedSink(object sender, ScrollChangedEventArgs e)
        {
            this.loadMediaItemListViewImages(ContentBrowseListControl);
        }

        protected void setContentBrowserListOnUpdate(Boolean _isOnUpdate = true)
        {
            ContentBrowseListControl.IsEnabled = !_isOnUpdate;
            ContentBrowselistProgressRingControl.Visibility = _isOnUpdate ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        public void saveContentBrowseListPosition()
        {
            MediaList mediaList = rfController.contentBrowser.getCurrentList();
            if (mediaList == null)
                return;

            this.saveMediaItemListPosition(ContentBrowseListControl, mediaList);
        }

        private void contentBrowserSwitchMyMusic_ClickSink(object sender, RoutedEventArgs e)
        {
            this.switchContentBrowserContent(ContentDirectoryMainContentType.MyMusic);
        }

        private void contentBrowserSwitchNapster_ClickSink(object sender, RoutedEventArgs e)
        {
            this.switchContentBrowserContent(ContentDirectoryMainContentType.Napster);
        }

        private void contentBrowserSwitchSimfy_ClickSink(object sender, RoutedEventArgs e)
        {
            this.switchContentBrowserContent(ContentDirectoryMainContentType.Simfy);
        }

        private void contentBrowserSwitchPlaylists_ClickSink(object sender, RoutedEventArgs e)
        {
            this.switchContentBrowserContent(ContentDirectoryMainContentType.Playlists);
        }

        private void contentBrowserSwitchLastFm_ClickSink(object sender, RoutedEventArgs e)
        {
            this.switchContentBrowserContent(ContentDirectoryMainContentType.LastFm);
        }

        private void contentBrowserSwitchTuneIn_ClickSink(object sender, RoutedEventArgs e)
        {
            this.switchContentBrowserContent(ContentDirectoryMainContentType.TuneIn);
        }

        private void contentBrowserSwitchRootButton_ClickSink(object sender, RoutedEventArgs e)
        {
            this.switchContentBrowserContent(ContentDirectoryMainContentType.Root);
        }

        private void switchContentBrowserContent(ContentDirectoryMainContentType _contentType)
        {
            if (rfController.contentBrowser == null)
                return;
            this.saveContentBrowseListPosition();
            this.setContentBrowserListOnUpdate(true);
            rfController.contentBrowser.switchMainContent(_contentType);           
            this.updateSearchControls();
        }
    }
}
