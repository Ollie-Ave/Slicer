using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slicer.App.Accessors;
using Slicer.App.Interfaces;

namespace Slicer.App.Entities;

public partial class GoblinNew : IEntity, ITexturedEntity, IEnemy
{
    private readonly IPhysicsHandlerService physicsHandlerService;

    private readonly IAnimationHandlerService animationHandlerService;

    private readonly IHealthHandlerService healthHandlerService;

    private readonly IEntityManagerService entityManagerService;

    private SpriteEffects spriteEffects = SpriteEffects.None;

    public GoblinNew(
		IAnimationHandlerServiceBuilder animationHandlerServiceBuilder,
		IPhysicsHandlerServiceBuilder physicsHandlerServiceBuilder,
		IHealthHandlerServiceBuilder healthHandlerServiceBuilder,
		IEntityManagerService entityManagerService)
	{
		physicsHandlerService = physicsHandlerServiceBuilder.Build(
			new Vector2(100, 0),
			HitBoxDimensions,
			SpriteScaling);

        animationHandlerService = animationHandlerServiceBuilder.Build(animations);
		healthHandlerService = healthHandlerServiceBuilder.Build(
			InitialHealth,
			HurtAnimationCooldownDuration,
			() =>
			{
				ArgumentNullException.ThrowIfNull(EntityName);

				entityManagerService.KillEntity(EntityName);
			});

        this.entityManagerService = entityManagerService;
    }

    public string? EntityName { get; set; }

    public void Draw(SpriteBatch spriteBatch)
    {
		ArgumentNullException.ThrowIfNull(Textures);

		var currentAnimationData = animationHandlerService.GetCurrentAnimationData();
		var texture = Textures[currentAnimationData.CurrentAnimation.Texture];
		var frame = animationHandlerService.GetCurrentAnimationFrame();

		const float NoRotation = 0;
		const int TopLayer = 1;

		spriteBatch.Draw(
			texture,
			physicsHandlerService.GetNextPosition(),
			frame,
			Color.White,
			NoRotation,
			Vector2.Zero,
			SpriteScaling,
			spriteEffects,
			TopLayer);
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

    public Rectangle GetHitBox()
    {
		  return physicsHandlerService.HitBox;
    }

    public void SetXPosition(int position)
    {
		  physicsHandlerService.Position = new Vector2(position, physicsHandlerService.Position.Y);
    }
}
