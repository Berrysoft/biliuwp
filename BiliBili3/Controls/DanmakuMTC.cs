﻿using System;
using BiliBili3.Helper;
using NSDanmaku.Controls;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace BiliBili3.Controls
{
    public enum PlayMode
    {
        Bangumi,
        Movie,
        VipBangumi,
        Video,
        QQ,
        Sohu,
        Local,
        FormLocal
    }

    public sealed class DanmakuMTC : MediaTransportControls
    {
        public DanmakuMTC()
        {
            this.DefaultStyleKey = typeof(DanmakuMTC);
            VideoTitle = string.Empty;
            ShowDanmakuBtn = Visibility.Visible;
            ShowSendDanmaku = Visibility.Visible;
            ShowCoinsBtn = Visibility.Visible;
            ShowShareBtn = Visibility.Visible;
            ShowSendDanmuBtn = false;
            ShowNextButton = false;
            ShowPreviousButton = false;
            Brightness = 0;
            ShowDanmakuSettingBtn = Visibility.Visible;
            ShowPlayListBtn = false;

            SubTitleBackground = new SolidColorBrush(Color.FromArgb(120, 0, 0, 0));
            SubTitleColor = new SolidColorBrush(Colors.Red);
            SubTitleFontFamily = new FontFamily("Microsoft YaHei UI");
            SubTitleFontSize = 25.0;

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;

            timer2 = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            timer2.Tick += Timer2_Tick;
        }

        private void Timer2_Tick(object sender, object e)
        {
            try
            {
                HideOrShowMTC();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            try
            {
                (GetTemplateChild("txt_time") as TextBlock).Text = DateTime.Now.ToString("HH:mm");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
            }
        }

        private DispatcherTimer timer;
        public DispatcherTimer timer2;
        public event EventHandler<Danmaku> DanmuLoaded;
        public event EventHandler OnMiniWindows;
        public event EventHandler ExitPlayer;
        public event EventHandler OnExitMiniWindows;
        public event EventHandler DanmakuSetting;
        public event EventHandler CoinsEvent;
        public event EventHandler ShareEvent;
        public event EventHandler<bool> OpenDanmaku;
        public event EventHandler Next;
        public event EventHandler Previous;
        public event EventHandler SelectList;
        public event EventHandler SendDanmakued;
        public event EventHandler FullWindows;
        public event EventHandler ExitFullWindows;
        public event EventHandler Captured;
        public event EventHandler<double> FastForward;
        public bool IsMiniWindow = false;
        public bool IsFullWindow = false;

        //public Danmaku danmuControls;
        protected override void OnApplyTemplate()
        {
            try
            {
                DanmuLoaded?.Invoke(this, GetTemplateChild("danmuControls") as Danmaku);
                MyDanmaku = GetTemplateChild("danmuControls") as Danmaku;
                (GetTemplateChild("MiniWindowsButton") as AppBarButton).Click += MiniWindowsButton_Click;
                (GetTemplateChild("btn_Back") as AppBarButton).Click += ExitButton_Click;
                (GetTemplateChild("btn_danmaku") as AppBarButton).Click += CloseOpenDanmaku_Click;
                (GetTemplateChild("btn_danmakusetting") as AppBarButton).Click += DanmakuSetting_Click;
                (GetTemplateChild("btn_Previous") as AppBarButton).Click += btn_Previous_Click;
                (GetTemplateChild("btn_Next") as AppBarButton).Click += btn_Next_Click;
                (GetTemplateChild("ListButton") as AppBarButton).Click += btn_Playlist_Click;
                (GetTemplateChild("CoinsBtn") as AppBarButton).Click += Btn_Coins_Click;
                (GetTemplateChild("ShareBtn") as AppBarButton).Click += Btn_Share_Click;
                (GetTemplateChild("btn_send") as AppBarButton).Click += Btn_Send_Click;
                (GetTemplateChild("MyFullWindowButton") as AppBarButton).Click += Btn_Full_Click;
                (GetTemplateChild("CaptureBtn") as AppBarButton).Click += Btn_Capture_Click; ;

                if (!SettingHelper.DMStatus)
                {
                    (GetTemplateChild("btn_danmaku") as AppBarButton).Icon = new BitmapIcon()
                    {
                        UriSource = new Uri("ms-appx:///Assets/PlayerAssets/ic_player_danmaku_input_options_rl_disabled.png")
                    };
                }

                (GetTemplateChild("ControlPanel_ControlPanelVisibilityStates_Border") as Border).Tapped += DanmakuMTC_Tapped;

                (GetTemplateChild("RootGrid") as Grid).ManipulationStarting += MTC_ManipulationStarting;
                (GetTemplateChild("RootGrid") as Grid).ManipulationDelta += Grid_ManipulationDelta;
                (GetTemplateChild("RootGrid") as Grid).ManipulationCompleted += Grid_ManipulationCompleted;
                try
                {
                    if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
                    {
                        (GetTemplateChild("MiniWindowsButton") as AppBarButton).IsEnabled = true;
                    }
                    else
                    {
                        (GetTemplateChild("MiniWindowsButton") as AppBarButton).IsEnabled = false;
                    }
                }
                catch (Exception)
                {
                    (GetTemplateChild("MiniWindowsButton") as AppBarButton).IsEnabled = false;
                }

                timer.Start();
                base.OnApplyTemplate();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
            }
        }

        public void HideOrShowMTC()
        {
            var root = GetTemplateChild("RootGrid") as Grid;
            var bar = GetTemplateChild("ControlPanel_ControlPanelVisibilityStates_Border") as Border;
            if (bar.Visibility == Visibility.Visible)
            {
                (root.Resources["FadeOut"] as Storyboard).Begin();
                (root.Resources["FadeOut"] as Storyboard).Completed += new EventHandler<object>((sender, e) =>
                {
                    bar.Visibility = Visibility.Collapsed;
                });
                timer2.Stop();
            }
            else
            {
                bar.Visibility = Visibility.Visible;
                (root.Resources["FadeIn"] as Storyboard).Begin();
                timer2.Start();
            }
        }

        private double ssValue = 0;
        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            e.Handled = true;

            //progress.Visibility = Visibility.Visible;
            double X = e.Delta.Translation.X;
            if (X > 0)
            {
                double dd = X / this.ActualWidth;
                double d = dd * 90;
                ssValue += d;
                Utils.ShowMessageToast("+" + ssValue.ToInt32() + "S", 2000);
            }
            else
            {
                double dd = Math.Abs(X) / this.ActualWidth;
                double d = dd * 90;
                ssValue -= d;
                Utils.ShowMessageToast("-" + ssValue.ToInt32() + "S", 2000);
            }
        }

        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;

            double X = e.Cumulative.Translation.X;
            if (ssValue != 0 && FastForward != null)
            {
                FastForward?.Invoke(sender, ssValue);
            }
        }

        private void MTC_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            e.Handled = true;
            ssValue = 0;
        }

        private void DanmakuMTC_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if ((sender as Border).Visibility == Visibility.Visible)
            {
                (sender as Border).Visibility = Visibility.Collapsed;
            }
            else
            {
                (sender as Border).Visibility = Visibility.Visible;
            }
        }

        private void Btn_Capture_Click(object sender, RoutedEventArgs e)
        {
            Captured?.Invoke(sender, EventArgs.Empty);
        }

        private void Btn_Full_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFullWindow)
            {
                IsFullWindow = true;
                (sender as AppBarButton).Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/PlayerAssets/narrow.png") };

                FullWindows?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                IsFullWindow = false;
                (sender as AppBarButton).Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/PlayerAssets/fullscreen.png") };
                ExitFullWindows?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ToFull()
        {
            IsFullWindow = true;
            (GetTemplateChild("MyFullWindowButton") as AppBarButton).Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/PlayerAssets/narrow.png") };

            FullWindows?.Invoke(this, EventArgs.Empty);
        }

        public void ExitFull()
        {
            IsFullWindow = false;
            (GetTemplateChild("MyFullWindowButton") as AppBarButton).Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/PlayerAssets/fullscreen.png") };
            ExitFullWindows?.Invoke(this, EventArgs.Empty);
        }

        public void SetSubtitle(string text)
        {
            (GetTemplateChild("text_subtitle") as TextBlock).Text = text;
        }
        public string GetSubtitle()
        {
            return (GetTemplateChild("text_subtitle") as TextBlock).Text;
        }
        public void HideSubtitle()
        {
            (GetTemplateChild("grid_subtitle") as Grid).Visibility = Visibility.Collapsed;
        }
        public void ShowSubtitle()
        {
            (GetTemplateChild("grid_subtitle") as Grid).Visibility = Visibility.Visible;
        }
        private void Btn_Send_Click(object sender, RoutedEventArgs e)
        {
            SendDanmakued?.Invoke(sender, EventArgs.Empty);
        }

        private void Btn_Share_Click(object sender, RoutedEventArgs e)
        {
            ShareEvent?.Invoke(sender, EventArgs.Empty);
        }

        private void Btn_Coins_Click(object sender, RoutedEventArgs e)
        {
            CoinsEvent?.Invoke(sender, EventArgs.Empty);
        }

        private void btn_Playlist_Click(object sender, RoutedEventArgs e)
        {
            SelectList?.Invoke(sender, EventArgs.Empty);
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            Next?.Invoke(sender, EventArgs.Empty);
        }

        private void btn_Previous_Click(object sender, RoutedEventArgs e)
        {
            Previous?.Invoke(sender, EventArgs.Empty);
        }

        private void DanmakuSetting_Click(object sender, RoutedEventArgs e)
        {
            DanmakuSetting?.Invoke(sender, EventArgs.Empty);
        }

        private void CloseOpenDanmaku_Click(object sender, RoutedEventArgs e)
        {
            MyDanmaku.ClearAll();
            if (MyDanmaku.Visibility == Visibility.Visible)
            {
                MyDanmaku.Visibility = Visibility.Collapsed;
                (sender as AppBarButton).Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/PlayerAssets/ic_player_danmaku_input_options_rl_disabled.png") };
            }
            else
            {
                MyDanmaku.Visibility = Visibility.Visible;
                (sender as AppBarButton).Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/PlayerAssets/ic_player_danmaku_input_options_rl_checked.png") };
            }
            OpenDanmaku?.Invoke(this, false);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitPlayer?.Invoke(sender, EventArgs.Empty);
        }

        private void MiniWindowsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsMiniWindow)
            {
                IsMiniWindow = true;
                (sender as AppBarButton).Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/PlayerAssets/down.png") };

                OnMiniWindows?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                IsMiniWindow = false;
                (sender as AppBarButton).Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/PlayerAssets/top.png") };
                OnExitMiniWindows?.Invoke(this, EventArgs.Empty);
            }
        }



        public void AddLog(string text)
        {
            var tb = GetTemplateChild("Txt_Log") as TextBlock;
            tb.Visibility = Visibility.Visible;
            tb.Text += $"[{DateTime.Now.ToString("HH:mm:ss")}]{text}\r\n";
        }
        public void ShowLog()
        {
            var tb = GetTemplateChild("Txt_Log") as TextBlock;
            tb.Visibility = Visibility.Visible;
        }
        public void HideLog()
        {
            var tb = GetTemplateChild("Txt_Log") as TextBlock;
            tb.Visibility = Visibility.Collapsed;
        }
        public void ClearLog()
        {
            var tb = GetTemplateChild("Txt_Log") as TextBlock;
            tb.Text = string.Empty;
        }

        public string VideoTitle
        {
            get => (string)GetValue(VideoTitleProperty);
            set => SetValue(VideoTitleProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VideoTitleProperty =
            DependencyProperty.Register(nameof(VideoTitle), typeof(string), typeof(MediaTransportControls), new PropertyMetadata(string.Empty));

        public Danmaku MyDanmaku
        {
            get => (Danmaku)GetValue(DanmakuProperty);
            set => SetValue(DanmakuProperty, value);
        }

        // Using a DependencyProperty as the backing store for DanmakuProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DanmakuProperty =
            DependencyProperty.Register(nameof(MyDanmaku), typeof(Danmaku), typeof(MediaTransportControls), new PropertyMetadata(null));

        public double Brightness
        {
            get => (double)GetValue(BrightnessProperty);
            set => SetValue(BrightnessProperty, value);
        }

        // Using a DependencyProperty as the backing store for BrightnessProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BrightnessProperty =
            DependencyProperty.Register(nameof(Brightness), typeof(double), typeof(MediaTransportControls), new PropertyMetadata(0));

        public Visibility ShowDanmakuBtn
        {
            get => (Visibility)GetValue(ShowDanmakuBtnProperty);
            set => SetValue(ShowDanmakuBtnProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowDanmakuBtn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowDanmakuBtnProperty =
            DependencyProperty.Register(nameof(ShowDanmakuBtn), typeof(Visibility), typeof(MediaTransportControls), new PropertyMetadata(0));

        public Visibility ShowSendDanmaku
        {
            get => (Visibility)GetValue(ShowSendDanmakuProperty);
            set => SetValue(ShowSendDanmakuProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowSendDanmaku.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSendDanmakuProperty =
            DependencyProperty.Register(nameof(ShowSendDanmaku), typeof(Visibility), typeof(MediaTransportControls), new PropertyMetadata(Visibility.Visible));

        public UIElement QuityContent
        {
            get => (UIElement)GetValue(QuityContentProperty);
            set => SetValue(QuityContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for QuityContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuityContentProperty =
            DependencyProperty.Register(nameof(QuityContent), typeof(UIElement), typeof(MediaTransportControls), new PropertyMetadata(null));

        public FlyoutBase MoreMenuFlyout
        {
            get => (FlyoutBase)GetValue(MoreMenuFlyoutProperty);
            set => SetValue(MoreMenuFlyoutProperty, value);
        }

        // Using a DependencyProperty as the backing store for MoreMenuFlyout.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoreMenuFlyoutProperty =
            DependencyProperty.Register(nameof(MoreMenuFlyout), typeof(FlyoutBase), typeof(MediaTransportControls), new PropertyMetadata(0));

        public Visibility ShowCoinsBtn
        {
            get => (Visibility)GetValue(ShowCoinsBtnProperty);
            set => SetValue(ShowCoinsBtnProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowCoinsBtn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCoinsBtnProperty =
            DependencyProperty.Register(nameof(ShowCoinsBtn), typeof(Visibility), typeof(MediaTransportControls), new PropertyMetadata(Visibility.Visible));

        public Visibility ShowShareBtn
        {
            get => (Visibility)GetValue(ShowShareBtnProperty);
            set => SetValue(ShowShareBtnProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowShareBtn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowShareBtnProperty =
            DependencyProperty.Register(nameof(ShowShareBtn), typeof(Visibility), typeof(MediaTransportControls), new PropertyMetadata(Visibility.Visible));

        public Visibility ShowDanmakuSettingBtn
        {
            get => (Visibility)GetValue(ShowDanmakuSettingBtnProperty);
            set => SetValue(ShowDanmakuSettingBtnProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowDanmakuSettingBtn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowDanmakuSettingBtnProperty =
            DependencyProperty.Register(nameof(ShowDanmakuSettingBtn), typeof(Visibility), typeof(MediaTransportControls), new PropertyMetadata(Visibility.Visible));

        public bool ShowNextButton
        {
            get => (bool)GetValue(ShowNextButtonProperty);
            set => SetValue(ShowNextButtonProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowNextButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowNextButtonProperty =
            DependencyProperty.Register(nameof(ShowNextButton), typeof(bool), typeof(MediaTransportControls), new PropertyMetadata(false));

        public bool ShowPreviousButton
        {
            get => (bool)GetValue(ShowPreviousButtonProperty);
            set => SetValue(ShowPreviousButtonProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowNextButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPreviousButtonProperty =
            DependencyProperty.Register(nameof(ShowPreviousButton), typeof(bool), typeof(MediaTransportControls), new PropertyMetadata(false));

        public bool ShowPlayListBtn
        {
            get => (bool)GetValue(ShowPlayListBtnProperty);
            set => SetValue(ShowPlayListBtnProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowPlayListBtn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPlayListBtnProperty =
            DependencyProperty.Register(nameof(ShowPlayListBtn), typeof(bool), typeof(MediaTransportControls), new PropertyMetadata(false));

        public bool ShowSendDanmuBtn
        {
            get => (bool)GetValue(ShowSendDanmuBtnProperty);
            set => SetValue(ShowSendDanmuBtnProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowSendDanmuBtn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSendDanmuBtnProperty =
            DependencyProperty.Register(nameof(ShowSendDanmuBtn), typeof(bool), typeof(MediaTransportControls), new PropertyMetadata(false));

        public UIElement GestureContent
        {
            get => (UIElement)GetValue(GestureContentProperty);
            set => SetValue(GestureContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for GestureContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GestureContentProperty =
            DependencyProperty.Register(nameof(GestureContent), typeof(UIElement), typeof(MediaTransportControls), new PropertyMetadata(null));

        //CCSelectFlyout
        public FlyoutBase CCSelectFlyout
        {
            get => (FlyoutBase)GetValue(CCSelectFlyoutProperty);
            set => SetValue(CCSelectFlyoutProperty, value);
        }

        public static readonly DependencyProperty CCSelectFlyoutProperty =
            DependencyProperty.Register(nameof(CCSelectFlyout), typeof(FlyoutBase), typeof(MediaTransportControls), new PropertyMetadata(0));

        /// <summary>
        /// 字幕字体大小
        /// </summary>
        public double SubTitleFontSize
        {
            get => (int)GetValue(SubTitleFontSizeProperty);
            set => SetValue(SubTitleFontSizeProperty, value);
        }
        public static readonly DependencyProperty SubTitleFontSizeProperty =
            DependencyProperty.Register(nameof(SubTitleFontSize), typeof(double), typeof(MediaTransportControls), new PropertyMetadata(25.0));

        /// <summary>
        /// 字幕字体
        /// </summary>
        public FontFamily SubTitleFontFamily
        {
            get => (FontFamily)GetValue(SubTitleFontFamilyProperty);
            set => SetValue(SubTitleFontFamilyProperty, value);
        }

        public static readonly DependencyProperty SubTitleFontFamilyProperty =
            DependencyProperty.Register(nameof(SubTitleFontFamily), typeof(FontFamily), typeof(MediaTransportControls), new PropertyMetadata(new FontFamily("Segoe UI")));

        /// <summary>
        /// 字幕背景颜色
        /// </summary>
        public Brush SubTitleBackground
        {
            get => (Brush)GetValue(SubTitleBackgroundProperty);
            set => SetValue(SubTitleBackgroundProperty, value);
        }

        public static readonly DependencyProperty SubTitleBackgroundProperty =
            DependencyProperty.Register(nameof(SubTitleBackground), typeof(Brush), typeof(MediaTransportControls), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(120, 0, 0, 0))));

        /// <summary>
        /// 字幕字体颜色
        /// </summary>
        public Brush SubTitleColor
        {
            get => (Brush)GetValue(SubTitleColorProperty);
            set => SetValue(SubTitleColorProperty, value);
        }

        public static readonly DependencyProperty SubTitleColorProperty =
            DependencyProperty.Register(nameof(SubTitleColor), typeof(Brush), typeof(MediaTransportControls), new PropertyMetadata(new SolidColorBrush(Colors.White)));
    }
}
