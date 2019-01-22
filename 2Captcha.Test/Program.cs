using System.Diagnostics;
using System.IO;

namespace _2Captcha.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var twoCaptcha = new TwoCaptcha(" ## YOUR API KEY ## ");

            // Solve image captcha
            var image1 = twoCaptcha.SolveImage(new FileStream("captcha.png", FileMode.Open)).Result;
            var image2 = twoCaptcha.SolveImage("data:image/png;base64,iVBORw0KGgo...").Result;

            // Solve text captcha
            var question = twoCaptcha.SolveQuestion("1 + 3 = ?").Result;

            // Solve ReCaptchaV2
            var recaptcha = twoCaptcha.SolveReCaptchaV2("GOOGLE_SITE_KEY", "https://example.com").Result;
            var recaptchaInvisible = twoCaptcha.SolveReCaptchaV2("GOOGLE_SITE_KEY", "https://example.com", true).Result;

            // Solve ClickCaptcha
            var click1 = twoCaptcha.SolveClickCaptcha(new FileStream("captcha.png", FileMode.Open), "Click on ghosts").Result;
            var click2 = twoCaptcha.SolveClickCaptcha("data:image/png;base64,iVBORw0KGgo...", "Click on ghosts").Result;

            // Solve RotateCaptcha
            var rotate = twoCaptcha.SolveRotateCaptcha(new Stream[] {new FileStream("captcha.png", FileMode.Open)}, "40").Result;

            // Solve FunCaptcha
            var fun = twoCaptcha.SolveFunCaptcha("FUN_CAPTCHA_PUBLIC_KEY", "https://example.com").Result;
            var funNoJavaScript = twoCaptcha.SolveFunCaptcha("FUN_CAPTCHA_PUBLIC_KEY", "https://example.com", true).Result;

            // Solve KeyCaptcha
            var key = twoCaptcha.SolveKeyCaptcha("USER_ID", "SESSION_ID", "WEB_SIGN_1", "WEB_SIGN_2", "https://example.com").Result;

            Debugger.Break();
        }
    }
}
