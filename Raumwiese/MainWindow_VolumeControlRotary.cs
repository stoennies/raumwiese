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

        protected void initVolumeControlsRotary()
        {
        }

        private void ButtonVolumeRotaryControl_ClickSink(object sender, RoutedEventArgs e)
        {
            this.toggleVolumeControlFlyout();
        }

        private void toggleVolumeControlFlyout()
        {
            this.setVolumeControlFlyoutOpen(!Flyouts[2].IsOpen);
        }

        private void setVolumeControlFlyoutOpen(Boolean _open = true)
        {
            Flyouts[2].IsOpen = _open;
        }

        void volumeRotaryControl_valueChangedTimeStepSink(RaumfeldNET.WPFControls.RotaryVolumeControl _control, uint _newValue)
        {
            Object objContext = this.getContextObjectFromDataContext(_control.DataContext);
            if (objContext == null)
                return;
            this.setVolumeForContext(objContext, _newValue);
        }

        void volumeRotaryControl_muteStateChangedSink(RaumfeldNET.WPFControls.RotaryVolumeControl _control, bool _newValue)
        {
            Object objContext = this.getContextObjectFromDataContext(_control.DataContext);
            if (objContext == null)
                return;
            this.setMuteForContext(objContext, _newValue);
        }

    }
}
