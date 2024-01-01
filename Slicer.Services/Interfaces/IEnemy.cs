using Microsoft.Xna.Framework;

namespace Slicer.App.Interfaces;

public interface IEnemy
{
	Rectangle GetHitBox();

	void TakeDamage(float damage);

    void SetXPosition(int position);
}