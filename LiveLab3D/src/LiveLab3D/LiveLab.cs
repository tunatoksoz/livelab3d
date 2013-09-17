using LiveLab3D.Screens.CameraFeed;

namespace LiveLab3D
{
	using System;
	using System.Linq;
	using Castle.Windsor;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Screens;
	using LiveLab3D.Statistics.Visualization;
	using LiveLab3D.Visual.Components.ConsoleModal;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;
	using TomShane.Neoforce.Controls;
	using Component = Castle.MicroKernel.Registration.Component;
	using EventArgs = System.EventArgs;

	public delegate void GameUpdated(TimeSpan gameTime);

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class LiveLab : Game
	{
		private readonly IWindsorContainer container;
		private readonly GraphicsDeviceManager graphics;
		private readonly Manager gui;
		private readonly IModelSource modelSource;
		private CommandBoxComponent commandBox;
		private ConsoleModal console;
		private IEnvironment environment;

		public LiveLab(IWindsorContainer container)
		{
			this.container = container;
			this.graphics = new GraphicsDeviceManager(this);

			Content.RootDirectory = "Content";
			this.gui = new Manager(this, this.graphics);
			container.Register(Component.For<Manager>().Instance(this.gui));
			this.gui.DrawOrder = 4123;
			this.gui.Input.KeyDown += (x, y) => ShowConsole(y);
			this.modelSource = new SimpleModelSource(container);
			IsMouseVisible = true;
			IsFixedTimeStep = true;
		}

		private void ShowConsole(KeyEventArgs args)
		{
			if (args.Key == Keys.OemTilde)
			{
				if (this.console.Visible)
					this.console.Hide();
				else
					this.console.Show();
			}
		}

		protected override void BeginRun()
		{
			Mouse.SetPosition(this.graphics.GraphicsDevice.Viewport.Width/2, this.graphics.GraphicsDevice.Viewport.Height/2);
		}

		protected override void Initialize()
		{
			this.environment = this.container.Resolve<IEnvironment>();

			this.graphics.PreferredBackBufferWidth = this.environment.Width;
			this.graphics.PreferredBackBufferHeight = this.environment.Height;
			//graphics.IsFullScreen = true;
			this.graphics.ApplyChanges();


			var components = this.container.ResolveAll<DrawableGameComponent>();
			components = components.Where(x =>! (x is Manager)).ToArray();
			var visualizationComponent = new VisualizationComponent(this, this.environment, this.modelSource);
			Components.Add(visualizationComponent);
			foreach (var drawableGameComponent in components)
			{
				Components.Add(drawableGameComponent);
			}

			base.Initialize();
		}

		protected override void LoadContent()
		{
			this.console = this.container.Resolve<ConsoleModal>();

			this.console.Init();
			this.gui.Add(this.console);
			this.commandBox = this.container.Resolve<CommandBoxComponent>();
			this.commandBox.Left = this.gui.ScreenWidth - this.commandBox.Width;
			this.commandBox.Init();
			this.gui.Add(this.commandBox);

			int height = (this.gui.ScreenHeight - this.commandBox.Height)/2;

			var cameraFeeds = this.container.Resolve<CameraFeedModal>();
			cameraFeeds.Init();


			var perVehicleStatistics = this.container.Resolve<PerVehicleStatisticsWindow>();
			perVehicleStatistics.Left = this.gui.ScreenWidth - this.commandBox.Width;
			perVehicleStatistics.Top = this.commandBox.Height;
			perVehicleStatistics.Width = this.commandBox.Width;
			perVehicleStatistics.Height = height;
			perVehicleStatistics.Init();


			var perStatisticsStatistics = this.container.Resolve<PerStatisticsStatisticsWindow>();
			perStatisticsStatistics.Left = this.gui.ScreenWidth - this.commandBox.Width;
			perStatisticsStatistics.Top = perVehicleStatistics.Top + perVehicleStatistics.Height;
			perStatisticsStatistics.Width = this.commandBox.Width;
			perStatisticsStatistics.Height = height;
			perStatisticsStatistics.Init();


			this.gui.Add(perVehicleStatistics);
			this.gui.Add(perStatisticsStatistics);
			this.gui.Add(cameraFeeds);
			base.LoadContent();
		}

		public event GameUpdated GameUpdated = delegate { };

		protected override void Update(GameTime gameTime)
		{
			this.environment.Camera.UpdateCamera(gameTime);
			base.Update(gameTime);
			GameUpdated(gameTime.ElapsedGameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			base.Draw(gameTime);
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			Environment.Exit(0);
		}
	}
}