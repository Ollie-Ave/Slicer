using Microsoft.Xna.Framework;

namespace Slicer.App.Interfaces;

public interface IEnemy
{
	Rectangle GetHitbox();

	void TakeDamage(float damage);

	void SetXPosition(int position);
}