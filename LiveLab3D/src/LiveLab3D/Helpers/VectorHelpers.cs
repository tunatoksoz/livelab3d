namespace LiveLab3D.Helpers
{
	using System;
	using Microsoft.Xna.Framework;

	public static class VectorHelpers
	{
		public static bool Equals2(this Vector2 vector,Vector2 vector2)
		{
			return (vector2 - vector).Length() < 0.0001;
		}
		public static Vector2 GetXYProjection(this Vector3 vector)
		{
			return new Vector2(vector.X,vector.Y);
		}
		public static float Get(this Vector3 vector, byte dim)
		{
			if (dim == 0)
				return vector.X;
			else if (dim == 1)
				return vector.Y;
			else if (dim == 2)
				return vector.Z;
			else
				throw new IndexOutOfRangeException();
		}
		public static float Get(this Vector2 vector, byte dim)
		{
			if (dim == 0)
				return vector.X;
			else if (dim == 1)
				return vector.Y;
			else
				throw new IndexOutOfRangeException();
		}

		public static Vector3 Set(this Vector3 vector, byte dim,float value)
		{
			if (dim == 0)
				vector.X = value;
			else if (dim == 1)
				vector.Y = value;
			else if (dim == 2)
				vector.Z = value;
			else
				throw new IndexOutOfRangeException();
			return vector;
		}
		public static Vector2 Set(this Vector2 vector, byte dim, float value)
		{
			if (dim == 0)
				vector.X = value;
			else if (dim == 1)
				vector.Y = value;
			else
				throw new IndexOutOfRangeException();
			return vector;
		}

		public static Vector2 SlopeUnit(this Vector2 vector)
		{
			if (Math.Abs(vector.X) > 1e-6f)
				return vector/vector.X;
			else
			{
				try
				{
					return new Vector2(1.0f, Math.Sign(vector.Y)*1e5f);
				}
				catch
				{
					return new Vector2(0.0f, 0.0f);
				}
			}
		}

		public static Vector2 Unit(this Vector2 vector)
		{
			if (vector.Length() > float.Epsilon)
				return vector/vector.Length();
			return Vector2.Zero;
		}
	}
}
