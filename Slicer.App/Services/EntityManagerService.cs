using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using Slicer.App.Interfaces;

namespace Slicer.App.Services;

internal class EntityManagerService : IEntityManagerService
{
	private readonly IServiceProvider serviceProvider;

	private readonly Dictionary<string, IEntity> entities = [];

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

	public T CreateEntity<T>(string entityName, ContentManager contentManager)
		where T : IEntity
	{
		var entity = serviceProvider.GetRequiredService<T>();

		entity.ContentManager = contentManager;

		entities.Add(entityName, entity);

		return entity;
	}
}