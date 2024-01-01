namespace Slicer.App.Interfaces
{
	public interface IHealthHandlerServiceBuilder
	{
		IHealthHandlerService Build(float initialHealth, int damageCooldownDuration, Action deathCallback);
	}
}