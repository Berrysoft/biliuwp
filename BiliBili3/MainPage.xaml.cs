using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using muxc = Microsoft.UI.Xaml.Controls;

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

        private bool IsClicks = false;
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

        private Account account;

        private DispatcherTimer timer;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ChangeTheme();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
            timer.Tick += Timer_Tick;
            MessageCenter.ChanageThemeEvent += MessageCenter_ChanageThemeEvent;
            MessageCenter.MainNavigateToEvent += MessageCenter_MainNavigateToEvent;
            MessageCenter.InfoNavigateToEvent += MessageCenter_InfoNavigateToEvent;
            MessageCenter.PlayNavigateToEvent += MessageCenter_PlayNavigateToEvent;
            MessageCenter.HomeNavigateToEvent += MessageCenter_HomeNavigateToEvent;
            MessageCenter.BgNavigateToEvent += MessageCenter_BgNavigateToEvent; ;
            MessageCenter.Logined += MessageCenter_Logined;
            MessageCenter.ChangeBg += MessageCenter_ChangeBg;
            MessageCenter_ChangeBg();
            main_frame.Visibility = Visibility.Visible;
            NavigateTagList("NewFeed");
            sp_View.SelectedItem = sp_View.MenuItems[0];
            //Can_Nav = false;
            //bottom.SelectedIndex = 0;
            //Can_Nav = true;
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
                    Text = AppHelper.GetLastVersionStr(),
                    IsTextSelectionEnabled = true,
                    TextWrapping = TextWrapping.Wrap
                };
                await new ContentDialog() { Content = tx, PrimaryButtonText = "知道了" }.ShowAsync();

                SettingHelper.First = false;
            }

            account = new Account();
            //检查登录状态
            if (!string.IsNullOrEmpty(SettingHelper.AccessKey))
            {
                if ((await account.CheckLoginState(ApiHelper.AccessKey)).success)
                {

                    MessageCenter_Logined();
                    await account.SSO(ApiHelper.AccessKey);
                }
                else
                {
                    var data = await account.RefreshToken(SettingHelper.AccessKey, SettingHelper.RefreshToken);
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
            MessageCenter.MainNavigateToEvent -= MessageCenter_MainNavigateToEvent;
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
                    img_bg.HorizontalAlignment = (HorizontalAlignment)SettingHelper.BGHor;
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

        private void MessageCenter_MainNavigateToEvent(Type page, params object[] par)
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

            MessageCenter.ChangeTheme(this);
            ChangeTitlebarColor();
        }
        private void ChangeTitlebarColor()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = ((SolidColorBrush)grid_Top.Background).Color;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = ((SolidColorBrush)sp_View.Background).Color;
            titleBar.ButtonBackgroundColor = ((SolidColorBrush)grid_Top.Background).Color;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.InactiveBackgroundColor = ((SolidColorBrush)grid_Top.Background).Color;
            titleBar.ButtonInactiveBackgroundColor = ((SolidColorBrush)grid_Top.Background).Color;
        }

        private async void Timer_Tick(object sender, object e)
        {
            var hasMessage = await HasMessage();
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                bor_TZ.Visibility = hasMessage ? Visibility.Visible : Visibility.Collapsed;
            });
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
                string url = string.Format("http://message.bilibili.com/api/notify/query.notify.count.do?_device=wp&_ulv=10000&access_key={0}&actionKey=appkey&appkey={1}&build=5250000&platform=android&ts={2}", ApiHelper.AccessKey, ApiHelper.AndroidKey.Appkey, ApiHelper.TimeStamp);
                url += "&sign=" + ApiHelper.GetSign(url);
                string results = await WebClientClass.GetResults(new Uri(url));
                MessageModel model = JsonConvert.DeserializeObject<MessageModel>(results);

                if (model.code == 0)
                {
                    MessageModel list = JsonConvert.DeserializeObject<MessageModel>(model.data.ToString());
                    return list.reply_me != 0 || list.chat_me != 0 || list.notify_me != 0 || list.praise_me != 0 || list.at_me != 0;
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

        private static readonly Dictionary<string, Type> navigateList = new Dictionary<string, Type>
        {
            ["NewFeed"] = typeof(NewFeedPage),
            ["Home"] = typeof(HomePage),
            ["Live"] = typeof(LiveV2Page),
            ["Bangumi"] = typeof(BangumiPage),
            ["Attention"] = typeof(AttentionPage),
            ["Find"] = typeof(FindPage),
            ["ToView"] = typeof(ToViewPage)
        };

        private async void NavigateTagList(string tag)
        {
            if (tag == "ToView")
            {
                if (!ApiHelper.IsLogin() && !await Utils.ShowLoginDialog())
                {
                    Utils.ShowMessageToast("请先登录");
                    return;
                }
            }
            if (tag == "Setting")
            {
                frame.Navigate(typeof(SettingPage));
            }
            else if (navigateList.TryGetValue(tag, out Type type))
            {
                main_frame.Navigate(type);
            }
        }

        private void sp_View_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigateTagList("Setting");
            }
            else
            {
                NavigateTagList(args.InvokedItemContainer.Tag.ToString());
            }
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

            frame.Navigate(typeof(MyQrPage), new object[] {
                new MyqrModel() {
                    name = Account.myInfo.name,
                    photo = Account.myInfo.face,
                    qr = $"http://qr.liantu.com/api.php?w=500&text={Uri.EscapeDataString("http://space.bilibili.com/" + ApiHelper.GetUserId())}&inpt=00AAF0&logo={Uri.EscapeDataString(Account.myInfo.face)}",
                    sex = Account.myInfo.Sex
                }
            });
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

        //private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    var item = (e.ClickedItem as StackPanel).Tag.ToString();
        //    if (item == "ToView")
        //    {
        //        if (!ApiHelper.IsLogin() && !await Utils.ShowLoginDialog())
        //        {
        //            Utils.ShowMessageToast("请先登录");
        //            return;
        //        }
        //        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(ToViewPage));
        //    }
        //    else if (item == "Test")
        //    {
        //        try
        //        {
        //            var url = new Uri($"https://www.biliplus.com/login?act=savekey&mid={UserManage.Uid}&access_key={ApiHelper.AccessKey}&expire=");
        //            using (HttpClient httpClient = new HttpClient())
        //            {
        //                var rq = await httpClient.GetAsync(url);
        //                var setCookie = rq.Headers["set-cookie"];
        //                StringBuilder stringBuilder = new StringBuilder();
        //                var matches = Regex.Matches(setCookie, "(.*?)=(.*?); ", RegexOptions.Singleline);
        //                foreach (Match match in matches)
        //                {
        //                    var key = match.Groups[1].Value.Replace("HttpOnly, ", "");
        //                    var value = match.Groups[2].Value;
        //                    if (key != "expires" && key != "Max-Age" && key != "path" && key != "domain")
        //                    {
        //                        stringBuilder.Append(match.Groups[0].Value.Replace("HttpOnly, ", string.Empty));
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Authentication failed. Handle parameter, SSL/TLS, and Network Unavailable errors here. 
        //            Debug.WriteLine(ex);
        //            throw;
        //        }
        //    }
        //    else
        //    {
        //        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(SettingPage));
        //    }
        //}

        private void btn_user_moviecollect_Click(object sender, RoutedEventArgs e)
        {
            MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(FollowSeasonPage), Modules.SeasonType.cinema);
            fy.Hide();
        }

        private void btn_ClearMedia_Click(object sender, RoutedEventArgs e)
        {
            MusicHelper.ClearMediaList();
        }

        private bool isSetMusic = false;
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
