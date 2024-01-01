using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slicer.App.Accessors;
using Slicer.App.Interfaces;
using Slicer.App.Models;

namespace Slicer.App.Entities;

public class Goblin :IEntity, ITexturedEntity, IEnemy
{
	private const int SpriteScaling = 2;

	private float health = 30f;

	private const int HurtAnimationCooldownDuration = 310;

	private int hurtAnimationCooldown;

	private static Vector2 DefaultFrameSize = new(150, 150);

	private static readonly List<Animation> animations =
	[
		new Animation()
		{
			Texture = "Goblin/_Idle",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 4,
				NumberOfFrames = 4,
				TimeBetweenFrames = 100,
			},
		},
		new Animation()
		{
			Texture = "Goblin/_Run",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 8,
				NumberOfFrames = 8,
				TimeBetweenFrames = 70,
			},
		},
		new Animation()
		{
			Texture = "Goblin/_Attack",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 8,
				NumberOfFrames = 8,
				TimeBetweenFrames = 100,
			},
		},
		new Animation()
		{
			Texture = "Goblin/_Death",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 4,
				NumberOfFrames = 4,
				TimeBetweenFrames = 70,
			},
		},
		new Animation()
		{
			Texture = "Goblin/_TakeHit",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 4,
				NumberOfFrames = 4,
				TimeBetweenFrames = 70,
			},
		},
	];

	private readonly IAnimationHandlerService animationHandlerService;

    private readonly IEntityManagerService entityManagerService;

    private SpriteEffects spriteEffects = SpriteEffects.None;

	private Vector2 position;

	private Rectangle hitbox;

	public Goblin(IAnimationHandlerServiceBuilder animationHandlerServiceBuilder, IEntityManagerService entityManagerService)
    {
		animationHandlerService = animationHandlerServiceBuilder.Build(animations);
        this.entityManagerService = entityManagerService;
    }

    public string? EntityName { get; set; }

	public Dictionary<string, Texture2D>? Textures { get; set; }


    public void UpdateHandler(GameTime gameTime)
    {
		ArgumentNullException.ThrowIfNull(EntityName);

		var frame = GetCurrentAnimationFrame();
        position = new Vector2(position.X, Constants.FloorHeight - frame.Height * SpriteScaling);

		// This is a hack to get around the weird textures I've put in :)
		position.Y += 100;

		animationHandlerService.HandleAnimationState(gameTime);

		UpdateHitbox();

		if (GameEnvironment.IsDebugMode)
		{
			DrawHitBox();
		}

		if (hurtAnimationCooldown == 0)
		{
			if (health <= 0 && animationHandlerService.GetCurrentAnimationData().CurrentAnimation.Texture != "Goblin/_Death")
			{
				animationHandlerService.SetCurrentAnimation("Goblin/_Death");

				hurtAnimationCooldown = HurtAnimationCooldownDuration;
			}
			else if (health <= 0)
			{
				entityManagerService.KillEntity(EntityName);
			}
			else if (animationHandlerService.GetCurrentAnimationData().CurrentAnimation.Texture != "Goblin/_Idle")
			{
				animationHandlerService.SetCurrentAnimation("Goblin/_Idle");
			}
		}
		else if (hurtAnimationCooldown > 0)
		{
			hurtAnimationCooldown = Math.Max(hurtAnimationCooldown - gameTime.ElapsedGameTime.Milliseconds, 0);
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

    public void LoadTextures()
    {
		var content = ContentManagerAccessor.GetContentManager();

		foreach (var animation in animations)
		{
			Textures ??= [];

			Textures.Add(animation.Texture, content.Load<Texture2D>(animation.Texture));
		}
    }

	private Rectangle GetCurrentAnimationFrame()
	{
		var currentAnimationData = animationHandlerService.GetCurrentAnimationData();

		return animationHandlerService.LoadAnimationFrames(currentAnimationData.CurrentAnimation.MetaData)[currentAnimationData.AnimationState.CurrentFrame];
	}

	private void DrawHitBox()
	{
		var debugBox1 = entityManagerService.CreateEntity<SingleFrameDebugBox>("Goblin_Hitbox");

		debugBox1.Colour = Color.Green;
		debugBox1.Bounds = hitbox;
	}

    private void UpdateHitbox()
    {
		  hitbox.X = (int) position.X + 110;
		  hitbox.Y = (int) position.Y + 100;
		  hitbox.Width = 75;
		  hitbox.Height = 100;
    }

    public Rectangle GetHitBox()
    {
		  return hitbox;
    }

    public void TakeDamage(float damage)
    {
		  if (hurtAnimationCooldown == 0)
		  {
			health -= damage;

			animationHandlerService.SetCurrentAnimation( "Goblin/_TakeHit");

			hurtAnimationCooldown = HurtAnimationCooldownDuration;
		  }
    }

    public void SetXPosition(int position)
    {
		  this.position.X = position;
    }
}
