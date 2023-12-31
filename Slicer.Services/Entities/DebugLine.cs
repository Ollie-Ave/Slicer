using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using Slicer.App.Interfaces;

namespace Slicer.App.Entities;

public class DebugLine : IEntity
{
	private const int timeToLive = 5000;
    private readonly IEntityManagerService entityManagerService;
    private int timeAlive = 0;

	public string?  EntityName { get; set;}

	public Vector2 point1 { get; set; }

	public Vector2 point2 { get; set; }

	public ContentManager? ContentManager { get; set; }

	public DebugLine(IEntityManagerService entityManagerService)
	{
        this.entityManagerService = entityManagerService;
    }
	public void Draw(SpriteBatch spriteBatch)
	{

		spriteBatch.DrawLine(point1, point2, Color.Red);
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
