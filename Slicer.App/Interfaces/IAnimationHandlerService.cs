using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Slicer.App.Models;

namespace Slicer.App.Interfaces;

public interface IAnimationHandlerService
{
    void RegisterAnimations(List<Animation> animations);

	(Animation CurrentAnimation, AnimationState AnimationState) GetCurrentAnimationData();

	void HandleAnimationState(GameTime gameTime);

	void SetCurrentAnimation(string animationName);
}