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
    public partial class VolumeSliderControl : UserControl
    {
        // is set when volume slider is been currently sliding by mouse or touch
        protected Boolean isSliding;
        public uint freezeChangeTimeMS;
        public uint timeStepTimeMS;

        System.Timers.Timer freezeTimer;
        System.Timers.Timer timeStepTimer;

        // event for valueChanged (by mouse, touch or any userinteraction with UI)
        public delegate void valueChangedHandler(VolumeSliderControl _volumeSliderControl, uint _newValue);
        public event valueChangedHandler valueChanged;

        // event for valueChanged (by mouse, touch or any userinteraction with UI) and did not change within the "freezeChangeTimeMS"
        public event valueChangedHandler valueChangedFreeze;

        public event valueChangedHandler valueChangedTimeStep;

        // event for valueChanged (by mouse, touch or any userinteraction with UI)
        public delegate void muteStateChangedHandler(VolumeSliderControl _volumeSliderControl, Boolean _newValue);
        public event muteStateChangedHandler muteStateChanged;

        public VolumeSliderControl()
        {
            InitializeComponent();
            MainSlider.MouseMove += MainSlider_MouseMove;
            MainSlider.TouchMove += MainSlider_TouchMove;

            MainSlider.PreviewMouseUp += MainSlider_PreviewMouseUp;
            MainSlider.PreviewTouchUp += MainSlider_PreviewTouchUp;

            MainSlider.PreviewMouseDown += MainSlider_PreviewMouseDown;
            MainSlider.PreviewTouchDown += MainSlider_PreviewTouchDown;

            freezeChangeTimeMS = 200;
            timeStepTimeMS = 200;
            ComplementaryColor = Color.FromRgb(255, 255, 255);
        }
  

        protected void MainSlider_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            SetIsSliding(true);
        }

        protected void MainSlider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SetIsSliding(true);
        }

        protected void MainSlider_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            SetIsSliding(false);
        }

        protected void MainSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            SetIsSliding(false);
        }

        protected void MainSlider_TouchMove(object sender, TouchEventArgs e)
        {     
            MainSliderValueChanged();
        }

        protected void MainSlider_MouseMove(object sender, MouseEventArgs e)
        {      
            if (e.LeftButton == MouseButtonState.Pressed)
                MainSliderValueChanged();
        }

        protected void SetIsSliding(Boolean _isSliding = true)
        {
            isSliding = _isSliding;                            
        }

        protected void MainSliderValueChanged()
        {
            uint volumeValue = (uint)MainSlider.Value;

            if (timeStepTimeMS > 0)
            {                
                if (timeStepTimer == null)     
                {
                    timeStepTimer = new System.Timers.Timer(timeStepTimeMS);
                    timeStepTimer.Elapsed += (sender, e) =>stepTimer_Elapsed(sender, e, volumeValue);
                    timeStepTimer.Start();
                }
            }

            if (freezeChangeTimeMS > 0)
            {
                if (freezeTimer != null)
                    freezeTimer.Dispose();
                freezeTimer = new System.Timers.Timer(freezeChangeTimeMS);
                freezeTimer.Elapsed += (sender, e) => freezeTimer_Elapsed(sender, e, volumeValue);
                freezeTimer.Start();
            }

            if (valueChanged != null) this.valueChanged(this, volumeValue);
        }

        delegate void invokeFreezeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, uint _oldValue);
        void freezeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, uint _oldValue)
        {
            if (this.MainSlider.Dispatcher.CheckAccess())
            {
                if (freezeTimer != null)    
                {
                    if (_oldValue == (uint)MainSlider.Value)
                    {
                        freezeTimer.Stop();
                        if (valueChangedFreeze != null) this.valueChangedFreeze(this, (uint)MainSlider.Value);
                    }
                }
            }
            else
                this.MainSlider.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new invokeFreezeTimer_Elapsed(this.freezeTimer_Elapsed), sender, e, _oldValue);
        }

        delegate void invokeStepTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, uint _oldValue);
        void stepTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, uint _oldValue)
        {
            if (this.MainSlider.Dispatcher.CheckAccess())
            {
                if (timeStepTimer != null)
                {
                    if (!isSliding)
                    {
                        timeStepTimer.Stop();
                        timeStepTimer.Dispose();
                        timeStepTimer = null;
                    }
                    if (valueChangedTimeStep != null) this.valueChangedTimeStep(this, (uint)MainSlider.Value);
                }
            }
            else
                this.MainSlider.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new invokeStepTimer_Elapsed(this.stepTimer_Elapsed), sender, e, _oldValue);
        }

        public uint Value
        {
            get { return (uint)MainSlider.Value; }
            set 
            {
                if (!isSliding)
                    MainSlider.Value = value; 
            }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(VolumeSliderControl), new PropertyMetadata(default(Color)));
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set 
            { 
                SetValue(ColorProperty, value);
                this.ComplementaryColor = getComplementaryColor(Color);
            }
        }

        public System.Windows.Media.Color getComplementaryColor(System.Windows.Media.Color _color)
        {
            Byte b255 = new Byte();
            b255 = Convert.ToByte(255);
            return System.Windows.Media.Color.FromRgb((byte)(b255 - _color.R), (byte)(b255 - _color.G), (byte)(b255 - _color.B));
        }

        public static readonly DependencyProperty ComplementaryColorProperty = DependencyProperty.Register("ComplementaryColor", typeof(Color), typeof(VolumeSliderControl), new PropertyMetadata(default(Color)));
        public Color ComplementaryColor
        {
            get { return (Color)GetValue(ComplementaryColorProperty); }
            set { SetValue(ComplementaryColorProperty, value); }
        }

        public static readonly DependencyProperty IsMuteProperty = DependencyProperty.Register("IsMute", typeof(Boolean), typeof(VolumeSliderControl), new PropertyMetadata(default(Boolean)));
        public Boolean IsMute
        {
            get { return (Boolean)GetValue(IsMuteProperty); }
            set
            {
                SetValue(IsMuteProperty, value);
                UpdateMuteButton();
            }
        }

        public static readonly DependencyProperty SliderTextProperty = DependencyProperty.Register("SliderText", typeof(String), typeof(VolumeSliderControl), new PropertyMetadata(default(String)));
        public String SliderText
        {
            get { return (String)GetValue(SliderTextProperty); }
            set
            {
                SetValue(SliderTextProperty, value);  
                if(String.IsNullOrEmpty(SliderText))
                    Text.Visibility = System.Windows.Visibility.Collapsed;
                else
                    Text.Visibility = System.Windows.Visibility.Visible;
            }
        }

        protected void UpdateMuteButton()
        {
            if (!IsMute)
            {
                MuteButton.Visibility = System.Windows.Visibility.Visible;
                UnMuteButton.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                MuteButton.Visibility = System.Windows.Visibility.Hidden;
                UnMuteButton.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsMute = true;
            if (muteStateChanged != null) muteStateChanged(this, IsMute);
        }

        private void UnMuteButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsMute = false;
            if (muteStateChanged != null) muteStateChanged(this, IsMute);
        }

    }
}
