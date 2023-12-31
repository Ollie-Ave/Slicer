using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slicer.App.Interfaces;

namespace Slicer.App.Entities;

public class PlayerNew : IEntity, ITexturedEntity
{
	   public PlayerNew(IAnimationHandlerServiceBuilder an)
	   {

	   }

    public string? EntityName { get; set; }

    public void Draw(SpriteBatch spriteBatch)
    {
		// ArgumentNullException.ThrowIfNull(Textures);

		// var currentAnimationData = animationHandlerService.GetCurrentAnimationData();
		// var texture = Textures[currentAnimationData.CurrentAnimation.Texture];
		// var frame = GetCurrentAnimationFrame();

		// spriteBatch.Draw(texture, position, frame, Color.White, 0, Vector2.Zero, SpriteScaling, spriteEffects, 0);
    }

    public void LoadTextures()
    {
    }

    public void UpdateHandler(GameTime gameTime)
    {
    }
}
