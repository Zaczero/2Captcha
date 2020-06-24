using System;

namespace _2CaptchaAPI
{
	public enum FileType
	{
		Png,
		Jpg,
		Jpeg,
	}

	internal static class FileTypEx
	{
		internal static string GetExtension(this FileType fileType)
		{
			switch (fileType)
			{
				case FileType.Png:
					return "png";
				case FileType.Jpg:
					return "jpg";
				case FileType.Jpeg:
					return "jpeg";
				default:
					throw new ArgumentOutOfRangeException(nameof(fileType), fileType, "Unsupported file type");
			}
		}

		internal static string GetMime(this FileType fileType)
		{
			switch (fileType)
			{
				case FileType.Png:
					return "image/png";
				case FileType.Jpg:
				case FileType.Jpeg:
					return "image/jpeg";
				default:
					throw new ArgumentOutOfRangeException(nameof(fileType), fileType, "Unsupported file type");
			}
		}
	}
}
