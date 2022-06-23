# ![Zaczero/2Captcha logo](https://github.com/Zaczero/2Captcha/blob/master/resources/2captcha.png)

[![Build Status](https://travis-ci.com/Zaczero/2Captcha.svg?branch=master)](https://travis-ci.com/Zaczero/2Captcha)
[![Github Release](https://img.shields.io/github/release/Zaczero/2Captcha.svg)](https://github.com/Zaczero/2Captcha/releases/latest)
[![NuGet Release](https://img.shields.io/nuget/v/2CaptchaAPI.svg)](https://www.nuget.org/packages/2CaptchaAPI/)
[![License](https://img.shields.io/github/license/Zaczero/2Captcha.svg)](https://github.com/Zaczero/2Captcha/blob/master/LICENSE)

Simple HTTP API wrapper for [2captcha.com](https://2captcha.com/).
An online captcha solving and image recognition service.

*By using this library you will get **10% cashback** on captchas you solve.*

## 🌤️ Installation

### Install with NuGet (recommended)

`Install-Package 2CaptchaAPI`

### Install manually

[Browse latest GitHub release](https://github.com/Zaczero/2Captcha/releases/latest)

## 🏁 Getting started

### Sample code

```cs
/*
 * Class initialization
 * Optionally you can pass 2nd parameter `httpClient` with custom HttpClient to use while requesting API
 */
var captcha = new _2Captcha("API_KEY");
var captchaCustomHttp = new _2Captcha("API_KEY", new HttpClient());

/*
 * Set custom API url (optional)
 */
captcha.SetApiUrl("https://CUSTOM_URL");

/*
 * Get current balance
 */
var balance = await captcha.GetBalance();

/*
 * Type: Image
 *
 * Documentation (2captcha): https://2captcha.com/2captcha-api#solving_normal_captcha
 */
var image = await captcha.SolveImage(new FileStream("captcha.png", FileMode.Open), FileType.Png);
var image2 = await captcha.SolveImage(File.ReadAllBytes("captcha.png"), FileType.Png);
var image3 = await captcha.SolveImage("BASE64_IMAGE", FileType.Png);

var imageCaseSensitive = await captcha.SolveImage(new FileStream("captcha.png", FileMode.Open), FileType.Png, new KeyValuePair<string, string>("regsense", "1"));
var imageCaseSensitive2 = await captcha.SolveImage(File.ReadAllBytes("captcha.png"), FileType.Png, new KeyValuePair<string, string>("regsense", "1"));
var imageCaseSensitive3 = await captcha.SolveImage("BASE64_IMAGE", FileType.Png, new KeyValuePair<string, string>("regsense", "1"));

/*
 * Type: Text
 *
 * Documentation (2captcha): https://2captcha.com/2captcha-api#solving_text_captcha
 */
var question = await captcha.SolveQuestion("1 + 3 = ?");

/*
 * Type: ReCaptcha V2
 * Optionally you can pass 3rd parameter `isInvisible` to indicate if the reCaptcha is setup as invisible
 *
 * Homepage: https://www.google.com/recaptcha/
 * Documentation (vendor): https://developers.google.com/recaptcha/docs/display
 * Documentation (2captcha): https://2captcha.com/2captcha-api#solving_recaptchav2_new
 */
var reCaptcha = await captcha.SolveReCaptchaV2("SITE_KEY", "https://WEBSITE_URL");
var reCaptchaInvisible = await captcha.SolveReCaptchaV2("SITE_KEY", "https://WEBSITE_URL", true);
var reCaptchaWithProxy = await captcha.SolveReCaptchaV2("SITE_KEY", "https://WEBSITE_URL", "username:password@address:port", ProxyType.Http);

/*
 * Type: ReCaptcha V3
 *
 * Homepage: https://www.google.com/recaptcha/
 * Documentation (vendor): https://developers.google.com/recaptcha/docs/v3
 * Documentation (2captcha): https://2captcha.com/2captcha-api#solving_recaptchav3
 */
var reCaptchaV3 = await captcha.SolveReCaptchaV3("SITE_KEY", "https://WEBSITE_URL", "ACTION", 0.4);
var reCaptchaV3WithProxy = await captcha.SolveReCaptchaV3("SITE_KEY", "https://WEBSITE_URL", "username:password@address:port", ProxyType.Http, "ACTION", 0.4);

/*
 * Type: hCaptcha
 *
 * Homepage: https://www.hcaptcha.com/
 * Documentation (vendor): https://docs.hcaptcha.com/
 * Documentation (2captcha): https://2captcha.com/2captcha-api#solving_hcaptcha
 */
var hCaptcha = await captcha.SolveHCaptcha("SITE_KEY", "https://WEBSITE_URL");
var hCaptchaWithProxy = await captcha.SolveHCaptcha("SITE_KEY", "https://WEBSITE_URL", "username:password@address:port", ProxyType.Http);

/*
 * Type: GeeTest
 *
 * Homepage: https://www.geetest.com/en
 * Documentation (vendor): https://docs.geetest.com/en
 * Documentation (2captcha): https://2captcha.com/2captcha-api#solving_geetest
 */
var geeTest = await captcha.SolveGeeTest("SITE_KEY", "https://WEBSITE_URL", "CHALLENGE");

/*
 * Type: ClickCaptcha
 *
 * Documentation (2captcha): https://2captcha.com/2captcha-api#solving_clickcaptcha
 */
var clickCaptcha = await captcha.SolveClickCaptcha(new FileStream("captcha.png", FileMode.Open), FileType.Png, "TASK");
var clickCaptcha2 = await captcha.SolveClickCaptcha(File.ReadAllBytes("captcha.png"), FileType.Png, "TASK");
var clickCaptcha3 = await captcha.SolveClickCaptcha("BASE64_IMAGE", FileType.Png, "TASK");

/*
 * Type: RotateCaptcha
 *
 * Documentation (2captcha): https://2captcha.com/2captcha-api#solving_rotatecaptcha
 */
var rotateCaptcha = await captcha.SolveRotateCaptcha(
    new Stream[]
    {
        new FileStream("captcha1.png", FileMode.Open),
        new FileStream("captcha2.png", FileMode.Open),
        new FileStream("captcha3.png", FileMode.Open),
    }, FileType.Png, "40");

/*
 * Type: FunCaptcha
 *
 * Documentation (2captcha): https://2captcha.com/2captcha-api#solving_funcaptcha_new
 */
var funCaptcha = await captcha.SolveFunCaptcha("PUBLIC_KEY", "https://WEBSITE_URL");
var funCaptchaNoJavaScript = await captcha.SolveFunCaptcha("PUBLIC_KEY", "https://WEBSITE_URL", true);
var funCaptchaWithProxy = await captcha.SolveFunCaptcha("PUBLIC_KEY", "https://WEBSITE_URL", "username:password@address:port", ProxyType.Http);

/*
 * Type: KeyCaptcha
 *
 * Homepage: https://www.keycaptcha.com/
 * Documentation (2captcha): https://2captcha.com/2captcha-api#solving_keycaptcha
 */
var keyCaptcha = await captcha.SolveKeyCaptcha("USER_ID", "SESSION_ID", "WEB_SIGN_1", "WEB_SIGN_2", "https://WEBSITE_URL");
```

### And here is the result structure *(the same for all methods)*

```cs
public struct Result
{
    public readonly bool Success;
    public readonly JToken ResponseObject;

    public string ResponseJson;
    public string Response;
    public double ResponseDouble;
    public Coordinates[] ResponseCoordinates;
}
```

## Footer

### 📧 Contact

* Email: [kamil@monicz.pl](mailto:kamil@monicz.pl)

### 📃 License

* [Zaczero/2Captcha](https://github.com/Zaczero/2Captcha/blob/master/LICENSE)
* [JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)
