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
using System.Windows.Interop;

using RaumfeldNET;
using RaumfeldControllerWPFControls;
using MahApps;
using MahApps.Metro.Controls;
using GongSolutions.Wpf.DragDrop;
//using WpfAnimatedGif;

namespace Raumwiese.Behaviours
{

    /*public static class GifImageBehaviour
    {
        public static bool GetStartAnimation(Image target)
        {
            return (bool)target.GetValue(StartAnimationAttachedProperty);
        }

        public static bool GetStopAnimation(Image target)
        {
            return (bool)target.GetValue(StopAnimationAttachedProperty);
        }

        public static void SetStartAnimation(Image target, bool value)
        {
            target.SetValue(StartAnimationAttachedProperty, value);
        }

        public static void SetStopAnimation(Image target, bool value)
        {
            target.SetValue(StopAnimationAttachedProperty, value);
        }

        public static void SetAnimate(Image target, bool value)
        {
            target.SetValue(AnimateAttachedProperty, value);
        }

        public static bool GetAnimate(Image target)
        {
            return (bool)target.GetValue(AnimateAttachedProperty);
        }

        public static readonly DependencyProperty AnimateAttachedProperty = DependencyProperty.RegisterAttached("Animate", typeof(bool), typeof(GifImageBehaviour), new UIPropertyMetadata(false, OnAnimatePropertyChangedSink));
        static void OnAnimatePropertyChangedSink(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var controller = ImageBehavior.GetAnimationController((Image)o);
            if (controller == null)
                return;
            if ((Boolean)e.NewValue == true)
                controller.Play();
            else
                controller.Pause();
        }

        public static readonly DependencyProperty StartAnimationAttachedProperty = DependencyProperty.RegisterAttached("StartAnimation", typeof(bool), typeof(GifImageBehaviour), new UIPropertyMetadata(false, OnStartAnimationPropertyChangedSink));
        static void OnStartAnimationPropertyChangedSink(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var controller = ImageBehavior.GetAnimationController((Image)o);
            if (controller != null)
                controller.Play();
        }

        public static readonly DependencyProperty StopAnimationAttachedProperty = DependencyProperty.RegisterAttached("StopAnimation", typeof(bool), typeof(GifImageBehaviour), new UIPropertyMetadata(false, OnStopAnimationPropertyChangedSink));
        static void OnStopAnimationPropertyChangedSink(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var controller = ImageBehavior.GetAnimationController((Image)o);
            if (controller!=null)
                controller.Pause();
        }
    }*/
    
}
