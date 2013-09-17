namespace LiveLab3D.Statistics.Visualization
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using TomShane.Neoforce.Controls;

	public class PerVehicleStatisticsWindow : Window
	{
		private readonly ComboBox comboBox;
		private readonly IObjectSource objectSource;
		private readonly CustomStackPanel stackPanel;
		private readonly IStatisticsVisualizationRegistry statisticsVisualizationRegistry;

		private IList<Pair<IPerVehicleStatisticsCollector, Control>> controls;

		public PerVehicleStatisticsWindow(Manager manager, IObjectSource objectSource,
		                                  IList<IPerVehicleStatisticsCollector> statisticsCollectors,
		                                  IStatisticsVisualizationRegistry statisticsVisualizationRegistry) : base(manager)
		{
			this.objectSource = objectSource;
			statisticsCollectors.ToArray();
			this.statisticsVisualizationRegistry = statisticsVisualizationRegistry;
			this.comboBox = new ComboBox(manager);

			this.comboBox.Parent = this;
			this.comboBox.Left = 5;
			this.comboBox.Top = 10;
			this.comboBox.Width = 200;
			this.comboBox.Enabled = true;
			this.comboBox.Init();
			this.comboBox.TextColor = Color.White;
			this.comboBox.Color = Color.White;
			this.stackPanel = new CustomStackPanel(Manager, Orientation.Horizontal);
			this.stackPanel.Parent = this;
			this.stackPanel.Init();
			this.stackPanel.Left = 5;
			this.stackPanel.Height = 200;
			this.stackPanel.Width = this.comboBox.Width;
			this.stackPanel.Top = this.comboBox.Top + this.comboBox.Height + this.comboBox.Top;
			Alpha = 200;
			Text = "Per vehicle statistics";
			ClientWidth = this.comboBox.Width + this.comboBox.Left*2;
			ClientHeight = this.comboBox.Height + this.comboBox.Top*2 + 200;


			IEnumerable<ObjectBase> allobjects = objectSource.GetObjects().ToArray().OrderBy(x => x.Name);

			foreach (ObjectBase vehicle in allobjects)
				this.comboBox.Items.Add(new ComboBoxItem {Vehicle = vehicle});

			this.controls = new List<Pair<IPerVehicleStatisticsCollector, Control>>();
			foreach (IPerVehicleStatisticsCollector perVehicleStatisticsCollector in statisticsCollectors)
			{
				Type statisticsType =
					perVehicleStatisticsCollector.GetType().GetMethods().Where(x => x.Name == "GetStatisticsForVehicle").Select(
						x => x.ReturnType).Where(x => x != typeof (IStatistics)).FirstOrDefault();
				string statisticsText;
				StatisticsDetail detailAttribute =
					statisticsType.GetCustomAttributes(typeof (StatisticsDetail), true).Cast<StatisticsDetail>().FirstOrDefault();
				if (detailAttribute != null)
					statisticsText = detailAttribute.Name;
				else
					statisticsText = statisticsType.Name;


				var panel = new Panel(Manager);
				panel.Parent = this.stackPanel;


				Control ctrl = statisticsVisualizationRegistry.GetControl(statisticsType);
				ctrl.Width = 80;
				ctrl.Height = 60;
				ctrl.Parent = panel;
				ctrl.Init();


				var label = new Label(Manager);
				label.Text = statisticsText;
				label.Top = ctrl.Height;
				label.Parent = panel;
				label.Init();
				label.Width = ctrl.Width;

				panel.ClientHeight = label.Top + label.Height + ctrl.Top;
				panel.Width = ctrl.Width;
				this.stackPanel.Add(panel);

				var controlItem = new Pair<IPerVehicleStatisticsCollector, Control>(perVehicleStatisticsCollector, ctrl);
				this.controls.Add(controlItem);
			}
			this.stackPanel.Refresh();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (!string.IsNullOrEmpty(this.comboBox.Text))
			{
				ObjectBase selectedVehicle = this.objectSource.GetObject(this.comboBox.Text);
				UpdateStatistics(selectedVehicle);
			}
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			if (this.stackPanel == null)
				return;
			this.stackPanel.Width = ClientWidth - (this.comboBox.Left*3);
			this.stackPanel.Height = ClientHeight - ((this.comboBox.Top + this.comboBox.Height + this.comboBox.Top) + 10);
		}

		protected void UpdateStatistics(ObjectBase vehicle)
		{
			foreach (var controlItem in this.controls)
			{
				IPerVehicleStatistics statistics = controlItem.Item1.GetStatisticsForVehicle(vehicle);
				this.statisticsVisualizationRegistry.BindValue(statistics, controlItem.Item2);
			}
		}

		#region Nested type: ComboBoxItem

		private class ComboBoxItem
		{
			public ObjectBase Vehicle { get; set; }

			public override string ToString()
			{
				return Vehicle.Name;
			}
		}

		#endregion
	}
}