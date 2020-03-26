namespace _2Captcha
{
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
}
