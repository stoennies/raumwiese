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
    public partial class PlaylistManagementContextMenuControl : UserControl
    {


        public delegate void actionRenamePlaylistHandler(PlaylistManagementContextMenuControl _control, String _playlistObjectId, String _newName);
        public event actionRenamePlaylistHandler actionRenamePlaylist;

        public delegate void actionDeletePlaylistHandler(PlaylistManagementContextMenuControl _control, String _playlistObjectId);
        public event actionDeletePlaylistHandler actionDeletePlaylist;

        public delegate void actionHandler(PlaylistManagementContextMenuControl _control, RoutedEventArgs e);
        public event actionHandler actionCloseMenue;

        public PlaylistManagementContextMenuControl()
        {
            InitializeComponent();
        }

        private void buttonRenamePlaylist_ClickSink(object sender, RoutedEventArgs e)
        {
            MediaItem mediaItem = (MediaItem)this.DataContext;
            if (actionRenamePlaylist != null) actionRenamePlaylist(this, mediaItem.objectId, PlaylistNameTextBox.Text);
        }

        private void buttonDeletePlaylist_ClickSink(object sender, RoutedEventArgs e)
        {
            MediaItem mediaItem = (MediaItem)this.DataContext;
            if (actionDeletePlaylist != null) actionDeletePlaylist(this, mediaItem.objectId);
        }

        private void buttonClose_ClickSink(object sender, RoutedEventArgs e)
        {
            if (actionCloseMenue != null) actionCloseMenue(this, e);
        }
       
    }
}
