namespace LiveLab3D.ObjectSources
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using LiveLab3D.Objects;
	using LiveLab3D.Streams;
	using Microsoft.Xna.Framework;

	public class ViconObjectSource : IObjectSource
	{
		private static readonly Regex regex =
			new Regex(
				@"(?<name>\w+) (?<id>\d+),(?<x>(\+|\-)\d+\.\d+),(?<y>(\+|\-)\d+\.\d+),(?<z>(\+|\-)\d+\.\d+),(?<roll>(\+|\-)\d+\.\d+),(?<pitch>(\+|\-)\d+\.\d+),(?<yaw>(\+|\-)\d+\.\d+),(?<vx>(\+|\-)\d+\.\d+),(?<vy>(\+|\-)\d+\.\d+),(?<vz>(\+|\-)\d+\.\d+)",
				RegexOptions.Compiled);

		private readonly object lockObject = new object();

		private readonly IObjectBuilder objectBuilder;
		private readonly IDictionary<int, ObjectBase> objectsById;
		private readonly IDictionary<string, ObjectBase> objectsByName;
		private readonly IUdpListener positionListener;


		public ViconObjectSource(IUdpListener positionListener,
		                         IObjectBuilder objectBuilder)
		{
			this.objectsById = new Dictionary<int, ObjectBase>();
			this.objectsByName = new Dictionary<string, ObjectBase>();
			this.positionListener = positionListener;
			this.objectBuilder = objectBuilder;
			this.positionListener = positionListener;
			this.positionListener.PacketReceived += UpdateObjects;
		}

		#region IObjectSource Members

		public IEnumerable<ObjectBase> GetObjects()
		{
			return this.objectsByName.Values;
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

		protected void UpdateObjects(string input)
		{
			//TODO: USE REGEX
			//currentTime = long.Parse(input.Substring(0, input.IndexOf(";")));
			input = input.Substring(input.IndexOf(";") + 1);
			string fullString = input;
			string[] pieces = fullString.Split(';');
			for (int i = 0; i < pieces.Length - 1; i++)
			{
				string str = pieces[i];
				Match match = regex.Match(str);

				string name = match.Groups["name"].Value;
                int id;
                bool result=Int32.TryParse(match.Groups["id"].Value,out id);
                if (result == false)
                    continue;
				float x = float.Parse(match.Groups["x"].Value);
				float y = float.Parse(match.Groups["y"].Value);
				float z = float.Parse(match.Groups["z"].Value);
				float yaw = float.Parse(match.Groups["yaw"].Value);
				float pitch = float.Parse(match.Groups["pitch"].Value);
				float roll = float.Parse(match.Groups["roll"].Value);

				float vx = float.Parse(match.Groups["vx"].Value);
				float vy = float.Parse(match.Groups["vy"].Value);
				float vz = float.Parse(match.Groups["vz"].Value);

				var motionalData = new MotionalData();
				var position = new Vector3(x, y, z);
				motionalData.Position = position;
				motionalData.Yaw = yaw;
				motionalData.Pitch = pitch;
				motionalData.Roll = roll;
				motionalData.Velocity = new Vector3(vx, vy, vz);
				if (!this.objectsById.ContainsKey(id))
				{
					ObjectBase o = this.objectBuilder.BuildObject(name);
					o.Id = id;
					o.Name = name;
					lock (this.lockObject)
					{
						this.objectsById[id] = o;
						this.objectsByName[name] = o;
					}
				}
				ObjectBase currentObject = this.objectsById[id];
				currentObject.PositionalData = motionalData;
			}
		}
	}
}