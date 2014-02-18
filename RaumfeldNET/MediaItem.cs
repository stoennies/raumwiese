using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Drawing;
using System.Net;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;

using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;
using RaumfeldNET.Renderer;

namespace RaumfeldNET
{
    public enum MediaItemSourceType
    {
        ContentDirectory,
        Rhapsody
    };

    public enum MediaItemItemType
    {
        Track,
        Album,
        Artist,
        Genre,
        Composer,
        TrackContainer,
        Container,
        Radio,
        LastFm,
        Playlist,
        Shuffle,
        RhapsodyRadio,
        StorageFolder,
        LineIn
    };

    public enum MediaItemStatus
    {
        Available,
        Unavailable
    };

    public enum TrackPlayState
    {
        Playing,
        Stopped,
        Paused
    };

    
    public class MediaItem : INotifyPropertyChanged
    {
        public MediaItemSourceType sourceType;
        public MediaItemItemType itemType;

        public String objectId;
        public String sourceId;

        public String upnpItemClass;

        public String imageUrl;

        public String imageId;
        public Boolean imageLoaded;

        public Int32 listIndex;

        public MediaItemStatus itemStatus; 

        protected Base.BaseXmlDocument xmlDocumentHelper;
       
        protected Image uncachedImage;
       

        // notify event
        public event PropertyChangedEventHandler PropertyChanged;
        protected void notifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }


        protected String _text;
        public String text
        {
            get { return _text; }
            set { _text = value; notifyPropertyChanged("text");}
        }

        // we can't store the BitmapImage directly into the class because of multithreading!
        // we always have to create a new BitmapImage to stay thread safe here
        public BitmapImage imageBitmapThumbnail
        {
            get
            {
                Image image;
                BitmapImage imageBitmap = null;              
                Boolean imageFound = Global.getImageDataCache().getImage(imageId, out image, true);

                if (uncachedImage!=null)
                    image = (Image)uncachedImage.Clone();
                    
                if(imageFound || image!=null)
                {
                    MemoryStream memory = new MemoryStream();
                    image.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    memory.Position = 0;
                    imageBitmap = new BitmapImage();
                    imageBitmap.BeginInit();
                    imageBitmap.StreamSource = memory;
                    imageBitmap.CacheOption = BitmapCacheOption.OnLoad;
                    imageBitmap.EndInit();
                }
                return imageBitmap;
            }
        }


        public MediaItem() 
            :base()
        {
            xmlDocumentHelper = new BaseXmlDocument();
            itemStatus = MediaItemStatus.Available;
            itemType = MediaItemItemType.Container;
            this.loadStandardImage();
        }

        public static Boolean isAllowedToDropOnTrackList(List<MediaItem> _mediaItems)
        {
            for (int idx = 0; idx < _mediaItems.Count; idx++)
            {
                if (!_mediaItems[idx].isAllowedToDropOnTrackList())
                    return false;
            }
            return true;
        }

        // use this method to load a standard image for the mediaItem
        public virtual void loadStandardImage()
        {
            Global.getImageDataCache().requestImageFromFile(@"resources/empty.png", (_imageId) => imageReadySink(_imageId, false));
        }

        // should return 'true' is media item is browsable and may containe some subItems
        public virtual Boolean isBrowsable()
        {
            return true;
        }

        // should return 'true' is media item is a container item (something like 'isBrowseable')
        public virtual Boolean isContainer()
        {
            return true;
        }

        // should return 'true' if media Item is allowed to be dropped in a playlist or tracklist
        public virtual Boolean isAllowedToDropOnTrackList()
        {
            return false;
        }

        // starts the loading and creating of the image linked to the mediaItem
        public virtual void loadImage()
        {
            if (String.IsNullOrWhiteSpace(imageUrl) || imageLoaded)
                return;
            imageLoaded = true;
            Global.getImageDataCache().requestImageFromUrl(imageUrl, (_imageid) => imageReadySink(_imageid, true));
        }

        protected void imageReadySink(String _imageId, Boolean _calledFromLoadImage)
        {
            imageId = _imageId;
            notifyPropertyChanged("imageBitmapThumbnail");
        }

        public virtual void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            this.text = xmlDocumentHelper.getChildNodeValue(_nodeItem, "dc:title", _manager);
            this.objectId = xmlDocumentHelper.getNodeAttributeValue(_nodeItem, "id", _manager);
            this.sourceId = xmlDocumentHelper.getChildNodeAttributeValue(_nodeItem, "ns:res", "sourceID", _manager);
            this.imageUrl = xmlDocumentHelper.getChildNodeValue(_nodeItem, "upnp:albumArtURI", _manager);
        }

        public static MediaItem newFromUpnpItemClass(String _upnpItemClass, String _raumfeldSection)
        {
            MediaItem mediaItem;
            
            switch (_upnpItemClass)
            {
                case "object.item.audioItem.musicTrack":
                    mediaItem = new MediaItem_Track();
                    break;
                case "object.container.album.musicAlbum":
                    mediaItem = new MediaItem_Album();
                    break;
                case "object.container.albumContainer":
                    mediaItem = new MediaItem(); // @@@
                    break;
                case "object.container.person.musicArtist":
                    mediaItem = new MediaItem_Artist();
                    break;
                case "object.container.genre.musicGenre":
                    mediaItem = new MediaItem_Genre();
                    break;
                case "object.container.person.musicComposer":
                    mediaItem = new MediaItem_Composer();
                    break;
                case "object.container.trackContainer":
                    mediaItem = new MediaItem_TrackContainer();
                    break;
                case "object.container.trackContainer.napster":
                    mediaItem = new MediaItem_TrackContainer();
                    break;                
                case "object.item.audioItem.audioBroadcast.radio":
                    mediaItem = new MediaItem_Radio();
                    break;
                case "object.item.audioItem.audioBroadcast.lastFM":
                    mediaItem = new MediaItem_LastFm();
                    break;
                case "object.item.audioItem.audioBroadcast.rhapsody":
                    mediaItem = new MediaItem_RhapsodyRadio();
                    break;
                case "object.item.audioItem.audioBroadcast.lineIn":
                    mediaItem = new MediaItem_LineIn();
                    break;
                case "object.container.playlistContainer":
                    mediaItem = new MediaItem_Playlist();
                    break;
                case "object.container.playlistContainer.queue":
                    if (_raumfeldSection == "Playlists")
                    {
                        mediaItem = new MediaItem_Playlist();
                    }
                    else
                    {
                        mediaItem = new MediaItem();
                    }
                    break;
                case "object.container":
                    mediaItem = new MediaItem();
                    break;
                case "object.container.storageFolder":
                    mediaItem = new MediaItem_StorageFolder();
                    break;
                case "object.container.favoritesContainer":
                    mediaItem = new MediaItem();
                    break;
                case "object.container.playlistContainer.shuffle":
                    mediaItem = new MediaItem_Shuffle();
                    break;
                default:
                    mediaItem = new MediaItem();
                    break;
            }

            mediaItem.upnpItemClass = _upnpItemClass;

            return mediaItem;
        }

    }

    public class MediaItem_StorageFolder : MediaItem
    {
        public MediaItem_StorageFolder() 
            :base()
        {
            this.itemType = MediaItemItemType.StorageFolder;
        }

        public override Boolean isAllowedToDropOnTrackList()
        {
            //return true;
            return false;
        }
    }

    public class MediaItem_Artist : MediaItem
    {
        protected String _artist;
        public String artist
        {
            get { return _artist; }
            set { _artist = value; notifyPropertyChanged("artist"); }
        }

        public MediaItem_Artist() 
            :base()
        {
            this.itemType = MediaItemItemType.Artist;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            base.initValuesFromXMLNode(_nodeItem, _manager);

            this.artist = xmlDocumentHelper.getChildNodeValue(_nodeItem, "upnp:artist", _manager);
            if (String.IsNullOrWhiteSpace(this.artist))
                this.artist = "Unbekannter Interpret";
        }

        public override Boolean isAllowedToDropOnTrackList()
        {
            return true;
        }
    }

    public class MediaItem_TrackContainer : MediaItem_Artist
    {
        public MediaItem_TrackContainer() 
            :base()
        {
            this.itemType = MediaItemItemType.TrackContainer;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            base.initValuesFromXMLNode(_nodeItem, _manager);
        }

        public override Boolean isAllowedToDropOnTrackList()
        {
            return true;
        }

        public override void loadImage()
        {
            if (imageLoaded)
                return;

            this.imageReadySink(this.objectId, false);

            imageLoaded = true;
            ImageBuilder coverBuilder = new ImageBuilder();
            coverBuilder.buildRandomImageCover(3, 40, randomImageReadySink);
        }

        public override void loadStandardImage()
        {
            base.loadStandardImage();
        }

        protected void randomImageReadySink(Image _image)
        {
             Global.getImageDataCache().addImage(this.objectId, _image);
             this.imageReadySink(this.objectId, false);
        }
    }

    public class MediaItem_Shuffle : MediaItem_TrackContainer
    {

        public MediaItem_Shuffle()
            : base()
        {
            this.itemType = MediaItemItemType.Shuffle;
        }

        public override Boolean isBrowsable()
        {
            return false;
        }
    }

    public class MediaItem_Playlist : MediaItem_TrackContainer
    {
        uint _trackCount;
        public uint trackCount
        {
            get { return _trackCount; }
            set { _trackCount = value; notifyPropertyChanged("trackCount"); }
        }

          public MediaItem_Playlist() 
            :base()
        {
            this.itemType = MediaItemItemType.Playlist;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            base.initValuesFromXMLNode(_nodeItem, _manager);

            String childCount = xmlDocumentHelper.getNodeAttributeValue(_nodeItem, "childCount", _manager);
            if (!String.IsNullOrEmpty(childCount))
                this.trackCount = Convert.ToUInt16(childCount); 
        }
    }

    public class MediaItem_Album : MediaItem_Artist
    {
        protected String _album;
        public String album
        {
            get { return _album; }
            set { _album = value; notifyPropertyChanged("album"); }
        }

        protected String _genre;
        public String genre
        {
            get { return _genre; }
            set { _genre = value; notifyPropertyChanged("genre"); }
        }

        protected String _date;
        public String date
        {
            get { return _date; }
            set { _date = value; notifyPropertyChanged("date"); }
        }

        public override Boolean isAllowedToDropOnTrackList()
        {
            return true;
        }

        public MediaItem_Album() 
            :base()
        {
            this.itemType = MediaItemItemType.Album;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            String dateString;
            
            base.initValuesFromXMLNode(_nodeItem, _manager);
 
            this.album = xmlDocumentHelper.getChildNodeValue(_nodeItem, "upnp:album", _manager);
            if (String.IsNullOrWhiteSpace(this.album))
                this.album = "Unbekanntes Album";
            this.genre = xmlDocumentHelper.getChildNodeValue(_nodeItem, "upnp:genre", _manager);
            dateString = xmlDocumentHelper.getChildNodeValue(_nodeItem, "dc:date", _manager);
            if (!String.IsNullOrWhiteSpace(dateString))
                this.date = dateString.Substring(0, 4);
        }
    }

    public class MediaItem_Track : MediaItem_Album
    {
        public Boolean _isSelectedForPlaying;
        public Boolean isSelectedForPlaying
        {
            get { return _isSelectedForPlaying; }
            set { _isSelectedForPlaying = value; notifyPropertyChanged("isSelectedForPlaying"); }
        }
        
        protected String _title;
        public String title
        {
            get { return _title; }
            set { _title = value; notifyPropertyChanged("title"); }
        }

        protected TrackPlayState _playState;
        public TrackPlayState playState
        {
            get { return _playState; }
            set { _playState = value; notifyPropertyChanged("playState"); }
        }

        public UInt32 _trackNr;
        public UInt32 trackNr
        {
            get { return _trackNr; }
            set { _trackNr = Convert.ToUInt32(value); notifyPropertyChanged("trackNr"); }
        }
        public String trackNrFormatted
        {
            get
            {
                String ret = Convert.ToString(_trackNr);
                if (ret.Length == 1)
                    ret = "0" + ret;
                return " " + ret;
            }
            set 
            { 
                int result;
                if (int.TryParse(value, out result))
                    _trackNr = Convert.ToUInt32(value);
                else
                    _trackNr = 0;
                notifyPropertyChanged("trackNr");
            }
        }

        protected UInt32 _duration;
        public UInt32 duration
        {
            get { return _duration; }
            set { _duration = value; notifyPropertyChanged("duration"); notifyPropertyChanged("durationFormated"); }
        }
        public String durationFormatted
        {
            get
            {
                TimeSpan t = TimeSpan.FromSeconds(_duration);
                string duration = string.Format("{0:D2}:{1:D2}",
                                t.Minutes + 60 * t.Hours + t.Days * 24 *60,
                                t.Seconds);
                return duration;
            }
        }


        public override Boolean isBrowsable()
        {
            return false;
        }

        public override Boolean isContainer()
        {
            return false;
        }

        public MediaItem_Track() 
            :base()
        {
            this.itemType = MediaItemItemType.Track;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            String duration;
            String convertString;

            base.initValuesFromXMLNode(_nodeItem, _manager);

            this.title = xmlDocumentHelper.getChildNodeValue(_nodeItem, "dc:title", _manager);
            if (String.IsNullOrWhiteSpace(this.title))
                this.title = "Unbekannter Titel";
            if (this.title == "Unavailable Resource")
            {
                this.itemStatus = MediaItemStatus.Unavailable;
                return;
            }
            convertString= xmlDocumentHelper.getChildNodeValue(_nodeItem, "upnp:originalTrackNumber", _manager);
            if (!String.IsNullOrWhiteSpace(convertString))
                this.trackNrFormatted = convertString;
            duration = xmlDocumentHelper.getChildNodeAttributeValue(_nodeItem, "ns:res", "duration", _manager);
            if (!String.IsNullOrWhiteSpace(duration))
            {
                TimeSpan timeSpan;
                TimeSpan.TryParse(duration, out timeSpan);
                this.duration = Convert.ToUInt16(timeSpan.Days * 68400 + timeSpan.Hours * 3600 + timeSpan.Minutes * 60 + timeSpan.Seconds);
            }
        }
    }

    public class MediaItem_LineIn : MediaItem_Track
    {
        public MediaItem_LineIn()
            : base()
        {
            this.itemType = MediaItemItemType.LineIn;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            base.initValuesFromXMLNode(_nodeItem, _manager);
            this.artist = "Line In";
        }
    }

    public class MediaItem_Radio : MediaItem_Track
    {
        protected String _radioName;
        public String radioName
        {
            get { return _radioName; }
            set { _radioName = value; notifyPropertyChanged("radioName"); }
        }

        protected UInt16 _signalStrength;
        public UInt16 signalStrength
        {
            get { return _signalStrength; }
            set { _signalStrength = value; notifyPropertyChanged("signalStrength"); }
        }

        public UInt16 _durability;
        public UInt16 durability
        {
            get { return _durability; }
            set { _durability = value; notifyPropertyChanged("durability"); }
        }

        protected String _region;
        public String region
        {
            get { return _region; }
            set { _region = value; notifyPropertyChanged("region"); }
        }

        protected String _protocolInfo;
        public String protocolInfo
        {
            get { return _protocolInfo; }
            set { _protocolInfo = value; notifyPropertyChanged("protocolInfo"); }
        }

        protected UInt16 _bitrate;
        public UInt16 bitrate
        {
            get { return _bitrate; }
            set { _bitrate = value; notifyPropertyChanged("bitrate"); }
        }
                      
        public string radioInfo
        {
            get { return "Region: " + _region + " / Signal Strength: " + _signalStrength; }
        }

        public MediaItem_Radio() 
            :base()
        {
            this.itemType = MediaItemItemType.Radio;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            String convertString;

            base.initValuesFromXMLNode(_nodeItem, _manager);

            this.radioName = xmlDocumentHelper.getChildNodeValue(_nodeItem, "dc:title", _manager);
            convertString = xmlDocumentHelper.getChildNodeValue(_nodeItem, "upnp:signalStrength", _manager);
            if (!String.IsNullOrWhiteSpace(convertString))
                this.signalStrength = Convert.ToUInt16(convertString);
            this.region = xmlDocumentHelper.getChildNodeValue(_nodeItem, "upnp:region", _manager);
            convertString = xmlDocumentHelper.getChildNodeValue(_nodeItem, "raumfeld:durability", _manager);
            if (!String.IsNullOrWhiteSpace(convertString))
                this.durability = Convert.ToUInt16(convertString);
            convertString = xmlDocumentHelper.getChildNodeAttributeValue(_nodeItem, "ns:res", "bitrate", _manager);
            if (!String.IsNullOrWhiteSpace(convertString))
                this.bitrate = Convert.ToUInt16(convertString);
            this.protocolInfo = xmlDocumentHelper.getChildNodeValue(_nodeItem, "res", _manager);
        }
    }

    public class MediaItem_LastFm : MediaItem_Radio
    {

        public MediaItem_LastFm() 
            :base()
        {
            this.itemType = MediaItemItemType.LastFm;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            base.initValuesFromXMLNode(_nodeItem, _manager);
        }
    }

    public class MediaItem_RhapsodyRadio : MediaItem_Radio
    {

        public MediaItem_RhapsodyRadio()
            : base()
        {
            this.itemType = MediaItemItemType.RhapsodyRadio;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            base.initValuesFromXMLNode(_nodeItem, _manager);
        }
    }

    public class MediaItem_Genre : MediaItem
    {

        public MediaItem_Genre()
            : base()
        {
            this.itemType = MediaItemItemType.Genre;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            base.initValuesFromXMLNode(_nodeItem, _manager);
        }
    }

    public class MediaItem_Composer : MediaItem
    {

        public MediaItem_Composer()
            : base()
        {
            this.itemType = MediaItemItemType.Composer;
        }

        public override void initValuesFromXMLNode(XmlNode _nodeItem, XmlNamespaceManager _manager)
        {
            base.initValuesFromXMLNode(_nodeItem, _manager);
        }
    }
}
