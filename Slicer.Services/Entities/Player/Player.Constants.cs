using Microsoft.Xna.Framework;

namespace Slicer.App.Entities;

public partial class Player
{
	private const float BaseMovementSpeed = 7.5f;

	private const float BaseJumpForce = 14f;

	private const int SpriteScaling = 3;

	private const float AttackDashSpeed = 20f;

	private const int AttackReach = 350;

	private const int AttackCooldownDuration = 400;

	private const int AttackDuration = 200;

	private const float AttackDamage = 10f;

	private static Rectangle HitBoxDimensions => new(110, 80, 140 ,160);
}