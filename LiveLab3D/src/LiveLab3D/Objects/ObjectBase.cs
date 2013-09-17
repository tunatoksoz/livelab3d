namespace LiveLab3D.Objects
{
	using LiveLab3D.Commands;
	using Microsoft.Xna.Framework;

	public delegate void CommandChangedEventHandler(ObjectBase obj, VehicleCommandBase command);

	public abstract class ObjectBase
	{
		protected ObjectBase()
		{
			PositionalData = new MotionalData();
		}

		public int Id { get; set; }
		public string Name { get; set; }


		public MotionalData PositionalData { get; set; }

		public Matrix RotationMatrix
		{
			get
			{
				Vector3 x = Vector3.UnitX;
				Vector3 y = Vector3.UnitY;
				Vector3 z = Vector3.UnitZ;

				float yaw = PositionalData.Yaw;
				float roll = PositionalData.Roll;
				float pitch = PositionalData.Pitch;

				x = Vector3.Transform(x, Matrix.CreateFromAxisAngle(z, -yaw));
				y = Vector3.Transform(y, Matrix.CreateFromAxisAngle(z, -yaw));

				y = Vector3.Transform(y, Matrix.CreateFromAxisAngle(x, pitch));
				z = Vector3.Transform(z, Matrix.CreateFromAxisAngle(x, pitch));


				z = Vector3.Transform(z, Matrix.CreateFromAxisAngle(y, roll));
				x = Vector3.Transform(x, Matrix.CreateFromAxisAngle(y, roll));
				return Matrix.CreateWorld(new Vector3(0, 0, 0), y, z);
			}
		}

		public Matrix ThreeDPositionMatrix
		{
			get { return RotationMatrix*Matrix.CreateTranslation(PositionalData.Position); }
		}


		public override string ToString()
		{
			return string.Format("{0} - {1}", Id, Name);
		}
	}
}