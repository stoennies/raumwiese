using System;
using System.IO;

#if __MonoCS__
namespace System.Windows.Media.Imaging
{
	public enum BitmapCacheOption {
		OnLoad
	}

	public class BitmapImage {
		public Stream StreamSource;
		public BitmapCacheOption CacheOption;

		public void BeginInit() {
		}
		public void EndInit() {
		}
	}
}
#endif
