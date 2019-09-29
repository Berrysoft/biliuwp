using System;
using Windows.Foundation.Metadata;
using Windows.UI.Popups;

namespace BiliBiliJS
{
    /// <summary>
    /// 用这个跟B站Web交互
    /// </summary>
    [AllowForWeb]
    public sealed class Biliapp
    {
        public async void Alert(string message)
        {
            MessageDialog md;

            md = new MessageDialog(message);

            await md.ShowAsync();
        }
        public event EventHandler<string> ValidateLoginEvent;
        public void ValidateLogin(string data) => ValidateLoginEvent?.Invoke(this, data);

        public event EventHandler<string> CloseBrowserEvent;
        public void CloseBrowser() => CloseBrowserEvent?.Invoke(this, string.Empty);
    }
}
