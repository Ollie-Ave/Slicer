using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using Slicer.App.Interfaces;

namespace Slicer.App.Entities;

public class SingleFrameDebugBox : IEntity
{
    private readonly IEntityManagerService entityManagerService;

    public string? EntityName { get; set; }

    public Color Colour { get; set; } = Color.Red;

	public SingleFrameDebugBox (IEntityManagerService entityManagerService)
    {
        this.entityManagerService = entityManagerService;
    }

	public Rectangle Bounds { get; set; }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.DrawRectangle(Bounds, Colour);
    }

    public void UpdateHandler(GameTime gameTime)
    {
		ArgumentNullException.ThrowIfNull(EntityName);

		entityManagerService.KillEntity(EntityName);
    }
}
