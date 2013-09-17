namespace LiveLab3D.Helpers
{
	using Microsoft.Xna.Framework;

	public class PhysicsHelper
	{
		public static float DistanceBetweenPointAndLineDistance(Vector3 p1,Vector3 p2,Vector3 p3,out Vector3 intersection,out bool onTheLine)
		{
			float u = (p3.X - p1.X)*(p2.X - p1.X) +
			          (p3.Y - p1.Y)*(p2.Y - p1.Y) +
			          (p3.Z - p1.Z)*(p2.Z - p1.Z);
			u = u/(p2 - p1).LengthSquared();
			float x = p1.X + u*(p2.X - p1.X);
			float y = p1.Y + u*(p2.Y - p1.Y);
			float z = p1.Z + u*(p2.Z - p1.Z);
			onTheLine = u <= 1;
			intersection=new Vector3(x,y,z);
			return (intersection - p3).Length();
			
		}
	}
}
