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
    public partial class RoomStripControl : UserControl
    {
        Boolean IsDragging;

        public delegate void removeAddButtonClickedHandler(RoomStripControl _roomStripControl, RoutedEventArgs e);
        public event removeAddButtonClickedHandler removeAddButtonClicked;

        public RoomStripControl()
        {
            InitializeComponent();
            RemoveAddButton.Click += RemoveAddButton_Click;

            this.PreviewMouseLeftButtonDown += RoomStripControl_PreviewMouseLeftButtonDown;
        }


        void RoomStripControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !IsDragging)
            {
                Point position = e.GetPosition(null);

                //if (Math.Abs(position.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                //    Math.Abs(position.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    //StartDrag(e);
                }
            }
        }
        
        private void StartDrag(MouseEventArgs e)
        {
            IsDragging = true;
            DataObject data = new DataObject("RoomStripControl", this); // ????
            DragDropEffects de = DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            IsDragging = false;
        } 


        /*void PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !IsDragging)
            {
                Point position = e.GetPosition(null);

                if (Math.Abs(position.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(e);

                }
            }
        }

        void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }
        */

        /*
         * 
         *  private TranslateTransform transform = new TranslateTransform();
        private void root_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                currentPoint = e.GetPosition(null);

                transform.X += currentPoint.X - anchorPoint.X;
                transform.Y += (currentPoint.Y - anchorPoint.Y);
                this.RenderTransform = transform;
                anchorPoint = currentPoint;
            }
        }
         * */

        void RemoveAddButton_Click(object sender, RoutedEventArgs e)
        {            
            if (removeAddButtonClicked != null) this.removeAddButtonClicked(this, e);
        }

        public static readonly DependencyProperty RoomColorProperty = DependencyProperty.Register("RoomColor", typeof(Color), typeof(RoomStripControl), new PropertyMetadata(default(Color)));
        public Color RoomColor
        {
            get { return (Color)GetValue(RoomColorProperty);}
            set { 
                    SetValue(RoomColorProperty, value);
                    RoomNameTextBlock.Foreground = new SolidColorBrush(getComplementaryColor(RoomColor));                    
                }
        }

        public static readonly DependencyProperty RoomNameProperty = DependencyProperty.Register("RoomName", typeof(String), typeof(RoomStripControl), new PropertyMetadata(default(String)));
        public String RoomName
        {
            get { return (String)GetValue(RoomNameProperty); }
            set { SetValue(RoomNameProperty, value); }
        }

        public static readonly DependencyProperty RoomIdProperty = DependencyProperty.Register("RoomId", typeof(String), typeof(RoomStripControl), new PropertyMetadata(default(String)));
        public String RoomId
        {
            get { return (String)GetValue(RoomIdProperty); }
            set { SetValue(RoomIdProperty, value); }
        }

        public System.Windows.Media.Color getComplementaryColor(System.Windows.Media.Color _color)
        {
            Byte b255 = new Byte();
            b255 = Convert.ToByte(255);
            return System.Windows.Media.Color.FromRgb((byte)(b255 - _color.R), (byte)(b255 - _color.G), (byte)(b255 - _color.B));
        }
        
    }
}
