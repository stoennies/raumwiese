using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RaumfeldNET;
using RaumfeldControllerWPFControls;
using MahApps;
using MahApps.Metro.Controls;
using GongSolutions.Wpf.DragDrop;

namespace Raumwiese
{
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow, IDropTarget
    {
        Boolean mediaServerPresent;

        delegate void invoke_rfController_mediaServerRemovedSink();
        void rfController_mediaServerRemovedSink()
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                mediaServerPresent = false;
                this.switchVisualContent(VisualContent.Loading);
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_mediaServerRemovedSink(this.rfController_mediaServerRemovedSink));
        }

        delegate void invoke_rfController_mediaServerFoundSink();
        void rfController_mediaServerFoundSink()
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                this.switchVisualContent(VisualContent.Main);
                this.switchContentBrowserContent(ContentDirectoryMainContentType.MyMusic);
                rfController.playlistBrowser.browseToPlaylistRoot();
                mediaServerPresent = true;
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_mediaServerFoundSink(this.rfController_mediaServerFoundSink));
        }

        delegate void invoke_rfController_allRenderersLinkedSink();
        void rfController_allRenderersLinkedSink()
        {
            if (this.MainGridStartup.Dispatcher.CheckAccess())
            {
                //this.switchVisualContent(VisualContent.Main);
            }
            else
                this.MainGridStartup.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new invoke_rfController_allRenderersLinkedSink(this.rfController_allRenderersLinkedSink));
        }
    }
}
