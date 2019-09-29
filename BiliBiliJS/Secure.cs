using System;
using Windows.Foundation.Metadata;

namespace BiliBiliJS
{
    [AllowForWeb]
    public sealed class Secure
    {
        public event EventHandler<string> CaptchaEvent;
        public void Captcha() => CaptchaEvent?.Invoke(this, string.Empty);

        public event EventHandler<string> CloseCaptchaEvent;
        public void CloseCaptcha() => CloseCaptchaEvent?.Invoke(this, string.Empty);
    }
}
