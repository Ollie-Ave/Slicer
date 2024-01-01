using Microsoft.Xna.Framework;

namespace Slicer.App.Interfaces;

public interface IAttackHandlerService
{
	bool CanAttack { get; }

	bool IsAttacking { get; }

	void Attack(Action attackCallback, Action attackHitCallback);

	public void HandleAttackState(GameTime gameTime);
}