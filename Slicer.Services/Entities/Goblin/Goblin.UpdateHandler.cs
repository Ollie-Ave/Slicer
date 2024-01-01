using Microsoft.Xna.Framework;
using Slicer.App.Accessors;

namespace Slicer.App.Entities;

public partial class GoblinNew
{
	private bool currentAnimationIsDeath => animationHandlerService
			.GetCurrentAnimationData()
			.CurrentAnimation.Texture == "Goblin/_Death";

	public void TakeDamage(float damage)
	{
		healthHandlerService.TakeDamage(damage);
	}

	public void UpdateHandler(GameTime gameTime)
	{
		if (GameEnvironment.IsDebugMode)
		{
			DrawHitBox();
		}

		HandleAnimations();

		animationHandlerService.HandleAnimationState(gameTime);
		physicsHandlerService.HandlePhysicsState(gameTime);
		healthHandlerService.HandleHealthState(gameTime);
	}

	private void HandleAnimations()
	{

		if (healthHandlerService.IsDying)
		{
			animationHandlerService.SetCurrentAnimation("Goblin/_Death");
		}
		else if (!healthHandlerService.CanTakeDamage)
		{
			animationHandlerService.SetCurrentAnimation("Goblin/_TakeHit");
		}
		else
		{
			animationHandlerService.SetCurrentAnimation("Goblin/_Idle");
		}
	}

	private void DrawHitBox()
	{
		var hitBoxDebugBox = entityManagerService.CreateEntity<SingleFrameDebugBox>("Player_Hitbox");

		hitBoxDebugBox.Colour = Color.Green;
		hitBoxDebugBox.Bounds = physicsHandlerService.HitBox;
	}
}