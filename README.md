
# ![2Captcha logo](https://i.imgur.com/sCDANG3.png)
Simple API wrapper for https://2captcha.com/

## Download
* https://github.com/Zaczero/2Captcha/releases/latest

## Thank You!
If you find this project useful and you are new to 2captcha please consider registering from my [referrral link](http://2captcha.com/?from=6591885) 😊

## Sample code

```cs
var twoCaptcha = new TwoCaptcha(" ## YOUR API KEY ## ");

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
```

View the [result structure](https://github.com/Zaczero/2Captcha/blob/master/2Captcha/TwoCaptchaResult.cs)