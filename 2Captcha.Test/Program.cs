using System.Diagnostics;
using System.IO;

namespace _2Captcha.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var twoCaptcha = new TwoCaptcha("11111111111111111111111111111111");

            var image1 = twoCaptcha.SolveImage(new FileStream("captcha.png", FileMode.Open));
            var image2 = twoCaptcha.SolveImage("data:image/png;base64,iVBORw0KGgo...");

            var question = twoCaptcha.SolveQuestion("1 + 3 = ?");
            
            var recaptcha = twoCaptcha.SolveReCaptchaV2("GOOGLE_SITE_KEY", "http://example.com");
            var recaptchaInvisible = twoCaptcha.SolveReCaptchaV2("GOOGLE_SITE_KEY", "http://example.com", true);
            
            var click1 = twoCaptcha.SolveClickCaptcha(new FileStream("captcha.png", FileMode.Open), "Click on ghosts");
            var click2 = twoCaptcha.SolveClickCaptcha("data:image/png;base64,iVBORw0KGgo...", "Click on ghosts");

            var rotate = twoCaptcha.SolveRotateCaptcha(new Stream[] {new FileStream("captcha.png", FileMode.Open)}, "40");

            var fun = twoCaptcha.SolveFunCaptcha("FUN_CAPTCHA_PUBLIC_KEY", "http://example.com");
            var funNoJavaScript = twoCaptcha.SolveFunCaptcha("FUN_CAPTCHA_PUBLIC_KEY", "http://example.com", true);

            var key = twoCaptcha.SolveKeyCaptcha("USER_ID", "SESSION_ID", "WEB_SIGN_1", "WEB_SIGN_2", "http://example.com");

            Debugger.Break();
        }
    }
}
