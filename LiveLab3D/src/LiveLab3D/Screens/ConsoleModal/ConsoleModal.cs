namespace LiveLab3D.Visual.Components.ConsoleModal
{
	using System;
	using System.Collections.Generic;
	using LiveLab3D.Screens.ConsoleModal.Commands;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;
	using TomShane.Neoforce.Controls;
	using EventArgs = TomShane.Neoforce.Controls.EventArgs;

	public class ConsoleModal : Window
	{
		private const int LargeSeperation = 10;
		private readonly IConsoleCommandInterpreterRegistry commandParserRegistry;
		private readonly TextBox commandTextBox;
		private readonly Label oldCommandsTextBox;

		public ConsoleModal(Manager manager, IConsoleCommandInterpreterRegistry commandParserRegistry)
			: base(manager)
		{
			Text = "Command Console";
			BackColor = Color.Black;
			this.commandParserRegistry = commandParserRegistry;
			this.oldCommandsTextBox = new Label(manager);
			this.oldCommandsTextBox.Enabled = false;
			this.oldCommandsTextBox.Left = LargeSeperation;
			this.oldCommandsTextBox.Top = LargeSeperation;
			this.oldCommandsTextBox.Height = 250;
			this.oldCommandsTextBox.Parent = this;
			this.oldCommandsTextBox.Text = "";
			this.oldCommandsTextBox.Alignment = Alignment.TopLeft;
			this.oldCommandsTextBox.Anchor = Anchors.Left | Anchors.Right | Anchors.Top;
			this.oldCommandsTextBox.Width = ClientWidth - 2*LargeSeperation;
			Name = "ConsoleModal";

			this.oldCommandsTextBox.Init();

			this.commandTextBox = new TextBox(manager);
			this.commandTextBox.Anchor = Anchors.Left | Anchors.Right | Anchors.Bottom;
			this.commandTextBox.Left = LargeSeperation;
			this.commandTextBox.Top = this.oldCommandsTextBox.Height + LargeSeperation + LargeSeperation;
			this.commandTextBox.Width = this.oldCommandsTextBox.Width;
			this.commandTextBox.KeyDown += new KeyEventHandler(commandTextBox_KeyDown);
			this.commandTextBox.Parent = this;
			this.commandTextBox.Text = "";

			this.commandTextBox.Init();
			ClientWidth = this.oldCommandsTextBox.Width + LargeSeperation + LargeSeperation;
			ClientHeight = this.oldCommandsTextBox.Height + this.commandTextBox.Height + 3*LargeSeperation;
			Center();
			Visible = false;
			VisibleChanged += ConsoleModal_VisibleChanged;
		}

		private void ConsoleModal_VisibleChanged(object sender, EventArgs e)
		{
			this.commandTextBox.Focused = true;
		}


		private void commandTextBox_KeyDown(object sender, KeyEventArgs args)
		{
			switch (args.Key)
			{
				case Keys.Enter:
					HandleEnter();
					break;
				case Keys.Escape:
					HandleEscape();
					break;
				default:
					break;
			}
		}

		protected virtual void HandleEnter()
		{
			string command = this.commandTextBox.Text;
			IEnumerable<IConsoleCommandInterpreter> interpreters = this.commandParserRegistry.GetInterpreters();
			foreach (IConsoleCommandInterpreter interpreter in interpreters)
			{
				if (interpreter.CanInterpret(command))
				{
					interpreter.Interpret(command);
					this.oldCommandsTextBox.Text += ">>" + command + "\n";
					this.commandTextBox.Text = String.Empty;
					return;
				}
			}
		}

		protected virtual void HandleEscape()
		{
			Close();
			//Game.Components.Remove(this);
		}
	}
}