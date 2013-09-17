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

	public class PerStatisticsStatisticsWindow : Window
	{
		private readonly ComboBox comboBox;
		private readonly IObjectSource objectSource;
		private readonly IStatisticsVisualizationRegistry statisticsVisualizationRegistry;
		private IList<Pair<ObjectBase, Control>> controls;
		private string lastVehicleName = "";
		private CustomStackPanel stackPanel;

		public PerStatisticsStatisticsWindow(Manager manager, IObjectSource objectSource,
		                                     IList<IPerVehicleStatisticsCollector> statisticsCollectors,
		                                     IStatisticsVisualizationRegistry statisticsVisualizationRegistry)
			: base(manager)
		{
			this.objectSource = objectSource;
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
			this.controls = new List<Pair<ObjectBase, Control>>();

			this.stackPanel = new CustomStackPanel(Manager, Orientation.Horizontal);
			this.stackPanel.Parent = this;
			this.stackPanel.Init();
			this.stackPanel.Left = 5;
			this.stackPanel.Height = 200;
			this.stackPanel.Width = this.comboBox.Width + 10;
			this.stackPanel.Top = this.comboBox.Top + this.comboBox.Top + this.comboBox.Height;

			BackColor = Color.Red;

			foreach (IPerVehicleStatisticsCollector perVehicleStatisticsCollector in statisticsCollectors)
			{
				Type statisticsType =
					perVehicleStatisticsCollector.GetType().GetMethods()
						.Where(x => x.Name == "GetStatisticsForVehicle")
						.Select(x => x.ReturnType)
						.Where(x => x != typeof (IStatistics))
						.FirstOrDefault();

				string statisticsText;
				StatisticsDetail detailAttribute =
					statisticsType.GetCustomAttributes(typeof (StatisticsDetail), true).Cast<StatisticsDetail>().FirstOrDefault();
				if (detailAttribute != null)
					statisticsText = detailAttribute.Name;
				else
					statisticsText = statisticsType.Name;
				var comboItem = new ComboBoxItem
				                	{
				                		Name = statisticsText,
				                		StatisticsCollector = perVehicleStatisticsCollector,
				                		StatisticsType = statisticsType,
				                	};
				this.comboBox.Items.Add(comboItem);
			}


			Refresh();
			Alpha = 200;
			Text = "Per statistics";
			ClientWidth = this.comboBox.Width + this.comboBox.Left*2;
			ClientHeight = this.comboBox.Height + this.comboBox.Top*2 + 200;
		}


		private void ComboBoxSelectionChanged()
		{
			ComboBoxItem selectedStatistics = this.comboBox.Items.Cast<ComboBoxItem>()
				.Where(x => x.Name.Equals(this.comboBox.Text)).FirstOrDefault();
			ObjectBase[] objects = this.objectSource.GetObjects().ToArray().OrderBy(x => x.Name).ToArray();
			Type statisticsType = selectedStatistics.StatisticsType;
			this.controls.Clear();
			((ControlsList) this.stackPanel.Controls).Clear();
			((ControlsList) this.stackPanel.ClientArea.Controls).Clear();
			foreach (ObjectBase o in objects)
			{
				var panel = new Panel(Manager);
				panel.Parent = this.stackPanel;


				Control ctrl = this.statisticsVisualizationRegistry.GetControl(statisticsType);
				ctrl.Width = 80;
				ctrl.Height = 60;
				ctrl.Parent = panel;
				ctrl.Init();


				var label = new Label(Manager);
				label.Text = o.Name;
				label.Top = ctrl.Height;
				label.Parent = panel;
				label.Init();
				label.Width = ctrl.Width;

				panel.ClientHeight = label.Top + label.Height + ctrl.Top;
				panel.Width = ctrl.Width;
				((ControlsList) this.stackPanel.Controls).Add(panel);

				var controlItem = new Pair<ObjectBase, Control>(o, ctrl);
				this.controls.Add(controlItem);
			}
			this.stackPanel.Refresh();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			string selectedVehicleName = this.comboBox.Text;
			if (!string.IsNullOrEmpty(selectedVehicleName) && !this.lastVehicleName.Equals(selectedVehicleName))
			{
				ComboBoxSelectionChanged();
				this.lastVehicleName = selectedVehicleName;
			}
			UpdateStatistics();
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			if (this.stackPanel == null)
				return;
			this.stackPanel.Width = ClientWidth - (this.comboBox.Left*3);
			this.stackPanel.Height = ClientHeight - ((this.comboBox.Top + this.comboBox.Height + this.comboBox.Top) + 10);
		}

		protected void UpdateStatistics()
		{
			if (string.IsNullOrEmpty(this.comboBox.Text))
				return;
			ComboBoxItem selectedStatistics =
				this.comboBox.Items.Cast<ComboBoxItem>().Where(x => x.Name.Equals(this.comboBox.Text)).FirstOrDefault();
			IPerVehicleStatisticsCollector collector = selectedStatistics.StatisticsCollector;
			foreach (var control in this.controls)
			{
				IPerVehicleStatistics statistics = collector.GetStatisticsForVehicle(control.Item1);
				Control ctrl = control.Item2;
				this.statisticsVisualizationRegistry.BindValue(statistics, ctrl);
			}
		}

		#region Nested type: ComboBoxItem

		private class ComboBoxItem
		{
			public IPerVehicleStatisticsCollector StatisticsCollector { get; set; }
			public Type StatisticsType { get; set; }
			public string Name { get; set; }

			public override string ToString()
			{
				return Name;
			}
		}

		#endregion
	}
}