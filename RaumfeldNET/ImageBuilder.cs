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
using System.Threading;
using System.Data.SQLite;

namespace RaumfeldNET
{
    public class ImageBuilder : DataDB
    {
        public delegate void delegate_OnImageReady(Image _image);  

        public ImageBuilder()
            : base()
        {
            dbName = "Raumwiese.db";
        }

        public void buildRandomImageCover(int _imageRowCount = 3, int _width = 40, delegate_OnImageReady _delegate = null)
        {

            Thread thread = new Thread(() => buildRandomImageCoverThread(_imageRowCount, _width, _delegate));
            thread.Start();
        }

        protected void buildRandomImageCoverThread(int _imageRowCount = 3, int _width = 40, delegate_OnImageReady _delegate = null)
        {
            int imageCount = _imageRowCount * _imageRowCount;
            int subImageWidth = _width / _imageRowCount;
            int imageWidth = _width, imageHeight = _width;
            int x=1,y=1;
            List<Image> imageList = this.loadRandomImagesFromDB(imageCount+1);         
            
            Image image = null;

            if (imageList == null)
                return;

            if (imageList.Count >= imageCount)
            {
                Bitmap outputImage = new Bitmap(imageWidth, imageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics graphics = Graphics.FromImage(outputImage);
                
                foreach(Image img in imageList)
                {                    
                    graphics.DrawImage(img, x, y, subImageWidth, subImageWidth);
                    x += subImageWidth;
                    if (x >= imageWidth)
                    {
                        x = 0;
                        y += subImageWidth;
                        if (y >= imageHeight)
                            break;
                    }
                }

                image = (Image)outputImage;

            }

            if (_delegate != null) _delegate(image);

        }

        protected List<Image> loadRandomImagesFromDB(int _imageCount)
        {
            List<Image> imageList = new List<Image>();

            try
            {

                this.connect();

                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteDataReader reader;
                String key;
                byte[] imageBytes;

                command.CommandText = String.Format("SELECT * FROM imageData ORDER BY RANDOM() LIMIT {0};", _imageCount);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    key = reader.GetString(0);
                    imageBytes = (System.Byte[])reader["data"];
                    imageList.Add(this.byteArrayToImage(imageBytes));
                }

                command.Dispose();

                this.close();
            }
            catch (Exception _exception)
            {
                // do nothing
            }

            return imageList;
        }
    }
}
