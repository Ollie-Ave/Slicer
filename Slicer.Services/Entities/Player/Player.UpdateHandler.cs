using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slicer.App.Accessors;
using Slicer.App.Interfaces;
using Slicer.App.Models;

namespace Slicer.App.Entities;

public partial class Player
{
	public void UpdateHandler(GameTime gameTime)
	{
		if (GameEnvironment.IsDebugMode)
		{
			DrawHitBox();
		}

		var keyboard = Keyboard.GetState();
		var mouse = Mouse.GetState();

		HandleAttack(mouse);
		HandleWalking(keyboard);
		HandleJumping(keyboard);


		HandleDisplayDirection();
		HandleAnimations();

		animationHandlerService.HandleAnimationState(gameTime);
		physicsHandlerService.HandlePhysicsState(gameTime);
		attackHandlerService.HandleAttackState(gameTime);
	}

	private void HandleAttack(MouseState mouse)
	{
		if (physicsHandlerService.Forces.TryGetValue("Attack", out var force)
				&& force.Velocity != Vector2.Zero)
		{
			physicsHandlerService.AddForce("Attack", new()
			{
				Velocity = new Vector2(
					DashFriction * GetDirection(force.Velocity.X),
					DashFriction * GetDirection(force.Velocity.Y)),
			});

			physicsHandlerService.SetForce("Attack", new()
			{
				Velocity = new Vector2(
					force.Velocity.X > -1 && force.Velocity.X < 1
						? 0
						: force.Velocity.X,
					force.Velocity.Y > -1 && force.Velocity.Y < 1
						? 0
						: force.Velocity.Y),
			});
		}

		var mousePosition = new Vector2(mouse.X, mouse.Y);

		Vector2 rayOrigin = physicsHandlerService.HitBoxPosition + new Vector2(physicsHandlerService.HitBox.Width / 2, physicsHandlerService.HitBox.Height / 2);
		Ray2 ray = new(rayOrigin, mousePosition, AttackReach);

		if (GameEnvironment.IsDebugMode)
		{
			var debugRay = entityManagerService.CreateEntity<DebugLine>("Player_Attack_Ray");

			debugRay.StartPosition = ray.Origin;
			debugRay.EndPosition = ray.End;
		}

		if (mouse.LeftButton == ButtonState.Pressed
				&& attackHandlerService.CanAttack)
		{
			attackHandlerService.Attack(() =>
			{
				physicsHandlerService.SetForce("Attack", new()
				{
					Velocity = ray.Direction * AttackDashSpeed,
					TerminalVelocity = (new Vector2(AttackDashSpeed * KnockbackMultiplier), new Vector2(-AttackDashSpeed * KnockbackMultiplier)),
				});
			},
			() =>
			{
				List<IEnemy> enemies = entityManagerService
					.GetAllEntities()
					.Where(x => x.Value is IEnemy)
					.Select(x => (IEnemy)x.Value)
					.ToList();

				foreach (var enemy in enemies)
				{
					if (enemy.GetHitBox().Intersects(physicsHandlerService.HitBox))
					{
						enemy.TakeDamage(AttackDamage);

						var enemyHitBox = enemy.GetHitBox();
						Vector2 enemyPosition = new(enemyHitBox.X, enemyHitBox.Y);

						Vector2 direction = Vector2.Normalize(physicsHandlerService.HitBoxPosition - enemyPosition);

						physicsHandlerService.SetForce("Attack", new()
						{
							Velocity = direction * AttackDashSpeed * KnockbackMultiplier,
						});
					}
				}
			});
		}
	}

	private static int GetDirection(float value)
	{
		return value switch
		{
			> 0 => -1,
			< 0 => 1,
			_ => 0,
		};
	}

	private void DrawHitBox()
	{
		var hitBoxDebugBox = entityManagerService.CreateEntity<SingleFrameDebugBox>("Player_Hitbox");

		hitBoxDebugBox.Colour = Color.Green;
		hitBoxDebugBox.Bounds = physicsHandlerService.HitBox;
	}

	private void HandleJumping(KeyboardState keyboard)
	{
		if (physicsHandlerService.Forces.TryGetValue("Jumping", out var force)
				&& force.Velocity.Y < 0)
		{
			if (!physicsHandlerService.IsTouchingGround)
			{
				physicsHandlerService.AddForce("Jumping", new()
				{
					Velocity = new Vector2(0, 0.1f),
					TerminalVelocity = (Vector2.Zero, new Vector2(0, -BaseJumpForce)),
				});
			}
			else if (physicsHandlerService.IsTouchingGround)
			{
				physicsHandlerService.SetForce("Jumping", new()
				{
					Velocity = Vector2.Zero,
					TerminalVelocity = (Vector2.Zero, new Vector2(0, -BaseJumpForce)),
				});
			}
		}

		if (keyboard.IsKeyDown(Keys.Space) && physicsHandlerService.IsTouchingGround)
		{
			physicsHandlerService.SetForce("Jumping", new()
			{
				Velocity = new Vector2(0, -BaseJumpForce),
				TerminalVelocity = (Vector2.Zero, new Vector2(0, -BaseJumpForce)),
			});
		}
	}

	private void HandleWalking(KeyboardState keyboard)
	{
		physicsHandlerService.SetForce("Walking", new()
		{
			Velocity = new Vector2(0, 0),
			TerminalVelocity = (new Vector2(BaseMovementSpeed, 0), new Vector2(-BaseMovementSpeed, 0)),
		});

		if (attackHandlerService.IsAttacking)
		{
			return;
		}

		if (keyboard.IsKeyDown(Keys.D))
		{
			physicsHandlerService.AddForce("Walking", new()
			{
				Velocity = new Vector2(BaseMovementSpeed, 0),
			});

		}
		else if (keyboard.IsKeyDown(Keys.A))
		{
			physicsHandlerService.AddForce("Walking", new()
			{
				Velocity = new Vector2(-BaseMovementSpeed, 0),
			});
		}
	}

	private void HandleAnimations()
	{
		if (attackHandlerService.IsAttacking)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Attack");
		}
		else if (physicsHandlerService.Velocity.Y != 0
			&& physicsHandlerService.Velocity.Y < 1f)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Jump");
		}
		else if (physicsHandlerService.Velocity.Y > -4f
			&& physicsHandlerService.Velocity.Y < 4f && !physicsHandlerService.IsTouchingGround)
		{
			animationHandlerService.SetCurrentAnimation("Player/_JumpFallInbetween");
		}
		else if (physicsHandlerService.Velocity.Y != 0
			&& physicsHandlerService.Velocity.Y > 1f)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Fall");
		}
		else if (physicsHandlerService.Velocity.X > 0)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Run");
		}
		else if (physicsHandlerService.Velocity.X < 0)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Run");
		}
		else
		{
			animationHandlerService.SetCurrentAnimation("Player/_Idle");
		}
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
}
