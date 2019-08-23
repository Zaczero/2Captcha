namespace _2Captcha
{
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
}
