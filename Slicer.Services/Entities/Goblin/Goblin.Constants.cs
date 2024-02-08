using Microsoft.Xna.Framework;

namespace Slicer.App.Entities;

public partial class Goblin
{
	private const int SpriteScaling = 2;

	private const int HurtAnimationCooldownDuration = 310;

	private const float InitialHealth = 20f;

    private const float WalkSpeed = 5;

	private static Rectangle HitBoxDimensions = new(110, 100, 75, 100);
}
