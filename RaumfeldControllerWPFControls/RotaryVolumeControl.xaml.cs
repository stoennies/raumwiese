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
    public partial class RotaryVolumeControl : UserControl
    {
        protected Boolean isSliding;
        public uint freezeChangeTimeMS;
        public uint timeStepTimeMS;

        System.Timers.Timer freezeTimer;
        System.Timers.Timer timeStepTimer;

        public delegate void valueChangedHandler(RotaryVolumeControl _rotaryVolumeControl, uint _newValue);
        public event valueChangedHandler valueChanged;
        public event valueChangedHandler valueChangedFreeze;
        public event valueChangedHandler valueChangedTimeStep;

        public delegate void muteStateChangedHandler(RotaryVolumeControl _rotaryVolumeControl, Boolean _newValue);
        public event muteStateChangedHandler muteStateChanged;

        public RotaryVolumeControl()
        {
            InitializeComponent();
            RotaryButton.valueChanged += rotaryButton_valueChangedSink;
            Slider.muteStateChanged += slider_muteStateChangedSink;
            Slider.valueChanged += slider_valueChangedSink;

            freezeChangeTimeMS = 200;
            timeStepTimeMS = 200;
        }

        void slider_muteStateChangedSink(VolumeSliderControl _volumeSliderControl, bool _newValue)
        {
            if (muteStateChanged != null) muteStateChanged(this, _newValue);
        }
       
        void slider_valueChangedSink(VolumeSliderControl _volumeSliderControl, uint _newValue)
        {
            RotaryButton.Value = Slider.Value;
            this.volumeValueChanged();
        }

        void rotaryButton_valueChangedSink(RotaryButtonControl _rotaryButtonControl, double _newValue)
        {
            Slider.Value = (uint)RotaryButton.Value;
            this.volumeValueChanged();
        }

        public static readonly DependencyProperty ValueFromProperty = DependencyProperty.Register("ValueFrom", typeof(double), typeof(RotaryVolumeControl), new PropertyMetadata(default(double)));
        public double ValueFrom
        {
            get { return (double)GetValue(ValueFromProperty); }
            set { SetValue(ValueFromProperty, value); }
        }

        public static readonly DependencyProperty ValueToProperty = DependencyProperty.Register("ValueTo", typeof(double), typeof(RotaryVolumeControl), new PropertyMetadata(default(double)));
        public double ValueTo
        {
            get { return (double)GetValue(ValueToProperty); }
            set { SetValue(ValueToProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(RotaryVolumeControl), new PropertyMetadata(default(double)));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { 
                    SetValue(ValueProperty, value);
                    RotaryButton.Value = Value;
                    Slider.Value = (uint)Value;
                }
        }

        public static readonly DependencyProperty IsMuteProperty = DependencyProperty.Register("IsMute", typeof(Boolean), typeof(RotaryVolumeControl), new PropertyMetadata(default(Boolean)));
        public Boolean IsMute
        {
            get { return (Boolean)GetValue(IsMuteProperty); }
            set
            {
                SetValue(IsMuteProperty, value);
                Slider.IsMute = IsMute;
            }
        }

        public static readonly DependencyProperty SliderTextProperty = DependencyProperty.Register("SliderText", typeof(String), typeof(RotaryVolumeControl), new PropertyMetadata(default(String)));
        public String SliderText
        {
            get { return (String)GetValue(SliderTextProperty); }
            set
            {
                SetValue(SliderTextProperty, value);
                Slider.SliderText = SliderText;
            }
        }

        protected void volumeValueChanged()
        {
            uint volumeValue = (uint)Slider.Value;

            if (timeStepTimeMS > 0)
            {
                if (timeStepTimer == null)
                {
                    timeStepTimer = new System.Timers.Timer(timeStepTimeMS);
                    timeStepTimer.Elapsed += (sender, e) => stepTimer_Elapsed(sender, e, volumeValue);
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
            if (this.Slider.Dispatcher.CheckAccess())
            {
                if (freezeTimer != null)
                {
                    if (_oldValue == (uint)Slider.Value)
                    {
                        freezeTimer.Stop();
                        if (valueChangedFreeze != null) this.valueChangedFreeze(this, (uint)Slider.Value);
                    }
                }
            }
            else
                this.Slider.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new invokeFreezeTimer_Elapsed(this.freezeTimer_Elapsed), sender, e, _oldValue);
        }

        delegate void invokeStepTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, uint _oldValue);
        void stepTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, uint _oldValue)
        {
            if (this.Slider.Dispatcher.CheckAccess())
            {
                if (timeStepTimer != null)
                {
                    if (!isSliding)
                    {
                        timeStepTimer.Stop();
                        timeStepTimer.Dispose();
                        timeStepTimer = null;
                    }
                    if (valueChangedTimeStep != null) this.valueChangedTimeStep(this, (uint)Slider.Value);
                }
            }
            else
                this.Slider.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new invokeStepTimer_Elapsed(this.stepTimer_Elapsed), sender, e, _oldValue);
        }
    }

}
