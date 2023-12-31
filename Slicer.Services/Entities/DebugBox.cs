using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using Slicer.App.Interfaces;

namespace Slicer.App.Entities;

public class DebugBox : IEntity
{
	private const int timeToLive = 2000;
    private readonly IEntityManagerService entityManagerService;
    private int timeAlive = 0;

    public string? EntityName { get; set; }

    public Color Colour { get; set; } = Color.Red;

	public DebugBox(IEntityManagerService entityManagerService)
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

		if (timeAlive > timeToLive)
		{
			entityManagerService.KillEntity(EntityName);
		}

		timeAlive += gameTime.ElapsedGameTime.Milliseconds;
    }
}
