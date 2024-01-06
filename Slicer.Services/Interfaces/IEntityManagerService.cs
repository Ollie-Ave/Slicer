using Microsoft.Xna.Framework;

namespace Slicer.App.Interfaces;

public interface IEntityManagerService
{
	IEntity GetEntity(string entityName);

	Dictionary<string, IEntity> GetAllEntities();

	T CreateEntity<T>(string entityName)
		where T : IEntity;

	T CreateEntity<T>(string entityName, Vector2 initialPosition)
		where T : IEntity, IPhysicsEntity;

	void KillEntity(string entityName);
}