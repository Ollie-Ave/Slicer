using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using Slicer.App.Interfaces;

namespace Slicer.App.Services;

internal class EntityManagerService : IEntityManagerService
{
	private readonly IServiceProvider serviceProvider;

	private readonly Dictionary<string, IEntity> entities = [];

	private readonly Dictionary<string, int> duplicateEntityIds = [];

	public EntityManagerService(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}

	public IEntity GetEntity(string entityName)
	{
		return entities[entityName];
	}

	public Dictionary<string, IEntity> GetAllEntities()
	{
		return entities;
	}

	public T CreateEntity<T>(string entityName)
		where T : IEntity
	{
		var entity = serviceProvider.GetRequiredService<T>();

		if (entities.ContainsKey(entityName))
		{
			if (duplicateEntityIds.TryGetValue(entityName, out var numberOfDuplicateEntities))
			{
				duplicateEntityIds[entityName] = numberOfDuplicateEntities + 1;
			}
			else
			{
				duplicateEntityIds.Add(entityName, 1);
			}

			entityName += $"-{duplicateEntityIds[entityName]}";
		}

		entity.EntityName = entityName;

		if (entity is ITexturedEntity texturedEntity)
		{
			texturedEntity.LoadTextures();
		}

		entities.Add(entityName, entity);

		return entity;
	}

    public void KillEntity(string entityName)
    {
        entities.Remove(entityName);
    }
}