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
        String currentSelectedZoneUDN;

        // if zones are updated, we have to rebuild the zone controls and set new current selected zoneId
        delegate void invoke_rfController_zonesRetrievedSink();
        void rfController_zonesRetrievedSink()
        {            
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                this.updateZoneControls();
                this.setZoneDataOnUpdate(false);
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_zonesRetrievedSink(this.rfController_zonesRetrievedSink));
        }
        
        void zoneTileControl_selectedSink(RaumfeldNET.WPFControls.ZoneTileControl _zoneTileControl)
        {
            this.zoneSelected((String)_zoneTileControl.DataContext);
            this.setFlyoutSelectedZonesOpen(false);
        }

        void zoneTileControl_MoveRemoveRoomSink(RaumfeldNET.WPFControls.ZoneTileControl _zoneTileControl, RaumfeldNET.WPFControls.RoomStripControl _roomStripControl, RoutedEventArgs e)
        {
            Room room = rfController.zoneManager.getRoom(_roomStripControl.RoomId);
            if (room != null)
                this.showAvailableZonesPopup(_roomStripControl, room.zoneUDN);
        }

        void roomStripControl_AddRoomToZoneSink(RaumfeldNET.WPFControls.RoomStripControl _roomStripControl, RoutedEventArgs e)
        {
            Room room = rfController.zoneManager.getRoom(_roomStripControl.RoomId);
            if (room != null)
                this.showAvailableZonesPopup(_roomStripControl, room.zoneUDN);
        }

        void zoneSelected(String _zoneUDN)
        {
            currentSelectedZoneUDN = _zoneUDN;
            if (rfController.zoneManager.getZone(_zoneUDN) == null)
                return;
            SelectedZoneInfo.Text = rfController.zoneManager.getZone(_zoneUDN).name.Replace("\n",", ");            
            this.setZoneTrackListData(_zoneUDN);
            this.zoneTracklistSetTrackPlayingState();
            this.createAndInitVolumeControls(_zoneUDN);
        }

        protected void setZoneDataOnUpdate(Boolean _isOnUpdate = true)
        {
            FlyoutZonesGrid.IsEnabled = !_isOnUpdate;
            FlyoutZoneSelectionProgressRing.Visibility = _isOnUpdate ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        public void updateZoneControls()
        {
            List<Zone> zoneList = rfController.zoneManager.getZones();
            List<Room> roomList = rfController.zoneManager.getRooms(); 
            Zone zone;
            Room room;

            lock (zoneList)
            {

                // remove all prior added controls
                FlyoutZonesDockPanel.Children.RemoveRange(0, 1000);
                FlyoutUnassignedRoomsDockPanel.Children.RemoveRange(0, 1000);

                // add zone tiles
                for (int i = 0; i < zoneList.Count; i++)
                {
                    zone = (Zone)zoneList[i];
                    RaumfeldNET.WPFControls.ZoneTileControl zoneTileControl = new RaumfeldNET.WPFControls.ZoneTileControl();

                    zoneTileControl.Title = zone.name;
                    zoneTileControl.DataContext = zone.udn;
                    zoneTileControl.Background = this.Resources["AccentColorBrush"] as SolidColorBrush;

                    zoneTileControl.selected +=zoneTileControl_selectedSink;
                    zoneTileControl.roomRemoveAddButtonClicked +=zoneTileControl_MoveRemoveRoomSink;
                
                    foreach (var roomUdn in zone.roomUDNs)
                    {
                        room = rfController.zoneManager.getRoom(roomUdn);
                        if (room!=null)
                            zoneTileControl.AddRoomControl(room);
                    }

                    DockPanel.SetDock(zoneTileControl, Dock.Bottom);
                    FlyoutZonesDockPanel.Children.Add(zoneTileControl);

                    // reselect selected zone, if no zone was selected we select the first one his will be doen outside the for loop
                    if (!String.IsNullOrWhiteSpace(currentSelectedZoneUDN) && currentSelectedZoneUDN == zone.udn)
                        this.zoneSelected(currentSelectedZoneUDN);
                }

                // add unnasigned room tiles
                for (int i = 0; i < roomList.Count; i++)
                {
                    room = (Room)roomList[i];
                    if (String.IsNullOrWhiteSpace(room.zoneUDN))
                    {
                        RaumfeldNET.WPFControls.RoomStripControl roomStripControl = new RaumfeldNET.WPFControls.RoomStripControl();
                        roomStripControl.RoomId = room.udn;
                        roomStripControl.RoomName = room.name;
                        roomStripControl.RoomColor = (Color)ColorConverter.ConvertFromString(room.color);
                        roomStripControl.DataContext = room;

                        roomStripControl.removeAddButtonClicked += roomStripControl_AddRoomToZoneSink;

                        DockPanel.SetDock(roomStripControl, Dock.Bottom);
                        FlyoutUnassignedRoomsDockPanel.Children.Add(roomStripControl);
                    }
                }
            }         

            // old selected list was not found, or its the first startup, so select first zone as selected zone!
            if (String.IsNullOrWhiteSpace(currentSelectedZoneUDN) && zoneList.Count > 0)
            {
                currentSelectedZoneUDN = zoneList[0].udn;
                this.zoneSelected(currentSelectedZoneUDN);
            }
        }

        private void buttonFlyoutSelectZones_ClickSink(object sender, RoutedEventArgs e)
        {
            this.toggleFlyoutSelectedZones();
        }

        private void toggleFlyoutSelectedZones()
        {
            this.setFlyoutSelectedZonesOpen(!Flyouts[0].IsOpen);
        }

        private void setFlyoutSelectedZonesOpen(Boolean _setOpen = true)
        {
            Flyouts[0].IsOpen = _setOpen;
        }

        protected void showAvailableZonesPopup(Control _object, String _callerZoneUDN)
        {
            List<String> zoneOptions = new List<string>();
            Zone zoneDummy;
            MenuItem menuItem;

            if (_object != null)
            {
                _object.ContextMenu = new ContextMenu();

                if (String.IsNullOrEmpty(_callerZoneUDN))
                {
                    zoneDummy = new Zone();
                    zoneDummy.name = "Neue Zone erstellen...";
                    zoneDummy.uuid = "NEW";
                }
                else
                {
                    zoneDummy = new Zone();
                    zoneDummy.name = "Von Zone entfernen...";
                    zoneDummy.uuid = "REMOVE";
                }

                menuItem = new MenuItem();
                menuItem.DataContext = zoneDummy;
                menuItem.Tag = _object;
                menuItem.Header = zoneDummy.name;
                menuItem.Click += menuItemRoomAdd_ClickSink;

                _object.ContextMenu.Items.Add(menuItem);
                foreach (var zone in rfController.zoneManager.getZones())
                {
                    if (String.IsNullOrWhiteSpace(_callerZoneUDN) && _callerZoneUDN == zone.uuid)
                        continue;
                    if (!String.IsNullOrEmpty(zone.udn))
                    {
                        menuItem = new MenuItem();
                        menuItem.DataContext = zone;
                        menuItem.Tag = _object;
                        menuItem.Header = zone.name;
                        menuItem.Click += menuItemRoomAdd_ClickSink;
                        _object.ContextMenu.Items.Add(menuItem);
                    }

                }

                _object.ContextMenu.PlacementTarget = this;
                _object.ContextMenu.IsOpen = true;
            }


        }

        protected void menuItemRoomAdd_ClickSink(object sender, RoutedEventArgs e)
        {;
            MenuItem menuItem = (MenuItem)sender;
            Zone zoneToAddRoom = (Zone)menuItem.DataContext;
            RaumfeldNET.WPFControls.RoomStripControl roomStripControl = (RaumfeldNET.WPFControls.RoomStripControl)menuItem.Tag;
            Room roomInfo = (Room)roomStripControl.DataContext;
            if (zoneToAddRoom.uuid == "REMOVE")
                rfController.zoneManager.dropRoomFromZone(roomInfo.udn);
            else if (zoneToAddRoom.uuid == "NEW")
                rfController.zoneManager.connectRoomToZone(roomStripControl.RoomId, "");
            else
                rfController.zoneManager.connectRoomToZone(roomStripControl.RoomId, zoneToAddRoom.udn);

            this.setZoneDataOnUpdate(true);
        }

        public Zone currentZone()
        {
            return rfController.zoneManager.getZone(currentSelectedZoneUDN);
        }
    }
}