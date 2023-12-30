using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Slicer.App.Interfaces;

public interface IEntity
{
	ContentManager? ContentManager { get; set; }

	void Draw(SpriteBatch spriteBatch);

	void UpdateHandler(GameTime gameTime);
}