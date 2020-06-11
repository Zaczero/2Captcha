using System;

namespace _2Captcha
{
	public enum ProxyType
	{
		Http,
		Https,
		Socks4,
		Socks5,
	}

	internal static class ProxyTypeEX
	{
		internal static string GetExtension(this ProxyType proxyType)
		{
			switch (proxyType)
			{
				case ProxyType.Http:
					return "HTTP";
				case ProxyType.Https:
					return "Https";
				case ProxyType.Socks4:
					return "SOCKS4";
				case ProxyType.Socks5:
					return "SOCKS5";
				default:
					throw new ArgumentOutOfRangeException(nameof(proxyType), proxyType, "Unsupported file type");
			}
		}

	}
	
}
