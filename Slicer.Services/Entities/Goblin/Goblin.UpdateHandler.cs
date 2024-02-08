using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slicer.App.Accessors;

namespace Slicer.App.Entities;

public partial class Goblin
{
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

        if (!this.healthHandlerService.IsDying)
        {
            HandleBehaviour();
            HandleDisplayDirection();
        }
        else
        {
            this.physicsHandlerService.SetForce("Walking", new()
            {
                Velocity = Vector2.Zero,
            });
        }

		animationHandlerService.HandleAnimationState(gameTime);
		physicsHandlerService.HandlePhysicsState(gameTime);
		healthHandlerService.HandleHealthState(gameTime);
	}

	private void HandleDisplayDirection()
	{
		if (physicsHandlerService.Velocity.X > 0)
		{
			spriteEffects = SpriteEffects.None;
		}
		else if (physicsHandlerService.Velocity.X < 0)
		{
			spriteEffects = SpriteEffects.FlipHorizontally;
		}
	}

    private void HandleBehaviour()
    {
        Player player = (Player)this.entityManagerService.GetEntity(Constants.EntityNames.Player);

        Vector2 direction = Vector2.Normalize(player.HitBoxPosition - physicsHandlerService.HitBoxPosition );

        physicsHandlerService.SetForce("Walking", new()
        {
            Velocity = new Vector2(direction.X, 0) * WalkSpeed,
        });
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
		else if (physicsHandlerService.Velocity.X > 0)
		{
			animationHandlerService.SetCurrentAnimation("Goblin/_Run");
		}
		else if (physicsHandlerService.Velocity.X < 0)
		{
			animationHandlerService.SetCurrentAnimation("Goblin/_Run");
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
