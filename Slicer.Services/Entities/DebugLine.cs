using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using Slicer.App.Interfaces;

namespace Slicer.App.Entities;

public class DebugLine : IEntity
{
	// private const int timeToLive = 2500;
	private const int timeToLive =0;
    private readonly IEntityManagerService entityManagerService;
    private int timeAlive = 0;

	public string?  EntityName { get; set;}

	public Vector2 StartPosition { get; set; }

	public Vector2 EndPosition { get; set; }

	public ContentManager? ContentManager { get; set; }

	public DebugLine(IEntityManagerService entityManagerService)
	{
        this.entityManagerService = entityManagerService;
    }
	public void Draw(SpriteBatch spriteBatch)
	{

		spriteBatch.DrawLine(StartPosition, EndPosition, Color.Red);
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
