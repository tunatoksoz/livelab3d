namespace LiveLab3D.ObjectSources
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using LiveLab3D.Objects;
	using LiveLab3D.Streams;
	using Microsoft.Xna.Framework;

	public class BlackboxObjectSource : IObjectSource
	{
		private readonly IObjectBuilder objectBuilder;
		private readonly Dictionary<int, ObjectBase> objectsById;
		private readonly Dictionary<string, ObjectBase> objectsByName;
		private readonly IUdpListener udpListener;

		public BlackboxObjectSource(IUdpListener positionListener, IObjectBuilder objectBuilder)
		{
			this.udpListener = positionListener;
			this.objectBuilder = objectBuilder;
			this.objectsById = new Dictionary<int, ObjectBase>();
			this.objectsByName = new Dictionary<string, ObjectBase>();
			this.objectBuilder = objectBuilder;
			this.udpListener.PacketReceived += UpdateObjects;
		}

		#region IObjectSource Members

		public IEnumerable<ObjectBase> GetObjects()
		{
			return this.objectsByName.Values.Cast<ObjectBase>();
		}

		public ObjectBase GetObject(int id)
		{
			if (!this.objectsById.ContainsKey(id))
				return null;
			return this.objectsById[id];
		}

		public ObjectBase GetObject(string name)
		{
			if (!this.objectsByName.ContainsKey(name))
				return null;
			return this.objectsByName[name];
		}

		#endregion

		public event CommandReceivedHandler CommandReceived = delegate { };

		protected void UpdateObjects(string input)
		{
			input = input.Substring(input.IndexOf(" ") + 1);
			string[] parts = input.Split(' ');

			string name = parts[1];
			int id = Convert.ToInt32(parts[0]);
			float x = float.Parse(parts[2]);
			float y = float.Parse(parts[3]);
			float z = float.Parse(parts[4]);
			float yaw = float.Parse(parts[7]);
			float pitch = float.Parse(parts[6]);
			float roll = float.Parse(parts[5]);

			ObjectBase item;

			var motionalData = new MotionalData();

			var position = new Vector3(x, y, z);
			motionalData.Position = position;
			motionalData.Yaw = -yaw;
			motionalData.Pitch = pitch;
			motionalData.Roll = roll;
			if (!this.objectsById.ContainsKey(id))
			{
				ObjectBase o = this.objectBuilder.BuildObject(name);
				o.Id = id;
				o.Name = name;
				o.PositionalData = motionalData;
				this.objectsById[id] = o;
				this.objectsByName[name] = o;
			}
			else
				this.objectsById[id].PositionalData = motionalData;
		}
	}
}