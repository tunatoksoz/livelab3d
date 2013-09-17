namespace LiveLab3D.Screens
{
	using System.Collections.Generic;
	using System.Linq;
	using LiveLab3D.Commands;
	using LiveLab3D.Events;
	using LiveLab3D.Events.Impl;
	using LiveLab3D.Visual.CommandTextifiers;
	using TomShane.Neoforce.Controls;

	public class CommandBoxComponent : Window
	{
		private readonly ICommandTextifierRegistry commandTextifierRegistry;
		private readonly Label label;
		private readonly List<string> statuses;
		private int maxNumberOfVisibleCommands = 20;

		public CommandBoxComponent(Manager manager, ICommandTextifierRegistry commandTextifierRegistry,
		                           IEventAggregator eventAggregator,
		                           int width, int height)
			: base(manager)
		{
			Alpha = 180;
			Text = "Activity History";
			this.commandTextifierRegistry = commandTextifierRegistry;
			this.statuses = new List<string>();
			eventAggregator.Subscribe<CommandReceivedEvent>(HandleCommand);
			Width = width;
			Height = height;
			this.label = new Label(manager);
			this.label.Top = 5;
			this.label.Left = 5;
			this.label.Text = "";
			this.label.Parent = this;
			this.label.Height = ClientHeight - 2*this.label.Top;
			this.label.Width = ClientWidth - 2*this.label.Left;
			this.label.ResizerSize = 3;
			this.label.Name = "RavenCommandHistory";
			this.label.Init();
			Top = 0;
			Anchor = Anchors.Top;
			this.label.Alignment = Alignment.TopLeft;
		}


		protected void AddStatusString(string statusString)
		{
			if (!string.IsNullOrEmpty(statusString))
			{
				this.statuses.Insert(0, statusString);
				if (this.statuses.Count > this.maxNumberOfVisibleCommands)
					this.statuses.RemoveAt(this.maxNumberOfVisibleCommands);
				this.label.Text = this.statuses.Aggregate("", (x, y) => x + y + "\n");
			}
		}

		public void HandleCommand(CommandReceivedEvent commandReceivedEvent)
		{
			ICommandTextifier textifier =
				this.commandTextifierRegistry.GetTextifierForCommand(commandReceivedEvent.Command.GetType());
			string statusString = textifier.ToText(commandReceivedEvent.Command, commandReceivedEvent.Destination!=null?commandReceivedEvent.Destination.Name:"");
			AddStatusString(statusString);
		}
	}
}