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


namespace RaumfeldNET.WPFControls
{
    public partial class SearchContentControl : UserControl
    {
        public uint freezeChangeTimeMS;
        System.Timers.Timer freezeTimer;

        // event for valueChanged (by mouse, touch or any userinteraction with UI)
        public delegate void searchStringChangedHandler(SearchContentControl _searchContentControl, String _newValue);
        public event searchStringChangedHandler searchStringChanged;

        // event for valueChanged (by mouse, touch or any userinteraction with UI) and did not change within the "freezeChangeTimeMS"
        public event searchStringChangedHandler searchStringChangedFreeze;

        public delegate void searchTypeChangedHandler(SearchContentControl _searchContentControl, ContentDirectorySearchType _newValue);
        public event searchTypeChangedHandler searchTypeChanged;

        public static readonly DependencyProperty SelectedSearchTypeProperty = DependencyProperty.Register("SelectedSearchType", typeof(ContentDirectorySearchType), typeof(SearchContentControl), new PropertyMetadata(default(ContentDirectorySearchType)));
        public ContentDirectorySearchType SelectedSearchType
        {
            get { return (ContentDirectorySearchType)GetValue(SelectedSearchTypeProperty); }
            set 
            { 
                SetValue(SelectedSearchTypeProperty, value);
                UpdateSearchTypeSelection();
            }
        }

        public SearchContentControl()
        {
            InitializeComponent();
            TextBoxSearchObject.TextChanged += TextBoxSearchObject_TextChanged;
            TextBoxSearchObject.KeyUp += TextBoxSearchObject_KeyUp;
            freezeChangeTimeMS = 500;
            SelectedSearchType = ContentDirectorySearchType.Artist;
        }

        void TextBoxSearchObject_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (searchStringChanged != null) this.searchStringChanged(this, TextBoxSearchObject.Text);
                if (searchStringChangedFreeze != null) this.searchStringChangedFreeze(this, TextBoxSearchObject.Text);                
            }
        }

        protected void UpdateSearchTypeSelection()
        {
            Color stdTextColor = Color.FromRgb(0, 0, 0);
            SolidColorBrush stdTxtBrush = new SolidColorBrush(stdTextColor);

            SearchButtonArtist.Foreground = stdTxtBrush;
            SearchButtonAlbum.Foreground = stdTxtBrush;
            SearchButtonTrack.Foreground = stdTxtBrush;

            switch (this.SelectedSearchType)
            {
                case ContentDirectorySearchType.Album:
                    SearchButtonAlbum.Foreground = this.Resources["AccentColorBrush"] as SolidColorBrush;
                    break;
                case ContentDirectorySearchType.Artist:
                    SearchButtonArtist.Foreground = this.Resources["AccentColorBrush"] as SolidColorBrush;
                    break;
                case ContentDirectorySearchType.Track:
                    SearchButtonTrack.Foreground = this.Resources["AccentColorBrush"] as SolidColorBrush;
                    break;
            }
        }

        public void SetFocusToSearchText()
        {
            Keyboard.Focus(TextBoxSearchObject);
        }

        public void SetSearchEnabled(Boolean _enabled = true)
        {
            if (!_enabled)
            {
                SearchGrid.Visibility = System.Windows.Visibility.Hidden;
                SearchGridNotAvailable.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                SearchGrid.Visibility = System.Windows.Visibility.Visible;
                SearchGridNotAvailable.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void TextBoxSearchObject_TextChanged(object _sender, TextChangedEventArgs _e)
        {
            String text = TextBoxSearchObject.Text;
            if (freezeChangeTimeMS > 0)
            {
                if (freezeTimer != null)
                    freezeTimer.Dispose();
                freezeTimer = new System.Timers.Timer(freezeChangeTimeMS);
                freezeTimer.Elapsed += (sender, e) => freezeTimer_Elapsed(sender, e, text);
                freezeTimer.Start();
            }

            if (searchStringChanged != null) this.searchStringChanged(this, text);
        }

        public String getSearchString()
        {
            return TextBoxSearchObject.Text;
        }

        delegate void invokeFreezeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, String _oldValue);
        void freezeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, String _oldValue)
        {
            if (this.TextBoxSearchObject.Dispatcher.CheckAccess())
            {
                if (freezeTimer != null)
                {
                    if (_oldValue == TextBoxSearchObject.Text)
                    {
                        freezeTimer.Stop();
                        if (searchStringChangedFreeze != null) this.searchStringChangedFreeze(this, TextBoxSearchObject.Text);
                    }
                }
            }
            else
                this.TextBoxSearchObject.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new invokeFreezeTimer_Elapsed(this.freezeTimer_Elapsed), sender, e, _oldValue);
        }

        private void SearchButtonArtist_Click(object sender, RoutedEventArgs e)
        {
            SelectedSearchType = ContentDirectorySearchType.Artist;
            UpdateSearchTypeSelection();
            if (searchTypeChanged != null) this.searchTypeChanged(this, SelectedSearchType);
        }

        private void SearchButtonTrack_Click(object sender, RoutedEventArgs e)
        {
           SelectedSearchType = ContentDirectorySearchType.Track;
           UpdateSearchTypeSelection();
           if (searchTypeChanged != null) this.searchTypeChanged(this, SelectedSearchType);
        }

        private void SearchButtonAlbum_Click(object sender, RoutedEventArgs e)
        {
            SelectedSearchType = ContentDirectorySearchType.Album;
            UpdateSearchTypeSelection();
            if (searchTypeChanged != null) this.searchTypeChanged(this, SelectedSearchType);
        }
    }
}
