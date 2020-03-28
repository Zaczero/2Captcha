# ![Zaczero/2Captcha logo](https://github.com/Zaczero/2Captcha/blob/master/resources/2captcha.png)

![github version](https://img.shields.io/github/release/Zaczero/2Captcha.svg)
![nuget version](https://img.shields.io/nuget/v/2CaptchaAPI.svg)
![license type](https://img.shields.io/github/license/Zaczero/2Captcha.svg)

Simple HTTP API wrapper for [2captcha.com](https://2captcha.com/)  
An online captcha solving and image recognition service.

## 🌤️ Installation

### Install with NuGet (recommended)

`Install-Package 2CaptchaAPI`

### Install manually

[Browse latest GitHub release](https://github.com/Zaczero/2Captcha/releases/latest)

## 🏁 Getting started

### Sample code

```cs
var captcha = new _2Captcha(" ## YOUR API KEY ## ");
// .. additionally you can pass your own httpClient class
var captchaWithHttpClient = new _2Captcha(" ## YOUR API KEY ## ", new HttpClient());

// Need to set a custom api url? This step is optional.
captcha.SetApiUrl("https://CUSTOM_URL/");

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
```

### And here is the result structure *(the same for all methods)*

```cs
public struct _2CaptchaResult
{
    public bool Success;
    public string Response;

    public _2CaptchaResult(bool success, string response)
    {
        Success = success;
        Response = response;
    }
}
```

## Footer

### 📧 Contact

* Email: [kamil@monicz.pl](mailto:kamil@monicz.pl)

### 📃 License

* [Zaczero/2Captcha](https://github.com/Zaczero/2Captcha/blob/master/LICENSE)
* [JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)
