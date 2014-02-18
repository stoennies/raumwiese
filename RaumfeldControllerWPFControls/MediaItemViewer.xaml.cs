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
using System.Globalization;


namespace RaumfeldNET.WPFControls
{
    public partial class MediaItemViewerControl : UserControl
    {      

        public MediaItemViewerControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(BitmapImage), typeof(MediaItemViewerControl), new PropertyMetadata(default(BitmapImage)));
        public BitmapImage ImageSource
        {
            get { return (BitmapImage)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageVisibilityProperty = DependencyProperty.Register("ImageVisibility", typeof(Visibility), typeof(MediaItemViewerControl), new PropertyMetadata(default(Visibility)));
        public Visibility ImageVisibility
        {
            get { return (Visibility)GetValue(ImageVisibilityProperty); }
            set { SetValue(ImageVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TextLeftProperty = DependencyProperty.Register("TextLeft", typeof(String), typeof(MediaItemViewerControl), new PropertyMetadata(default(String)));
        public String TextLeft
        {
            get { return (String)GetValue(TextLeftProperty); }
            set { SetValue(TextLeftProperty, value); }
        }

        public static readonly DependencyProperty TextRightProperty = DependencyProperty.Register("TextRight", typeof(String), typeof(MediaItemViewerControl), new PropertyMetadata(default(String)));
        public String TextRight
        {
            get { return (String)GetValue(TextRightProperty); }
            set { SetValue(TextRightProperty, value); }
        }

        public static readonly DependencyProperty SubTextLeftProperty = DependencyProperty.Register("SubTextLeft", typeof(String), typeof(MediaItemViewerControl), new PropertyMetadata(default(String)));
        public String SubTextLeft
        {
            get { return (String)GetValue(SubTextLeftProperty); }
            set { SetValue(SubTextLeftProperty, value); }
        }

        public static readonly DependencyProperty SubTextRightProperty = DependencyProperty.Register("SubTextRight", typeof(String), typeof(MediaItemViewerControl), new PropertyMetadata(default(String)));
        public String SubTextRight
        {
            get { return (String)GetValue(SubTextRightProperty); }
            set { SetValue(SubTextRightProperty, value); }
        }
    }
   
}
