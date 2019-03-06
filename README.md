# ![2Captcha logo](https://i.imgur.com/sCDANG3.png)

![](https://img.shields.io/github/release/Zaczero/2Captcha.svg)
![](https://img.shields.io/nuget/v/2CaptchaAPI.svg)
![](https://img.shields.io/github/license/Zaczero/2Captcha.svg)

Simple HTTP API wrapper for https://2captcha.com/  
An online captcha solving and image recognition service.

## Download
* Latest release: https://github.com/Zaczero/2Captcha/releases/latest

## Thank You!
If you find this project useful and you are new to 2captcha please consider registering from my [referrral link](http://2captcha.com/?from=6591885) 😊

## Sample code

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

### And here is the result structure: *(same for all methods)*

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

## License

MIT License

Copyright (c) 2018 Kamil Monicz

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.