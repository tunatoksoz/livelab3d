namespace LiveLab3D.Windsor
{
	public interface IModeInstaller<TModeSetting> where TModeSetting : IModeSetting
	{
		void Register(TModeSetting modeSetting);
	}
}