using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace _2Captcha.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Foo().GetAwaiter().GetResult();
        }

        static async Task Foo()
        {
            var captcha = new _2Captcha(" ## YOUR API KEY ## ");
            // .. additionally you can pass your own httpClient class
            var captchaWithHttpClient = new _2Captcha(" ## YOUR API KEY ## ", new HttpClient());

            // Get current balance
            var balance = await captcha.GetBalance();

            // Solve image captcha
            var image1 = await captcha.SolveImage(new FileStream("captcha.png", FileMode.Open));
            var image2 = await captcha.SolveImage("data:image/png;base64,iVBORw0KGgo...");

            // Solve text captcha
            var question = await captcha.SolveQuestion("1 + 3 = ?");

            // Solve hCaptcha
            var hcaptcha = await captcha.SolveHCaptcha("HCAPTCHA_SITE_KEY", "https://example.com");

            // Solve ReCaptchaV2
            var recaptcha = await captcha.SolveReCaptchaV2("GOOGLE_SITE_KEY", "https://example.com");
            var recaptchaInvisible = await captcha.SolveReCaptchaV2("GOOGLE_SITE_KEY", "https://example.com", true);

            // Solve ReCaptchaV3
            var recaptcha3 = await captcha.SolveReCaptchaV3("GOOGLE_SITE_KEY", "https://example.com", "ACTION", 0.4);

            // Solve ClickCaptcha
            var click1 = await captcha.SolveClickCaptcha(new FileStream("captcha.png", FileMode.Open), "Click on ghosts");
            var click2 = await captcha.SolveClickCaptcha("data:image/png;base64,iVBORw0KGgo...", "Click on ghosts");

            // Solve RotateCaptcha
            var rotate = await captcha.SolveRotateCaptcha(new Stream[] {new FileStream("captcha.png", FileMode.Open)}, "40");

            // Solve FunCaptcha
            var fun = await captcha.SolveFunCaptcha("FUN_CAPTCHA_PUBLIC_KEY", "https://example.com");
            var funNoJavaScript = await captcha.SolveFunCaptcha("FUN_CAPTCHA_PUBLIC_KEY", "https://example.com", true);

            // Solve KeyCaptcha
            var key = await captcha.SolveKeyCaptcha("USER_ID", "SESSION_ID", "WEB_SIGN_1", "WEB_SIGN_2", "https://example.com");

            Debugger.Break();
        }
    }
}
