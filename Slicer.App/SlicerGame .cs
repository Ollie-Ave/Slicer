﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using Slicer.App.Accessors;
using Slicer.App.Entities;
using Slicer.App.Interfaces;

namespace Slicer.App;

public class SlicerGame : Game
{
    private readonly IEntityManagerService entityManagerService;

    private SpriteBatch? spriteBatch;

    private IGameWorldHandler gameWorldHandler;

    public SlicerGame(IEntityManagerService entityManagerService, IGameWorldHandler gameWorldHandler)
    {
        this.entityManagerService = entityManagerService;
        this.gameWorldHandler = gameWorldHandler;

        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1280,
            PreferredBackBufferHeight = 720,
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    public GraphicsDeviceManager graphics {get; private set;}

    protected override void Initialize()
    {
        ContentManagerAccessor.SetContentManager(Content);

        entityManagerService.CreateEntity<Player>(Constants.EntityNames.Player);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // If we need to do something more complex in the future with loading content, we can do that here.
        // For now, we will just re-load the textures as required...
        // Slow... but works and we'll see if we need to opitimize at a later date.
    }

    protected override void Update(GameTime gameTime)
    {
        this.gameWorldHandler.UpdateHandler(gameTime, this);

        var entities = entityManagerService
            .GetAllEntities()
            .Select(x => x.Value)
            .ToList();

        foreach (var entity in entities)
        {
            entity.UpdateHandler(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);

        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        var entities = entityManagerService
            .GetAllEntities()
            .Select(x => x.Value)
            .ToList();

        foreach (var entity in entities)
        {
            entity.Draw(spriteBatch);
        }

        spriteBatch.DrawLine(new Vector2(0, Constants.FloorHeight), new Vector2(1280, Constants.FloorHeight), Color.Red);

        spriteBatch.End();

        base.Draw(gameTime);
    }
}
