using Microsoft.Xna.Framework;
using Slicer.App.Interfaces;

namespace Slicer.App.Services;

internal class HealthHandlerService : IHealthHandlerService
{
	private float health;

	private int damageCooldownDuration;

	private int damageCooldown;

	private Action deathCallback;


	public HealthHandlerService(float initialHealth, int damageCooldownDuration, Action deathCallback)
	{
		this.damageCooldownDuration =  damageCooldownDuration;
		this.deathCallback = deathCallback;
		health =  initialHealth;
	}

	public bool IsDying { get; private set; }

	public bool IsAlive => health > 0;

	public bool CanTakeDamage => damageCooldown == 0;

	public void TakeDamage(float damage)
	{
		if (CanTakeDamage)
		{
			health -= damage;

			damageCooldown = damageCooldownDuration;
		}
	}

	public void HandleHealthState(GameTime gameTime)
	{
		if (damageCooldown > 0)
		{
			damageCooldown = Math.Max(damageCooldown - gameTime.ElapsedGameTime.Milliseconds, 0);
		}

		if (!IsAlive && CanTakeDamage && !IsDying)
		{
			TakeDamage(0);
			IsDying = true;
		}

		if (!IsAlive && CanTakeDamage && IsDying)
		{
			deathCallback();
		}
	}
}