# ![Zaczero/2Captcha logo](https://i.imgur.com/sCDANG3.png)

![github version](https://img.shields.io/github/release/Zaczero/2Captcha.svg)
![nuget version](https://img.shields.io/nuget/v/2CaptchaAPI.svg)
![license type](https://img.shields.io/github/license/Zaczero/2Captcha.svg)

Simple HTTP API wrapper for [2captcha.com](https://2captcha.com/)  
An online captcha solving and image recognition service.

### 🔗 Download

* Latest release - [github.com/Zaczero/2Captcha/releases/latest](https://github.com/Zaczero/2Captcha/releases/latest)

### 🏁 Sample code

```cs
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

// Solve ReCaptchaV3
var recaptcha3 = await twoCaptcha.SolveReCaptchaV3("GOOGLE_SITE_KEY", "https://example.com", "ACTION", 0.4);

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
```

#### And here is the result structure *(same for all methods)*:

```cs
public struct TwoCaptchaResult
{
	public bool Success;
	public string Response;

	public TwoCaptchaResult(bool success, string response)
	{
		Success = success;
		Response = response;
	}
}
```

### ☕ Support me

* Bitcoin: `35n1y9iHePKsVTobs4FJEkbfnBg2NtVbJW`
* Ethereum: `0xc69C7FC9Ce691c95f38798506EfdBB8d14005B67`

### 📃 License

* [Zaczero/2Captcha](https://github.com/Zaczero/2Captcha/blob/master/LICENSE)
* [JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)
