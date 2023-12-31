using System;
using Microsoft.Xna.Framework.Content;

namespace Slicer.App.Accessors;

public static class ContentManagerAccessor
{
	private static ContentManager? ContentManager;

	public static void SetContentManager(ContentManager contentManager)
	{
		ContentManager = contentManager;
	}

	public static ContentManager GetContentManager()
	{
		ArgumentNullException.ThrowIfNull(ContentManager);

		return ContentManager;
	}
}