using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Drawing;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Web;
using System.Net;
using System.Xml;
using System.Collections;

using RaumfeldNET.Base;
using RaumfeldNET.Log;
using RaumfeldNET.UPNP;
using RaumfeldNET.Renderer;

namespace RaumfeldNET
{
    public class DataDB : Base.BaseHelper
    {
        protected SQLiteConnection connection;
        protected String dbName;
        protected Boolean isConnected;

        public void connect()
        {
            try
            {
                connection = new SQLiteConnection();
                connection.ConnectionString = "Data Source=" + dbName;
                connection.Open();
                isConnected = true;
            }
            catch (Exception e)
            {
                this.writeLog(LogType.Error, String.Format("Fehler beim erstellen oder Verbinden zur Datenbank '{0}'", dbName), e);
            }
        }

        public void close()
        {
            isConnected = false;
            connection.Close(); 
        }

        public virtual void saveToDB()
        {
        }

        public virtual void loadFromDB()
        {
        }

        public virtual void initDatabase()
        {         
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }

    public class ImageDataCache : DataDB
    {
        protected Dictionary<String, Image> imageCache;
        protected Dictionary<String, Image> imageCacheSmall;
        protected Dictionary<String, Image> imageCacheNew;
        protected Dictionary<String, Image> imageCacheNewSmall;

        protected Dictionary<String, List<delegate_OnReqestImageDone>> imageUrlOnRequest;

        public delegate void delegate_OnReqestImageDone(String _imageId);

        public ImageDataCache()
            : base()
        {
            dbName = "Raumwiese.db";
            imageCache = new Dictionary<String, Image>();
            imageCacheNew = new Dictionary<String, Image>();
            imageCacheSmall = new Dictionary<String, Image>();
            imageUrlOnRequest = new Dictionary<String, List<delegate_OnReqestImageDone>>();
            this.webRequestHelper.denyRequestErrorLog = true;
        }

        ~ImageDataCache()
        {
            this.clearImageCache();
        }

        protected Image createThumbnailImage(Image _image)
        {
            Image imageSmall = (Image)_image.Clone();

            Bitmap b = new Bitmap(60, 60);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(imageSmall, 0, 0, 60, 60);
            g.Dispose();

            return (Image)b;
        }

        public void addImage(String _imageId, Image _image)
        {
            if (String.IsNullOrWhiteSpace(_imageId) || _image == null || imageCache == null || imageCacheNew == null)
                return;

            if (imageCache.ContainsKey(_imageId))
                imageCache.Remove(_imageId);
            imageCache.Add(_imageId, (Image)_image.Clone());
            if (imageCacheNew.ContainsKey(_imageId))
                imageCacheNew.Remove(_imageId);
            imageCacheNew.Add(_imageId, (Image)_image.Clone());
        }

        public Boolean getImage(String _imageId, out Image _imageClone, Boolean _getThumb = false)
        {
            Image image;
            Boolean ret;

            try
            {

                if (_getThumb)
                {
                    ret = imageCacheSmall.TryGetValue(_imageId, out image);
                    if (ret)
                    {
                        _imageClone = (Image)image.Clone();
                        return true;
                    }
                }

                ret = imageCache.TryGetValue(_imageId, out image);
                if (ret)
                {
                    _imageClone = (Image)image.Clone();
                    if (_getThumb)
                    {
                        _imageClone = this.createThumbnailImage(_imageClone);
                        imageCacheSmall.Add(_imageId, _imageClone);
                    }
                }
                else
                    _imageClone = null;
            }
            catch (Exception e)
            {
                this.writeLog(LogType.Warning, String.Format("Fehler beim laden von Bild '{0}'", _imageId, e));
                _imageClone = null;
                ret = false;
            }
            return ret;
        }

        public Boolean existsImage(String _imageId)
        {
            return imageCache.ContainsKey(_imageId);
        }

        public void clearImageCache()
        {
            imageCache.Clear();
            imageCacheNew.Clear();
        }

        public override void initDatabase()
        {
            this.connect();

            try
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "CREATE TABLE IF NOT EXISTS imageData ( id VARCHAR(255) NOT NULL PRIMARY KEY, data BLOB);";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE INDEX IF NOT EXISTS imageData_Id_Idx ON imageData (id);";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE TABLE IF NOT EXISTS imageDataSmall ( id VARCHAR(255) NOT NULL PRIMARY KEY, data BLOB);";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE INDEX IF NOT EXISTS imageData_Id_Idx ON imageDataSmall (id);";
                command.ExecuteNonQuery();

                command.Dispose();
            }
            catch (Exception e)
            {
                this.writeLog(LogType.Error, "Fehler beim erstellen der Datenbenktabellen für ImageCache!", e);                
            }

            this.close();
        }

        public  void saveToDBThread()
        {
            if (imageCacheNew.Count == 0)
                return;

            Thread thread = new Thread(saveToDB);
            thread.Start();
        }

        public override void saveToDB()
        {
            if (imageCacheNew.Count == 0)
                return;

            Dictionary<String, Image> tempData;

            try
            {

                tempData = new Dictionary<string, Image>(imageCacheNew);
                imageCacheNew.Clear();

                this.connect();

                SQLiteCommand command = new SQLiteCommand(connection);

                // only store new images
                foreach (var imageData in tempData)
                {
                    command.CommandText = String.Format("REPLACE INTO imageData (id, data) VALUES ('{0}', @0);", imageData.Key);
                    SQLiteParameter parameter = new SQLiteParameter("@0", System.Data.DbType.Binary);
                    parameter.Value = imageToByteArray(imageData.Value);
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQueryAsync();
                }

                command.Dispose();

                this.close();

                tempData.Clear();
            }
            catch (Exception e)
            {
                this.writeLog(LogType.Warning, "Fehler beim speichern der Bilder auf der Datenbank", e);                
            }
        }

        public override void loadFromDB()
        {
            this.connect();

            SQLiteCommand command = new SQLiteCommand(connection);
            SQLiteDataReader reader;            
            String key;
            byte[] imageBytes;
          
            command.CommandText = String.Format("SELECT id, data FROM imageData;");
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                key = reader.GetString(0);       
                imageBytes= (System.Byte[])reader["data"];
                imageCache.Add(key, this.byteArrayToImage(imageBytes));
            }
               
            command.Dispose();

            this.close(); 
        }

        // use this method to request an image from an url and to store it in the cache
        public void requestImageFromUrl(String _imageUrl, delegate_OnReqestImageDone _sink)
        {
            try
            {

                // if image is already in cache, we immediately notify the sinks
                // otherwise start web request for url
                if (this.existsImage(_imageUrl))
                {
                    if (_sink != null) ((delegate_OnReqestImageDone)_sink)(_imageUrl);
                }
                else
                {
                    lock (imageUrlOnRequest)
                    {
                        if (!imageUrlOnRequest.ContainsKey(_imageUrl))
                        {
                            imageUrlOnRequest.Add(_imageUrl, new List<delegate_OnReqestImageDone>());
                            webRequestHelper.httpPostRequestAsync(_imageUrl, null, null, (response) => requestImageFromUrlSink(response, _sink, _imageUrl));
                        }
                        else
                        {
                            imageUrlOnRequest[_imageUrl].Add(_sink);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.writeLog(LogType.Error, String.Format("Fehler bei 'requestImageFromUrl' für Url '{0}'", _imageUrl), e);
            }
        }

        // use this method to request an image from a file and to store it in the cache
        public void requestImageFromFile(String _fileName, delegate_OnReqestImageDone _sink)
        {
            try
            {
                String imageId = _fileName;
                // if image is already in cache, we immediately notify the sinks           
                if (this.existsImage(_fileName))
                {
                    if (_sink != null) ((delegate_OnReqestImageDone)_sink)(_fileName);
                }
                else
                {
                    this.addImage(imageId, Image.FromFile(_fileName));
                    if (_sink != null) ((delegate_OnReqestImageDone)_sink)(_fileName);
                }
            }
            catch (Exception e)
            {
                this.writeLog(LogType.Error, String.Format("Fehler bei 'requestImageFromFile' für Datei '{0}'", _fileName),e);
            }
        }

        // this method is called if a reqestImageFromUrl action has retrieved the data
        protected virtual void requestImageFromUrlSink(HttpWebResponse _reponse, delegate_OnReqestImageDone _sink, String _imageId)
        {
            try
            {
                String imageId = _imageId;
                Stream stream = _reponse.GetResponseStream();
                if (stream == null)
                    return;

                this.addImage(imageId, Image.FromStream(stream));
                if (_sink != null) ((delegate_OnReqestImageDone)_sink)(imageId);
            }
            catch(Exception e)
            {
                this.writeLog(LogType.Error, String.Format("Fehler bei 'requestImageFromUrlSink' für Url '{0}'", _reponse.ResponseUri.AbsoluteUri), e);
            }

            lock (imageUrlOnRequest)
            {
                if (imageUrlOnRequest.ContainsKey(_imageId))
                {
                    foreach(var delegateLink in imageUrlOnRequest[_imageId])
                    {
                        if (delegateLink != null) ((delegate_OnReqestImageDone)delegateLink)(_imageId);
                    }
                    imageUrlOnRequest.Remove(_imageId);
                }
            }
            
        }

    }
}
