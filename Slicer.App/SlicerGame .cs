using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slicer.App.Entities;
using Slicer.App.Interfaces;

namespace Slicer.App;

public class SlicerGame : Game
{
    private readonly IEntityManagerService entityManagerService;

    private GraphicsDeviceManager graphics;

    private SpriteBatch? spriteBatch;

    public SlicerGame(IEntityManagerService entityManagerService)
    {
        this.entityManagerService = entityManagerService;

        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1280,
            PreferredBackBufferHeight = 720,
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        entityManagerService.CreateEntity<Player>("Player", Content);

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
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

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

        spriteBatch.End();

        base.Draw(gameTime);
    }
}