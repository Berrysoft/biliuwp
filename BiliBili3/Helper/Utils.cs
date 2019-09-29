using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using BiliBili3.Controls;
using Newtonsoft.Json.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;

namespace BiliBili3
{
    public static class Utils
    {
        public static ReturnJObject ToDynamicJObject(this string json)
        {
            try
            {
                var obj = JObject.Parse(json);
                ReturnJObject returnJObject = new ReturnJObject()
                {
                    Code = obj["code"].ToInt32(),
                    Message = (obj["message"] == null) ? string.Empty : obj["message"].ToString(),
                    Msg = (obj["msg"] == null) ? string.Empty : obj["msg"].ToString(),
                    Json = obj
                };
                return returnJObject;
            }
            catch (Exception)
            {
                return new ReturnJObject()
                {
                    Code = -999,
                    Message = "解析JSON失败",
                    Msg = "解析JSON失败"
                };
            }
        }

        public static void ReadB(this Stream stream, byte[] buffer, int offset, int count)
        {
            if (offset + count > buffer.Length)
                throw new ArgumentException();
            var read = 0;
            while (read < count)
            {
                var available = stream.Read(buffer, offset, count - read);
                if (available == 0)
                {
                    throw new ObjectDisposedException(null);
                }
                read += available;
                offset += available;
            }
        }

        public static string RegexMatch(string input, string regular)
        {
            var data = Regex.Match(input, regular);
            if (data.Groups.Count >= 2 && !string.IsNullOrEmpty(data.Groups[1].Value))
            {
                return data.Groups[1].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void SetClipboard(string content)
        {
            DataPackage pack = new DataPackage();
            pack.SetText(content);
            Clipboard.SetContent(pack); // 保存 DataPackage 对象到剪切板
            Clipboard.Flush();
        }

        public static async Task<bool> ShowLoginDialog()
        {
            LoginDialog login = new LoginDialog();
            await login.ShowAsync();
            return ApiHelper.IsLogin();
        }

        /// <summary>
        /// 根据Epid取番剧ID
        /// </summary>
        /// <returns></returns>
        public static async Task<string> BangumiEpidToSid(string url)
        {
            try
            {
                if (!url.Contains("http"))
                {
                    url = "https://www.bilibili.com/bangumi/play/ep" + url;
                }

                var re = await WebClientClass.GetResultsUTF8Encode(new Uri(url));
                var data = RegexMatch(re, @"ss(\d+)");
                if (!string.IsNullOrEmpty(data))
                {
                    return data;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public static int ToInt32(this object obj)
        {
            if (int.TryParse(obj.ToString(), out var value))
            {
                return value;
            }
            else
            {
                return 0;
            }
        }
        public static Color ToColor(this string obj)
        {
            obj = obj.Replace("#", "");
            obj = Convert.ToInt32(obj).ToString("X2");

            Color color = new Color();
            if (obj.Length == 4)
            {
                obj = "00" + obj;
            }
            if (obj.Length == 6)
            {
                color.R = byte.Parse(obj.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                color.G = byte.Parse(obj.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                color.B = byte.Parse(obj.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                color.A = 255;
            }
            if (obj.Length == 8)
            {
                color.R = byte.Parse(obj.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                color.G = byte.Parse(obj.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                color.B = byte.Parse(obj.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                color.A = byte.Parse(obj.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return color;
        }
        public static Color ToColor2(this string obj)
        {
            obj = obj.Replace("#", "");

            Color color = new Color();
            if (obj.Length == 4)
            {
                obj = "00" + obj;
            }
            if (obj.Length == 6)
            {
                color.R = byte.Parse(obj.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                color.G = byte.Parse(obj.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                color.B = byte.Parse(obj.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                color.A = 255;
            }
            if (obj.Length == 8)
            {
                color.R = byte.Parse(obj.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                color.G = byte.Parse(obj.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                color.B = byte.Parse(obj.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                color.A = byte.Parse(obj.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return color;
        }

        public static void ShowMessageToast(string message)
        {
            MessageToast ms = new MessageToast(message, TimeSpan.FromSeconds(2));
            ms.Show();
        }

        public static void ShowMessageToast(string message, int time)
        {
            MessageToast ms = new MessageToast(message, TimeSpan.FromMilliseconds(time));
            ms.Show();
        }

        public static string ToW(this object par)
        {
            try
            {
                var num = Convert.ToDouble(par);
                if (num >= 10000)
                {
                    return (num / 10000).ToString("0.00") + "万";
                }
                else
                {
                    return num.ToString("0");
                }
            }
            catch (Exception)
            {
                return "0";
            }
        }

        public static DateTime GetTime(long timeStamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            TimeSpan toNow = TimeSpan.FromSeconds(timeStamp);
            DateTime dt = dtStart.Add(toNow).ToLocalTime();
            return dt;
        }
    }


    public sealed class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                this.MyExecute(parameter);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public Action<object> MyExecute { get; set; }
    }
}
