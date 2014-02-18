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
using RaumfeldNET.Renderer;
using RaumfeldControllerWPFControls;
using MahApps;
using MahApps.Metro.Controls;
using GongSolutions.Wpf.DragDrop;

namespace Raumwiese
{
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow, IDropTarget
    {

        protected void initZoneTrackListProgressControl()
        {
            ZoneTracklistProgressBar.PreviewMouseLeftButtonUp += ZoneTracklistProgressBar_PreviewMouseLeftButtonUpSink;
        }

        protected void ZoneTracklistProgressBar_PreviewMouseLeftButtonUpSink(object sender, MouseButtonEventArgs e)
        {
            ProgressBar progressBar = (ProgressBar)sender;
            double progressBarValueClicked;

            if (String.IsNullOrWhiteSpace(currentSelectedZoneUDN) || String.IsNullOrWhiteSpace(this.currentZone().rendererUDN))
                return;

            progressBarValueClicked = getProgressBarValueFromMousePos(progressBar, e);
            progressBar.Value = (int)progressBarValueClicked;

            TimeSpan t = TimeSpan.FromSeconds(progressBarValueClicked);
            string seek = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);

            if (this.currentZone() == null || this.currentZone().getRenderer() == null)
                return;
            this.currentZone().getRenderer().seek(seek);
        }

        private double getProgressBarValueFromMousePos(ProgressBar progressBar, MouseButtonEventArgs e)
        {
            double onePixelDuration;
            double mousePosition = e.GetPosition(progressBar).X;

            onePixelDuration = (progressBar.Maximum / progressBar.ActualWidth);
            return onePixelDuration * mousePosition;
        }


        delegate void invoke_rfController_zoneTrackChangedSink(String _zoneUDN, uint _newTrackIdx);
        protected void rfController_zoneTrackChangedSink(String _zoneUDN, uint _newTrackIdx)
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                if (String.IsNullOrWhiteSpace(currentSelectedZoneUDN) || String.IsNullOrWhiteSpace(this.currentZone().rendererUDN))
                    return;

                if (currentSelectedZoneUDN == _zoneUDN && this.currentZone().getRenderer() != null)
                {
                    ZoneTracklistProgressBar.Maximum = this.currentZone().getRenderer().currentTrack.getDurationInSeconds();
                }
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_zoneTrackChangedSink(this.rfController_zoneTrackChangedSink), _zoneUDN, _newTrackIdx);
        }

        delegate void invoke_rfController_zoneTrackPositionChangedSink(String _zoneUDN, String _absPos);
        void rfController_zoneTrackPositionChangedSink(String _zoneUDN, String _absPos)
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                if (String.IsNullOrWhiteSpace(currentSelectedZoneUDN) || this.currentZone() == null || String.IsNullOrWhiteSpace(this.currentZone().rendererUDN))
                    return;

                if (currentSelectedZoneUDN == _zoneUDN && this.currentZone().getRenderer() != null)
                {
                    if (ZoneTracklistProgressBar.Value == 0)
                        ZoneTracklistProgressBar.Maximum = this.currentZone().getRenderer().currentTrack.getDurationInSeconds();
                    ZoneTracklistProgressBar.Value = this.currentZone().getRenderer().currentTrack.getAbsTimePosInSeconds();
                }
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_zoneTrackPositionChangedSink(this.rfController_zoneTrackPositionChangedSink), _zoneUDN, _absPos);
        }

    }
}
