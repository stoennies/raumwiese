using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RaumfeldNET.Renderer;

namespace RaumfeldNET
{
    public static class Global
    {
        static private Log.LogWriter logWriter;
        static public Log.LogWriter getLogWriter()
        {
            if (logWriter == null)
            {
                logWriter = new Log.LogWriter();
            }
            return logWriter;
        }

        static public String getCrashInfo()
        {
            return "Ausnahmefehler im Programm! Bitte überprüfen Sie das Logfile!";
        }

        static private MediaServerManager mediaServerManager;
        static public void setMediaServerManager(MediaServerManager _mediaServerManager)
        {
            mediaServerManager = _mediaServerManager;
        }
        static public MediaServerManager getMediaServerManager()
        {
            return mediaServerManager;
        }

        static private RendererManager rendererManager;
        static public void setRendererManager(RendererManager _rendererManager)
        {
            rendererManager = _rendererManager;
        }
        static public RendererManager getRendererManager()
        {
            return rendererManager;
        }

        static private ZoneManager zoneManager;
        static public void setZoneManager(ZoneManager _zoneManager)
        {
            zoneManager = _zoneManager;
        }
        static public ZoneManager getZoneManager()
        {
            return zoneManager;
        }

        static private ZoneTitleListManager zoneTitleListManager;
        static public void setZoneTitleListManager(ZoneTitleListManager _zoneTitleListManager)
        {
            zoneTitleListManager = _zoneTitleListManager;
        }
        static public ZoneTitleListManager getZoneTitleListManager()
        {
            return zoneTitleListManager;
        }

        static private ConfigManager configManager;
        static public void setConfigManager(ConfigManager _configManager)
        {
            configManager = _configManager;
        }
        static public ConfigManager getConfigManager()
        {
            return configManager;
        }

        static private ImageDataCache imageDataCache;
        static public void setImageDataCache(ImageDataCache _imageDataCache)
        {
            imageDataCache = _imageDataCache;
        }
        static public ImageDataCache getImageDataCache()
        {
            return imageDataCache;
        }


        static private ContentDirectoryBrowserMulti contentBrowser;
        static public void setContentBrowser(ContentDirectoryBrowserMulti _contentBrowser)
        {
            contentBrowser = _contentBrowser;
        }
        static public ContentDirectoryBrowserMulti getContentBrowser()
        {
            return contentBrowser;
        }

        static private PlaylistBrowser playlistBrowser;
        static public void setPlaylistBrowser(PlaylistBrowser _playlistBrowser)
        {
            playlistBrowser = _playlistBrowser;
        }
        static public PlaylistBrowser getPlaylistBrowser()
        {
            return playlistBrowser;
        }

        static public TrackPlayState rendererPlayStateToTrackPlayState(RendererPlayState _playState)
        {
            switch (_playState)
            {
                case RendererPlayState.Playing:
                    return TrackPlayState.Playing;
                case RendererPlayState.Stopped:
                    return TrackPlayState.Stopped;
                case RendererPlayState.Paused:
                    return TrackPlayState.Paused;
                case RendererPlayState.Transitioning:
                    return TrackPlayState.Playing;
                default:
                    return TrackPlayState.Stopped;
            }
        }

    }
}
