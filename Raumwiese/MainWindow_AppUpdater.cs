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
using AppUpdater;

namespace Raumwiese
{
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow, IDropTarget
    {
        AppUpdater.AppUpdater appUpdater;

        protected void initAppUpdater()
        {
            String curVersion= System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            appUpdater = new AppUpdater.AppUpdater();
            appUpdater.updateCheckDone += appUpdater_updateCheckDoneSink;
            appUpdater.setAppVersionInfoFile(@"http://www.bassmaniacs.com/data/appversion.xml");
            //appUpdater.setAppVersionInfoFile(@"appversion.xml");
            appUpdater.setPollTime(60 * 60);
            appUpdater.checkForUpdate("raumwiese", curVersion);
        }

        private void VersionUpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            if (appUpdater != null)
                appUpdater.applyUpdate();
        }

        delegate void invoke_appUpdater_updateCheckDoneSink(AppUpdater.AppUpdater.UpdateAvailable _updateAvailable, AppUpdater.AppUpdater.AppVersionInfo _appVersionInfo);
        protected void appUpdater_updateCheckDoneSink(AppUpdater.AppUpdater.UpdateAvailable _updateAvailable, AppUpdater.AppUpdater.AppVersionInfo _appVersionInfo)
        {
            if (this.VersionUpdateInfo.Dispatcher.CheckAccess())
            {
                if (_updateAvailable == AppUpdater.AppUpdater.UpdateAvailable.Yes && !String.IsNullOrEmpty(_appVersionInfo.version))
                {
                    VersionUpdateInfo.Content = String.Format("Neue Version ({0}) verfügbar!", _appVersionInfo.version);
                    VersionUpdateInfo.Visibility = System.Windows.Visibility.Visible;
                }
                if (_updateAvailable == AppUpdater.AppUpdater.UpdateAvailable.Unknown)
                {
                    VersionUpdateInfo.Content = String.Format("Check auf neue Version fehlgeschlagen!");
                    VersionUpdateInfo.Visibility = System.Windows.Visibility.Visible;
                }
                if (_updateAvailable == AppUpdater.AppUpdater.UpdateAvailable.No)
                {
                    VersionUpdateInfo.Content = "";
                    VersionUpdateInfo.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
                this.VersionUpdateInfo.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_appUpdater_updateCheckDoneSink(this.appUpdater_updateCheckDoneSink), _updateAvailable, _appVersionInfo); 
        }

    }
}