namespace LiveLab3D.Visual.Filters
{
	using System;
	using Commands;
	using Events;
	using Events.Impl;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Objects;
	using System.Linq;
	[FilterKey(Key = "ObjectVisual")]
	public class GoodGuysBadGuysFilter : IDrawingFilter
	{
		private readonly IEventAggregator eventAggregator;
		private int[] goodGuyIds;
		private int[] badGuyIds; 
		public GoodGuysBadGuysFilter(IEventAggregator eventAggregator)
		{
			this.eventAggregator = eventAggregator;
			this.eventAggregator.Subscribe<CommandReceivedEvent<GoodGuysCommand>>(x => goodGuyIds = x.Command.GoodGuyIds);
			this.eventAggregator.Subscribe<CommandReceivedEvent<BadGuysCommand>>(x => badGuyIds = x.Command.BadGuyIds);
			this.goodGuyIds = new int[0];
			this.badGuyIds=new int[0];
		}

		#region IDrawingFilter Members

		public void Modify(Model model, object vehicle)
		{
			var item = (ObjectBase) vehicle;
			foreach (var mesh in model.Meshes)
			{
					foreach (BasicEffect effect in mesh.Effects)
					{
						if (goodGuyIds.Contains(item.Id))
							effect.DiffuseColor = new Vector3(0, 0, 1);
						if (badGuyIds.Contains(item.Id))
							effect.DiffuseColor = new Vector3(1, 0, 0);
					}
			}
		}

		#endregion

	}
}
