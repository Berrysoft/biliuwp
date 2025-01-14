﻿using BiliBili3.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Media.Imaging;
using BiliBili3.Helper;
using System.Text.RegularExpressions;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace BiliBili3.Controls
{
    public sealed partial class LoginDialog : ContentDialog
    {
        Account account;
        public LoginDialog()
        {
            this.InitializeComponent();
            account = new Account();
            _biliapp.CloseBrowserEvent += _biliapp_CloseBrowserEvent;
            _biliapp.ValidateLoginEvent += _biliapp_ValidateLoginEvent;
            _secure.CloseCaptchaEvent += _biliapp_CloseCaptchaEvent;
            _secure.CaptchaEvent += _biliapp_CaptchaEvent;
        }

        private void _biliapp_CaptchaEvent(object sender, string e)
        {
            //throw new NotImplementedException();
        }

        private void _biliapp_CloseCaptchaEvent(object sender, string e)
        {
            //throw new NotImplementedException();
        }

        BiliBiliJS.Biliapp _biliapp = new BiliBiliJS.Biliapp();
        BiliBiliJS.Secure _secure = new BiliBiliJS.Secure();
        private void _biliapp_CloseBrowserEvent(object sender, string e)
        {
            UserManage.Logout();
            this.Hide();
        }
       
        private async void _biliapp_ValidateLoginEvent(object sender, string e)
        {
            try
            {
                JObject jObject = JObject.Parse(e);
                if (jObject["access_token"] != null)
                {
                    var m = await account.CheckAgainLogin(jObject["access_token"].ToString(), jObject["refresh_token"].ToString(), jObject["expires_in"].ToInt32(), Convert.ToInt64(jObject["mid"]));
                    if (m.success)
                    {
                        this.Hide();
                        if (cb_AuthBP.IsChecked.Value)
                        {
                            var auth=await Account.AuthBiliPlus();
                            if (auth=="")
                            {
                                Utils.ShowMessageToast("登录成功,但BiliPlus授权失败");
                            }
                            else
                            {
                                Utils.ShowMessageToast("登录成功");
                            }
                        }
                    }
                    else
                    {
                        Title = "登录";
                        IsPrimaryButtonEnabled = true;
                        webView.Visibility = Visibility.Collapsed;
                        Utils.ShowMessageToast("登录失败,请重试");
                    }
                    //await UserManage.LoginSucess(jObject["access_token"].ToString());
                }
                else
                {
                    Title = "登录";
                    IsPrimaryButtonEnabled = true;
                    webView.Visibility = Visibility.Collapsed;
                    Utils.ShowMessageToast("登录失败,请重试");
                }

            }
            catch (Exception)
            {
            }

        }
        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            if (txt_Username.Text.Length == 0)
            {
                txt_Username.Focus(FocusState.Pointer);
                Utils.ShowMessageToast("请输入用户名");
                return;
            }
            if (txt_Password.Password.Length == 0)
            {
                txt_Password.Focus(FocusState.Pointer);
                Utils.ShowMessageToast("请输入密码");
                return;
            }
            if (chatcha.Visibility == Visibility.Visible && txt_captcha.Text.Length == 0)
            {
                txt_Password.Focus(FocusState.Pointer);
                Utils.ShowMessageToast("请输入验证码");
                return;
            }
            IsPrimaryButtonEnabled = false;
            //var results = await account.LoginV3(txt_Username.Text, txt_Password.Password);
            var results = await account.LoginV2(txt_Username.Text, txt_Password.Password,txt_captcha.Text);
            switch (results.status)
            {
                case Modules.LoginStatus.Success:
                    this.Hide();
                    break;
                case Modules.LoginStatus.Fail:
                case Modules.LoginStatus.Error:
                    IsPrimaryButtonEnabled = true;
                    break;
                case Modules.LoginStatus.NeedCaptcha:
                    //V2
                    chatcha.Visibility = Visibility.Visible;
                    IsPrimaryButtonEnabled = true;
                    GetCaptcha();
                    //V3
                    //webView.Visibility = Visibility.Visible;
                    //var httpRequestMessage = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Get, new Uri(results.url));
                    //var userAgent = "BiliMangaUwp/0.3.0.0 Windows BiliApp BiliComic/0.3.0.0";
                    //httpRequestMessage.Headers.Add("User-Agent", userAgent);
                    //httpRequestMessage.Headers.Add("Upgrade-Insecure-Requests", "1");
                    //webView.NavigateWithHttpRequestMessage(httpRequestMessage);
                    //webView.Source = new Uri(results.url);
                    break;
                case Modules.LoginStatus.NeedValidate:
                    Title = "安全验证";
                    webView.Visibility = Visibility.Visible;
                    webView.Source = new Uri(results.url.Replace("&ticket=1",""));
                    break;
                default:
                    break;
            }
            Utils.ShowMessageToast(results.message);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void txt_Password_GotFocus(object sender, RoutedEventArgs e)
        {
            hide.Visibility = Visibility.Visible;
        }
        private void txt_Password_LostFocus(object sender, RoutedEventArgs e)
        {
            hide.Visibility = Visibility.Collapsed;
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GetCaptcha();
        }
        private async void GetCaptcha()
        {
            try
            {
                var m = await WebClientClass.GetBuffer(new Uri("https://passport.bilibili.com/captcha?ts=" + ApiHelper.TimeStamp));
                var steam = m.AsStream();
                var img = new BitmapImage();
                await img.SetSourceAsync(steam.AsRandomAccessStream());
                img_Captcha.Source = img;
            }
            catch (Exception)
            {
                Utils.ShowMessageToast("无法加载验证码");
            }


        }

        private async void webView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri.AbsoluteUri.Contains("access_key="))
            {
                var access = Regex.Match(args.Uri.AbsoluteUri, "access_key=(.*?)&").Groups[1].Value;
                var mid = Regex.Match(args.Uri.AbsoluteUri, "mid=(.*?)&").Groups[1].Value;
                await account.SetLoginSuccess(access, mid);
                this.Hide();
                return;
            }
            try
            {
                this.webView.AddWebAllowedObject("biliapp", _biliapp);
                this.webView.AddWebAllowedObject("secure", _secure);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex);
            }
          
        }

        private void webView_ScriptNotify(object sender, NotifyEventArgs e)
        {

        }

        private void BtnWebLogin_Click(object sender, RoutedEventArgs e)
        {
            Title = "网页登录";
            webView.Visibility = Visibility.Visible;
            webView.Source = new Uri("https://passport.bilibili.com/ajax/miniLogin/minilogin");
            IsPrimaryButtonEnabled = false;
            webView.Width = 440;
            webView.Height = 480;
            //
        }

        private async void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.Uri.AbsoluteUri== "https://passport.bilibili.com/ajax/miniLogin/redirect")
            {
                var results= await WebClientClass.GetResults(new Uri("https://passport.bilibili.com/login/app/third?appkey=27eb53fc9058f8c3&api=http%3A%2F%2Flink.acg.tv%2Fforum.php&sign=67ec798004373253d60114caaad89a8c"));
                var obj = JObject.Parse(results);
                if (obj["code"].ToInt32()==0)
                {
                    webView.Navigate(new Uri(obj["data"]["confirm_uri"].ToString()));
                }
                else
                {
                    Utils.ShowMessageToast("登录失败，请重试");
                }
                return;
            }
            


        }
    }
}
