namespace LiveLab3D.Helpers
{
	public static class FormatHelper
	{
		public static string ToFormattedFloat(this float f)
		{
			return (f >= 0 ? "+" : "") + string.Format("{0:0.00000}", f);
		}

		public static string ToFormattedFloat(this int f)
		{
			return ((float) f).ToFormattedFloat();
		}
	}
}