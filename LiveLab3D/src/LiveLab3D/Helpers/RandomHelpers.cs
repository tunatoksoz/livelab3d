using System;

namespace LiveLab3D.Helpers
{
	public static class RandomHelpers
	{
		private static Random random;
		static RandomHelpers()
		{
			random=new Random();
		}
		public static Random GetRandom()
		{
			return random;
		}

		public static float NextFloat()
		{
			return random.NextFloat();
		}
		public static float NextFloat(this Random random)
		{
			return random.NextDouble().ToFloat();
		}

		public static float NextGaussianFloat(this Random random)
		{
			return random.NextGaussianDouble().ToFloat();
		}
		public static float NextGaussianFloat()
		{
			return random.NextFloat();
		}


		public static double NextGaussianDouble(this Random random)
		{
			var u = 2*random.NextDouble() - 1;
			var v = 2 * random.NextDouble() - 1;
			var r = u*u + v*v;
			/*if outside interval [0,1] start over*/
			if (r == 0 || r > 1)
				return NextGaussianDouble(random);

			var c = Math.Sqrt(-2*Math.Log(r)/r);
			return u*c;

			/* todo: optimize this algorithm by caching (v*c) 
			 * and returning next time gaussRandom() is called.
			 * left out for simplicity */

		}
		public static double NextGaussianDouble()
		{
			return random.NextGaussianFloat();
		}
	}
}
