namespace LiveLab3D.Visual
{
	using System.Linq;

	public class DrawingPipelineRegistry : IDrawingPipelineRegistry
	{
		private readonly IDrawingPipeline[] pipelines;

		public DrawingPipelineRegistry(IDrawingPipeline[] pipelines)
		{
			this.pipelines = pipelines;
		}

		#region IDrawingPipelineRegistry Members

		public IDrawingPipeline GetPipeline(string key)
		{
			return this.pipelines.Where(x => x.Key.Equals(key)).FirstOrDefault();
		}

		#endregion
	}
}