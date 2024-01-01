using Slicer.App.Interfaces;
using Slicer.App.Services;

namespace Slicer.App.Builders;

internal class HealthHandlerServiceBuilder : IHealthHandlerServiceBuilder
{
	public IHealthHandlerService Build(float initialHealth, int damageCooldownDuration, Action deathCallback)
	{
		return new HealthHandlerService(initialHealth, damageCooldownDuration, deathCallback);
	}
}