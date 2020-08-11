using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Animation;

namespace WwtbamOld.Model.Old
{
    public sealed class PhoneAFriendTimerOld : DependencyObject, INotifyPropertyChanged
    {
        public PhoneAFriendTimerOld(PhoneAFriendOld model)
        {
            _model = model;
        }

        public void Start()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(100.0, 0.0, new Duration(new TimeSpan(0, 0, Duration)));
            _storyboard = new Storyboard();
            _storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(PercentageProperty));
            doubleAnimation.Completed += (s, e) => CountdownCompleted(this, EventArgs.Empty);
            _storyboard.Begin();
        }

        public void Stop()
        {
            if (_storyboard != null)
                _storyboard.Stop();
        }

        public event EventHandler CountdownCompleted;

        public int Duration
        {
            get => (int)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        private static void DurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PhoneAFriendTimerOld phoneTimer = d as PhoneAFriendTimerOld;
            phoneTimer.RaisePropertyChanged(nameof(Percentage));
            phoneTimer.RaisePropertyChanged(nameof(SecondsLeft));
        }

        public double Percentage
        {
            get => (double)GetValue(PercentageProperty);
            set => SetValue(PercentageProperty, value);
        }

        private static void PercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PhoneAFriendTimerOld phoneTimer = d as PhoneAFriendTimerOld;
            phoneTimer.SecondsLeft = (int)Math.Ceiling(phoneTimer.Duration * phoneTimer.Percentage / 100.0);
        }

        public int SecondsLeft
        {
            get => (int)GetValue(SecondsLeftProperty);
            set => SetValue(SecondsLeftProperty, value);
        }

        private static void SecondsLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PhoneAFriendTimerOld).RaisePropertyChanged(nameof(Percentage));
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateAllProperties()
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
                if (propertyInfo.CanRead)
                    RaisePropertyChanged(propertyInfo.Name);
        }

        private readonly PhoneAFriendOld _model;
        private Storyboard _storyboard;
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(int), typeof(PhoneAFriendTimerOld), new PropertyMetadata(30, new PropertyChangedCallback(DurationChanged)));
        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register("Percentage", typeof(double), typeof(PhoneAFriendTimerOld), new PropertyMetadata(0.0, new PropertyChangedCallback(PercentageChanged)));
        public static readonly DependencyProperty SecondsLeftProperty = DependencyProperty.Register("SecondsLeft", typeof(int), typeof(PhoneAFriendTimerOld), new PropertyMetadata(0, new PropertyChangedCallback(SecondsLeftChanged)));
    }
}