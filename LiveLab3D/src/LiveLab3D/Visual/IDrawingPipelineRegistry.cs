namespace LiveLab3D.Visual
{
	public interface IDrawingPipelineRegistry
	{
		IDrawingPipeline GetPipeline(string key);
	}
}