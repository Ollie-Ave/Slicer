using Microsoft.Xna.Framework;

namespace Slicer.App.Interfaces;

public interface IEnemy
{
	Rectangle GetHitBox();

	void TakeDamage(float damage);
}