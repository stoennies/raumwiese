using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;

namespace RaumfeldNET.Renderer
{

    public enum RendererPlayState
    {
        Playing,
        Stopped,
        Paused,
        Transitioning
    };

    public enum RepeatState
    {
        None,
        RepeatTrack,
        RepeatAll
    };

    public enum RandomizeState
    {
        None,
        Randomize
    };

    public struct TrackInfo
    {
        public String duration;
        public String trackMetadata;
        public String absTimePos;
        public uint trackNr;

        public int getDurationInSeconds()
        {
            TimeSpan timeSpan;
            if (duration != "")
            {
                TimeSpan.TryParse(duration, out timeSpan);
                return timeSpan.Days * 68400 + timeSpan.Hours * 3600 + timeSpan.Minutes * 60 + timeSpan.Seconds;
            }
            return 0;
        }

        public int getAbsTimePosInSeconds()
        {
            TimeSpan timeSpan;
            if (absTimePos != "")
            {
                TimeSpan.TryParse(absTimePos, out timeSpan);
                return timeSpan.Days * 68400 + timeSpan.Hours * 3600 + timeSpan.Minutes * 60 + timeSpan.Seconds;
            }
            return 0;
        }
    }   


    public class Renderer : Base.Base
    {
        protected CpAVRenderer renderer;
        public String udn;
        public RendererPlayState playState;
        public TrackInfo currentTrack;

        public RandomizeState randomizeState;
        public RepeatState repeatState;

        protected Boolean isSetVolumeInProgress;
        protected int storedVolume;
        public uint currentVolume;
        public Boolean isMuted;
        public AvTransportPlayMode playMode;

        const String manufacturerRaumfeldRenderer = "Raumfeld GmbH";

        public delegate void delegate_OnMuteStateChanged(String _rendererUDN, Boolean _mute);
        public event delegate_OnMuteStateChanged muteStateChanged;

        public delegate void delegate_OnVolumeChanged(String _rendererUDN, uint _volume);
        public event delegate_OnVolumeChanged volumeChanged;

        public Renderer(CpAVRenderer _avRenderer)
        {
            renderer = _avRenderer;
            udn = renderer.UniqueDeviceName;
            currentTrack = new TrackInfo();
           
        }

        public void setListen()
        {
            renderer.Connections[0].onPlayStateChanged += playStateChangedSink;
            renderer.Connections[0].onTrackChanged += trackChangedSink;
            renderer.Connections[0].onPositionChanged += trackPositionChangedSink;
            renderer.Connections[0].onPlayModeChanged += playModeChangedSink;

            renderer.onVolumeChanged += volumeChangedSink;
            renderer.onMuteChanged += muteStatusChangedSink;
        }

        public CpAVRenderer getRendererObject()
        {
            return renderer;
        }

        public virtual Boolean isVirtualRenderer()
        {
            return false;
        }

        public virtual Boolean isRoomRenderer()
        {
            return false;
        }

        public virtual Boolean isRaumfeldRenderer()
        {
            if (renderer.Manufacturer == manufacturerRaumfeldRenderer)
                return true;
            return false;
        }

        public virtual void play()
        {
            renderer.Connections[0].Play();
        }              

        public virtual void stop()
        {
            renderer.Connections[0].Stop();
        }

        public virtual void seek(System.String _target)
        {
            renderer.Connections[0].Seek(_target);
        }

        public virtual void next()
        {
            renderer.Connections[0].Next();
        }

        public virtual void previous()
        {
            renderer.Connections[0].Previous();
        }

        public virtual void pause()
        {
            renderer.Connections[0].Pause();
        }

        public virtual void playPauseToggle()
        {
            if (playState == RendererPlayState.Stopped || playState == RendererPlayState.Paused)
                this.play();
            else if (playState == RendererPlayState.Playing)
                this.pause();           
        }

        protected virtual void playModeChangedSink(CpAVConnection _connection, AvTransportPlayMode _playMode)
        {
            playMode = _playMode;
        }

        protected virtual void playStateChangedSink(CpAVConnection connection, Object _playState)
        {
            String playStateString = (String)_playState;

            switch (playStateString)
            {
                case "PLAYING":
                    playState = RendererPlayState.Playing;
                    break;
                case "PAUSED_PLAYBACK":
                    playState = RendererPlayState.Paused;
                    break;
                case "TRANSITIONING":
                    playState = RendererPlayState.Transitioning;
                    break;
                case "STOPPED":
                    playState = RendererPlayState.Stopped;
                    break;
            }            

        }

        protected virtual void trackChangedSink(CpAVConnection _connection, uint _newTrack)
        {
            currentTrack.trackNr = _newTrack;
        }
        
        protected virtual void trackPositionChangedSink(CpAVConnection _connection, string duration, string trackMetadata, string absTime)
        {
            currentTrack.duration = duration;
            currentTrack.trackMetadata = trackMetadata;
            currentTrack.absTimePos = absTime;
        }

        public virtual void setVolume(uint _volume)
        {
            if (_volume != currentVolume)
            {
                if (!isSetVolumeInProgress)
                {
                    this.renderer.SetVolume(_volume, null, resultSetVolumeSink);
                    isSetVolumeInProgress = true;
                }
                else
                {
                    storedVolume = (int)_volume;
                }
            }
        }

        protected void resultSetVolumeSink(CpAVRenderer _renderer, object _o)
        {
            isSetVolumeInProgress = false;
            if (storedVolume > 0)
            {
                setVolume((uint)storedVolume);
                storedVolume = -1;
            }
        }

        public virtual void setMute(Boolean _bMute)
        {
            this.renderer.SetMute(_bMute);
        }

        public virtual void setPlayMode(AvTransportPlayMode _playMode)
        {
            this.renderer.Connections[0].SetPlayMode(_playMode);
        }


        protected void volumeChangedSink(CpAVRenderer _connection, ushort _volume)
        {
            currentVolume = _volume;
            if (volumeChanged != null) volumeChanged(this.udn, _volume);
        }

        protected void muteStatusChangedSink(CpAVRenderer _connection, bool _newMuteState)
        {
            isMuted = _newMuteState;
            if (muteStateChanged != null) muteStateChanged(this.udn, _newMuteState);
        }

    }

    public class RendererRoom : Renderer
    {
        public String roomUDN;

        public RendererRoom(CpAVRenderer _avRenderer)
            : base(_avRenderer)
        {
            this.setListen();
        }

        public override Boolean isRoomRenderer()
        {
            return true;
        }

        public Room getRoom()
        {
            return (Room)Global.getZoneManager().getRoom(roomUDN);
        }

        public void roomLinked()
        {
        }
    }

    public class RendererVirtual : Renderer
    {
        public String zoneUDN;
        protected Int32 currentTrackIndex;

        public RendererVirtual(CpAVRenderer _avRenderer) 
            : base(_avRenderer)
        {
            renderer.Connections[0].initTimer(); // hmmm.... doesnt work for any reason?!            
            this.setListen();
        }

        protected override void playModeChangedSink(CpAVConnection _connection, AvTransportPlayMode _playMode)
        {
            base.playModeChangedSink(_connection, _playMode);
            if (this.getZone() != null)
                this.getZone().setPlayModeChanged(_playMode);
        }

        public ZoneTrackMediaList getTrackMediaList()
        {
            if (String.IsNullOrWhiteSpace(zoneUDN))
                return null;
            return Global.getZoneTitleListManager().getListForZone(zoneUDN);
        }

        protected void updatePlayingTrackIndexOnTrackList()
        {
            ZoneTrackMediaList mediaList = this.getTrackMediaList();
            if (mediaList != null)
            {
                mediaList.currentTrackIndexPlaying = currentTrackIndex;
                mediaList.setListItemSelectedForPlaying();
            }
        }

        public override Boolean isVirtualRenderer()
        {
            return true;
        }

        public Zone getZone()
        {
            if (String.IsNullOrWhiteSpace(zoneUDN))
                return null;
            return (Zone)Global.getZoneManager().getZone(zoneUDN);
        }

        public void zoneLinked()
        {
            renderer.Connections[0].onAVTransportURIChanged += AVTransportURIChangedSink;
            this.updatePlayingTrackIndexOnTrackList();
        }

        protected void AVTransportURIChangedSink(CpAVConnection _connection)
        {
            this.writeLog(LogType.Info, String.Format("AVTransportUri '{1}' für Renderer '{0}' empfangen", udn, _connection.AVTransportUri));
            // retrieve list for zone. we use the ZoneTitleList Manager
            Global.getZoneTitleListManager().retrieveListFromAvTransportUri(zoneUDN, _connection.AVTransportUri, _connection.AVTransportUriMetaData);
            this.getZone().trackListId = zoneUDN;
        }

        public virtual void playTrack(int _trackIndex)
        {
            ZoneTrackMediaList mediaList = Global.getZoneTitleListManager().getListForZone(zoneUDN);
            if (_trackIndex >= mediaList.list.Count)
                return;

            String metaData = Global.getContentBrowser().getMetaDataForObjectId(mediaList.list[_trackIndex].objectId);
            renderer.Connections[0].SetAvTransportUri(mediaList.buildAvTransportUri(_trackIndex), metaData);
        }

        protected override void playStateChangedSink(CpAVConnection _connection, Object _playState)
        {
            base.playStateChangedSink(_connection, _playState);
            if(this.getZone()!=null)
                this.getZone().setPlayStateChanged(playState);
        }

        protected override void trackChangedSink(CpAVConnection _connection, uint _newTrack)
        {
            base.trackChangedSink(_connection, _newTrack);
            currentTrackIndex = Convert.ToInt32(_newTrack) - 1;
            this.updatePlayingTrackIndexOnTrackList();
            if (this.getZone() != null)
                this.getZone().setTrackChanged(_newTrack);
        }

        protected override void trackPositionChangedSink(CpAVConnection _connection, string _duration, string _trackMetadata, string _absTime)
        {
            base.trackPositionChangedSink(_connection, _duration, _trackMetadata, _absTime);
            if (this.getZone() != null)
                this.getZone().setTrackPositionChanged(_absTime);
        }
    }
}
