using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Slicer.App.Interfaces;

public interface IEntityManagerService
{
	IEntity GetEntity(string entityName);

	Dictionary<string, IEntity> GetAllEntities();

	T CreateEntity<T>(string entityName, ContentManager contentManager)
		where T : IEntity;
}