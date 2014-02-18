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
        protected void initScopeHandler()
        {
            
        }


        protected void switchVisualContent(VisualContent _visualContent)
        {
            switch (_visualContent)
            {
                case VisualContent.Loading:
                    MainGrid.Visibility = System.Windows.Visibility.Hidden;
                    MainGridStartup.Visibility = System.Windows.Visibility.Visible;
                    break;
                case VisualContent.Main:
                    MainGridStartup.Visibility = System.Windows.Visibility.Hidden;
                    MainGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }
        
        private void buttonPlaylistManagementControl_ClickSink(RaumfeldNET.WPFControls.ImageButtonControl _imageButtonControl, MouseButtonEventArgs e)
        {
            if (!mediaServerPresent)
                return;
            this.switchVisualContent(VisualContent.Main);
            GridZonePlaylistControl.Visibility = System.Windows.Visibility.Hidden;
            GridPlaylistManagementControl.Visibility = System.Windows.Visibility.Visible;
        }

        private void buttonHomeControl_ClickSink(RaumfeldNET.WPFControls.ImageButtonControl _imageButtonControl, MouseButtonEventArgs e)
        {
            if (!mediaServerPresent)
                return;
            this.switchVisualContent(VisualContent.Main);
            GridZonePlaylistControl.Visibility = System.Windows.Visibility.Visible;
            GridPlaylistManagementControl.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
