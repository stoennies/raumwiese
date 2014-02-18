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

        protected void initVolumeControls()
        {
            rfController.rendererVolumeChanged += rfController_rendererVolumeChangedSink;
            rfController.rendererMuteStateChanged += rfController_rendererMuteStateChangedSink;
        }

        // sets the volume for the context object which may be a zone or a room (renderer volume set)       
        protected void setVolumeForContext(Object _object, uint _volume)
        {
            Zone zone;
            Room roomInfo;

            if (_object is Zone)
            {
                zone = (Zone)_object;
                if (zone.getRenderer()!=null)
                    zone.getRenderer().setVolume(_volume);               
            }
            if (_object is Room)
            {
                roomInfo = (Room)_object;
                if (roomInfo.getRenderer() != null)
                    roomInfo.getRenderer().setVolume(_volume);                 
            }
        }

        protected void setMuteForContext(Object _object, Boolean _mute)
        {
            Zone zone;
            Room roomInfo;

            if (_object is Zone)
            {
                zone = (Zone)_object;
                if (zone.getRenderer() != null)
                    zone.getRenderer().setMute(_mute);
            }
            if (_object is Room)
            {
                roomInfo = (Room)_object;
                if (roomInfo.getRenderer() != null)
                    roomInfo.getRenderer().setMute(_mute);
            }
        }

        // creates the volume controls in a Stack panel
        protected void createAndInitVolumeControls(String _zoneUDN)
        {
            Room roomInformation;
            Zone zone = rfController.zoneManager.getZone(_zoneUDN);
            RaumfeldNET.WPFControls.VolumeSliderControl volumeControl = new RaumfeldNET.WPFControls.VolumeSliderControl();

            if (_zoneUDN != currentSelectedZoneUDN)
                return;

            if (zone.getRenderer() != null)
            {
                ZoneMainVolumeControl.IsMute = zone.getRenderer().isMuted;
                ZoneMainVolumeControl.Value = zone.getRenderer().currentVolume;

                MainVolumeRotaryControl.IsMute = zone.getRenderer().isMuted;
                MainVolumeRotaryControl.Value = zone.getRenderer().currentVolume;
            }
            ZoneMainVolumeControl.DataContext = new Object[2] { typeof(Zone), _zoneUDN };
            ZoneMainVolumeControl.valueChangedTimeStep += volumeControl_valueChangedTimeStepSink;
            ZoneMainVolumeControl.muteStateChanged += volumeControl_muteStateChangedSink;

            MainVolumeRotaryControl.DataContext = new Object[2] { typeof(Zone), _zoneUDN };
            MainVolumeRotaryControl.valueChangedTimeStep += volumeRotaryControl_valueChangedTimeStepSink;
            MainVolumeRotaryControl.muteStateChanged += volumeRotaryControl_muteStateChangedSink;
            MainVolumeRotaryControl.SliderText = "Master";

            ZoneVolumePopupRendererStackPanel.Children.Clear();

            /*
            volumeControl.DataContext = new Object[2] { typeof(Zone), _zoneUDN};
            //volumeControl.Width = SelectedZoneVolumeSlider.ActualWidth;
            //volumeControl.Width = 300;
            if (volumeControl.Width < volumeControl.MinWidth)
                volumeControl.Width = volumeControl.MinWidth;
            volumeControl.valueChangedTimeStep += volumeControl_valueChangedTimeStepSink;
            volumeControl.muteStateChanged += volumeControl_muteStateChangedSink;
            volumeControl.SliderText = "Master";

            ZoneVolumePopupRendererStackPanel.Children.Add(volumeControl);*/
            
            // create for rooms (if more than one room exists)       
            List<String> roomUDNs = rfController.zoneManager.getLinkedRooms(_zoneUDN);
            if (roomUDNs.Count() > 1)
            {
                for (int i = 0; i < roomUDNs.Count(); i++)
                {
                    // create for room renderer                                                  
                    roomInformation = rfController.zoneManager.getRoom(roomUDNs[i]);

                    volumeControl = new RaumfeldNET.WPFControls.VolumeSliderControl();
                    volumeControl.DataContext = new Object[2] { typeof(Room), roomInformation.udn};
                    volumeControl.Color = (Color)ColorConverter.ConvertFromString(roomInformation.color);
                    //volumeControl.Width = SelectedZoneVolumeSlider.ActualWidth;
                    //volumeControl.Width = 300;
                    if (volumeControl.Width < volumeControl.MinWidth)
                        volumeControl.Width = volumeControl.MinWidth;
                    volumeControl.valueChangedTimeStep += volumeControl_valueChangedTimeStepSink;
                    volumeControl.muteStateChanged += volumeControl_muteStateChangedSink;
                    volumeControl.SliderText = roomInformation.name;

                    ZoneVolumePopupRendererStackPanel.Children.Add(volumeControl);
                }
            }

            this.updateVolumeControlValues(_zoneUDN);           
        }

        void volumeControl_valueChangedTimeStepSink(RaumfeldNET.WPFControls.VolumeSliderControl _volumeSliderControl, uint _newValue)
        {
            Object objContext = this.getContextObjectFromDataContext(_volumeSliderControl.DataContext);
            if (objContext == null)
                return;
            this.setVolumeForContext(objContext, _newValue);
        }

        void volumeControl_muteStateChangedSink(RaumfeldNET.WPFControls.VolumeSliderControl _volumeSliderControl, bool _newValue)
        {
            Object objContext = this.getContextObjectFromDataContext(_volumeSliderControl.DataContext);
            if (objContext == null)
                return;
            this.setMuteForContext(objContext, _newValue);
        }

        Object getContextObjectFromDataContext(Object _dataContext)
        {
            Object[] obj = (Object[])_dataContext;
            Object objContext = null;
            if(obj[0] == typeof(Zone))
                objContext =  rfController.zoneManager.getZone((String)obj[1]);
            if (obj[0] == typeof(Room))
                objContext = rfController.zoneManager.getRoom((String)obj[1]);
            return objContext;
        }

        protected void updateVolumeControlValues(String _zoneUDN)
        {
            RaumfeldNET.WPFControls.VolumeSliderControl volumeControl;
            var volumeControls = ZoneVolumePopupRendererStackPanel.Children.OfType<RaumfeldNET.WPFControls.VolumeSliderControl>();
            Zone zone;
            Room roomInfo;
            double currentVolume = 0;
            Boolean isMuted = false;
            Object contextObject;

            if (_zoneUDN != currentSelectedZoneUDN)
                return;

            for (int i = 0; i < volumeControls.Count<RaumfeldNET.WPFControls.VolumeSliderControl>(); i++)
            {
                volumeControl = volumeControls.ElementAt<RaumfeldNET.WPFControls.VolumeSliderControl>(i);
                contextObject = getContextObjectFromDataContext(volumeControl.DataContext);

                if (contextObject is Zone)
                {
                    zone = (Zone)contextObject;
                    if (zone.getRenderer() != null)
                    {
                        currentVolume = zone.getRenderer().currentVolume;
                        isMuted = zone.getRenderer().isMuted;
                    }
                }
                if (contextObject is Room)
                {
                    roomInfo = (Room)contextObject;
                    if (roomInfo.getRenderer() != null)
                    {
                        currentVolume = roomInfo.getRenderer().currentVolume;
                        isMuted = roomInfo.getRenderer().isMuted;
                    }
                }

                volumeControl.Value = (uint)currentVolume;
                volumeControl.IsMute = isMuted;
            }

        }

        private void buttonZoneVolumeControl_ClickSink(object sender, RoutedEventArgs e)
        {
            ZoneVolumePopup.IsOpen = !ZoneVolumePopup.IsOpen;
            //ZoneVolumePopup.StaysOpen = false;
        }

        delegate void invokRendererMuteStateChangedSink(string _rendererUDN, bool _mute);
        void rfController_rendererMuteStateChangedSink(string _rendererUDN, bool _mute)
        {
            if (this.ZoneVolumePopup.Dispatcher.CheckAccess())
            {
                RaumfeldNET.Renderer.Renderer renderer = rfController.rendererManager.getRenderer(_rendererUDN);
                if (!String.IsNullOrWhiteSpace(currentSelectedZoneUDN) && renderer != null && renderer.isVirtualRenderer())
                {
                    RaumfeldNET.Renderer.RendererVirtual virtualRenderer = (RaumfeldNET.Renderer.RendererVirtual)renderer;
                    if (currentSelectedZoneUDN == virtualRenderer.zoneUDN)
                    {
                        ZoneMainVolumeControl.IsMute = virtualRenderer.isMuted;
                        MainVolumeRotaryControl.IsMute = virtualRenderer.isMuted;
                    }
                }
                this.updateVolumeControlValues(currentSelectedZoneUDN);
            }
            else
                this.ZoneVolumePopup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new invokRendererMuteStateChangedSink(this.rfController_rendererMuteStateChangedSink), _rendererUDN, _mute);
        }

        delegate void invokeRendererVolumeChangedSink(string _rendererUDN, uint _volume);
        void rfController_rendererVolumeChangedSink(string _rendererUDN, uint _volume)
        {
            if (this.ZoneVolumePopup.Dispatcher.CheckAccess())
            {
                RaumfeldNET.Renderer.Renderer renderer = rfController.rendererManager.getRenderer(_rendererUDN);
                if (!String.IsNullOrWhiteSpace(currentSelectedZoneUDN) && renderer != null && renderer.isVirtualRenderer())
                {
                    RaumfeldNET.Renderer.RendererVirtual virtualRenderer = (RaumfeldNET.Renderer.RendererVirtual)renderer;
                    if (currentSelectedZoneUDN == virtualRenderer.zoneUDN)
                    {
                        ZoneMainVolumeControl.Value = virtualRenderer.currentVolume;
                        MainVolumeRotaryControl.Value = virtualRenderer.currentVolume;
                    }
                }
                this.updateVolumeControlValues(currentSelectedZoneUDN);
            }
            else
                this.ZoneVolumePopup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new invokeRendererVolumeChangedSink(this.rfController_rendererVolumeChangedSink), _rendererUDN, _volume);
        }

    }
}
