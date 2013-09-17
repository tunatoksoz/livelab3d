namespace LiveLab3D.Parsers
{
	public interface IParser
	{
		object Parse(string line);
	}

	public interface IParser<T>
	{
		T Parse(string line);
	}
}