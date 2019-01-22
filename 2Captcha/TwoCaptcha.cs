using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace _2Captcha
{
    public class TwoCaptcha
    {
        [Serializable]
        private struct TwoCaptchaResponse
        {
            public int Status;
            public string Request;
        }

        private const string baseUrl = "https://2captcha.com/";

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TwoCaptcha(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        
        private async Task<TwoCaptchaResult> Solve(string method, int delaySeconds, MultipartFormDataContent httpContent)
        {
            httpContent.Add(new StringContent(_apiKey), "key");
            httpContent.Add(new StringContent(method), "method");
            httpContent.Add(new StringContent("1"), "json");

            var inResponse = await _httpClient.PostAsync(baseUrl + "in.php", httpContent);
            var inJson = await inResponse.Content.ReadAsStringAsync();

            var @in = JsonConvert.DeserializeObject<TwoCaptchaResponse>(inJson);
            if (@in.Status == 0)
            {
                return new TwoCaptchaResult(false, @in.Request);
            }
            
            await Task.Delay(delaySeconds * 1000);
            return await GetResponse(@in.Request);
        }

        private async Task<TwoCaptchaResult> Solve(string method, int delaySeconds, params KeyValuePair<string, string>[] args)
        {
            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("key", _apiKey),
                new KeyValuePair<string, string>("method", method),
                new KeyValuePair<string, string>("json", "1")
            };

            postData.AddRange(args);

            var inResponse = await _httpClient.PostAsync(baseUrl + "in.php", new FormUrlEncodedContent(postData));
            var inJson = await inResponse.Content.ReadAsStringAsync();

            var @in = JsonConvert.DeserializeObject<TwoCaptchaResponse>(inJson);
            if (@in.Status == 0)
            {
                return new TwoCaptchaResult(false, @in.Request);
            }
            
            await Task.Delay(delaySeconds * 1000);
            return await GetResponse(@in.Request);
        }

        private async Task<TwoCaptchaResult> GetResponse(string solveId)
        {
            var apiKeySafe = Uri.EscapeUriString(_apiKey);

            while (true)
            {
                var resJson = await _httpClient.GetStringAsync(baseUrl + $"res.php?key={apiKeySafe}&id={solveId}&action=get&json=1");

                var res = JsonConvert.DeserializeObject<TwoCaptchaResponse>(resJson);
                if (res.Status == 0)
                {
                    if (res.Request == "CAPTCHA_NOT_READY")
                    {
                        await Task.Delay(5 * 1000);
                        continue;
                    }
                    else
                    {
                        return new TwoCaptchaResult(false, res.Request);
                    }
                }

                return new TwoCaptchaResult(true, res.Request);
            }
        }

        
        public async Task<TwoCaptchaResult> SolveImage(Stream imageStream)
        {
            var httpContent = new MultipartFormDataContent
            {
                { new StreamContent(imageStream), "file" }
            };

            return await Solve("post", 5, httpContent);
        }

        public async Task<TwoCaptchaResult> SolveImage(string imageBase64)
        {
            return await Solve("base64", 5,
                new KeyValuePair<string, string>("body", imageBase64));
        }

        public async Task<TwoCaptchaResult> SolveQuestion(string question)
        {
            return await Solve("textcaptcha", 5,
                new KeyValuePair<string, string>("textcaptcha", question));
        }

        public async Task<TwoCaptchaResult> SolveReCaptchaV2(string googleSiteKey, string pageUrl, bool invisible = false)
        {
            return await Solve("userrecaptcha", 15,
                new KeyValuePair<string, string>("googlekey", googleSiteKey),
                new KeyValuePair<string, string>("pageurl", pageUrl),
                new KeyValuePair<string, string>("invisible", invisible ? "1" : "0"));
        }
        
        public async Task<TwoCaptchaResult> SolveClickCaptcha(Stream imageStream, string task)
        {
            var httpContent = new MultipartFormDataContent
            {
                { new StringContent("1"), "coordinatescaptcha" },
                { new StreamContent(imageStream), "file" },
                { new StringContent(task), "textinstructions" }
            };

            return await Solve("post", 5, httpContent);
        }

        public async Task<TwoCaptchaResult> SolveClickCaptcha(string imageBase64, string task)
        {
            return await Solve("base64", 5,
                new KeyValuePair<string, string>("coordinatescaptcha", "1"),
                new KeyValuePair<string, string>("body", imageBase64),
                new KeyValuePair<string, string>("textinstructions", task));
        }

        public async Task<TwoCaptchaResult> SolveRotateCaptcha(Stream[] imageStreams, string rotateAngle)
        {
            var httpContent = new MultipartFormDataContent
            {
                { new StringContent(rotateAngle), "angle" }
            };

            for (var i = 0; i < imageStreams.Length; i++)
            {
                httpContent.Add(new StreamContent(imageStreams[i]), "file_" + (i + 1));
            }

            return await Solve("rotatecaptcha", 5, httpContent);
        }

        public async Task<TwoCaptchaResult> SolveFunCaptcha(string funCaptchaPublicKey, string pageUrl, bool noJavaScript = false)
        {
            return await Solve("funcaptcha", 10,
                new KeyValuePair<string, string>("publickey", funCaptchaPublicKey),
                new KeyValuePair<string, string>("pageurl", pageUrl),
                new KeyValuePair<string, string>("nojs", noJavaScript ? "1" : "0"));
        }

        public async Task<TwoCaptchaResult> SolveKeyCaptcha(string userId, string sessionId, string webServerSign, string webServerSign2, string pageUrl)
        {
            return await Solve("keycaptcha", 15,
                new KeyValuePair<string, string>("s_s_c_user_id", userId),
                new KeyValuePair<string, string>("s_s_c_session_id", sessionId),
                new KeyValuePair<string, string>("s_s_c_web_server_sign", webServerSign),
                new KeyValuePair<string, string>("s_s_c_web_server_sign2", webServerSign2),
                new KeyValuePair<string, string>("pageurl", pageUrl));
        }
    }
}
