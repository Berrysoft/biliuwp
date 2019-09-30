using BiliBili3.Helper;
using BiliBili3.Models;
using BiliBili3.Modules;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BiliBili3.Pages
{
    public sealed partial class WebPage : Page
    {
        public WebPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        BiliBiliJS.Biliapp _biliapp = new BiliBiliJS.Biliapp();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {

                _biliapp.CloseBrowserEvent += _biliapp_CloseBrowserEvent;
                _biliapp.ValidateLoginEvent += _biliapp_ValidateLoginEvent;

                webView.Navigate(new Uri((e.Parameter as object[])[0].ToString()));
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                webView.NavigateToString(string.Empty);
                this.NavigationCacheMode = NavigationCacheMode.Disabled;
            }
            base.OnNavigatedFrom(e);
        }
        private void _biliapp_CloseBrowserEvent(object sender, string e)
        {
            this.Frame.GoBack();
        }

        private async void _biliapp_ValidateLoginEvent(object sender, string e)
        {
            try
            {
                JObject jObject = JObject.Parse(e);
                if (jObject["access_token"] != null)
                {
                    Account account = new Account();
                    var m = await account.CheckAgainLogin(jObject["access_token"].ToString(), jObject["refresh_token"].ToString(), jObject["expires_in"].ToInt32(), Convert.ToInt64(jObject["mid"]));
                    if (m.success)
                    {
                        Utils.ShowMessageToast("登录成功");
                    }
                    else
                    {
                        Utils.ShowMessageToast("登录失败");
                    }
                }
                else
                {
                    Utils.ShowMessageToast("登录失败");
                }
                this.Frame.GoBack();
            }
            catch (Exception)
            {
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private async void webview_WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri == null)
            {
                return;
            }
            if (await MessageCenter.HandelUrl(args.Uri.AbsoluteUri))
            {
                args.Cancel = true;
            }
            try
            {
                this.webView.AddWebAllowedObject("biliapp", _biliapp);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
            }
        }

        private void webview_WebView_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            webview_progressBar.Visibility = Visibility.Collapsed;
        }


        private async void webview_WebView_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            args.Handled = true;
            var re = await MessageCenter.HandelUrl(args.Uri.AbsoluteUri);
            if (!re)
            {
                var md = new MessageDialog("是否调用外部浏览器打开此链接？");
                md.Commands.Add(new UICommand("确定", new UICommandInvokedHandler(async (e) => { await Windows.System.Launcher.LaunchUriAsync(args.Uri); })));
                md.Commands.Add(new UICommand("取消", new UICommandInvokedHandler((e) => { })));
                await md.ShowAsync();
            }
        }

        private void menu_copy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage pack = new DataPackage();
            pack.SetText(webView.Source.AbsoluteUri);
            Clipboard.SetContent(pack); // 保存 DataPackage 对象到剪切板
            Clipboard.Flush();
        }

        private async void menu_open_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(webView.Source);
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            webView.Refresh();
        }

        private async void web_UnsupportedUriSchemeIdentified(WebView sender, WebViewUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            if (args.Uri.AbsoluteUri.Contains("bilibili://"))
            {
                args.Handled = true;

                var re = await MessageCenter.HandelUrl(args.Uri.AbsoluteUri);
                if (!re)
                {
                    Utils.ShowMessageToast("不支持打开的链接" + args.Uri.AbsoluteUri);
                }

            }
        }

        private void btn_WebBack_Click(object sender, RoutedEventArgs e)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }

        }

        private void btn_WebRefresh_Click(object sender, RoutedEventArgs e)
        {
            webView.Refresh();
        }

        private async void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            try
            {
                if (args.Uri.AbsoluteUri.Contains("23344273.aspx"))
                {
                    string appVer = SettingHelper.GetVersion();

                    string systemVer = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily + " " + SystemHelper.SystemVersion();
                    string js = $"document.getElementById('q2').value='{appVer}';document.getElementById('q3').value='{systemVer}';";
                    await webView.InvokeScriptAsync("eval", new string[] { js });
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
