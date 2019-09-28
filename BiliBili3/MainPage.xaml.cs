using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BiliBili3.Controls;
using BiliBili3.Helper;
using BiliBili3.Models;
using BiliBili3.Modules;
using BiliBili3.Pages;
using BiliBili3.Pages.FindMore;
using BiliBili3.Pages.Music;
using BiliBili3.Pages.User;
using BiliBili3.Views;
using Microsoft.Graphics.Canvas.Effects;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

namespace BiliBili3
{
    public enum StartTypes
    {
        None,
        Video,
        Live,
        Bangumi,
        MiniVideo,
        Web,
        File,
        Article,
        Music,
        Album,
        User,
        HandelUri
    }
    public class StartModel
    {
        public StartTypes StartType { get; set; }
        public string Par1 { get; set; }
        public object Par3 { get; set; }
    }

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MusicHelper.InitializeMusicPlay();
            music.Visibility = Visibility.Collapsed;
            MusicHelper.MediaChanged += MusicHelper_MediaChanged;
            MusicHelper.DisplayEvent += MusicHelper_DisplayEvent;
            MusicHelper.UpdateList += MusicHelper_UpdateList1;
            musicplayer.SetMediaPlayer(MusicHelper._mediaPlayer);
            MessageCenter.NetworkError += MessageCenter_NetworkError;
            MessageCenter.ShowError += MessageCenter_ShowError;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            Window.Current.Content.PointerPressed += MainPage_PointerEntered;
        }

        private async void MessageCenter_ShowError(object sender, Exception e)
        {
            try
            {
                ErrorDialog errorDialog = new ErrorDialog(e);
                await errorDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void MessageCenter_NetworkError(object sender, string e)
        {
            network_error.Visibility = Visibility.Visible;
        }

        private async void MusicHelper_UpdateList1(object sender, List<MusicPlayModel> e)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                isSetMusic = true;

                ls_music.Items.Clear();
                foreach (var item in e)
                {
                    ls_music.Items.Add(item);
                }
                ls_music.SelectedIndex = MusicHelper._mediaPlaybackList.CurrentItemIndex.ToInt32();
                ls_music.UpdateLayout();
                isSetMusic = false;
            });
        }


        private async void MusicHelper_DisplayEvent(object sender, Visibility e)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                music.Visibility = e;
            });
        }

        private async void MusicHelper_MediaChanged(object sender, MusicPlayModel e)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                isSetMusic = true;
                btn_showMusicInfo.Tag = e.songid;
                music_img.Source = new BitmapImage(new Uri(e.pic));
                txt_musicInfo.Text = e.title + " - " + e.artist;
                ls_music.SelectedIndex = MusicHelper._mediaPlaybackList.CurrentItemIndex.ToInt32();
                isSetMusic = false;
            });
        }

        bool IsClicks = false;
        private async void RequestBack()
        {
            if (play_frame.CanGoBack)
            {
                play_frame.GoBack();
            }
            else
            {
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                }
                else
                {
                    if (_InBangumi)
                    {
                        main_frame.GoBack();
                    }
                    else
                    {
                        if (IsClicks)
                        {
                            Application.Current.Exit();
                        }
                        else
                        {
                            IsClicks = true;
                            Utils.ShowMessageToast("再按一次退出应用", 1500);
                            await Task.Delay(1500);
                            IsClicks = false;
                        }
                    }
                }
            }
        }

        private void MainPage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var par = e.GetCurrentPoint(sender as Frame).Properties.PointerUpdateKind;
            if (par == Windows.UI.Input.PointerUpdateKind.XButton1Pressed || par == Windows.UI.Input.PointerUpdateKind.MiddleButtonPressed)
            {
                if (!SettingHelper.MouseBack)
                {
                    return;
                }
                e.Handled = true;
                RequestBack();
            }
        }

        Account account;
        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            RequestBack();
        }

        DispatcherTimer timer;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            sp_View.DisplayMode = SplitViewDisplayMode.CompactOverlay;
            ChangeTheme();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
            timer.Tick += Timer_Tick;
            MessageCenter.ChanageThemeEvent += MessageCenter_ChanageThemeEvent;
            MessageCenter.MianNavigateToEvent += MessageCenter_MianNavigateToEvent;
            MessageCenter.InfoNavigateToEvent += MessageCenter_InfoNavigateToEvent;
            MessageCenter.PlayNavigateToEvent += MessageCenter_PlayNavigateToEvent;
            MessageCenter.HomeNavigateToEvent += MessageCenter_HomeNavigateToEvent;
            MessageCenter.BgNavigateToEvent += MessageCenter_BgNavigateToEvent; ;
            MessageCenter.Logined += MessageCenter_Logined;
            MessageCenter.ChangeBg += MessageCenter_ChangeBg;
            MessageCenter_ChangeBg();
            main_frame.Visibility = Visibility.Visible;
            menu_List.SelectedIndex = 0;
            Can_Nav = false;
            bottom.SelectedIndex = 0;
            Can_Nav = true;
            frame.Visibility = Visibility.Visible;
            frame.Navigate(typeof(BlankPage));

            play_frame.Visibility = Visibility.Visible;
            play_frame.Navigate(typeof(BlankPage));

            if (e.Parameter != null)
            {
                var m = e.Parameter as StartModel;
                switch (m.StartType)
                {
                    case StartTypes.None:
                        break;
                    case StartTypes.Video:
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(VideoViewPage), m.Par1);
                        break;
                    case StartTypes.Live:
                        MessageCenter.SendNavigateTo(NavigateMode.Play, typeof(LiveRoomPage), m.Par1);
                        break;
                    case StartTypes.Bangumi:
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(BanInfoPage), m.Par1);
                        break;
                    case StartTypes.MiniVideo:
                        MessageCenter.ShowMiniVideo(m.Par1);
                        break;
                    case StartTypes.Web:
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(WebPage), m.Par1);
                        break;
                    case StartTypes.Album:
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(DynamicInfoPage), m.Par1);
                        break;
                    case StartTypes.Article:
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(ArticleContentPage), m.Par1);
                        break;
                    case StartTypes.Music:
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(MusicInfoPage), m.Par1);
                        break;
                    case StartTypes.User:
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(UserInfoPage), m.Par1);
                        break;
                    case StartTypes.File:
                        var files = m.Par3 as IReadOnlyList<IStorageItem>;
                        List<PlayerModel> ls = new List<PlayerModel>();
                        int i = 1;
                        foreach (StorageFile file in files)
                        {
                            ls.Add(new PlayerModel() { Mode = PlayMode.FormLocal, No = i.ToString(), VideoTitle = "", Title = file.DisplayName, Parameter = file, Aid = file.DisplayName, Mid = file.Path });
                            i++;
                        }
                        play_frame.Navigate(typeof(PlayerPage), new object[] { ls, 0 });
                        break;
                    case StartTypes.HandelUri:
                        if (!await MessageCenter.HandelUrl(m.Par1))
                        {
                            ContentDialog contentDialog = new ContentDialog()
                            {
                                PrimaryButtonText = "确定",
                                Title = "不支持跳转的地址"
                            };
                            TextBlock textBlock = new TextBlock()
                            {
                                Text = m.Par1,
                                IsTextSelectionEnabled = true
                            };
                            contentDialog.Content = textBlock;
                            await contentDialog.ShowAsync();
                        }
                        break;
                    default:
                        break;
                }

            }


            if (SettingHelper.First)
            {
                TextBlock tx = new TextBlock()
                {
                    Text = string.Format(@"{0}", AppHelper.GetLastVersionStr()),
                    IsTextSelectionEnabled = true,
                    TextWrapping = TextWrapping.Wrap
                };
                await new ContentDialog() { Content = tx, PrimaryButtonText = "知道了" }.ShowAsync();


                SettingHelper.First = false;
            }


            new AppHelper().GetDeveloperMessage();

            account = new Account();
            //检查登录状态
            if (!string.IsNullOrEmpty(SettingHelper.Get_Access_key()))
            {
                if ((await account.CheckLoginState(ApiHelper.access_key)).success)
                {

                    MessageCenter_Logined();
                    await account.SSO(ApiHelper.access_key);
                }
                else
                {
                    var data = await account.RefreshToken(SettingHelper.Get_Access_key(), SettingHelper.Get_Refresh_Token());
                    if (!data.success)
                    {
                        Utils.ShowMessageToast("登录过期，请重新登录");
                        await Utils.ShowLoginDialog();
                    }
                }
            }

            if (SettingHelper.UseDASH && SystemHelper.GetSystemBuild() < 17763)
            {
                SettingHelper.UseDASH = false;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            MessageCenter.ChanageThemeEvent -= MessageCenter_ChanageThemeEvent;
            MessageCenter.MianNavigateToEvent -= MessageCenter_MianNavigateToEvent;
            MessageCenter.InfoNavigateToEvent -= MessageCenter_InfoNavigateToEvent;
            MessageCenter.PlayNavigateToEvent -= MessageCenter_PlayNavigateToEvent;
            MessageCenter.HomeNavigateToEvent -= MessageCenter_HomeNavigateToEvent;
            MessageCenter.BgNavigateToEvent -= MessageCenter_BgNavigateToEvent; ;
            MessageCenter.Logined -= MessageCenter_Logined;
            MessageCenter.ChangeBg -= MessageCenter_ChangeBg;
        }
        private async void MessageCenter_ChangeBg()
        {
            if (SettingHelper.CustomBG && SettingHelper.BGPath.Length != 0)
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(SettingHelper.BGPath);
                if (file != null)
                {
                    img_bg.Stretch = (Stretch)SettingHelper.BGStretch;
                    img_bg.HorizontalAlignment = (HorizontalAlignment)SettingHelper._BGHor;
                    img_bg.VerticalAlignment = (VerticalAlignment)SettingHelper.BGVer;
                    img_bg.Opacity = Convert.ToDouble(SettingHelper.BGOpacity) / 10;

                    if (SettingHelper.BGMaxWidth != 0)
                    {
                        img_bg.MaxWidth = SettingHelper.BGMaxWidth;
                    }
                    else
                    {
                        img_bg.MaxWidth = double.PositiveInfinity;
                    }
                    if (SettingHelper.BGMaxHeight != 0)
                    {
                        img_bg.MaxHeight = SettingHelper.BGMaxHeight;
                    }
                    else
                    {
                        img_bg.MaxHeight = double.PositiveInfinity;
                    }


                    var st = await file.OpenReadAsync();
                    BitmapImage bit = new BitmapImage();
                    await bit.SetSourceAsync(st);
                    img_bg.Source = bit;
                    if (SettingHelper.FrostedGlass != 0)
                    {
                        GlassHost.Visibility = Visibility.Visible;
                        InitializedFrostedGlass(GlassHost, SettingHelper.FrostedGlass);
                    }
                    else
                    {
                        GlassHost.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                img_bg.Source = null;
            }
        }


        private void InitializedFrostedGlass(UIElement glassHost, int d)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(glassHost);
            Compositor compositor = hostVisual.Compositor;

            // Create a glass effect, requires Win2D NuGet package
            var glassEffect = new GaussianBlurEffect
            {
                BlurAmount = d * 5.0f,
                BorderMode = EffectBorderMode.Hard,
                Source = new ArithmeticCompositeEffect
                {
                    MultiplyAmount = 0,
                    Source1Amount = 0.5f,
                    Source2Amount = 0.5f,
                    Source1 = new CompositionEffectSourceParameter("backdropBrush"),
                    Source2 = new ColorSourceEffect
                    {
                        Color = Color.FromArgb(255, 245, 245, 245)
                    }
                }
            };

            //  Create an instance of the effect and set its source to a CompositionBackdropBrush
            var effectFactory = compositor.CreateEffectFactory(glassEffect);
            var backdropBrush = compositor.CreateBackdropBrush();
            var effectBrush = effectFactory.CreateBrush();

            effectBrush.SetSourceParameter("backdropBrush", backdropBrush);

            // Create a Visual to contain the frosted glass effect
            var glassVisual = compositor.CreateSpriteVisual();
            glassVisual.Brush = effectBrush;

            // Add the blur as a child of the host in the visual tree
            ElementCompositionPreview.SetElementChildVisual(glassHost, glassVisual);

            // Make sure size of glass host and glass visual always stay in sync
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);

            glassVisual.StartAnimation("Size", bindSizeAnimation);
        }


        private void MessageCenter_BgNavigateToEvent(Type page, params object[] par)
        {
            bg_Frame.Navigate(page, par);
        }

        private void Storyboard_Completed(object sender, object e)
        {
            row_bottom.Height = new GridLength(0);
        }

        private async void MessageCenter_Logined()
        {
            btn_Login.Visibility = Visibility.Collapsed;
            btn_UserInfo.Visibility = Visibility.Visible;
            gv_User.Visibility = Visibility.Visible;
            try
            {
                var data = await account.GetMyInfo();
                if (data.success)
                {
                    var m = data.data;
                    gv_user.DataContext = data.data;
                    if (m.rank == 0 || m.rank == 5000)
                    {
                        dtzz.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        dtzz.Visibility = Visibility.Collapsed;
                    }
                    if (m.vip != null && m.vip.type != 0)
                    {
                        img_VIP.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        img_VIP.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    Utils.ShowMessageToast(data.message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void MessageCenter_HomeNavigateToEvent(Type page, params object[] par)
        {
            main_frame.Navigate(page, par);
        }

        private void MessageCenter_PlayNavigateToEvent(Type page, params object[] par)
        {
            if (SettingHelper.NewWindow)
            {
                MessageCenter.OpenNewWindow(page, par);
            }
            else
            {
                play_frame.Navigate(page, par);
            }
        }

        private void MessageCenter_InfoNavigateToEvent(Type page, params object[] par)
        {
            frame.Navigate(page, par);
        }

        private void MessageCenter_MianNavigateToEvent(Type page, params object[] par)
        {
            this.Frame.Navigate(page, par);
        }

        private void MessageCenter_ChanageThemeEvent(object par, params object[] par1)
        {
            ChangeTheme();
        }

        private void ChangeTheme()
        {

            switch (SettingHelper.Rigth)
            {
                case 1:
                    bg_Frame.Navigate(typeof(FastNavigatePage));
                    break;
                case 2:
                    bg_Frame.Navigate(typeof(HomePage));
                    break;
                case 3:
                    bg_Frame.Navigate(typeof(RankPage));
                    break;
                case 4:
                    bg_Frame.Navigate(typeof(TimelinePage));
                    break;
                case 5:
                    bg_Frame.Navigate(typeof(LiveAllPage));
                    break;
                default:
                    bg_Frame.Navigate(typeof(BlankPage));
                    break;
            }




            string ThemeName = SettingHelper.Theme;
            ResourceDictionary newDictionary = new ResourceDictionary();
            switch (ThemeName)
            {
                case "Dark":
                    RequestedTheme = ElementTheme.Dark;

                    break;
                case "Red":
                case "Blue":
                case "Green":
                case "Pink":
                case "Purple":
                case "Yellow":
                case "EMT":
                    newDictionary.Source = new Uri($"ms-appx:///Theme/{ThemeName}Theme.xaml", UriKind.RelativeOrAbsolute);
                    Application.Current.Resources.MergedDictionaries.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(newDictionary);
                    RequestedTheme = ElementTheme.Light;
                    break;
            }
            ChangeTitlebarColor();
        }
        private void ChangeTitlebarColor()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = ((SolidColorBrush)grid_Top.Background).Color;
            titleBar.ForegroundColor = Color.FromArgb(255, 254, 254, 254);//Colors.White纯白用不了。。。
            titleBar.ButtonHoverBackgroundColor = ((SolidColorBrush)sp_View.PaneBackground).Color;
            titleBar.ButtonBackgroundColor = ((SolidColorBrush)grid_Top.Background).Color;
            titleBar.ButtonForegroundColor = Color.FromArgb(255, 254, 254, 254);
            titleBar.InactiveBackgroundColor = ((SolidColorBrush)grid_Top.Background).Color;
            titleBar.ButtonInactiveBackgroundColor = ((SolidColorBrush)grid_Top.Background).Color;
        }

        private async void Timer_Tick(object sender, object e)
        {
            //if (ApiHelper.IsLogin())
            //{
            if (await HasMessage())
            {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    //menu_bor_HasMessage

                    bor_TZ.Visibility = Visibility.Visible;
                });
            }
            else
            {

                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    bor_TZ.Visibility = Visibility.Collapsed;
                });
            }
        }

        private async Task<bool> HasMessage()
        {
            try
            {
                if (!ApiHelper.IsLogin())
                {
                    return false;
                }
                // http://message.bilibili.com/api/msg/query.room.list.do?access_key=a36a84cc8ef4ea2f92c416951c859a25&actionKey=appkey&appkey=c1b107428d337928&build=414000&page_size=100&platform=android&ts=1461404884000&sign=5e212e424761aa497a75b0fb7fbde775
                string url = string.Format("http://message.bilibili.com/api/notify/query.notify.count.do?_device=wp&_ulv=10000&access_key={0}&actionKey=appkey&appkey={1}&build=5250000&platform=android&ts={2}", ApiHelper.access_key, ApiHelper.AndroidKey.Appkey, ApiHelper.GetTimeSpan);
                url += "&sign=" + ApiHelper.GetSign(url);
                string results = await WebClientClass.GetResults(new Uri(url));
                MessageModel model = JsonConvert.DeserializeObject<MessageModel>(results);

                if (model.code == 0)
                {
                    MessageModel list = JsonConvert.DeserializeObject<MessageModel>(model.data.ToString());
                    if (list.reply_me != 0 || list.chat_me != 0 || list.notify_me != 0 || list.praise_me != 0 || list.at_me != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
                //Utils.ShowMessageToast("读取通知失败", 3000);
            }
        }

        //侧滑来源http://www.cnblogs.com/hebeiDGL/p/4775377.html
        #region  从屏幕左侧边缘滑动屏幕时，打开 SplitView 菜单

        // SplitView 控件模板中，Pane部分的 Grid
        Grid PaneRoot;

        //  引用 SplitView 控件中， 保存从 Pane “关闭” 到“打开”的 VisualTransition
        //  也就是 <VisualTransition From="Closed" To="OpenOverlayLeft"> 这个 
        VisualTransition from_ClosedToOpenOverlayLeft_Transition;

        private void Border_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            e.Handled = true;

            // 仅当 SplitView 处于 Overlay 模式时（窗口宽度最小时）
            if (sp_View.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                if (PaneRoot == null)
                {
                    // 找到 SplitView 控件中，模板的父容器
                    Grid grid = FindVisualChild<Grid>(sp_View);

                    PaneRoot = grid.FindName("PaneRoot") as Grid;

                    if (from_ClosedToOpenOverlayLeft_Transition == null)
                    {
                        // 获取 SplitView 模板中“视觉状态集合”
                        IList<VisualStateGroup> stateGroup = VisualStateManager.GetVisualStateGroups(grid);

                        //  获取 VisualTransition 对象的集合。
                        IList<VisualTransition> transitions = stateGroup[0].Transitions;

                        // 找到 SplitView.IsPaneOpen 设置为 true 时，播放的 transition
                        from_ClosedToOpenOverlayLeft_Transition = transitions?.Where(train => train.From == "Closed" && train.To == "OpenOverlayLeft").First();
                    }
                }


                // 默认为 Collapsed，所以先显示它
                PaneRoot.Visibility = Visibility.Visible;

                // 当在 Border 上向右滑动，并且滑动的总距离需要小于 Panel 的默认宽度。否则会脱离左侧窗口，继续向右拖动
                if (e.Cumulative.Translation.X >= 0 && e.Cumulative.Translation.X < sp_View.OpenPaneLength)
                {
                    CompositeTransform ct = PaneRoot.RenderTransform as CompositeTransform;
                    ct.TranslateX = (e.Cumulative.Translation.X - sp_View.OpenPaneLength);
                }
            }
        }

        private void Border_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;

            // 仅当 SplitView 处于 Overlay 模式时（窗口宽度最小时）
            if (sp_View.DisplayMode == SplitViewDisplayMode.Overlay && PaneRoot != null)
            {
                // 因为当 IsPaneOpen 为 true 时，会通过 VisualStateManager 把 PaneRoot.Visibility  设置为
                // Visibility.Visible，所以这里把它改为 Visibility.Collapsed，以回到初始状态
                PaneRoot.Visibility = Visibility.Collapsed;

                // 恢复初始状态 
                CompositeTransform ct = PaneRoot.RenderTransform as CompositeTransform;


                // 如果大于 MySplitView.OpenPaneLength 宽度的 1/2 ，则显示，否则隐藏
                if ((sp_View.OpenPaneLength + ct.TranslateX) > sp_View.OpenPaneLength / 2)
                {
                    sp_View.IsPaneOpen = true;

                    // 因为上面设置 IsPaneOpen = true 会再次播放向右滑动的动画，所以这里使用 SkipToFill()
                    // 方法，直接跳到动画结束状态
                    from_ClosedToOpenOverlayLeft_Transition?.Storyboard?.SkipToFill();

                }

                ct.TranslateX = 0;
            }
        }


        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            int count = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }
        #endregion


        private static string GetTagString(string tag)
        {
            return tag switch
            {
                "Cn" => "国漫",
                "Jp" => "番剧",
                "Music" => "音频",
                "Article" => "专栏",
                _ => null
            };
        }

        bool _InBangumi = false;

        private void play_frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (play_frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                if (!frame.CanGoBack)
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }
            }
            var text = GetTagString((main_frame.Content as Page)?.Tag?.ToString());
            if (text != null)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                _InBangumi = true;
                txt_Header.Text = text;
            }
        }
        private void frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                if (!play_frame.CanGoBack)
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }

            }
            var text = GetTagString((main_frame.Content as Page)?.Tag?.ToString());
            if (text != null)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                _InBangumi = true;
                txt_Header.Text = text;
            }
        }

        private static readonly string[] tagList = { "首页", "频道", "直播", "番剧", "动态", "发现", "设置" };
        private static readonly Type[] tagPageList = { typeof(NewFeedPage), typeof(HomePage), typeof(LiveV2Page), typeof(BangumiPage), typeof(AttentionPage), typeof(FindPage) };

        private int GetTagListIndex(string tag)
        {
            return Array.IndexOf(tagList, tag);
        }

        private void main_frame_Navigated(object sender, NavigationEventArgs e)
        {
            if ((main_frame.Content as Page).Tag == null)
            {
                return;
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Can_Nav = false;
            _InBangumi = false;
            var tag = (main_frame.Content as Page)?.Tag?.ToString();
            if (tag != null)
            {
                var index = GetTagListIndex(tag);
                if (index >= 0)
                {
                    bottom.SelectedIndex = index;
                    menu_List.SelectedIndex = index;
                    txt_Header.Text = tag;
                }
                else
                {
                    var text = GetTagString(tag);
                    if (text != null)
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                        _InBangumi = true;
                        txt_Header.Text = text;
                    }
                }
            }
            Can_Nav = true;
        }
        bool Can_Nav = true;
        private void NavigateTagList()
        {
            if (!Can_Nav)
            {
                return;
            }
            var index = menu_List.SelectedIndex;
            if (index < 6)
            {
                main_frame.Navigate(tagPageList[index]);
                txt_Header.Text = tagList[index];
            }
            sp_View.IsPaneOpen = false;
        }

        private void menu_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigateTagList();
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(SearchPage));
        }

        private async void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            LoginDialog loginDialog = new LoginDialog();
            await loginDialog.ShowAsync();
            fy.Hide();
        }

        private void btn_LogOut_Click(object sender, RoutedEventArgs e)
        {
            UserManage.Logout();
            btn_Login.Visibility = Visibility.Visible;
            btn_UserInfo.Visibility = Visibility.Collapsed;
            gv_User.Visibility = Visibility.Collapsed;
            fy.Hide();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bor_Width.Width = (this.ActualWidth / 6) - 2;
        }

        private void bottom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigateTagList();
        }

        private void btn_user_myvip_Click(object sender, RoutedEventArgs e)
        {
            //http://big.bilibili.com/site/big.html
            frame.Navigate(typeof(WebPage), new object[] { "https://big.bilibili.com/mobile/home" });
            fy.Hide();
        }

        private void btn_user_mycollect_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(MyCollectPage));
            fy.Hide();
        }

        private void btn_user_mychistory_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(MyHistroryPage));
            fy.Hide();
        }

        private void btn_user_mywallet_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(MyWalletPage));
            fy.Hide();
        }

        private void dtzz_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(WebPage), "https://account.bilibili.com/answer/base");
            fy.Hide();
        }

        private void btn_UserInfo_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(UserInfoPage));
            fy.Hide();
        }

        private void btn_user_myGuanzhu_Click(object sender, RoutedEventArgs e)
        {
            MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(WebPage), "https://space.bilibili.com/h5/follow");
            //frame.Navigate(typeof(UserInfoPage), new object[] { null, 2 });
            fy.Hide();
        }

        private void btn_user_mymessage_Click(object sender, RoutedEventArgs e)
        {

            frame.Navigate(typeof(MyMessagePage));
            fy.Hide();
        }

        private void btn_user_Qr_Click(object sender, RoutedEventArgs e)
        {

            //var info = gv_user.DataContext as UserInfoModel;

            frame.Navigate(typeof(MyQrPage), new object[] { new MyqrModel() {
                  name=Account.myInfo.name,
                  photo=Account.myInfo.face,
                  qr=string.Format("http://qr.liantu.com/api.php?w=500&text={0}&inpt=00AAF0&logo={1}",Uri.EscapeDataString("http://space.bilibili.com/"+ApiHelper.GetUserId()),Uri.EscapeDataString(Account.myInfo.face)),
                  sex=Account.myInfo.Sex
            } });
            fy.Hide();
        }

        private void btn_Qr_Click(object sender, RoutedEventArgs e)
        {
            play_frame.Navigate(typeof(QRPage));
        }

        private void btn_Down_Click(object sender, RoutedEventArgs e)
        {

            frame.Navigate(typeof(Download2Page));
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (e.ClickedItem as StackPanel).Tag.ToString();
            if (item == "ToView")
            {
                if (!ApiHelper.IsLogin() && !await Utils.ShowLoginDialog())
                {
                    Utils.ShowMessageToast("请先登录");
                    return;
                }
                MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(ToViewPage));
            }
            else if (item == "Test")
            {
                try
                {
                    var url = new Uri($"https://www.biliplus.com/login?act=savekey&mid={UserManage.Uid}&access_key={ApiHelper.access_key}&expire=");
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var rq = await httpClient.GetAsync(url);
                        var setCookie = rq.Headers["set-cookie"];
                        StringBuilder stringBuilder = new StringBuilder();
                        var matches = Regex.Matches(setCookie, "(.*?)=(.*?); ", RegexOptions.Singleline);
                        foreach (Match match in matches)
                        {
                            var key = match.Groups[1].Value.Replace("HttpOnly, ", "");
                            var value = match.Groups[2].Value;
                            if (key != "expires" && key != "Max-Age" && key != "path" && key != "domain")
                            {
                                stringBuilder.Append(match.Groups[0].Value.Replace("HttpOnly, ", ""));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Authentication failed. Handle parameter, SSL/TLS, and Network Unavailable errors here. 
                    Debug.WriteLine(ex);
                    throw;
                }

            }
            else
            {
                MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(SettingPage));
            }
        }

        private void btn_user_moviecollect_Click(object sender, RoutedEventArgs e)
        {
            MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(FollowSeasonPage), Modules.SeasonType.cinema);
            fy.Hide();

        }

        private void btn_ClearMedia_Click(object sender, RoutedEventArgs e)
        {
            MusicHelper.ClearMediaList();
        }

        bool isSetMusic = false;
        private void ls_music_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isSetMusic)
            {
                MusicHelper._mediaPlaybackList.MoveTo(Convert.ToUInt32(ls_music.SelectedIndex));
            }
        }

        private void btn_showMusicInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(MusicInfoPage), (sender as AppBarButton).Tag.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MusicHelper._mediaPlayer.SystemMediaTransportControls.DisplayUpdater.Update();
        }

        private void btn_MiniPlayer_Click(object sender, RoutedEventArgs e)
        {
            MessageCenter.SendNavigateTo(NavigateMode.Play, typeof(MusicMiniPlayerPage));
        }

        private void btn_music_Shuffle_Click(object sender, RoutedEventArgs e)
        {
            MusicHelper._mediaPlaybackList.ShuffleEnabled = true;
            btn_music_Shuffle.Visibility = Visibility.Collapsed;
            btn_music_List.Visibility = Visibility.Visible;
        }

        private void btn_music_List_Click(object sender, RoutedEventArgs e)
        {
            MusicHelper._mediaPlaybackList.ShuffleEnabled = false;
            btn_music_Shuffle.Visibility = Visibility.Visible;
            btn_music_List.Visibility = Visibility.Collapsed;
        }

        private void btn_user_toView_Click(object sender, RoutedEventArgs e)
        {
            MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(ToViewPage));
        }

        private void btn_IKonwn_Click(object sender, RoutedEventArgs e)
        {
            network_error.Visibility = Visibility.Collapsed;
        }
    }
}
