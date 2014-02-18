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
    public enum TrackStateControlRepeat
    {
        Normal,
        RepeatOne,
        RepeatAll
    };

    public partial class TrackStateControl : UserControl
    {

        public delegate void clickedHandler(TrackStateControl _trackStateControl, RoutedEventArgs e);
        public event clickedHandler clickedPlayPause;
        public event clickedHandler clickedNext;
        public event clickedHandler clickedBack;
        public event clickedHandler clickedRandom;
        public event clickedHandler clickedRepeat;

        public TrackStateControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TrackPausedProperty = DependencyProperty.Register("TrackPaused", typeof(Boolean), typeof(TrackStateControl), new PropertyMetadata(default(Boolean)));
        public Boolean TrackPaused
        {
            get { return (Boolean)GetValue(TrackPausedProperty); }
            set 
            {
                SetValue(TrackPausedProperty, value);
                ZoneTracklistPlayControl.Visibility = !TrackPaused ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
                ZoneTracklistPauseControl.Visibility = !TrackPaused ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            }
        }

        public static readonly DependencyProperty RandomizeTrackProperty = DependencyProperty.Register("RandomizeTrack", typeof(Boolean), typeof(TrackStateControl), new PropertyMetadata(default(Boolean)));
        public Boolean RandomizeTrack
        {
            get { return (Boolean)GetValue(RandomizeTrackProperty); }
            set 
            { 
                SetValue(RandomizeTrackProperty, value);                
                ZoneTracklistRandomControlImage.Visibility = !RandomizeTrack ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                ZoneTracklistRandomControlImageActive.Visibility = RandomizeTrack ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden; 
            }
        }

        public static readonly DependencyProperty RepeatTacksProperty = DependencyProperty.Register("RepeatTacks", typeof(TrackStateControlRepeat), typeof(TrackStateControl), new PropertyMetadata(default(TrackStateControlRepeat)));
        public TrackStateControlRepeat RepeatTacks
        {
            get { return (TrackStateControlRepeat)GetValue(RepeatTacksProperty); }
            set 
            { 
                SetValue(RepeatTacksProperty, value);
                ZoneTracklistRepeatControlImage.Visibility = RepeatTacks == TrackStateControlRepeat.Normal ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                ZoneTracklistRepeatOneControlImage.Visibility = RepeatTacks == TrackStateControlRepeat.RepeatOne ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                ZoneTracklistRepeatAllControlImage.Visibility = RepeatTacks == TrackStateControlRepeat.RepeatAll ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            }
        }

        private void PlayPauseClickedSink(object sender, RoutedEventArgs e)
        {
            if (clickedPlayPause != null) clickedPlayPause(this, e);
        }

        private void RepeatClickedSink(object sender, RoutedEventArgs e)
        {
            if (clickedRepeat != null) clickedRepeat(this, e);
        }

        private void RandomClickedSink(object sender, RoutedEventArgs e)
        {
            if (clickedRandom != null) clickedRandom(this, e);
        }

        private void NextClickedSink(object sender, RoutedEventArgs e)
        {
            if (clickedNext != null) clickedNext(this, e);
        }

        private void BackClickedSink(object sender, RoutedEventArgs e)
        {
            if (clickedBack != null) clickedBack(this, e);
        }
      
    }
}
