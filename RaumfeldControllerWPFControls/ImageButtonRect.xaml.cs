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

namespace RaumfeldNET.WPFControls
{
    public partial class ImageButtonRectControl : UserControl
    {

        public delegate void clickedHandler(ImageButtonRectControl _imageButtonControl, MouseButtonEventArgs e);
        public event clickedHandler clicked;

        public ImageButtonRectControl()
        {
            InitializeComponent();
            MainGrid.PreviewMouseUp += mouseButtonUpSink;
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(String), typeof(ImageButtonRectControl), new PropertyMetadata(default(String)));
        public String ImageSource
        {
            get { return (String)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(String), typeof(ImageButtonRectControl), new PropertyMetadata(default(String)));
        public String ImageWidth
        {
            get { return (String)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(String), typeof(ImageButtonRectControl), new PropertyMetadata(default(String)));
        public String ImageHeight
        {
            get { return (String)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(ImageButtonRectControl), new PropertyMetadata(default(String)));
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        private void mouseButtonUpSink(object sender, MouseButtonEventArgs e)
        {
            if (clicked != null) clicked(this, e);
        }

    }
}
