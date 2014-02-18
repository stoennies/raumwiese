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

using RaumfeldNET;

namespace RaumfeldNET.WPFControls
{
    public partial class ZoneTileControl : UserControl
    {

        public delegate void roomRemoveAddButtonClickedHandler(ZoneTileControl _zoneTileControl, RoomStripControl _roomStripControl, RoutedEventArgs e);
        public event roomRemoveAddButtonClickedHandler roomRemoveAddButtonClicked;

        public delegate void roomDroppedHandler(ZoneTileControl _zoneTileControl, RoomStripControl _roomStripControl);
        public event roomDroppedHandler roomDropped;

        public delegate void selectedHandler(ZoneTileControl _zoneTileControl);
        public event selectedHandler selected;

        public ZoneTileControl()
        {
            InitializeComponent();
            this.MouseLeftButtonUp += ZoneTileControl_MouseLeftButtonUp;
            this.TouchUp += ZoneTileControl_TouchUp;
            this.AllowDrop = true;
            //this.Drop += ZoneTileControl_Drop;
            //this.DragOver += ZoneTileControl_DragOver;
        }

        void ZoneTileControl_DragOver(object sender, DragEventArgs e)
        {            
            RoomStripControl stripControl = (RoomStripControl)e.Data.GetData("RoomStripControl");
            if (stripControl != null)
                e.Effects = DragDropEffects.Move;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        void ZoneTileControl_Drop(object sender, DragEventArgs e)
        {
            RoomStripControl stripControl = (RoomStripControl)e.Data.GetData("RoomStripControl");
            if (stripControl != null)
            {
                if (roomDropped != null) this.roomDropped(this, stripControl);
            }            
        }

        void ZoneTileControl_TouchUp(object sender, TouchEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void ZoneTileControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;   
            RoomStripControl stripControl;
            stripControl = FindVisualParent<RoomStripControl>(obj);
            if (stripControl == null)
                this.ZoneTileSelected();      
  
        }

        protected void ZoneTileSelected()
        {
            if (this.selected != null) this.selected(this);   
        }

        public static T FindVisualParent<T>(DependencyObject child)
           where T : DependencyObject
        {
            // get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // we’ve reached the end of the tree
            if (parentObject == null) return null;

            // check if the parent matches the type we’re looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                // use recursion to proceed with next level
                return FindVisualParent<T>(parentObject);
            }
        }

        public static T FindParentLogical<T>(DependencyObject p_oElement)
            where T : DependencyObject
        {
            DependencyObject oParent = p_oElement;
            Type oTargetType = typeof(T);
            do
            {
                oParent = LogicalTreeHelper.GetParent(oParent);
            }
            while (
                !(
                    oParent == null
                    || oParent.GetType() == oTargetType
                    || oParent.GetType().IsSubclassOf(oTargetType)
                )
            );

            return oParent as T;
        }       

        public void AddRoomControl(RaumfeldNET.Room _roomInformation)
        {
            WPFControls.RoomStripControl roomStripControl = new WPFControls.RoomStripControl();
            roomStripControl.RoomId = _roomInformation.udn;
            roomStripControl.RoomName = _roomInformation.name;
            roomStripControl.DataContext = _roomInformation;
            roomStripControl.RoomColor = (Color)ColorConverter.ConvertFromString(_roomInformation.color);
            roomStripControl.removeAddButtonClicked += roomStripControl_removeAddButtonClicked;
            DockPanel.SetDock(roomStripControl, Dock.Bottom);
            RoomsDockPanel.Children.Add(roomStripControl);                     
        }

        void roomStripControl_removeAddButtonClicked(RoomStripControl _roomStripControl, RoutedEventArgs _e)
        {
            if (roomRemoveAddButtonClicked != null) this.roomRemoveAddButtonClicked(this, _roomStripControl, _e);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(String), typeof(ZoneTileControl), new PropertyMetadata(default(String)));
        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        
    }
}
