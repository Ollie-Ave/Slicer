using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slicer.App.Accessors;
using Slicer.App.Interfaces;

namespace Slicer.App.Entities;

public partial class Player : IEntity, ITexturedEntity, IPhysicsEntity
{
    private readonly IPhysicsHandlerService physicsHandlerService;

    private readonly IAnimationHandlerService animationHandlerService;

    private readonly IEntityManagerService entityManagerService;

    private readonly IAttackHandlerService attackHandlerService;

    private SpriteEffects spriteEffects = SpriteEffects.None;

    public Player(
		IAnimationHandlerServiceBuilder animationHandlerServiceBuilder,
		IPhysicsHandlerServiceBuilder physicsHandlerServiceBuilder,
		IEntityManagerService entityManagerService,
		IAttackHandlerServiceBuilder attackHandlerServiceBuilder)
	{
		physicsHandlerService = physicsHandlerServiceBuilder.Build(
			new Vector2(0, 0),
			HitBoxDimensions,
			SpriteScaling);
        animationHandlerService = animationHandlerServiceBuilder.Build(animations);
        this.attackHandlerService = attackHandlerServiceBuilder.Build(AttackCooldownDuration, AttackDuration);

        this.entityManagerService = entityManagerService;
    }

	public string? EntityName { get; set; }

	public Vector2 Position
	{
		get => physicsHandlerService.Position;
		set => physicsHandlerService.Position = value;
	}

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
}
