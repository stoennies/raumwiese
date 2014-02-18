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
    public partial class ContentContextMenuControl : UserControl
    {

        public delegate void actionHandler(ContentContextMenuControl _control, RoutedEventArgs e);
        public event actionHandler actionPlayNow;
        public event actionHandler actionPlayNext;
        public event actionHandler actionPlayAtEnd;
        public event actionHandler actionCloseMenue;

        public ContentContextMenuControl()
        {
            InitializeComponent();           
        }

        private void buttonPlayNow_ClickSink(object sender, RoutedEventArgs e)
        {
            if (actionPlayNow != null) actionPlayNow(this, e);
        }

        private void buttonPlayNext_ClickSink(object sender, RoutedEventArgs e)
        {
            if (actionPlayNext != null) actionPlayNext(this, e);
        }

        private void buttonPlayAtEnd_ClickSink(object sender, RoutedEventArgs e)
        {
            if (actionPlayAtEnd != null) actionPlayAtEnd(this, e);
        }

        private void buttonClose_ClickSink(object sender, RoutedEventArgs e)
        {
            if (actionCloseMenue != null) actionCloseMenue(this, e);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(ContentContextMenuControl), new PropertyMetadata(default(String)));
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


    }
}
