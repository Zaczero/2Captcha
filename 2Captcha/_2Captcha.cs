using _2CaptchaAPI.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo
#pragma warning disable 649

namespace _2CaptchaAPI
{
	public partial class _2Captcha
	{
		private string _apiUrl = "https://2captcha.com/";

		private readonly HttpClient _httpClient;
		private readonly string _apiKey;

		public _2Captcha(string apiKey, HttpClient httpClient = null)
		{
			_httpClient = httpClient ?? new HttpClient();
			_apiKey = apiKey;
		}

		public void SetApiUrl(string url)
		{
			if (!url.EndsWith("/"))
			{
				 _apiUrl = url + "/";
				 return;
			}

			_apiUrl = url;
		}

		public async Task<Result> GetBalance()
		{
			var getData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("key", _apiKey),
				new KeyValuePair<string, string>("action", "getbalance"),
				new KeyValuePair<string, string>("json", "1"),
				new KeyValuePair<string, string>("soft_id", "2670")
			};

			var inResponse = await _httpClient.PostAsync(_apiUrl + "res.php", new FormUrlEncodedContent(getData));
			var inJson = await inResponse.Content.ReadAsStringAsync();

			var @in = JsonConvert.DeserializeObject<ResultInternal>(inJson);
			return new Result(@in.Status == 1, @in.Request);
		}

		private async Task<Result> Solve(string method, int delaySeconds, IEnumerable<KeyValuePair<string, string>> options, MultipartFormDataContent httpContent)
		{
			httpContent.Add(new StringContent(_apiKey), "key");
			httpContent.Add(new StringContent(method), "method");
			httpContent.Add(new StringContent("1"), "json");
			httpContent.Add(new StringContent("2670"), "soft_id");

			foreach (var option in options)
				httpContent.Add(new StringContent(option.Value), option.Key);

			var inResponse = await _httpClient.PostAsync(_apiUrl + "in.php", httpContent);
			var inJson = await inResponse.Content.ReadAsStringAsync();

			var @in = JsonConvert.DeserializeObject<ResultInternal>(inJson);
			if (@in.Status == 0)
				return new Result(false, @in.Request);

			await Task.Delay(delaySeconds * 1000);
			return await GetResponse(@in.Request);
		}

		private async Task<Result> Solve(string method, int delaySeconds, IEnumerable<KeyValuePair<string, string>> options, params KeyValuePair<string, string>[] args)
		{
			var postData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("key", _apiKey),
				new KeyValuePair<string, string>("method", method),
				new KeyValuePair<string, string>("json", "1"),
				new KeyValuePair<string, string>("soft_id", "2670")
			};

			postData.AddRange(args);
			postData.AddRange(options);

			var inResponse = await _httpClient.PostAsync(_apiUrl + "in.php", new FormUrlEncodedContent(postData));
			var inJson = await inResponse.Content.ReadAsStringAsync();

			var @in = JsonConvert.DeserializeObject<ResultInternal>(inJson);
			if (@in.Status == 0)
				return new Result(false, @in.Request);

			await Task.Delay(delaySeconds * 1000);
			return await GetResponse(@in.Request);
		}

		private async Task<Result> GetResponse(JToken solveIdObject)
		{
			var apiKeySafe = Uri.EscapeUriString(_apiKey);
			var solveId = solveIdObject.ToObject<string>();

			while (true)
			{
				var resJson = await _httpClient.GetStringAsync(_apiUrl + $"res.php?key={apiKeySafe}&id={solveId}&action=get&json=1");

				var res = JsonConvert.DeserializeObject<ResultInternal>(resJson);
				if (res.Status == 0)
				{
					if (res.Request.ToObject<string>() == "CAPCHA_NOT_READY")
					{
						await Task.Delay(5 * 1000);
						continue;
					}

					return new Result(false, res.Request);
				}

				return new Result(true, res.Request);
			}
		}

		public async Task<Result> SolveImage(Stream imageStream, FileType fileType, params KeyValuePair<string, string>[] options)
		{
			var httpContent = new MultipartFormDataContent
			{
				{ new StreamContent(imageStream), "file", $"image.{fileType.GetExtension()}" }
			};

			return await Solve("post", 5, options, httpContent);
		}

		public async Task<Result> SolveImage(byte[] imageBytes, FileType fileType, params KeyValuePair<string, string>[] options)
		{
			var imageBase64 = Convert.ToBase64String(imageBytes);
			var imageData = EnsureValidData(imageBase64, fileType);

			return await Solve("base64", 5, options,
				new KeyValuePair<string, string>("body", imageData));
		}

		public async Task<Result> SolveImage(string imageBase64, FileType fileType, params KeyValuePair<string, string>[] options)
		{
			var imageData = EnsureValidData(imageBase64, fileType);

			return await Solve("base64", 5, options,
				new KeyValuePair<string, string>("body", imageData));
		}

		public async Task<Result> SolveQuestion(string question, params KeyValuePair<string, string>[] options)
		{
			return await Solve("textcaptcha", 5, options,
				new KeyValuePair<string, string>("textcaptcha", question));
		}

		public async Task<Result> SolveReCaptchaV2(string siteKey, string pageUrl, bool isInvisible = false, params KeyValuePair<string, string>[] options)
		{
			return await Solve("userrecaptcha", 10, options,
				new KeyValuePair<string, string>("googlekey", siteKey),
				new KeyValuePair<string, string>("pageurl", pageUrl),
				new KeyValuePair<string, string>("invisible", isInvisible ? "1" : "0"));
		}

		public async Task<Result> SolveReCaptchaV2(string siteKey, string pageUrl, string proxy, ProxyType proxyType, bool isInvisible = false, params KeyValuePair<string, string>[] options)
		{
			return await Solve("userrecaptcha", 10, options,
				new KeyValuePair<string, string>("googlekey", siteKey),
				new KeyValuePair<string, string>("pageurl", pageUrl),
				new KeyValuePair<string, string>("proxy", proxy),
				new KeyValuePair<string, string>("proxytype", proxyType.GetExtension()),
				new KeyValuePair<string, string>("invisible", isInvisible ? "1" : "0")) ;
		}

		public async Task<Result> SolveReCaptchaV3(string siteKey, string pageUrl, string action = "verify", double minScore = 0.4, params KeyValuePair<string, string>[] options)
		{
			return await Solve("userrecaptcha", 10, options,
				new KeyValuePair<string, string>("googlekey", siteKey),
				new KeyValuePair<string, string>("pageurl", pageUrl),
				new KeyValuePair<string, string>("action", action),
				new KeyValuePair<string, string>("version", "v3"),
				new KeyValuePair<string, string>("min_score", minScore.ToString(CultureInfo.InvariantCulture)));
		}

		public async Task<Result> SolveReCaptchaV3(string siteKey, string pageUrl, string proxy, ProxyType proxyType, string action = "verify", double minScore = 0.4, params KeyValuePair<string, string>[] options)
		{
			return await Solve("userrecaptcha", 10, options,
				new KeyValuePair<string, string>("googlekey", siteKey),
				new KeyValuePair<string, string>("pageurl", pageUrl),
				new KeyValuePair<string, string>("action", action),
				new KeyValuePair<string, string>("version", "v3"),
				new KeyValuePair<string, string>("min_score", minScore.ToString(CultureInfo.InvariantCulture)),
				new KeyValuePair<string, string>("proxy", proxy),
				new KeyValuePair<string, string>("proxytype", proxyType.GetExtension()));
		}

		public async Task<Result> SolveHCaptcha(string siteKey, string pageUrl, params KeyValuePair<string, string>[] options)
		{
			return await Solve("hcaptcha", 10, options,
				new KeyValuePair<string, string>("sitekey", siteKey),
				new KeyValuePair<string, string>("pageurl", pageUrl));
		}

		public async Task<Result> SolveHCaptcha(string siteKey, string pageUrl, string proxy, ProxyType proxyType, params KeyValuePair<string, string>[] options)
		{
			return await Solve("hcaptcha", 10, options,
				new KeyValuePair<string, string>("sitekey", siteKey),
				new KeyValuePair<string, string>("pageurl", pageUrl),
				new KeyValuePair<string, string>("proxy", proxy),
				new KeyValuePair<string, string>("proxytype", proxyType.GetExtension()));
		}

		public async Task<Result> SolveGeeTest(string siteKey, string pageUrl, string challenge, params KeyValuePair<string, string>[] options)
		{
			return await Solve("geetest", 10, options,
				new KeyValuePair<string, string>("gt", siteKey),
				new KeyValuePair<string, string>("challenge", challenge),
				new KeyValuePair<string, string>("pageurl", pageUrl));
		}

		public async Task<Result> SolveClickCaptcha(Stream imageStream, FileType fileType, string task, params KeyValuePair<string, string>[] options)
		{
			var httpContent = new MultipartFormDataContent
			{
				{ new StringContent("1"), "coordinatescaptcha" },
				{ new StreamContent(imageStream), "file", $"image.{fileType.GetExtension()}" },
				{ new StringContent(task), "textinstructions" }
			};

			return await Solve("post", 5, options, httpContent);
		}

		public async Task<Result> SolveClickCaptcha(byte[] imageBytes, FileType fileType, string task, params KeyValuePair<string, string>[] options)
		{
			var imageBase64 = Convert.ToBase64String(imageBytes);
			var imageData = EnsureValidData(imageBase64, fileType);

			return await Solve("base64", 5, options,
				new KeyValuePair<string, string>("coordinatescaptcha", "1"),
				new KeyValuePair<string, string>("body", imageData),
				new KeyValuePair<string, string>("textinstructions", task));
		}

		public async Task<Result> SolveClickCaptcha(string imageBase64, FileType fileType, string task, params KeyValuePair<string, string>[] options)
		{
			var imageData = EnsureValidData(imageBase64, fileType);

			return await Solve("base64", 5, options,
				new KeyValuePair<string, string>("coordinatescaptcha", "1"),
				new KeyValuePair<string, string>("body", imageData),
				new KeyValuePair<string, string>("textinstructions", task));
		}

		public async Task<Result> SolveRotateCaptcha(Stream[] imageStreams, FileType fileType, string rotateAngle, params KeyValuePair<string, string>[] options)
		{
			var httpContent = new MultipartFormDataContent
			{
				{ new StringContent(rotateAngle), "angle" }
			};

			var fileExtension = fileType.GetExtension();

			for (var i = 0; i < imageStreams.Length; i++)
				httpContent.Add(new StreamContent(imageStreams[i]), $"file_{i + 1}", $"image_{i + 1}.{fileExtension}");

			return await Solve("rotatecaptcha", 5, options, httpContent);
		}

		public async Task<Result> SolveFunCaptcha(string publicKey, string pageUrl, bool noJavaScript = false, string userAgent = null, string sUrl = null, Dictionary<string, string> data = null, params KeyValuePair<string, string>[] options)
		{
			var args = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("publickey", publicKey),
				new KeyValuePair<string, string>("pageurl", pageUrl),
				new KeyValuePair<string, string>("nojs", noJavaScript ? "1" : "0")
			};

			if (!string.IsNullOrEmpty(userAgent))
				args.Add(new KeyValuePair<string, string>("userAgent", userAgent));

			if (!string.IsNullOrEmpty(sUrl))
				args.Add(new KeyValuePair<string, string>("surl", sUrl));

			if (data != null)
				args.AddRange(
					data.Select(pair => new KeyValuePair<string, string>($"data[{pair.Key}]", pair.Value.ToString()))
				);

			return await Solve("funcaptcha", 10, options, args.ToArray());
		}

		public async Task<Result> SolveFunCaptcha(string publicKey, string pageUrl, string proxy, ProxyType proxyType, bool noJavaScript = false, string userAgent = null, string sUrl = null, Dictionary<string, string> data = null, params KeyValuePair<string, string>[] options)
		{
			var args = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("publickey", publicKey),
				new KeyValuePair<string, string>("pageurl", pageUrl),
				new KeyValuePair<string, string>("proxy", proxy),
				new KeyValuePair<string, string>("proxytype", proxyType.GetExtension()),
				new KeyValuePair<string, string>("nojs", noJavaScript ? "1" : "0")
			};

			if (!string.IsNullOrEmpty(userAgent))
				args.Add(new KeyValuePair<string, string>("userAgent", userAgent));

			if (!string.IsNullOrEmpty(sUrl))
				args.Add(new KeyValuePair<string, string>("surl", sUrl));

			if (data != null)
				args.AddRange(
					data.Select(pair => new KeyValuePair<string, string>($"data[{pair.Key}]", pair.Value.ToString()))
				);

			return await Solve("funcaptcha", 10, options, args.ToArray());
		}

		public async Task<Result> SolveKeyCaptcha(string userId, string sessionId, string webServerSign, string webServerSign2, string pageUrl, params KeyValuePair<string, string>[] options)
		{
			return await Solve("keycaptcha", 15, options,
				new KeyValuePair<string, string>("s_s_c_user_id", userId),
				new KeyValuePair<string, string>("s_s_c_session_id", sessionId),
				new KeyValuePair<string, string>("s_s_c_web_server_sign", webServerSign),
				new KeyValuePair<string, string>("s_s_c_web_server_sign2", webServerSign2),
				new KeyValuePair<string, string>("pageurl", pageUrl));
		}

		private static string EnsureValidData(string base64data, FileType fileType)
		{
			return base64data.StartsWith("data:") ? base64data : $"data:{fileType.GetMime()};base64,{base64data}";
		}
	}
}
