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
using System.Timers;
using System.Collections;
using System.Collections.ObjectModel;

using RaumfeldNET;
using RaumfeldNET.Renderer;
using RaumfeldNET.WPFControls;
//using RaumfeldControllerWPFControls;
using MahApps;
using MahApps.Metro.Controls;
using GongSolutions.Wpf.DragDrop;

namespace Raumwiese
{
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow, IDropTarget
    {
        Timer stateChangedTimer;
        
        protected void setZoneTrackListData(String _zoneUDN)
        {
            this.setZoneTrackListOnUpdate(true);
            Zone zone = rfController.zoneManager.getZone(_zoneUDN);
            if (!String.IsNullOrWhiteSpace(zone.trackListId))
            {
                MediaList mediaList = rfController.zoneTitleListManager.getList(zone.trackListId);
                if (mediaList != null)
                    this.setMediaItemListViewData(ZoneTracklistControl, mediaList);
                else
                {
                }
            }
            this.setZoneTrackListOnUpdate(false);
        }

        delegate void invoke_rfController_zoneTrackListReadySink(String _zoneUDN, String _trackListId);
        protected void rfController_zoneTrackListReadySink(String _zoneUDN, String _trackListId)
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                if (currentSelectedZoneUDN == _zoneUDN)                                    
                    this.setZoneTrackListData(_zoneUDN); 
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_zoneTrackListReadySink(this.rfController_zoneTrackListReadySink), _zoneUDN, _trackListId);
        }

        protected void initZoneTrackListControl()
        {
            ScrollViewer sv = (ScrollViewer)this.getScrollViewer(ZoneTracklistControl);
            sv.ScrollChanged += zoneTrackList_ScrollChangedSink;
            ZoneTracklistControl.KeyUp += ZoneTracklistControl_KeyUpSink;
            ZoneTracklistControl.MouseDoubleClick += ZoneTracklistControl_MouseDoubleClickSink;
            GongSolutions.Wpf.DragDrop.DragDrop.SetDropHandler(ZoneTracklistControl, this);
            rfController.zonePlayStateChanged += rfController_zonePlayStateChangedSink;
            rfController.zonePlayModeChanged += rfController_zonePlayModeChangedSink;
        }

        delegate void invoke_rfController_zonePlayModeChangedSink(String _zoneUDN, RaumfeldNET.UPNP.AvTransportPlayMode _playMode);
        private void rfController_zonePlayModeChangedSink(string _zoneUDN, RaumfeldNET.UPNP.AvTransportPlayMode _playMode)
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                this.setTrackListControlFromZone(ZoneTracklistTrackControl, _zoneUDN);
            }
            else
            this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                new invoke_rfController_zonePlayModeChangedSink(this.rfController_zonePlayModeChangedSink), _zoneUDN, _playMode);
        }

        protected void setTrackListControlFromZone(TrackStateControl _control,  String _zoneUdn)
        {
            Zone zone = rfController.zoneManager.getZone(_zoneUdn);
            RaumfeldNET.UPNP.AvTransportPlayMode playMode;
            RendererPlayState playState;

            if (zone.getRenderer() == null)
                return;

            playMode = zone.getRenderer().playMode;
            playState = zone.getRenderer().playState;

            _control.RandomizeTrack =
                    playMode == RaumfeldNET.UPNP.AvTransportPlayMode.Random ||
                    playMode == RaumfeldNET.UPNP.AvTransportPlayMode.Shuffle;

            if (playMode == RaumfeldNET.UPNP.AvTransportPlayMode.Random ||
                playMode == RaumfeldNET.UPNP.AvTransportPlayMode.RepeatAll)
                _control.RepeatTacks = TrackStateControlRepeat.RepeatAll;
            if (playMode == RaumfeldNET.UPNP.AvTransportPlayMode.RepeatOne)
                _control.RepeatTacks = TrackStateControlRepeat.RepeatOne;
            if (playMode == RaumfeldNET.UPNP.AvTransportPlayMode.Normal ||
                playMode == RaumfeldNET.UPNP.AvTransportPlayMode.Shuffle)
                _control.RepeatTacks = TrackStateControlRepeat.Normal;

            if (playState == RendererPlayState.Playing)
                _control.TrackPaused = false;
            else
                _control.TrackPaused = true;
        }

       /* delegate void invoke_rfController_zonePlayStateChangedSink(String _zoneUDN, RendererPlayState _playState);
        private void rfController_zonePlayStateChangedSink(String _zoneUDN, RendererPlayState _playState)
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                if (currentSelectedZoneUDN == _zoneUDN)
                {
                    this.zoneTracklistSetTrackPlayingState();
                    //this.zoneTracklistSetPlayPauseButton(_playState);
                }
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_zonePlayStateChangedSink(this.rfController_zonePlayStateChangedSink), _zoneUDN, _playState);
        }*/

        protected void ZoneTracklistControl_MouseDoubleClickSink(object sender, MouseButtonEventArgs e)
        {
            this.saveZoneTrackListPosition();
            this.zoneTracklistPlaySelectedTrack();
        }

        private void zoneTrackList_ScrollChangedSink(object sender, ScrollChangedEventArgs e)
        {
            this.loadMediaItemListViewImages(ZoneTracklistControl);
        }

        protected void setZoneTrackListOnUpdate(Boolean _isOnUpdate = true)
        {
            ZoneTracklistControl.IsEnabled = !_isOnUpdate;
            ZoneTracklistProgressRingControl.Visibility = _isOnUpdate ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        private void ZoneTracklistControl_KeyUpSink(object sender, KeyEventArgs e)
        {
            this.saveZoneTrackListPosition();
            if (e.Key == Key.Delete)
            {                
                this.setZoneTrackListOnUpdate(true);
                this.removeSelectedItemsFromZoneTrackList();
            }
            if (e.Key == Key.Enter)
            {
                this.saveZoneTrackListPosition();
                this.zoneTracklistPlaySelectedTrack();
            }
        }

        // plays selected track from currently selected playlist
        protected void zoneTracklistPlaySelectedTrack()
        {
            if (ZoneTracklistControl.SelectedItem == null)
                return;
            MediaItem_Track trackItem = (MediaItem_Track)ZoneTracklistControl.SelectedItem;
            RendererVirtual rendererVirtual = rfController.rendererManager.getRendererByZoneUDN(currentSelectedZoneUDN);
            if (rendererVirtual == null)
                return;
            rendererVirtual.playTrack(trackItem.listIndex);
        }

        public ZoneTrackMediaList currentZoneTrackList()
        {
            return rfController.zoneTitleListManager.getListForZone(currentSelectedZoneUDN);
        }

        public void saveZoneTrackListPosition()
        {
            if(String.IsNullOrWhiteSpace(currentSelectedZoneUDN))
                return;

            MediaList mediaList = rfController.zoneTitleListManager.getListForZone(currentSelectedZoneUDN);
            if (mediaList == null)
                return;

            this.saveMediaItemListPosition(ZoneTracklistControl, mediaList);
        }

        protected void zoneTracklistSetTrackPlayingState()
        {
            this.zoneTracklistSetPlayPauseButtonByZoneUDN(currentSelectedZoneUDN);
            //this.setPlayStateChangedTimer();
        }

        protected void setPlayStateChangedTimer()
        {
            stateChangedTimer = new Timer(200);
            stateChangedTimer.Elapsed += playStateChangedTimerElapsedSink;
            stateChangedTimer.AutoReset = false;
            stateChangedTimer.Start();
        }

        protected void playStateChangedTimerElapsedSink(object sender, ElapsedEventArgs e)
        {
            if (this.currentZoneTrackList()!=null)
                this.currentZoneTrackList().setListItemSelectedForPlaying();
        }

        protected void zoneTracklistSetPlayPauseButtonByZoneUDN(String _zoneUDN)
        {
            Zone zone = rfController.zoneManager.getZone(_zoneUDN);
            if (zone == null)
                return;
            RendererVirtual renderer = zone.getRenderer();
            if (renderer == null)
                return;
            this.zoneTracklistSetPlayPauseButton(renderer.playState);
        }

        protected void zoneTracklistSetPlayPauseButton(RendererPlayState _playState)
        {
            this.setTrackListControlFromZone(ZoneTracklistTrackControl, currentSelectedZoneUDN);
            /*if (_playState == RendererPlayState.Playing)
            {
                ZoneTracklistTrackControl.TrackPaused = false;
            }
            else
            {
                ZoneTracklistTrackControl.TrackPaused = true;
            }*/
        }

        private void ZoneTracklistControlNext_ClickSink(object sender, RoutedEventArgs e)
        {
            RendererVirtual rendererVirtual = rfController.rendererManager.getRendererByZoneUDN(currentSelectedZoneUDN);
            if (rendererVirtual == null)
                return;
            rendererVirtual.next();
        }

        private void ZoneTracklistControlBack_ClickSink(object sender, RoutedEventArgs e)
        {
            RendererVirtual rendererVirtual = rfController.rendererManager.getRendererByZoneUDN(currentSelectedZoneUDN);
            if (rendererVirtual == null)
                return;
            rendererVirtual.previous();
        }

        private void ZoneTracklistControlPlayPause_ClickSink(object sender, RoutedEventArgs e)
        {            
            TrackStateControl control = (TrackStateControl)sender;
            RendererVirtual rendererVirtual = rfController.rendererManager.getRendererByZoneUDN(currentSelectedZoneUDN);
            if (rendererVirtual == null)
                return;
            rendererVirtual.playPauseToggle();
        }

        delegate void invoke_rfController_zonePlayStateChangedSink(String _zoneUDN, RendererPlayState _playState);
        private void rfController_zonePlayStateChangedSink(String _zoneUDN, RendererPlayState _playState)
        {
            
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                if (currentSelectedZoneUDN == _zoneUDN)
                {
                    this.zoneTracklistSetTrackPlayingState();
                    //this.zoneTracklistSetPlayPauseButton(_playState);
                }
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_zonePlayStateChangedSink(this.rfController_zonePlayStateChangedSink), _zoneUDN, _playState);
        }

        private ListBoxItem getZoneTracklistListBoxItemPlaying()
        {
            lock (ZoneTracklistControl)
            {
                foreach (MediaItem_Track mediaItemTrack in ZoneTracklistControl.Items)
                {
                    if (mediaItemTrack._isSelectedForPlaying)
                    {
                        ListBoxItem listBoxItem = (ListBoxItem)ZoneTracklistControl.ItemContainerGenerator.ContainerFromItem(mediaItemTrack);
                        return listBoxItem;
                    }
                }
            }
            return null;
        }

        protected void removeSelectedItemsFromZoneTrackList()
        {
            SortedList<int, int> deleteList;
            int rowStartPos = -1, prevPos = -1, curPos = -1;
            IEnumerator selectedItemsEnum = ZoneTracklistControl.SelectedItems.GetEnumerator();
            IEnumerator<KeyValuePair<int, int>> listEnum;;

            deleteList = this.removeSelectedItemsFromMediaItemList(ZoneTracklistControl);
                                  
            ZoneTrackMediaList trackList = rfController.zoneTitleListManager.getListForZone(currentSelectedZoneUDN);

            // run through sorted list from the backwards    
            listEnum = deleteList.GetEnumerator();
            while (listEnum.MoveNext())
            {
                curPos = listEnum.Current.Value;
                if (rowStartPos == -1)
                    rowStartPos = curPos;
                if (prevPos != -1 && prevPos - 1 != curPos)
                {
                    trackList.removeItem(prevPos, rowStartPos);
                    rowStartPos = curPos;
                }
                prevPos = curPos;
            }
            trackList.removeItem(curPos, rowStartPos);
        }

        private RaumfeldNET.UPNP.AvTransportPlayMode getPlayModeFromRepeatAndRandom(Boolean _randomize, TrackStateControlRepeat _repeat)
        {
            if (_randomize && _repeat == TrackStateControlRepeat.Normal)
                return RaumfeldNET.UPNP.AvTransportPlayMode.Shuffle;
            if (_randomize && _repeat == TrackStateControlRepeat.RepeatAll)
                return RaumfeldNET.UPNP.AvTransportPlayMode.Random;
            if (_randomize && _repeat == TrackStateControlRepeat.RepeatOne)
                return RaumfeldNET.UPNP.AvTransportPlayMode.Random;
            if (!_randomize && _repeat == TrackStateControlRepeat.RepeatAll)
                return RaumfeldNET.UPNP.AvTransportPlayMode.RepeatAll;
            if (!_randomize && _repeat == TrackStateControlRepeat.RepeatOne)
                return RaumfeldNET.UPNP.AvTransportPlayMode.RepeatOne;
            return RaumfeldNET.UPNP.AvTransportPlayMode.Normal;
        }

        private TrackStateControlRepeat getNextTrackRepeatState(TrackStateControlRepeat _curState)
        {
            if (_curState == TrackStateControlRepeat.Normal)
                return TrackStateControlRepeat.RepeatOne;
            if (_curState == TrackStateControlRepeat.RepeatOne)
                return TrackStateControlRepeat.RepeatAll;
            return TrackStateControlRepeat.Normal;
        }

        private void ZoneTracklistControlRandom_ClickSink(object sender, RoutedEventArgs e)
        {
            RendererVirtual rendererVirtual = rfController.rendererManager.getRendererByZoneUDN(currentSelectedZoneUDN);
            if (rendererVirtual == null)
                return;
            rendererVirtual.setPlayMode(getPlayModeFromRepeatAndRandom(!ZoneTracklistTrackControl.RandomizeTrack, ZoneTracklistTrackControl.RepeatTacks));
        }

        private void ZoneTracklistControlRepeat_ClickSink(object sender, RoutedEventArgs e)
        {
            RendererVirtual rendererVirtual = rfController.rendererManager.getRendererByZoneUDN(currentSelectedZoneUDN);
            if (rendererVirtual == null)
                return;
            rendererVirtual.setPlayMode(getPlayModeFromRepeatAndRandom(ZoneTracklistTrackControl.RandomizeTrack, getNextTrackRepeatState(ZoneTracklistTrackControl.RepeatTacks)));
        }

    }

}

