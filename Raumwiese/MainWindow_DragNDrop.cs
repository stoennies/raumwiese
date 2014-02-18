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
        void IDropTarget.DragOver(IDropInfo _dropInfo)
        {
            ListBox listBoxTarget, listBoxSource;

            if (_dropInfo.DragInfo == null)
                return;

            listBoxTarget = (ListBox)_dropInfo.VisualTarget;
            listBoxSource = (ListBox)_dropInfo.DragInfo.VisualSource;

            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.DragOver(_dropInfo);

            if (listBoxTarget.Name == "ZoneTracklistControl")
                this.DragOverZoneTrackList(_dropInfo);
            if (listBoxTarget.Name == "PlaylistManagementListControl")
                this.DragOverPlaylistManagementList(_dropInfo);

        }

        void IDropTarget.Drop(IDropInfo _dropInfo)
        {
          
            ListBox listBoxTarget, listBoxSource;
            listBoxTarget = (ListBox)_dropInfo.VisualTarget;
            listBoxSource = (ListBox)_dropInfo.DragInfo.VisualSource;

            if (listBoxTarget.Name == "ZoneTracklistControl")
                this.DropOnZoneTrackList(_dropInfo);
            if (listBoxTarget.Name == "PlaylistManagementListControl")
                this.DropOnPlaylistManagementList(_dropInfo);
        }

        protected void DragOverZoneTrackList(IDropInfo _dropInfo)
        {
            MediaItem mediaItem = (MediaItem)_dropInfo.DragInfo.SourceItem;
            ListBox listBoxTarget, listBoxSource;

            listBoxTarget = (ListBox)_dropInfo.VisualTarget;
            listBoxSource = (ListBox)_dropInfo.DragInfo.VisualSource;

            if (!mediaItem.isAllowedToDropOnTrackList())
            {
                _dropInfo.Effects = System.Windows.DragDropEffects.None;
                return;
            }

            this.saveZoneTrackListPosition();

            // TODO: @@@ deny playlist update?! Or try Lock list?
            //currentlySelectedZone.SetDenyPlaylistUpdate(true);

            listBoxTarget = (ListBox)_dropInfo.VisualTarget;
            listBoxSource = (ListBox)_dropInfo.DragInfo.VisualSource;

            if (listBoxTarget.Name == listBoxSource.Name)
                _dropInfo.Effects = System.Windows.DragDropEffects.Move;
            else
                _dropInfo.Effects = System.Windows.DragDropEffects.Copy;
        }

        protected void DropOnZoneTrackList(IDropInfo _dropInfo)
        {
            ListBox listBoxTarget, listBoxSource;
            ZoneTrackMediaList zoneTrackList = this.currentZoneTrackList();
            

            listBoxTarget = (ListBox)_dropInfo.VisualTarget;
            listBoxSource = (ListBox)_dropInfo.DragInfo.VisualSource;

            if (String.IsNullOrWhiteSpace(currentSelectedZoneUDN))
                return;

            this.saveZoneTrackListPosition();

            this.setZoneTrackListOnUpdate(true);

            MediaItem mediaItem = (MediaItem)_dropInfo.DragInfo.SourceItem;
            if (listBoxTarget.Name == listBoxSource.Name)
                if (_dropInfo.InsertIndex > mediaItem.listIndex)
                    zoneTrackList.moveItem(mediaItem.listIndex, _dropInfo.InsertIndex - 1);
                else
                    zoneTrackList.moveItem(mediaItem.listIndex, _dropInfo.InsertIndex);
            else
            {
                List<MediaItem> selectedList = this.getSelectedItemsFromMediaItemList(listBoxSource);
                if (selectedList.Count > 1)
                    zoneTrackList.addItems(selectedList, _dropInfo.InsertIndex, true);
                else
                    zoneTrackList.addItem(mediaItem, _dropInfo.InsertIndex);
            } 
        }

        protected void DragOverPlaylistManagementList(IDropInfo _dropInfo)
        {
            MediaItem mediaItem = (MediaItem)_dropInfo.DragInfo.SourceItem;
            ListBox listBoxTarget, listBoxSource;
            PlaylistObjectMediaList playlistList = this.currentPlaylistManagementList();

            listBoxTarget = (ListBox)_dropInfo.VisualTarget;
            listBoxSource = (ListBox)_dropInfo.DragInfo.VisualSource;

            if (playlistList == null || !mediaItem.isAllowedToDropOnTrackList() || !playlistList.isEditable)
            {
                _dropInfo.Effects = System.Windows.DragDropEffects.None;
                return;
            }

            this.savePlaylistManagementListPosition();

            listBoxTarget = (ListBox)_dropInfo.VisualTarget;
            listBoxSource = (ListBox)_dropInfo.DragInfo.VisualSource;

            if (listBoxTarget.Name == listBoxSource.Name)
                _dropInfo.Effects = System.Windows.DragDropEffects.Move;
            else
                _dropInfo.Effects = System.Windows.DragDropEffects.Copy;
        }

        protected void DropOnPlaylistManagementList(IDropInfo _dropInfo)
        {
            ListBox listBoxTarget, listBoxSource;
            PlaylistObjectMediaList playlistList = this.currentPlaylistManagementList();

            listBoxTarget = (ListBox)_dropInfo.VisualTarget;
            listBoxSource = (ListBox)_dropInfo.DragInfo.VisualSource;

            if (playlistList == null || !playlistList.isEditable)
                return;

            this.savePlaylistManagementListPosition();

            this.setPlaylistManagementListOnUpdate(true);

            MediaItem mediaItem = (MediaItem)_dropInfo.DragInfo.SourceItem;
            if (listBoxTarget.Name == listBoxSource.Name)
                if (_dropInfo.InsertIndex > mediaItem.listIndex)
                    playlistList.moveItem(mediaItem.listIndex, _dropInfo.InsertIndex - 1);
                else
                    playlistList.moveItem(mediaItem.listIndex, _dropInfo.InsertIndex);
            else
            {
                List<MediaItem> selectedList = this.getSelectedItemsFromMediaItemList(listBoxSource);
                if (selectedList.Count > 1)
                    playlistList.addItems(selectedList, _dropInfo.InsertIndex, true);
                else
                    playlistList.addItem(mediaItem, _dropInfo.InsertIndex);
            }

        }
    }
}
