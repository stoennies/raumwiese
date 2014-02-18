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

        protected void initContentBrowserListSearch()
        { 
            ContentBrowserSearchControl.Click += contentBrowserSearchControl_ClickSink;
            FlyoutSearchContentControl.searchStringChangedFreeze += flyoutSearchContentControl_searchStringChangedFreezeSink;
            FlyoutSearchContentControl.searchTypeChanged += flyoutSearchContentControl_searchTypeChangedSink;
            GridSearchContentControl.searchStringChangedFreeze += gridSearchContentControl_searchStringChangedFreezeSink;
            GridSearchContentControl.searchTypeChanged += gridSearchContentControl_searchTypeChangedSink;
        }
            

        protected void searchContentBrowserList(String _searchString, ContentDirectorySearchType _searchType)
        {
            if (String.IsNullOrEmpty(_searchString))
                return;

            this.saveContentBrowseListPosition();
            this.setContentBrowserListOnUpdate(true);
            rfController.contentBrowser.search(_searchString, _searchType);
        }

        void gridSearchContentControl_searchTypeChangedSink(RaumfeldNET.WPFControls.SearchContentControl _searchContentControl, ContentDirectorySearchType _newValue)
        {
            this.searchContentBrowserList(_searchContentControl.getSearchString(), _searchContentControl.SelectedSearchType);
        }

        void gridSearchContentControl_searchStringChangedFreezeSink(RaumfeldNET.WPFControls.SearchContentControl _searchContentControl, string _newValue)
        {
            this.searchContentBrowserList(_searchContentControl.getSearchString(), _searchContentControl.SelectedSearchType);
        }

        void flyoutSearchContentControl_searchTypeChangedSink(RaumfeldNET.WPFControls.SearchContentControl _searchContentControl, ContentDirectorySearchType _newValue)
        {
            this.searchContentBrowserList(_searchContentControl.getSearchString(), _searchContentControl.SelectedSearchType);
        }

        void flyoutSearchContentControl_searchStringChangedFreezeSink(RaumfeldNET.WPFControls.SearchContentControl _searchContentControl, string _newValue)
        {
            this.searchContentBrowserList(_searchContentControl.getSearchString(), _searchContentControl.SelectedSearchType);
        }

        // search Browse list main content
        private void contentBrowserSearchControl_ClickSink(object sender, RoutedEventArgs e)
        {
            // Toggle Flyout or toogle inside search!            
            this.toggleFlyoutSearch();
            //this.toggleGridSearch();
        }

        public void updateSearchControls()
        {
            this.updateSearchFlyout();
            this.updateSearchGrid();
        }

        public void toggleFlyoutSearch()
        {
            Flyouts[1].IsOpen = !Flyouts[1].IsOpen;
            if (Flyouts[1].IsOpen)
                this.updateSearchFlyout();
        }
        
        public void toggleGridSearch()
        {

            if (GridSearchContentControl.Visibility == System.Windows.Visibility.Collapsed)
            {
                GridSearchContentControl.Visibility = System.Windows.Visibility.Visible;
                this.updateSearchGrid();
            }
            else
                GridSearchContentControl.Visibility = System.Windows.Visibility.Collapsed;
            
        }

        public void closeFlyoutSearch()
        {
            Flyouts[1].IsOpen = false;
        }

        protected void updateSearchFlyout()
        {
            if (!rfController.contentBrowser.isSearchAvailable(rfController.contentBrowser.getMainContentType()))
            {
                FlyoutSearchContentControl.SetSearchEnabled(false);
            }
            else
            {
                FlyoutSearchContentControl.SetSearchEnabled(true);
                FlyoutSearchContentControl.SetFocusToSearchText();
            }

            FlyoutSearch.Header = "Searching " + rfController.contentBrowser.getMainContentType().ToString();
        }

        protected void updateSearchGrid()
        {
            
            if (!rfController.contentBrowser.isSearchAvailable(rfController.contentBrowser.getMainContentType()))
            {
                GridSearchContentControl.SetSearchEnabled(false);
            }
            else
            {
                GridSearchContentControl.SetSearchEnabled(true);
                GridSearchContentControl.SetFocusToSearchText();
            }
            
        }
    }
}
