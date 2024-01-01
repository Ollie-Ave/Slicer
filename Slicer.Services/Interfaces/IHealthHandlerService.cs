using Microsoft.Xna.Framework;

namespace Slicer.App.Interfaces;

public interface IHealthHandlerService
{
	bool IsAlive { get; }

	bool CanTakeDamage { get; }

	bool IsDying { get; }

	void TakeDamage(float damage);

	void HandleHealthState(GameTime gameTime);
}