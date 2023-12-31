using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slicer.App.Accessors;
using Slicer.App.Interfaces;
using Slicer.App.Models;

namespace Slicer.App.Entities;

public class Player : IEntity, ITexturedEntity
{
	private const float BaseMovementSpeed = 7.5f;

	private const float BaseGravityForce = 0.5f;

	private const float BaseJumpForce = 12f;

	private const int SpriteScaling = 3;

	private const int AttackReach = 350;

	private const int AttackCooldownDuration = 500;

	private const int AttackDashDuration = 150;

	private const float BaseDamage = 10f;

	private static Vector2 DefaultFrameSize = new(120, 80);

	private static readonly List<Animation> animations =
	[
		new Animation()
		{
			Texture = "Player/_Idle",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 10,
				NumberOfFrames = 10,
				TimeBetweenFrames = 100,
			},
		},
		new Animation()
		{
			Texture = "Player/_Run",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 10,
				NumberOfFrames = 10,
				TimeBetweenFrames = 70,
			},
		},
		new Animation()
		{
			Texture = "Player/_Jump",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 3,
				NumberOfFrames = 3,
				TimeBetweenFrames = 70,
				Loop = false,
			},
		},
		new Animation()
		{
			Texture = "Player/_JumpFallInbetween",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 2,
				NumberOfFrames = 2,
				TimeBetweenFrames = 70,
				Loop = false,
			},
		},
		new Animation()
		{
			Texture = "Player/_Fall",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 3,
				NumberOfFrames = 3,
				TimeBetweenFrames = 70,
				Loop = false,
			},
		},
		new Animation()
		{
			Texture = "Player/_Attack",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 4,
				NumberOfFrames = 4,
				TimeBetweenFrames = 100,
				Loop = false,
			},
		},
	];

	private readonly IAnimationHandlerService animationHandlerService;

	private readonly IEntityManagerService entityManagerService;

	private Vector2 velocity = new(0, 0);

	private Vector2 position = new(0, 000);

	private SpriteEffects spriteEffects = SpriteEffects.None;

	private Rectangle hitbox;

	private int attackCooldown;

	private int attackDashCooldown;

	public Player(IAnimationHandlerServiceBuilder animationHandlerServiceBuilder, IEntityManagerService entityManagerService)
	{
		this.animationHandlerService = animationHandlerServiceBuilder.Build(animations);
		this.entityManagerService = entityManagerService;
	}

	public Dictionary<string, Texture2D>? Textures { get; set; }

	public string? EntityName { get; set; }

	public void LoadTextures()
	{
		var content = ContentManagerAccessor.GetContentManager();

		foreach (var animation in animations)
		{
			Textures ??= [];

			Textures.Add(animation.Texture, content.Load<Texture2D>(animation.Texture));
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		ArgumentNullException.ThrowIfNull(Textures);

		var currentAnimationData = animationHandlerService.GetCurrentAnimationData();
		var texture = Textures[currentAnimationData.CurrentAnimation.Texture];
		var frame = GetCurrentAnimationFrame();

		spriteBatch.Draw(texture, position, frame, Color.White, 0, Vector2.Zero, SpriteScaling, spriteEffects, 0);
	}

	public void UpdateHandler(GameTime gameTime)
	{
		animationHandlerService.HandleAnimationState(gameTime);

		UpdateHitbox();

		if (Environment.GetEnvironmentVariable("DEBUG") == "true")
		{
			DrawHitBox();
		}

		HandleMovement();
		HandleAttack(gameTime);
		HandleSpriteDisplayDirection();
		HandleSpriteAnimation();

		velocity.Y = Math.Clamp(velocity.Y, -15, 15);
		velocity.X = Math.Clamp(velocity.X, -25, 25);

		position += velocity;
	}

	private void UpdateHitbox()
	{
		hitbox.X = (int)position.X + 110;
		hitbox.Y = (int)position.Y + 80;
		hitbox.Width = 140;
		hitbox.Height = 160;
	}

	private void DrawHitBox()
	{
		var debugBox1 = entityManagerService.CreateEntity<SingleFrameDebugBox>("Player_Hitbox");

		debugBox1.Colour = Color.Green;
		debugBox1.Bounds = hitbox;
	}

	private void HandleAttack(GameTime gameTime)
	{
		const float KnockBackMultiplier = 3.2f;
		var mouse = Mouse.GetState();

		if (attackCooldown > 0)
		{
			attackCooldown = (int)MathF.Max(0, attackCooldown - gameTime.ElapsedGameTime.Milliseconds);
			attackDashCooldown = (int)MathF.Max(0, attackDashCooldown - gameTime.ElapsedGameTime.Milliseconds);
		}

		if (attackDashCooldown > 0)
		{
			var enemies = entityManagerService
				.GetAllEntities()
				.Where(x => x.Value is IEnemy)
				.Select(x => (IEnemy)x.Value)
				.ToList();

			foreach (var enemy in enemies)
			{
				var enemyHitbox = enemy.GetHitbox();

				if (enemyHitbox.Intersects(hitbox))
				{
					var mousePosition = new Vector2(mouse.X, mouse.Y);

					var currentTexture = GetCurrentAnimationFrame();

					var origin = position + new Vector2(currentTexture.Width * SpriteScaling / 2, currentTexture.Height * SpriteScaling / 3 * 2);

					Vector2 directionVector = mousePosition - origin;
					Vector2 normalizedDirectionVector = Vector2.Normalize(directionVector);

					normalizedDirectionVector *= -1;

					velocity += normalizedDirectionVector * KnockBackMultiplier * BaseMovementSpeed;
					velocity.Y = MathF.Min(velocity.Y, 1.5f);
				}
			}
		}

		if (mouse.LeftButton == ButtonState.Pressed && attackCooldown == 0)
		{
			var mousePosition = new Vector2(mouse.X, mouse.Y);

			var currentTexture = GetCurrentAnimationFrame();

			var origin = position + new Vector2(currentTexture.Width * SpriteScaling / 2, currentTexture.Height * SpriteScaling / 3 * 2);

			Vector2 directionVector = mousePosition - origin;
			Vector2 normalizedDirectionVector = Vector2.Normalize(directionVector);
			Vector2 rayPosition = origin + AttackReach * normalizedDirectionVector;

			Rectangle attackHitBox = new(
					(int)MathF.Min(origin.X, rayPosition.X),
					(int)MathF.Min(origin.Y, rayPosition.Y),
					(int)(MathF.Max(origin.X, rayPosition.X) - MathF.Min(origin.X, rayPosition.X)),
					(int)(MathF.Max(origin.Y, rayPosition.Y) - MathF.Min(origin.Y, rayPosition.Y)));

			if (Environment.GetEnvironmentVariable("DEBUG") == "true")
			{
				var debugLine = entityManagerService.CreateEntity<DebugLine>("DebugLine");

				debugLine.point1 = origin;
				debugLine.point2 = rayPosition;

				var debugBox = entityManagerService.CreateEntity<DebugBox>("DebugBo2");
				debugBox.Bounds = attackHitBox;
			}

			attackCooldown = AttackCooldownDuration;
			attackDashCooldown = AttackDashDuration;
			velocity += normalizedDirectionVector * KnockBackMultiplier * BaseMovementSpeed;
			Math.Clamp(velocity.Y, -7.5f, 7.5f);

			animationHandlerService.SetCurrentAnimation("Player/_Attack");

			var enemies = entityManagerService
				.GetAllEntities()
				.Where(x => x.Value is IEnemy)
				.Select(x => (IEnemy)x.Value)
				.ToList();

			foreach (var enemy in enemies)
			{
				var enemyHitbox = enemy.GetHitbox();

				if (enemyHitbox.Intersects(attackHitBox))
				{
					enemy.TakeDamage(BaseDamage);
				}
			}
		}
	}

	private void HandleSpriteDisplayDirection()
	{
		if (velocity.X > 0)
		{
			spriteEffects = SpriteEffects.None;
		}
		else if (velocity.X < 0)
		{
			spriteEffects = SpriteEffects.FlipHorizontally;
		}
	}

	private void HandleSpriteAnimation()
	{

		if (attackDashCooldown > 0)
		{
			return;
		}

		if (velocity.Y != 0 && velocity.Y < 1f)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Jump");
		}
		else if (velocity.Y > -4f && velocity.Y < 4f && !PlayerIsTouchingGround())
		{
			animationHandlerService.SetCurrentAnimation("Player/_JumpFallInbetween");
		}
		else if (velocity.Y != 0 && velocity.Y > 1f)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Fall");
		}
		else if (velocity.X > 0)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Run");
		}
		else if (velocity.X < 0)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Run");
		}
		else
		{
			animationHandlerService.SetCurrentAnimation("Player/_Idle");
		}
	}

	private void HandleMovement()
	{
		var keyboard = Keyboard.GetState();
		var mouse = Mouse.GetState();

		if (attackDashCooldown == 0)
		{
			if (velocity.X > 0 && velocity.X != 0)
			{
				velocity.X = MathF.Max(BaseMovementSpeed, velocity.X);
				velocity.X = MathF.Min(0, velocity.X - BaseMovementSpeed);
			}
			else if (velocity.X < 0 && velocity.X != 0)
			{
				velocity.X = MathF.Min(-BaseMovementSpeed, velocity.X);
				velocity.X = MathF.Max(0, velocity.X + BaseMovementSpeed);
			}
		}

		if (mouse.LeftButton != ButtonState.Pressed && keyboard.IsKeyDown(Keys.D))
		{
			velocity.X += BaseMovementSpeed;
		}

		if (mouse.LeftButton != ButtonState.Pressed && keyboard.IsKeyDown(Keys.A))
		{
			velocity.X -= BaseMovementSpeed;
		}

		if (mouse.LeftButton != ButtonState.Pressed && keyboard.IsKeyDown(Keys.Space) && PlayerIsTouchingGround())
		{
			velocity.Y -= BaseJumpForce;
		}
		else if (PlayerIsTouchingGround())
		{
			velocity.Y = 0;
			position.Y = Constants.FloorHeight - (GetCurrentAnimationFrame().Height * SpriteScaling);
		}
		else
		{
			velocity.Y += BaseGravityForce;
		}
	}

	private bool PlayerIsTouchingGround()
	{
		var texture = GetCurrentAnimationFrame();

		return position.Y + (texture.Height * SpriteScaling) >= Constants.FloorHeight;
	}

	private Rectangle GetCurrentAnimationFrame()
	{
		var currentAnimationData = animationHandlerService.GetCurrentAnimationData();

		return animationHandlerService.LoadAnimationFrames(currentAnimationData.CurrentAnimation.MetaData)[currentAnimationData.AnimationState.CurrentFrame];
	}
}
