﻿using BiliBili3.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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

namespace BiliBili3.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FastNavigatePage : Page
    {
        public FastNavigatePage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        private void autoSug_Box_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            txt_auto_Find.Text = args.SelectedItem as string;
        }

        public async Task<ObservableCollection<string>> GetSugges(string text)
        {
            try
            {
                string results = await WebClientClass.GetResults(new Uri("http://s.search.bilibili.com/main/suggest?suggest_type=accurate&sub_type=tag&main_ver=v1&term=" + text));
                JObject json = JObject.Parse(results);
                if (json["result"].Type == JTokenType.Object)
                {
                    List<SuggesModel> list = JsonConvert.DeserializeObject<List<SuggesModel>>(json["result"]["tag"].ToString());
                    return new ObservableCollection<string>(list.Select(item => item.value));
                }
                else
                {
                    return new ObservableCollection<string>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new ObservableCollection<string>();
            }

        }
        public class SuggesModel
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        private async void autoSug_Box_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (sender.Text.Length != 0)
            {
                sender.ItemsSource = await GetSugges(sender.Text);
            }
            else
            {
                sender.ItemsSource = null;
            }
        }

        private async void autoSug_Box_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (await MessageCenter.HandelUrl(txt_auto_Find.Text))
            {
                return;
            }
            MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(SearchPage), new object[] { txt_auto_Find.Text });

        }

        private void GridView_ItemClick(object sender, RoutedEventArgs e)
        {
            if (!ApiHelper.IsLogin())
            {
                Utils.ShowMessageToast("请先登录", 3000);
                return;
            }
            if (e.OriginalSource is FrameworkElement info)
            {
                switch (info.Tag.ToString())
                {
                    case "yh":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(UserInfoPage));
                        break;
                    case "dt":
                        MessageCenter.SendNavigateTo(NavigateMode.Home, typeof(AttentionPage));
                        break;
                    case "sc":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(MyCollectPage));
                        break;
                    case "ls":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(MyHistroryPage));
                        break;
                    case "fs":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(TimelinePage));
                        break;
                    case "sy":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(BanTagPage));
                        break;
                    case "rank":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(RankPage));
                        break;
                    case "part":
                        MessageCenter.SendNavigateTo(NavigateMode.Home, typeof(HomePage));
                        break;
                    case "tj":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(LiveAllPage));
                        //MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(TimelinePage));
                        break;
                    case "live":
                        // Utils.ShowMessageToast("开发中...", 3000);
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(LivePartPage));
                        break;
                    case "mini":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(LiveVideoPage));
                        break;
                    case "alive":
                        if (!ApiHelper.IsLogin())
                        {
                            Utils.ShowMessageToast("请先登录", 3000);
                            return;
                        }
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(LiveFeedPage));
                        break;
                    case "ht":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(TopicPage));
                        //MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(TimelinePage));
                        break;
                    case "hd":
                        // Utils.ShowMessageToast("开发中...", 3000);
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(ActivityPage));
                        break;
                    case "sq":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(VideoViewPage), new Random().Next(10000, 4999999).ToString());
                        break;
                    case "setting":
                        MessageCenter.SendNavigateTo(NavigateMode.Info, typeof(SettingPage));
                        break;
                    default:
                        break;
                }
            }
        }

        //public static T MyFindListBoxChildOfType<T>(DependencyObject root) where T : class
        //{
        //    var MyQueue = new Queue<DependencyObject>();
        //    MyQueue.Enqueue(root);
        //    while (MyQueue.Count > 0)
        //    {
        //        DependencyObject current = MyQueue.Dequeue();
        //        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
        //        {
        //            var child = VisualTreeHelper.GetChild(current, i);
        //            var typedChild = child as T;
        //            if (typedChild != null)
        //            {
        //                return typedChild;
        //            }
        //            MyQueue.Enqueue(child);
        //        }
        //    }
        //    return null;
        //}
    }
}
