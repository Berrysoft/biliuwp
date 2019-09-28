using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using BackTask;
using BiliBili3.Helper;
using BiliBili3.Modules;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using Windows.ApplicationModel.Background;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace BiliBili3
{
    public class LoadModel
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("data")]
        public List<LoadModel> Data { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("param")]
        public string Param { get; set; }
    }

    public sealed partial class SplashPage : Page
    {
        public SplashPage()
        {
            this.InitializeComponent();
            var bg = Color.FromArgb(255, 233, 233, 233);
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = bg;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = bg;
            titleBar.ButtonForegroundColor = Color.FromArgb(255, 254, 254, 254);
            titleBar.InactiveBackgroundColor = bg;
            titleBar.ButtonInactiveBackgroundColor = bg;
        }

        StartModel m;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            switch (new Random().Next(1, 4))
            {
                case 1:
                    LoadText.Text = "爱国、敬业、诚信、友善";
                    break;
                case 2:
                    LoadText.Text = "富强、民主、文明、和谐";
                    break;
                case 3:
                    LoadText.Text = "自由、平等、公正、法治";
                    break;
                default:
                    break;
            }
            try
            {
                RegisterBackgroundTask();
                DownloadHelper2.LoadDowned();
                ApiHelper.SetRegions();
                LiveRoom.GetTitleItems();
                ApiHelper.SetEmojis();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            m = e.Parameter as StartModel;
            if (m.StartType == StartTypes.None && SettingHelper.LoadSplash)
            {
                await GetResults();
                this.Frame.Navigate(typeof(MainPage), m);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(m.StartType == StartTypes.None && SettingHelper.LoadSplash))
            {
                this.Frame.Navigate(typeof(MainPage), m);
            }
        }

        private void InitializedFrostedGlass(UIElement glassHost)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(glassHost);
            Compositor compositor = hostVisual.Compositor;

            // Create a glass effect, requires Win2D NuGet package
            var glassEffect = new GaussianBlurEffect
            {
                BlurAmount = 20.0f,
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

        private async Task GetResults()
        {
            try
            {
                string url = "http://app.bilibili.com/x/splash?plat=0&build=414000&channel=master&width=1920&height=1080";

                string Result = await WebClientClass.GetResults(new Uri(url));
                LoadModel obj = JsonConvert.DeserializeObject<LoadModel>(Result);

                if (obj.Code == 0)
                {
                    if (obj.Data.Count != 0)
                    {
                        var buff = await WebClientClass.GetBuffer(new Uri(obj.Data[0].Image));
                        BitmapImage bit = new BitmapImage();
                        await bit.SetSourceAsync(buff.AsStream().AsRandomAccessStream());
                        BackImage.Source = bit;
                        InitializedFrostedGlass(GlassHost);
                        ForeImage.Source = bit;
                        _url = obj.Data[0].Param;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        string _url;
        private void ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            m.StartType = StartTypes.Web;
            m.Par1 = _url;
        }

        private void RegisterBackgroundTask()
        {
            BackgroundTaskHelper.Register(typeof(BackgroundTask), new TimeTrigger(15, false), true, true);
        }
    }
}
