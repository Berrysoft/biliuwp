using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace BiliBiliJS
{
    [AllowForWeb]
    public sealed class secure
    {
        public event EventHandler<string> CaptchaEvent;
        public void Captcha()
        {
            CaptchaEvent?.Invoke(this, "");
        }

        public event EventHandler<string> CloseCaptchaEvent;
        public void CloseCaptcha() 
        {
            CloseCaptchaEvent?.Invoke(this, "");
        }
    }
}
