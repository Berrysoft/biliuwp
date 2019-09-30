using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using BiliBili3.Class;
using Windows.Security.Cryptography.Certificates;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace BiliBili3
{
    class WebClientClass
    {
        public static async Task<string> GetResults(Uri url)
        {
            HttpBaseProtocolFilter fiter = new HttpBaseProtocolFilter();
            fiter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            using (HttpClient hc = new HttpClient(fiter))
            {
                if (url.AbsoluteUri.Contains("23moe"))
                {
                    var ts = ApiHelper.TimeStamp.ToString();
                    EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
                    hc.DefaultRequestHeaders.Add("client", "bilibili-uwp");
                    hc.DefaultRequestHeaders.Add("ts", ts);
                    hc.DefaultRequestHeaders.Add("appsign", MD5.GetMd5String("biliUwpXycz0423" + ts + "bilibili-uwp" + SettingHelper.GetVersion() + "0BJSDAHDUAHGAI5D45ADS5" + deviceInfo.Id.ToString()));
                    hc.DefaultRequestHeaders.Add("version", SettingHelper.GetVersion());
                    hc.DefaultRequestHeaders.Add("device-id", deviceInfo.Id.ToString());
                }

                hc.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 BiliDroid/4.34.0 (bbcallen@gmail.com)");
                hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                HttpResponseMessage hr = await hc.GetAsync(url);
                hr.EnsureSuccessStatusCode();
                var encodeResults = await hr.Content.ReadAsBufferAsync();
                string results = Encoding.UTF8.GetString(encodeResults.ToArray());
                return results;
            }
        }
        public static async Task<string> GetResults(Uri url, Dictionary<string, string> header)
        {
            HttpBaseProtocolFilter fiter = new HttpBaseProtocolFilter();
            fiter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            using (HttpClient hc = new HttpClient(fiter))
            {
                foreach (var item in header)
                {
                    hc.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                HttpResponseMessage hr = await hc.GetAsync(url);
                hr.EnsureSuccessStatusCode();
                var encodeResults = await hr.Content.ReadAsBufferAsync();
                string results = Encoding.UTF8.GetString(encodeResults.ToArray());
                return results;
            }
        }

        public static async Task<IBuffer> GetBuffer(Uri url)
        {
            using (HttpClient hc = new HttpClient())
            {
                HttpResponseMessage hr = await hc.GetAsync(url);
                hr.EnsureSuccessStatusCode();
                IBuffer results = await hr.Content.ReadAsBufferAsync();
                return results;
            }
        }

        public static async Task<string> PostResults(Uri url, string PostContent)
        {
            try
            {
                HttpBaseProtocolFilter fiter = new HttpBaseProtocolFilter();
                fiter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
                using (HttpClient hc = new HttpClient(fiter))
                {
                    hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                    var response = await hc.PostAsync(url, new HttpStringContent(PostContent, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                    response.EnsureSuccessStatusCode();
                    var encodeResults = await response.Content.ReadAsBufferAsync();
                    string result = Encoding.UTF8.GetString(encodeResults.ToArray(), 0, encodeResults.ToArray().Length);
                    return result;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static async Task<string> PostResultsJson(Uri url, string PostContent)
        {
            try
            {
                HttpBaseProtocolFilter fiter = new HttpBaseProtocolFilter();
                fiter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
                using (HttpClient hc = new HttpClient(fiter))
                {
                    var response = await hc.PostAsync(url, new HttpStringContent(PostContent, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static async Task<string> PostResultsUtf8(Uri url, string PostContent)
        {
            try
            {
                HttpBaseProtocolFilter fiter = new HttpBaseProtocolFilter();
                fiter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
                using (HttpClient hc = new HttpClient(fiter))
                {
                    hc.DefaultRequestHeaders.Referer = new Uri("http://www.bilibili.com/");
                    var response = await hc.PostAsync(url, new HttpStringContent(PostContent, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                    response.EnsureSuccessStatusCode();
                    var encodeResults = await response.Content.ReadAsBufferAsync();
                    string results = Encoding.UTF8.GetString(encodeResults.ToArray());
                    return results;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static async Task<string> PostResults(Uri url, string PostContent, string Referer)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    hc.DefaultRequestHeaders.Referer = new Uri(Referer);
                    var response = await hc.PostAsync(url, new HttpStringContent(PostContent, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static async Task<string> GetResults_Live(Uri url)
        {

            HttpBaseProtocolFilter fiter = new HttpBaseProtocolFilter();
            fiter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            using (HttpClient hc = new HttpClient(fiter))
            {
                HttpResponseMessage hr = await hc.GetAsync(url);
                hr.EnsureSuccessStatusCode();
                string results = await hr.Content.ReadAsStringAsync();
                return results;
            }
        }

        public static async Task<string> PostResults(Uri url, IInputStream PostContent, string Referer)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    hc.DefaultRequestHeaders.Referer = new Uri(Referer);
                    var response = await hc.PostAsync(url, new HttpStreamContent(PostContent));
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static async Task<string> GetResultsUTF8Encode(Uri url)
        {
            HttpBaseProtocolFilter fiter = new HttpBaseProtocolFilter();
            fiter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            using (HttpClient hc = new HttpClient(fiter))
            {
                HttpResponseMessage hr = await hc.GetAsync(url);
                hr.EnsureSuccessStatusCode();
                var encodeResults = await hr.Content.ReadAsBufferAsync();
                string results = Encoding.UTF8.GetString(encodeResults.ToArray());
                return results;
            }
        }
    }
}
