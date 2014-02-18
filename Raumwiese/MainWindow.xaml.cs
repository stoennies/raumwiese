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
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow, IDropTarget
    {
        RaumfeldNET.Controller  rfController;

        public enum VisualContent
        {
            Loading,
            Main
        };

        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledExceptionSink;
            InitializeComponent();           
            this.Loaded += MainWindow_LoadedSink;
            this.Closing += MainWindow_ClosingSink;
            this.SizeChanged += MainWindow_SizeChangedSink;
            this.LocationChanged += MainWindow_LocationChangedSink;
            MahApps.Metro.ThemeManager.ChangeTheme(this, MahApps.Metro.ThemeManager.DefaultAccents.First(x => x.Name == "Blue"), MahApps.Metro.Theme.Dark);
        }


        protected void CurrentDomain_UnhandledExceptionSink(object _sender, UnhandledExceptionEventArgs _eventArgs)
        {  
            try
            {
                Exception e = (Exception)_eventArgs.ExceptionObject;
                MessageBox.Show(e.Message, e.Source);
            }
            catch (Exception _x)
            {
                MessageBox.Show(_x.Message, _x.Source);
            }
          
        }

        protected void MainWindow_ClosingSink(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (rfController != null)
                rfController.shutDown();
        }

        protected String getArgValue(String _argId)
        {
            if (App.mArgs != null && App.mArgs.Contains(_argId))
            {
                int index = Array.FindIndex( App.mArgs, row => row.Contains(_argId));
                if (App.mArgs.Count() >= index + 1)
                    return App.mArgs[index + 1];
            }
            return "";
        }

        protected void MainWindow_LoadedSink(object sender, RoutedEventArgs e)
        {
            RaumfeldNET.UPNP.NetworkConnectInfo networkConnectionInfo;
            networkConnectionInfo.subNetIndex = 0;

            String subnetIndex = this.getArgValue("-subnetindex");
            if (!String.IsNullOrWhiteSpace(subnetIndex))
            {
                int result;
                if (int.TryParse(subnetIndex, out result))
                    networkConnectionInfo.subNetIndex = Convert.ToInt16(subnetIndex);
            }
            
            rfController = new RaumfeldNET.Controller(networkConnectionInfo);

            if (App.mArgs != null && App.mArgs.Contains("-logwarning"))
                rfController.logWriter.setLogLevel(RaumfeldNET.Log.LogType.Warning);
            if (App.mArgs != null && App.mArgs.Contains("-logall"))
                rfController.logWriter.setLogLevel(RaumfeldNET.Log.LogType.Info);

            rfController.mediaServerFound += rfController_mediaServerFoundSink;
            rfController.mediaServerRemoved += rfController_mediaServerRemovedSink;
            rfController.allRenderersLinked += rfController_allRenderersLinkedSink;
            rfController.zonesRetrieved += rfController_zonesRetrievedSink;
            rfController.zoneTrackListReady += rfController_zoneTrackListReadySink;
            rfController.zoneTrackChanged += rfController_zoneTrackChangedSink;
            rfController.zoneTrackPositionChanged += rfController_zoneTrackPositionChangedSink;

            this.initAppUpdater();
            this.initZoneTrackListControl();
            this.initContentBrowserListControl();
            this.initContentBrowserListSearch();
            this.initScopeHandler();
            this.initVolumeControls();
            this.initVolumeControlsRotary();
            this.initPlaylistManagement();
            this.initContentBrowserContextMenu();
            this.initPlaylistManagementContextMenu();

            this.switchVisualContent(VisualContent.Loading);
            this.setZoneDataOnUpdate(true);
            this.setZoneTrackListOnUpdate(true);

            rfController.init();

            VersionInfo.Content = "Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ComponentDispatcher.ThreadIdle += ComponentDispatcher_ThreadIdleSink;
        }

        void MainWindow_SizeChangedSink(object sender, SizeChangedEventArgs e)
        {
            ZoneVolumePopup.IsOpen = false;
            this.hideBrowseListActionContextMenu();
            this.hidePlaylistManagementActionContextMenu();
        }

        void MainWindow_LocationChangedSink(object sender, EventArgs e)
        {
            ZoneVolumePopup.IsOpen = false;
            this.hideBrowseListActionContextMenu();
            this.hidePlaylistManagementActionContextMenu();
        }

    }
}
