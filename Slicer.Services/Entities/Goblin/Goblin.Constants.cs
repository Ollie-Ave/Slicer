using Microsoft.Xna.Framework;

namespace Slicer.App.Entities;

public partial class GoblinNew
{
	private const int SpriteScaling = 2;

	private const int HurtAnimationCooldownDuration = 310;

	private const float InitialHealth = 30f;

	private static Rectangle HitBoxDimensions = new(110, 100, 75, 100);
}