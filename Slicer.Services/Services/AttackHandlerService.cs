using Microsoft.Xna.Framework;
using Slicer.App.Interfaces;

namespace Slicer.App.Services;

internal class AttackHandlerService : IAttackHandlerService
{
	private int attackCooldown;

	private int attack;

	private Action currentAttackHitCallback = () => { };

	internal int AttackDuration { get; set;}

	internal int AttackCooldownDuration { get; set;}

	public bool IsAttacking => attack != 0;

    public bool CanAttack => attackCooldown == 0;

    public void Attack(Action attackCallback, Action attackHitCallback)
    {
		if (attackCooldown > 0)
		{
			return;
		}

		attackCallback();

		attack = AttackDuration;
		attackCooldown = AttackCooldownDuration;
		currentAttackHitCallback = attackHitCallback;
	}

	public void HandleAttackState(GameTime gameTime)
	{
		if (IsAttacking)
		{
			currentAttackHitCallback();
			attack = Math.Max(attack - gameTime.ElapsedGameTime.Milliseconds, 0);
		}

		if (!CanAttack)
		{
			attackCooldown = Math.Max(attackCooldown - gameTime.ElapsedGameTime.Milliseconds, 0);
		}
	}
}