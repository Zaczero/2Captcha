using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace _2CaptchaAPI
{
	public partial class _2Captcha
	{
		[Serializable]
		private struct ResultInternal
		{
			[JsonProperty("status")]
			public int Status;

			[JsonProperty("request")]
			public JToken Request;
		}

		public struct Result
		{
			public readonly bool Success;
			public readonly JToken ResponseObject;

			public Result(bool success, JToken responseObject)
			{
				Success = success;
				ResponseObject = responseObject;

				_responseJson = null;
				_response = null;
				_responseDouble = null;
				_responseCoordinates = null;
			}

			private string _responseJson;
			public string ResponseJson
			{
				get
				{
					if (_responseJson == null)
						_responseJson = ResponseObject.ToString(Formatting.None);

					return _responseJson;
				}
			}

			private string _response;
			public string Response
			{
				get
				{
					if (_response == null)
						_response = ResponseObject.ToObject<string>();

					return _response;
				}
			}

			private double? _responseDouble;
			public double ResponseDouble
			{
				get
				{
					if (_responseDouble == null)
						_responseDouble = ResponseObject.ToObject<double>();

					return _responseDouble.Value;
				}
			}

			private Coordinates[] _responseCoordinates;
			public Coordinates[] ResponseCoordinates
			{
				get
				{
					if (_responseCoordinates == null)
						_responseCoordinates = ResponseObject.ToObject<Coordinates[]>();

					return _responseCoordinates;
				}
			}
		}

		[Serializable]
		public readonly struct Coordinates
		{
			[JsonProperty("x")]
			public readonly int X;

			[JsonProperty("y")]
			public readonly int Y;
		}
	}
}
