using System.Diagnostics;
using System.IO;
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
            var twoCaptcha = new TwoCaptcha(" ## YOUR API KEY ## ");

            // Get current balance
            var balance = await twoCaptcha.GetBalance();

            // Solve image captcha
            var image1 = await twoCaptcha.SolveImage(new FileStream("captcha.png", FileMode.Open));
            var image2 = await twoCaptcha.SolveImage("data:image/png;base64,iVBORw0KGgo...");

            // Solve text captcha
            var question = await twoCaptcha.SolveQuestion("1 + 3 = ?");

            // Solve ReCaptchaV2
            var recaptcha = await twoCaptcha.SolveReCaptchaV2("GOOGLE_SITE_KEY", "https://example.com");
            var recaptchaInvisible = await twoCaptcha.SolveReCaptchaV2("GOOGLE_SITE_KEY", "https://example.com", true);

            // Solve ClickCaptcha
            var click1 = await twoCaptcha.SolveClickCaptcha(new FileStream("captcha.png", FileMode.Open), "Click on ghosts");
            var click2 = await twoCaptcha.SolveClickCaptcha("data:image/png;base64,iVBORw0KGgo...", "Click on ghosts");

            // Solve RotateCaptcha
            var rotate = await twoCaptcha.SolveRotateCaptcha(new Stream[] {new FileStream("captcha.png", FileMode.Open)}, "40");

            // Solve FunCaptcha
            var fun = await twoCaptcha.SolveFunCaptcha("FUN_CAPTCHA_PUBLIC_KEY", "https://example.com");
            var funNoJavaScript = await twoCaptcha.SolveFunCaptcha("FUN_CAPTCHA_PUBLIC_KEY", "https://example.com", true);

            // Solve KeyCaptcha
            var key = await twoCaptcha.SolveKeyCaptcha("USER_ID", "SESSION_ID", "WEB_SIGN_1", "WEB_SIGN_2", "https://example.com");

            Debugger.Break();
        }
    }
}
