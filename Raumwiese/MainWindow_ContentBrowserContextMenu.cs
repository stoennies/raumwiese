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

        private void initContentBrowserContextMenu()
        {
            ContentBrowseListControl.PreviewMouseUp += contentBrowseListControl_PreviewMouseUpSink;
            ContentBrowseListControl.SelectionChanged += contentBrowseListControl_SelectionChangedSink;
            ContentBrowseListControl.MouseRightButtonUp += ContentBrowseListControl_MouseRightButtonUpSink;

            ContentBrowserContextMenu.actionCloseMenue += contentBrowserContextMenu_actionCloseMenueSink;
            ContentBrowserContextMenu.actionPlayNext += contentBrowserContextMenu_actionPlayNextSink;
            ContentBrowserContextMenu.actionPlayNow += contentBrowserContextMenu_actionPlayNowSink;
            ContentBrowserContextMenu.actionPlayAtEnd += contentBrowserContextMenu_actionPlayAtEndSink;
            
        }


        protected void ContentBrowseListControl_MouseRightButtonUpSink(object sender, MouseButtonEventArgs e)
        {
            MediaItem mediaInfo;
            ListBoxItem lbi = getListBoxItemFromMouse(ContentBrowseListControl, e);

            if (lbi == null)
                return;

            mediaInfo = (MediaItem)lbi.Content;
            if (mediaInfo == null)
                return;

            if (mediaInfo.isAllowedToDropOnTrackList())
            {
                ListBoxItem listBoxItem = (ListBoxItem)ContentBrowseListControl.ItemContainerGenerator.ContainerFromItem(mediaInfo);
                this.showBrowseListActionContextMenu(mediaInfo, e.GetPosition(ContentBrowseListControl), listBoxItem, this.getSelectedItemsFromMediaItemList(ContentBrowseListControl));
            }
        }

        void contentBrowseListControl_SelectionChangedSink(object sender, SelectionChangedEventArgs e)
        {
            this.hideBrowseListActionContextMenu();
        }

        void contentBrowserContextMenu_actionPlayAtEndSink(RaumfeldNET.WPFControls.ContentContextMenuControl _control, RoutedEventArgs e)
        {
            this.currentZoneTrackList().addAtEnd(this.getSelectedItemsFromMediaItemList(ContentBrowseListControl));
            this.hideBrowseListActionContextMenu();
        }

        void contentBrowserContextMenu_actionPlayNowSink(RaumfeldNET.WPFControls.ContentContextMenuControl _control, RoutedEventArgs e)
        {
            this.currentZoneTrackList().playNow(this.getSelectedItemsFromMediaItemList(ContentBrowseListControl));
            this.hideBrowseListActionContextMenu();
        }

        void contentBrowserContextMenu_actionPlayNextSink(RaumfeldNET.WPFControls.ContentContextMenuControl _control, RoutedEventArgs e)
        {
            this.currentZoneTrackList().addNext(this.getSelectedItemsFromMediaItemList(ContentBrowseListControl));
            this.hideBrowseListActionContextMenu();
        }

        protected void hideBrowseListActionContextMenu()
        {
            ContentBrowserContextMenuPopup.IsOpen = false;
        }

        void contentBrowserContextMenu_actionCloseMenueSink(RaumfeldNET.WPFControls.ContentContextMenuControl _control, RoutedEventArgs e)
        { 
            this.hideBrowseListActionContextMenu();
        }
        
        private void contentBrowseListControl_PreviewMouseUpSink(object sender, MouseButtonEventArgs e)
        {
            MediaItem mediaInfo;
            ListBoxItem lbi = getListBoxItemFromMouse(ContentBrowseListControl, e);

            if (lbi == null)
                return;

            mediaInfo = (MediaItem)lbi.Content;
            if (mediaInfo == null)
                return;

            if (mediaInfo.isAllowedToDropOnTrackList())
            {
                ListBoxItem listBoxItem = (ListBoxItem)ContentBrowseListControl.ItemContainerGenerator.ContainerFromItem(mediaInfo);
                if (e.GetPosition(ContentBrowseListControl).X <= 55)
                    this.showBrowseListActionContextMenu(mediaInfo, e.GetPosition(ContentBrowseListControl), listBoxItem, this.getSelectedItemsFromMediaItemList(ContentBrowseListControl));
            }
        }


        protected void showBrowseListActionContextMenu(MediaItem _mediaItem, Point _point, ListBoxItem lbi, List<MediaItem> _mediaItems=null)
        {
            String text;

            if (ContentBrowseListControl.SelectedItem == null)
                return;

            if (!_mediaItem.isAllowedToDropOnTrackList())
                return;

            if (_mediaItems != null && !MediaItem.isAllowedToDropOnTrackList(_mediaItems))
                return;

            text = _mediaItem.text;

            if (_mediaItems != null && _mediaItems.Count > 1)
            {
                text += String.Format(" und {0} weitere", _mediaItems.Count - 1);
            }

            Point relativeLocation = ContentBrowseListControl.TranslatePoint(new Point(0, 0), lbi);

            //BrowseListActionPopupContentImage.DataContext = _mediaInformation;
            //BrowseListActionPopupContentInfo.Text = _mediaInformation.text;            
            ContentBrowserContextMenuPopup.Width = ContentBrowseListControl.ActualWidth;            
            ContentBrowserContextMenuPopup.DataContext = _mediaItem;
            ContentBrowserContextMenu.Text = text;
            //ContentBrowserContextMenuPopup.Tag = _mediaItems;
            ContentBrowserContextMenuPopup.PlacementRectangle = new Rect(0, _point.Y - (_point.Y - (relativeLocation.Y * -1) - 1), ContentBrowseListControl.ActualWidth + 1, 50);
            ContentBrowserContextMenuPopup.IsOpen = true;
            ContentBrowserContextMenuPopup.StaysOpen = false;
        }
    }
}
