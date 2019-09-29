using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Web.Http;

namespace BackTask
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                //通知
                bool Update = SettingHelper.DTCT;

                if (Update)
                {
                    await GetNews();
                }
                else
                {
                    var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                    updater.Clear();
                }
            }
            finally
            {
                deferral.Complete();
            }
        }

        private async Task GetNews()
        {
            var response = await GetUserAttentionUpdate();

            if (response != null)
            {
                UpdatePrimaryTile(response);
            }
        }


        private void UpdatePrimaryTile(List<GetAttentionUpdate> news)
        {
            if (news == null || !news.Any())
            {
                return;
            }

            try
            {
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.EnableNotificationQueueForWide310x150(true);
                updater.EnableNotificationQueueForSquare150x150(true);
                updater.EnableNotificationQueueForSquare310x310(true);
                updater.EnableNotificationQueue(true);

                List<string> oldList = null;
                updater.Clear();
                bool updateVideo = SettingHelper.DT;
                bool updateBnagumi = SettingHelper.FJ;
                try
                {
                    if (!string.IsNullOrEmpty(SettingHelper.TsDt))
                    {
                        oldList = SettingHelper.TsDt.Split(',').ToList();
                    }
                    else
                    {
                        SettingHelper.TsDt = string.Join(',', news.Select(item => item.Addition.Aid));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                foreach (var n in news)
                {
                    if (news.IndexOf(n) <= 4)
                    {
                        var doc = new XmlDocument();
                        var xml = string.Format(TileTemplateXml, n.Addition.Picture, n.Addition.Title, n.Addition.Description);
                        doc.LoadXml(WebUtility.HtmlDecode(xml), new XmlLoadSettings
                        {
                            ProhibitDtd = false,
                            ValidateOnParse = false,
                            ElementContentWhiteSpace = false,
                            ResolveExternals = false
                        });
                        updater.Update(new TileNotification(doc));
                    }

                    //通知
                    if (oldList != null && oldList.Count != 0)
                    {
                        if (!oldList.Contains(n.Addition.Aid))
                        {
                            ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;
                            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
                            ((XmlElement)toastNode).SetAttribute("duration", "short");
                            ((XmlElement)toastNode).SetAttribute("launch", n.Addition.Aid);
                            if (n.Type == 3)
                            {
                                if (updateBnagumi)
                                {
                                    toastTextElements[0].AppendChild(toastXml.CreateTextNode("您关注的《" + n.Source.Title + "》" + "更新了第" + n.Content.Index + "话"));
                                    ToastNotification toast = new ToastNotification(toastXml);
                                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                                }
                            }
                            else
                            {
                                if (updateVideo)
                                {
                                    toastTextElements[0].AppendChild(toastXml.CreateTextNode(n.Source.Username + "" + "上传了《" + n.Addition.Title + "》"));
                                    ToastNotification toast = new ToastNotification(toastXml);
                                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                SettingHelper.TsDt = string.Join(',', news.Select(item => item.Addition.Aid));
            }
        }

        /// <summary>
        /// 关注动态
        /// </summary>
        /// <returns></returns>
        private async Task<List<GetAttentionUpdate>> GetUserAttentionUpdate()
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    StorageFolder folder = ApplicationData.Current.LocalFolder;
                    StorageFile file = await folder.CreateFileAsync("us.bili", CreationCollisionOption.OpenIfExists);
                    ApiHelper.access_key = await FileIO.ReadTextAsync(file);
                    string url = $"http://api.bilibili.com/x/feed/pull?ps=10&type=0&pn={1}&_={ApiHelper.GetTimeSpen()}";
                    HttpResponseMessage hr = await hc.GetAsync(new Uri(url));
                    hr.EnsureSuccessStatusCode();
                    string results = await hr.Content.ReadAsStringAsync();

                    //一层
                    GetAttentionUpdate model1 = JsonConvert.DeserializeObject<GetAttentionUpdate>(results);
                    if (model1.Code == 0)
                    {
                        GetAttentionUpdate model2 = JsonConvert.DeserializeObject<GetAttentionUpdate>(model1.Data.ToString());
                        return model2.Feeds;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        private const string TileTemplateXml = @"
<tile branding='name'> 
  <visual version='3'>
    <binding template='TileMedium'>
      <image src='{0}' placement='peek'/>
      <text>{1}</text>
      <text hint-style='captionsubtle' hint-wrap='true'>{2}</text>
    </binding>
    <binding template='TileWide'>
      <image src='{0}' placement='peek'/>
      <text>{1}</text>
      <text hint-style='captionsubtle' hint-wrap='true'>{2}</text>
    </binding>
    <binding template='TileLarge'>
      <image src='{0}' placement='peek'/>
      <text>{1}</text>
      <text hint-style='captionsubtle' hint-wrap='true'>{2}</text>
    </binding>
  </visual>
</tile>";
    }
    class GetAttentionUpdate
    {
        //必须有登录Cookie
        //Josn：http://api.bilibili.com/x/feed/pull?jsonp=jsonp&ps=20&type=1&pn=1
        //第一层
        [JsonProperty("code")]
        public int Code { get; set; }//状态，0为正常
        [JsonProperty("data")]
        public object Data { get; set; }//数据，包含第二层

        [JsonProperty("feeds")]
        public List<GetAttentionUpdate> Feeds { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("source")]
        public GetAttentionUpdate Source { get; set; }
        [JsonProperty("uname")]
        public string Username { get; set; }
        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("addition")]
        public GetAttentionUpdate Addition { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("aid")]
        public string Aid { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }//标题

        [JsonProperty("pic")]
        public string Picture { get; set; }//封面

        [JsonProperty("content")]
        public GetAttentionUpdate Content { get; set; }
    }
}
