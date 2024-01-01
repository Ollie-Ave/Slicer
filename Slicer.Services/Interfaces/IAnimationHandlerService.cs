using Microsoft.Xna.Framework;
using Slicer.App.Models;

namespace Slicer.App.Interfaces;

public interface IAnimationHandlerService
{
	(Animation CurrentAnimation, AnimationState AnimationState) GetCurrentAnimationData();

	void HandleAnimationState(GameTime gameTime);

	void SetCurrentAnimation(string animationName);

    List<Rectangle> LoadAnimationFrames(AnimationMetaData animationMetaData);

	Rectangle GetCurrentAnimationFrame();
}