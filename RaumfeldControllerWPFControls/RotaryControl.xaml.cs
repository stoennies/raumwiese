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
    public partial class RotaryButtonControl : UserControl
    {
        private double prevVal;
        private Point prevPos;
        private Object _templateCanvas = null;

        private enum RotaryDirection
        {
            Right,
            Left
        };

        private enum MouseMoveDirection
        {
            Right,
            Left,
            Up,
            Down
        };

        public delegate void valueChangedHandler(RotaryButtonControl _rotaryButtonControl, double _newValue);
        public event valueChangedHandler valueChanged;

        public RotaryButtonControl()
        {
            InitializeComponent();
        }
        
        
        public static readonly DependencyProperty ValueFromProperty = DependencyProperty.Register("ValueFrom", typeof(double), typeof(RotaryButtonControl), new PropertyMetadata(default(double)));
        public double ValueFrom
        {
            get { return (double)GetValue(ValueFromProperty); }
            set { SetValue(ValueFromProperty, value); }
        }

        public static readonly DependencyProperty ValueToProperty = DependencyProperty.Register("ValueTo", typeof(double), typeof(RotaryButtonControl), new PropertyMetadata(default(double)));
        public double ValueTo
        {
            get { return (double)GetValue(ValueToProperty); }
            set { SetValue(ValueToProperty, value); }
        }


        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(RotaryButtonControl), new PropertyMetadata(default(double)));
        public double Value
        {
            get { return (double)GetValue(ValueProperty);}
            set 
            {
                SetValue(ValueProperty, value);
                this.setVolumeInfo();
                this.Angle = this.getAngleFromValue(this.Value);
                if (valueChanged != null) valueChanged(this, Value);
            }
        }

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(RotaryButtonControl), new PropertyMetadata(default(double)));
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set
            {
                SetValue(AngleProperty, value);
            }
        }

        private RotaryDirection GetRotaryDirection(Point _posPrev, Point _pos)
        {
            MouseMoveDirection mouseMoveDirection;
            double dx = _pos.X - _posPrev.X;
            double dy = _pos.Y - _posPrev.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
                mouseMoveDirection = (dx > 0) ? MouseMoveDirection.Right : MouseMoveDirection.Left;
            else
                mouseMoveDirection = (dy > 0) ? MouseMoveDirection.Down : MouseMoveDirection.Up;

            //CurrentValue.Text = String.Format("{0}", mouseMoveDirection);

            // left upper quarter
            if (_pos.X <= this.ActualWidth / 2 && _pos.Y <= this.ActualHeight / 2)
            {
                if(mouseMoveDirection == MouseMoveDirection.Right || mouseMoveDirection == MouseMoveDirection.Up)
                    return RotaryDirection.Right;
                else
                    return RotaryDirection.Left;
            }
            // right upper quarter
            else if (_pos.X > this.ActualWidth / 2 && _pos.Y <= this.ActualHeight / 2)
            {
                if (mouseMoveDirection == MouseMoveDirection.Right || mouseMoveDirection == MouseMoveDirection.Down)
                    return RotaryDirection.Right;
                else
                    return RotaryDirection.Left;
            }
            // right lower quarter
            else if (_pos.X > this.ActualWidth / 2 && _pos.Y > this.ActualHeight / 2)
            {
                if (mouseMoveDirection == MouseMoveDirection.Left || mouseMoveDirection == MouseMoveDirection.Down)
                    return RotaryDirection.Right;
                else
                    return RotaryDirection.Left;
            }
            // left lower quarter
            else
            {
                if (mouseMoveDirection == MouseMoveDirection.Left || mouseMoveDirection == MouseMoveDirection.Up)
                    return RotaryDirection.Right;
                else
                    return RotaryDirection.Left;
            }
        }

        private void GeneralMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Find the parent canvas.
                if (_templateCanvas == null)
                {
                    if (e.Source is Ellipse)
                        _templateCanvas = MyHelper.FindParent<Canvas>(e.Source as Ellipse);
                    else
                    {
                        _templateCanvas = MyHelper.FindChild<Canvas>(knob as FrameworkElement);
                        //_templateCanvas = MyHelper.FindChild<Canvas>(_templateCanvas as FrameworkElement);
                    }

                    if (_templateCanvas == null) return;
                }
                //Calculate the current rotation angle and set the value.
                const double RADIUS = 150;
                Point newPos = e.GetPosition((IInputElement)_templateCanvas);
                double angle = MyHelper.GetAngleR(newPos, RADIUS);
                double diff;
                double val = Math.Round((knob.Maximum - knob.Minimum) * angle / (2 * Math.PI), 0);
                if (prevVal != 0)
                {
                    diff = val - prevVal;
                }
                else
                    diff = 0;

                RotaryDirection dir = GetRotaryDirection(prevPos, newPos);
                // Moving over Max Value
                if (dir == RotaryDirection.Right && prevPos.X <= this.ActualWidth / 2 && prevPos.Y <= this.ActualHeight / 2)
                {
                    if (prevVal > val + 10)
                        diff = (this.ValueTo - prevVal) + val;
                }

                double dummyValue = Value;

                if (Math.Abs(diff) > 10)
                    diff = 5;
              
                if (dir == RotaryDirection.Right)
                    dummyValue += Math.Abs(diff);
                else
                    dummyValue -= Math.Abs(diff);

                if (dummyValue > this.ValueTo)
                    Value = this.ValueTo;
                else if (dummyValue < this.ValueFrom)
                    Value = this.ValueFrom;
                else
                    Value = dummyValue;

                prevVal = val;
                //prevVal = Value;
                prevPos = newPos;
            }
        }

        private double getAngleFromValue(double _value)
        {
            return (360 / (this.ValueTo - this.ValueFrom)) * (this.Value - this.ValueFrom);
        }

        private void setVolumeInfo()
        {
            CurrentValue.Text = String.Format("{0}", Convert.ToInt16(Value));
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            this.GeneralMouseMove(sender, e);
        } 

        private void DummyLabel_MouseMove(object sender, MouseEventArgs e)
        {
            this.GeneralMouseMove(sender, e);
        }

        private void DummyLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            prevVal = 0;
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            prevVal = 0;
        }
    }

}
