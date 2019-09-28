using BiliBili3.Helper;
using BiliBili3.Modules;
using BiliBili3.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace BiliBili3.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.NavigationMode == NavigationMode.New)
            {
                GetSetting();
            }
        }
        bool get_ing = true;
        bool loadsetting = true;
        private async void GetSetting()
        {
            try
            {
                loadsetting = true;
                pr_Load.Visibility = Visibility.Visible;
                CustomBg.Visibility = Visibility.Visible;


                sw_NewWidnows.IsOn = SettingHelper.NewWindow;
                sw_UseDASH.IsOn = SettingHelper.UseDASH;

                btnOpenInstallHEVC.Visibility = Visibility.Visible;
                sw_DASHUseHEVC.IsOn = SettingHelper.DASHUseHEVC;
                sw_PriorityBiliPlus.IsOn = SettingHelper.PriorityBiliPlus;

                sw_LoadSe.IsOn = SettingHelper.LoadSplash;
                sw_ForceAudio.IsOn = SettingHelper.ForceAudio;
                sw_ForceVideo.IsOn = SettingHelper.ForceVideo;
                sw_DanmuBorder.IsOn = SettingHelper.DMBorder;
                sw_Use4GDown.IsOn = SettingHelper.Use4GDown;
                sw_RefreshButton.IsOn = SettingHelper.RefreshButton;

                sw_Play4G.IsOn = SettingHelper.Use4GPlay;
                sw_BackgroundPlay.IsOn = SettingHelper.BackPlay;
                sw_HideCursor.IsOn = SettingHelper.HideCursor;

                sw_MouseBack.IsOn = SettingHelper.MouseBack;
                sw_MergeDanmu.IsOn = SettingHelper.MergeDanmu;

                sw_NotSubtitle.IsOn = SettingHelper.DanmuNotSubtitle;
                sw_BoldDanmu.IsOn = SettingHelper.BoldDanmu;
                sw_StatusDanmu.IsOn = SettingHelper.DMStatus;

                sw_DTCT.IsOn = SettingHelper.DTCT;
                sw_DT.IsOn = SettingHelper.DT;
                sw_FJ.IsOn = SettingHelper.FJ;
                sw_CustomPath.IsOn = SettingHelper.CustomDownPath;
                sw_FFmpeg.IsOn = SettingHelper.FFmpeg;

                sw_NewFeed.IsOn = SettingHelper.NewFeed;

                tw_MNGA.IsOn = SettingHelper.UseHK;
                tw_MNTW.IsOn = SettingHelper.UseTW;
                tw_MNDL.IsOn = SettingHelper.UseCN;
                tw_PlayerMode.IsOn = SettingHelper.PlayerMode;
                tw_VipMode.IsOn = SettingHelper.UseVIP;
                tw_OtherSiteMode.IsOn = SettingHelper.UseOtherSite;

                sw_QZHP.IsOn = SettingHelper.QZHP;
                sw_AutoFull.IsOn = SettingHelper.AutoFull;

                slider_DanmuSize.Value = SettingHelper.NewDMSize;
                slider_Num.Value = SettingHelper.DMNumber;
                slider_DanmuTran.Value = SettingHelper.NewDMTran;
                slider_DanmuSpeed.Value = SettingHelper.DMSpeed;

                List<string> fonts = SystemHelper.GetSystemFontFamilies();
                cb_Font.ItemsSource = fonts;
                if (!string.IsNullOrEmpty(SettingHelper.DanmuFont))
                {
                    cb_Font.SelectedIndex = fonts.IndexOf(SettingHelper.DanmuFont);
                }
                else
                {
                    cb_Font.SelectedIndex = fonts.IndexOf(cb_Font.FontFamily.Source);
                }

                var c = SettingHelper.BiliplusCookie;
                if (!string.IsNullOrEmpty(c))
                {
                    txtBPState.Text = "(已授权)";
                }
                else
                {
                    txtBPState.Text = "";
                }

                cb_PlayQuality.SelectedIndex = SettingHelper.PlayQualit - 1;
                cb_DownQuality.SelectedIndex = SettingHelper.DownQualit - 1;
                cb_VideoType.SelectedIndex = SettingHelper.VideoType;
                cb_DanmuStyle.SelectedIndex = SettingHelper.DMStyle;

                cb_DownMode.SelectedIndex = SettingHelper.DownMode;

                sw_Playback.SelectedIndex = SettingHelper.Playback;


                sw_CustomBg.IsOn = SettingHelper.CustomBG;

                txt_BGPath.Text = SettingHelper.BGPath;
                txt_CustomDownPath.Text = SettingHelper.DownPath;


                cb_BGStretch.SelectedIndex = SettingHelper.BGStretch;
                cb_Ver.SelectedIndex = SettingHelper.BGVer;
                cbHor.SelectedIndex = SettingHelper.BGHor;
                cb_BGOpacity.SelectedIndex = SettingHelper.BGOpacity - 1;
                cb_FrostedGlass.SelectedIndex = SettingHelper.FrostedGlass;
                cb_ClaerLiveComment.SelectedIndex = SettingHelper.ClearLiveComment;
                sw_H5.IsOn = SettingHelper.UseH5;

                txt_BGMaxHeight.Text = SettingHelper.BGMaxHeight.ToString();
                txt_BGMaxWidth.Text = SettingHelper.BGMaxWidth.ToString();

                cb_BanPlayer.SelectedIndex = SettingHelper.BanPlayer;
                if (sw_CustomBg.IsOn)
                {
                    grid_CustomBg.Visibility = Visibility.Visible;
                    grid_BgHeigth.Visibility = Visibility.Visible;
                    grid_BgWdith.Visibility = Visibility.Visible;
                    grid_Hor.Visibility = Visibility.Visible;
                    grid_Stretch.Visibility = Visibility.Visible;
                    grid_Ver.Visibility = Visibility.Visible;
                    grid_Opacity.Visibility = Visibility.Visible;
                }
                else
                {
                    grid_CustomBg.Visibility = Visibility.Collapsed;
                    grid_BgHeigth.Visibility = Visibility.Collapsed;
                    grid_BgWdith.Visibility = Visibility.Collapsed;
                    grid_Hor.Visibility = Visibility.Collapsed;
                    grid_Stretch.Visibility = Visibility.Collapsed;
                    grid_Ver.Visibility = Visibility.Collapsed;
                    grid_Opacity.Visibility = Visibility.Collapsed;
                }


                txt_version.Text = "Ver " + SettingHelper.GetVersion();




                get_ing = true;
                switch (SettingHelper.Theme)
                {
                    case "Red":
                        cb_Theme.SelectedIndex = 2;
                        break;
                    case "Blue":
                        cb_Theme.SelectedIndex = 5;
                        break;
                    case "Green":
                        cb_Theme.SelectedIndex = 4;
                        break;
                    case "Pink":
                        cb_Theme.SelectedIndex = 0;
                        break;
                    case "Purple":
                        cb_Theme.SelectedIndex = 6;
                        break;
                    case "Yellow":
                        cb_Theme.SelectedIndex = 3;
                        break;
                    case "EMT":
                        cb_Theme.SelectedIndex = 7;
                        break;
                    case "Dark":
                        cb_Theme.SelectedIndex = 1;
                        break;
                    default:
                        break;
                }
                get_ing = false;

                cb_Rigth.SelectedIndex = SettingHelper.Rigth;


                if (SettingHelper.BGPath.Length != 0)
                {
                    StorageFile file = await StorageFile.GetFileFromPathAsync(SettingHelper.BGPath);
                    if (file == null)
                    {
                        txt_BGPath.Text = "没有背景啊，右侧自定义-->";
                    }
                    else

                    {
                        txt_BGPath.Text = file.DisplayName;
                    }

                }

                txt_Ver.Text = AppHelper.verStr.Replace("/", "");
                pr_Load.Visibility = Visibility.Collapsed;




                loadsetting = false;
            }
            catch (Exception)
            {
                Utils.ShowMessageToast("设置读取失败");
            }


        }

        private async void cb_Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cb_Theme.SelectedItem != null && !get_ing)
            {
                switch (cb_Theme.SelectedIndex)
                {
                    case 0:
                        SettingHelper.Theme = "Pink";
                        break;
                    case 1:
                        SettingHelper.Theme = "Dark";
                        break;
                    case 2:
                        SettingHelper.Theme = "Red";
                        break;
                    case 3:
                        SettingHelper.Theme = "Yellow";
                        break;
                    case 4:
                        SettingHelper.Theme = "Green";
                        break;
                    case 5:
                        SettingHelper.Theme = "Blue";
                        break;
                    case 6:
                        SettingHelper.Theme = "Purple";
                        break;
                    case 7:
                        SettingHelper.Theme = "EMT";
                        break;
                    default:
                        break;
                }
                MessageCenter.SendChanageThemeEvent(null);
                MessageDialog messageDialog = new MessageDialog("重启应用一下，效果更好，是否立即重启应用?", "是否重启应用");
                messageDialog.Commands.Add(new UICommand("确定", async (x) =>
                {
                    var result = await CoreApplication.RequestRestartAsync(string.Empty);
                    if (result == AppRestartFailureReason.NotInForeground || result == AppRestartFailureReason.Other)
                    {
                        Utils.ShowMessageToast("请收到重启应用");
                    }
                }));
                messageDialog.Commands.Add(new UICommand("取消", (x) => { }));
                await messageDialog.ShowAsync();
            }
            MessageCenter.SendChanageThemeEvent(null);



            //await CoreApplication.RequestRestartAsync(string.Empty);
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void cb_Rigth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.Rigth = cb_Rigth.SelectedIndex;

        }
        private void sw_LoadSe_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.LoadSplash = sw_LoadSe.IsOn;
        }

        private void cb_PlayQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.PlayQualit = cb_PlayQuality.SelectedIndex + 1;
        }

        private void cb_VideoType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.VideoType = cb_VideoType.SelectedIndex;
        }

        private void sw_ForceVideo_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.ForceVideo = sw_ForceVideo.IsOn;
        }

        private void sw_ForceAudio_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.ForceAudio = sw_ForceAudio.IsOn;
        }

        private void sw_Playback_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.Playback = sw_Playback.SelectedIndex;
        }

        private void sw_DanmuBorder_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.DMBorder = sw_DanmuBorder.IsOn;
        }

        private void slider_DanmuSize_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!loadsetting)
            {
                SettingHelper.NewDMSize = slider_DanmuSize.Value;
            }
        }

        private void cb_Font_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loadsetting)
            {
                return;
            }
            SettingHelper.DanmuFont = cb_Font.SelectedItem.ToString();
        }

        private void slider_DanmuSpeed_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

            SettingHelper.DMSpeed = slider_DanmuSpeed.Value;
        }

        private void slider_DanmuTran_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!loadsetting)
            {
                SettingHelper.NewDMTran = slider_DanmuTran.Value;
            }

        }

        private void slider_Num_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SettingHelper.DMNumber = Convert.ToInt32(slider_Num.Value);
        }

        private void cb_DownQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.DownQualit = cb_DownQuality.SelectedIndex + 1;
        }

        private void cb_DownMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.DownMode = cb_DownMode.SelectedIndex;
        }

        private void sw_Use4GDown_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.Use4GDown = sw_Use4GDown.IsOn;
            DownloadHelper2.UpdateDowningStatus();
        }

        private void sw_DTCT_Toggled(object sender, RoutedEventArgs e)
        {
            if (!sw_DTCT.IsOn)
            {
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.Clear();
            }
            SettingHelper.DTCT = sw_DTCT.IsOn;

        }

        private void sw_DT_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.DT = sw_DT.IsOn;
        }

        private void sw_FJ_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.FJ = sw_FJ.IsOn;
        }

        private void tw_MNGA_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.UseHK = tw_MNGA.IsOn;
            if (tw_MNGA.IsOn)
            {
                tw_MNTW.IsOn = false;
                tw_MNDL.IsOn = false;
            }
        }

        private void tw_MNTW_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.UseTW = tw_MNTW.IsOn;
            if (tw_MNTW.IsOn)
            {
                tw_MNGA.IsOn = false;
                tw_MNDL.IsOn = false;
            }
        }

        private void tw_MNDL_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.UseCN = tw_MNDL.IsOn;
            if (tw_MNDL.IsOn)
            {
                tw_MNGA.IsOn = false;
                tw_MNTW.IsOn = false;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.DataPackage pack = new Windows.ApplicationModel.DataTransfer.DataPackage();
            pack.SetText((sender as HyperlinkButton).Tag.ToString());
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(pack); // 保存 DataPackage 对象到剪切板
            Windows.ApplicationModel.DataTransfer.Clipboard.Flush();
            Utils.ShowMessageToast("已将内容复制到剪切板", 3000);
        }

        private async void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            var x = new ContentDialog();
            StackPanel st = new StackPanel();
            st.Children.Add(new Image()
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/zfb.jpg"))
            });
            st.Children.Add(new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                IsTextSelectionEnabled = true,
                Text = "\r\n如果觉得应用不错，给我点打赏我会很感谢的!\r\n支付宝：2500655055@qq.com,**程\r\n"
            });
            x.Content = st;
            x.PrimaryButtonText = "知道了";
            x.IsPrimaryButtonEnabled = true;
            await x.ShowAsync();
        }

        private void btn_GoManage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DMHideManagePage));
        }

        private void sw_RefreshButton_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.RefreshButton = sw_RefreshButton.IsOn;
        }

        private void sw_CustomBg_Toggled(object sender, RoutedEventArgs e)
        {
            if (sw_CustomBg.IsOn)
            {
                grid_CustomBg.Visibility = Visibility.Visible;
                grid_BgHeigth.Visibility = Visibility.Visible;
                grid_BgWdith.Visibility = Visibility.Visible;
                grid_Hor.Visibility = Visibility.Visible;
                grid_Stretch.Visibility = Visibility.Visible;
                grid_Ver.Visibility = Visibility.Visible;
                grid_Opacity.Visibility = Visibility.Visible;
            }
            else
            {
                grid_CustomBg.Visibility = Visibility.Collapsed;
                grid_BgHeigth.Visibility = Visibility.Collapsed;
                grid_BgWdith.Visibility = Visibility.Collapsed;
                grid_Hor.Visibility = Visibility.Collapsed;
                grid_Stretch.Visibility = Visibility.Collapsed;
                grid_Ver.Visibility = Visibility.Collapsed;
                grid_Opacity.Visibility = Visibility.Collapsed;
            }
            SettingHelper.CustomBG = sw_CustomBg.IsOn;
            MessageCenter.SendChangedBg();
        }

        private async void btn_ChanageBg_Click(object sender, RoutedEventArgs e)
        {

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.CommitButtonText = "选中此文件";
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".gif");
            openPicker.FileTypeFilter.Add(".bmp");
            // 弹出文件选择窗口
            StorageFile file = await openPicker.PickSingleFileAsync(); // 用户在“文件选择窗口”中完成操作后，会返回对应的 StorageFile 对象
            if (file != null)
            {
                StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile ex = null;
                try
                {
                    ex = await localFolder.GetFileAsync(file.Name);
                }
                catch (Exception)
                {
                }
                if (ex == null)
                {
                    var cp = await file.CopyAsync(localFolder);
                }
                SettingHelper.BGPath = ex.Path;

                txt_BGPath.Text = file.Name;


                MessageCenter.SendChangedBg();



            }

        }

        private void cb_BGStretch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.BGStretch = cb_BGStretch.SelectedIndex;
            MessageCenter.SendChangedBg();
        }

        private void cbHor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.BGHor = cbHor.SelectedIndex;
            MessageCenter.SendChangedBg();
        }

        private void cb_Ver_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            SettingHelper.BGVer = cb_Ver.SelectedIndex;
            MessageCenter.SendChangedBg();
        }

        private void cb_BGOpacity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.BGOpacity = cb_BGOpacity.SelectedIndex + 1;
            MessageCenter.SendChangedBg();
        }

        private void txt_BGMaxWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            int w = 0;
            if (!int.TryParse(txt_BGMaxWidth.Text, out w) || w < 0)
            {
                txt_BGMaxWidth.Text = "0";
                Utils.ShowMessageToast("请输入正整数！", 2000);
                return;
            }

            SettingHelper.BGMaxWidth = w;
            MessageCenter.SendChangedBg();

        }

        private void txt_BGMaxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(txt_BGMaxHeight.Text, out int h) || h < 0)
            {
                txt_BGMaxHeight.Text = "0";
                Utils.ShowMessageToast("请输入正整数！", 2000);
                return;
            }

            SettingHelper.BGMaxHeight = h;

            MessageCenter.SendChangedBg();


        }

        private async void sw_CustomPath_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.CustomDownPath = sw_CustomPath.IsOn;
            await DownloadHelper.GetfolderList();
            await DownloadHelper.SetfolderList();
        }

        private async void btn_CustomDoanPath_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker fp = new FolderPicker();
            fp.FileTypeFilter.Add(".mp4");
            var f = await fp.PickSingleFolderAsync();
            if (f == null)
            {
                return;
            }
            string mruToken = StorageApplicationPermissions.MostRecentlyUsedList.Add(f, f.Path);
            SettingHelper.DownPath = f.Path;
            txt_CustomDownPath.Text = f.Path;
            await DownloadHelper.GetfolderList();
            await DownloadHelper.SetfolderList();
            //读取文件夹
            // string mruFirstToken = StorageApplicationPermissions.MostRecentlyUsedList.Entries.First(x=>x.Metadata==f.Path).Token;
            //StorageFolder retrievedFile = await StorageApplicationPermissions.MostRecentlyUsedList.GetFolderAsync(mruFirstToken);

            //Utils.ShowMessageToast(mruToken, 3000);

        }

        private void cb_FrostedGlass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.FrostedGlass = cb_FrostedGlass.SelectedIndex;
            MessageCenter.SendChangedBg();
        }

        private void tw_PlayerMode_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.PlayerMode = tw_PlayerMode.IsOn;
        }

        private void sw_FFmpeg_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.FFmpeg = sw_FFmpeg.IsOn;
        }

        private void sw_H5_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.UseH5 = sw_H5.IsOn;
        }

        private void cb_ClaerLiveComment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.ClearLiveComment = cb_ClaerLiveComment.SelectedIndex;
        }

        private void sw_BackgroundPlay_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.BackPlay = sw_BackgroundPlay.IsOn;
        }

        private void sw_Play4G_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.Use4GPlay = sw_Play4G.IsOn;
        }

        private void sw_QZHP_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.QZHP = sw_QZHP.IsOn;
        }

        private void sw_AutoFull_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.AutoFull = sw_AutoFull.IsOn;
        }

        private void sw_HideCursor_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.HideCursor = sw_HideCursor.IsOn;
        }

        private void sw_NewFeed_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.NewFeed = sw_NewFeed.IsOn;
        }

        private void sw_NewWidnows_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.NewWindow = sw_NewWidnows.IsOn;
        }

        private void cb_BanPlayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingHelper.BanPlayer = cb_BanPlayer.SelectedIndex;
        }

        private async void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            await WebView.ClearTemporaryWebDataAsync();
            await new MessageDialog("清除完成,为了能正常使用，请重新登录").ShowAsync();
        }

        private void sw_toMp4_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.ToMp4 = sw_toMp4.IsOn;
        }

        private void btn_Help_Click(object sender, RoutedEventArgs e)
        {
            MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(WebPage), "https://www.showdoc.cc/biliuwp");
        }

        private void sw_MouseBack_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.MouseBack = sw_MouseBack.IsOn;
        }

        private void sw_MergeDanmu_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.MergeDanmu = sw_MergeDanmu.IsOn;
        }

        private void cb_DanmuStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cb_DanmuStyle.SelectedIndex)
            {
                case 0:
                    danmustyle_border.Visibility = Visibility.Visible;
                    danmustyle_noborder.Visibility = Visibility.Collapsed;
                    danmustyle_shadow.Visibility = Visibility.Collapsed;
                    danmustyle_borderV2.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    danmustyle_border.Visibility = Visibility.Collapsed;
                    danmustyle_noborder.Visibility = Visibility.Visible;
                    danmustyle_shadow.Visibility = Visibility.Collapsed;
                    danmustyle_borderV2.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    danmustyle_border.Visibility = Visibility.Collapsed;
                    danmustyle_noborder.Visibility = Visibility.Collapsed;
                    danmustyle_shadow.Visibility = Visibility.Visible;
                    danmustyle_borderV2.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    danmustyle_border.Visibility = Visibility.Collapsed;
                    danmustyle_noborder.Visibility = Visibility.Collapsed;
                    danmustyle_shadow.Visibility = Visibility.Collapsed;
                    danmustyle_borderV2.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
            SettingHelper.DMStyle = cb_DanmuStyle.SelectedIndex;
        }

        private void sw_NotSubtitle_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.DanmuNotSubtitle = sw_NotSubtitle.IsOn;
        }

        private void btn_Feedback_Click(object sender, RoutedEventArgs e)
        {
            MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(WebPage), "https://www.wjx.top/jq/23344273.aspx");
        }

        private void sw_StatusDanmu_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.DMStatus = sw_StatusDanmu.IsOn;
        }

        private void sw_BoldDanmu_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.BoldDanmu = sw_BoldDanmu.IsOn;
        }

        private void tw_VipMode_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.UseVIP = tw_VipMode.IsOn;
        }

        private void tw_OtherSiteMode_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.UseOtherSite = tw_OtherSiteMode.IsOn;
        }

        private async void BrnAuthBiliPlus_Click(object sender, RoutedEventArgs e)
        {
            if (!ApiHelper.IsLogin() && !await Utils.ShowLoginDialog())
            {
                Utils.ShowMessageToast("请登录后再执行此操作");
                return;
            }
            var re = await Account.AuthBiliPlus();
            if (!string.IsNullOrEmpty(re))
            {
                txtBPState.Text = "(已授权)";
            }
            else
            {
                Utils.ShowMessageToast("授权失败了");
            }
        }

        private void Sw_UseDASH_Toggled(object sender, RoutedEventArgs e)
        {

            if (sw_UseDASH.IsOn && SystemHelper.GetSystemBuild() < 17763)
            {
                Utils.ShowMessageToast("系统版本1809以上才可以开启");
                sw_UseDASH.IsOn = false;
                return;
            }
            SettingHelper.UseDASH = sw_UseDASH.IsOn;
        }

        private void Sw_DASHUseHEVC_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.DASHUseHEVC = sw_DASHUseHEVC.IsOn;
        }

        private void Sw_PriorityBiliPlus_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.PriorityBiliPlus = sw_PriorityBiliPlus.IsOn;
        }
    }
}
