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

        // this method fills a given listView with Raumfeld mediaList Data and scrolls the view to a specific position if given on the mediaList
        public void setMediaItemListViewData(ListBox _listView, MediaList _mediaList)
        {

            try
            {
                lock (_listView)
                {
        
                    _listView.ItemsSource = _mediaList.list;
                    _listView.Items.Refresh();

                    ScrollViewer sv = (ScrollViewer)this.getScrollViewer(_listView);
                    if (sv != null && _mediaList != null && _mediaList.visualPosition != null)
                        sv.ScrollToVerticalOffset((double)_mediaList.visualPosition);
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            this.loadMediaItemListViewImages(_listView);
        }

        // use this method ro start async requests to load the images on the view
        // this method will be called after a list is ready with new data or if some scrolling was done
        // it starts request for all visible items in list where there is no image data already loaded.
        public void loadMediaItemListViewImages(ListBox _listView)
        {
            IEnumerator<MediaItem> listEnum;
            MediaItem mediaInformation;

            listEnum = this.getVisibleMediaItemsInListBox(_listView).GetEnumerator();
            while (listEnum.MoveNext())
            {
                mediaInformation = (MediaItem)listEnum.Current;
                mediaInformation.loadImage();
            }
        }

        // save visual position of a list
        public void saveMediaItemListPosition(ListBox _list, MediaList _mediaList)
        {
            if (_list == null || _mediaList == null)
                return;

            ScrollViewer sv = (ScrollViewer)this.getScrollViewer(_list);
            if (sv != null)
                _mediaList.visualPosition = sv.VerticalOffset;
        }

        // this methods get the currently visible items in a ListBox Control
        public IList<MediaItem> getVisibleMediaItemsInListBox(ListBox _listView)
        {
            IList<MediaItem> itemList = new List<MediaItem>();
            ScrollViewer sv = (ScrollViewer)this.getScrollViewer(_listView);

            if (sv == null)
                return itemList;

            int FirstVisibleItem = (int)sv.VerticalOffset;
            int VisibleItemCount = (int)sv.ViewportHeight;
            int itemIdx;

            if (_listView.Items.Count == 0)
                return itemList;

            for (itemIdx = FirstVisibleItem; itemIdx <= FirstVisibleItem + VisibleItemCount && itemIdx < _listView.Items.Count; itemIdx++)
                itemList.Add((MediaItem)_listView.Items[itemIdx]);

            return itemList;
        }

        // method gets scroll viewer object of an object
        public DependencyObject getScrollViewer(DependencyObject _o)
        {
            // Return the DependencyObject if it is a ScrollViewer
            if (_o is ScrollViewer)
            { return _o; }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(_o); i++)
            {
                var child = VisualTreeHelper.GetChild(_o, i);

                var result = getScrollViewer(child);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
            return null;
        }

        // remove items from list
        protected SortedList<int, int> removeSelectedItemsFromMediaItemList(ListBox _listView)
        {
            IEnumerator selectedItemsEnum = _listView.SelectedItems.GetEnumerator();            
            SortedList<int, int> deleteList = new SortedList<int, int>();
            ListBoxItem listBoxItem;
            Object trackInfo;
            int listBoxItemIndex;
            MediaItem mediaItem;

            // selected list is unsorted so we have to sort it for deletion to begin from the last one
            while (selectedItemsEnum.MoveNext())
            {
                trackInfo = selectedItemsEnum.Current;
                mediaItem = (MediaItem)trackInfo;
                listBoxItem = (ListBoxItem)_listView.ItemContainerGenerator.ContainerFromItem(trackInfo);
                //if (listBoxItem != null) // TODO: @@@@@ WHYYYYY? Weil ich es invisible gmacht hab?
                {
                    //listBoxItemIndex = _list.ItemContainerGenerator.IndexFromContainer(listBoxItem);
                    listBoxItemIndex = mediaItem.listIndex;
                    deleteList.Add(999999 - listBoxItemIndex, listBoxItemIndex);
                }
            }

            return deleteList;            
        }

        protected ListBoxItem getListBoxItemFromMouse(ListBox _listBox, MouseButtonEventArgs e)
        {
            HitTestResult result = VisualTreeHelper.HitTest(_listBox, e.GetPosition(_listBox));
            if (result == null)
                return null;
            ListBoxItem lbi = findVisualParent<ListBoxItem>(result.VisualHit);
            return lbi;
        }


        protected List<MediaItem> getSelectedItemsFromMediaItemList(ListBox _listView)
        {
            Object trackInfo;
            MediaItem mediaItem;
            IEnumerator selectedItemsEnum = _listView.SelectedItems.GetEnumerator();
            List<MediaItem> list = new List<MediaItem>();
            SortedList<int, MediaItem> listSorted = new SortedList<int, MediaItem>();
            while (selectedItemsEnum.MoveNext())
            {
                trackInfo = selectedItemsEnum.Current;
                mediaItem = (MediaItem)trackInfo;
                listSorted.Add(mediaItem.listIndex, mediaItem);
            }
            selectedItemsEnum = listSorted.GetEnumerator();
            foreach (var pair in listSorted)
            {
                mediaItem = (MediaItem)pair.Value;
                list.Add(mediaItem);
            }
            return list;
        }

    }
}
