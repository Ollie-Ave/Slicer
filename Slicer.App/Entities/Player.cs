using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
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

	private const int AttackReach = 300;

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
	];

	private readonly IAnimationHandlerService animationHandlerService;

	private readonly IEntityManagerService entityManagerService;

	private Vector2 velocity = new(0, 0);

	private Vector2 position = new(0, 000);

	private SpriteEffects spriteEffects = SpriteEffects.None;

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

		if (Environment.GetEnvironmentVariable("DEBUG") == "true")
		{
			DrawHitBox();
		}

		HandleMovement();
		HandleAttack();
		HandleSpriteDisplayDirection();
		HandleSpriteAnimation();

		position += new Vector2(velocity.X * BaseMovementSpeed, velocity.Y);
	}

	private void DrawHitBox()
	{
		var entitySize = GetCurrentAnimationFrame();
		var debugBox2 = entityManagerService.CreateEntity<DebugBox>("Player_Hitbox");

		debugBox2.Bounds = new Rectangle((int)position.X, (int)position.Y, entitySize.Width * SpriteScaling, entitySize.Height * SpriteScaling);
	}

	private void HandleAttack()
	{
		var mouse = Mouse.GetState();

		if (mouse.LeftButton == ButtonState.Pressed)
		{
			var mousePosition = new Vector2(mouse.X, mouse.Y);

			var currentTexture = GetCurrentAnimationFrame();

			var origin = position + new Vector2(currentTexture.Width * SpriteScaling / 2, currentTexture.Height * SpriteScaling / 3 * 2);

			Vector2 directionVector =  mousePosition - origin;
			Vector2 normalizedDirectionVector = Vector2.Normalize(directionVector);
			Vector2 rayPosition = origin + AttackReach * normalizedDirectionVector;

			if (Environment.GetEnvironmentVariable("DEBUG") == "true")
			{
				var debugLine = entityManagerService.CreateEntity<DebugLine>("DebugLine");

				debugLine.point1 = origin;
				debugLine.point2 = rayPosition;
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

		if (velocity.Y != 0 && velocity.Y < 0.1f)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Jump");
		}
		else if (velocity.Y > -4f && velocity.Y < 4f && !PlayerIsTouchingGround())
		{
			animationHandlerService.SetCurrentAnimation("Player/_JumpFallInbetween");
		}
		else if (velocity.Y != 0 && velocity.Y > 0.1f)
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

		velocity.X = 0;

		if (keyboard.IsKeyDown(Keys.D))
		{
			velocity.X += 1;
		}

		if (keyboard.IsKeyDown(Keys.A))
		{
			velocity.X -= 1;
		}

		if (keyboard.IsKeyDown(Keys.Space) && PlayerIsTouchingGround())
		{
			velocity.Y -= BaseJumpForce;
		}
		else if (PlayerIsTouchingGround())
		{
			velocity.Y = 0;
			position.Y = 300 - GetCurrentAnimationFrame().Height;
		}
		else
		{
			velocity.Y += BaseGravityForce;
		}


	}

	private bool PlayerIsTouchingGround()
	{
		var texture = GetCurrentAnimationFrame();

		return position.Y + texture.Height >= 300f;
	}

	private Rectangle GetCurrentAnimationFrame()
	{
		var currentAnimationData = animationHandlerService.GetCurrentAnimationData();

		return animationHandlerService.LoadAnimationFrames(currentAnimationData.CurrentAnimation.MetaData)[currentAnimationData.AnimationState.CurrentFrame];
	}
}
