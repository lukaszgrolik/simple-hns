using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public interface ITransformScript
    {

    }

    // public interface IGameGetPosition
    // {
    //     Vector3 GetPosition(ITransformScript script);
    // }
    // public interface IGetProjectileSpawnPosition
    // {
    //     Vector3 GetProjectileSpawnPosition(ITransformScript script);
    // }

    // public interface IGameSpawnProjectile
    // {
    //     void SpawnProjectile(Projectile projectile, Vector3 pos, Quaternion rot);
    // }

    public abstract class Game
    {
        public readonly EngineTime engineTime = new EngineTime();

        private readonly List<Agent> agents = new List<Agent>();
        private readonly List<Projectile> projectiles = new List<Projectile>();

        // public class ProjectileSpawnedEvent : UnityEvent<Projectile, Vector3, Quaternion> {}
        // public readonly ProjectileSpawnedEvent projectileSpawned = new ProjectileSpawnedEvent();
        public event System.Action<Projectile, Vector3, Quaternion> projectileSpawned;
        public event System.Action<Projectile> projectileDeleted;

        // public class AgentSpawnedEvent : UnityEvent<Agent, Vector3, Quaternion> {}
        // public readonly AgentSpawnedEvent agentSpawned = new AgentSpawnedEvent();
        public event System.Action<Agent, Vector3, Quaternion> agentSpawned;

        public Game()
        {

        }

        abstract public Vector3 GetPosition(ITransformScript script);
        abstract public Vector3 GetProjectileSpawnPosition(Agent agent);
        abstract public List<Agent> FindAgentsInRadius(Vector3 pos, float radius);

        public void OnUpdate(float deltaTime)
        {
            engineTime.AddDeltaTime(deltaTime);

            for (int i = 0; i < agents.Count; i++)
                agents[i].OnUpdate(deltaTime);

            for (int i = 0; i < projectiles.Count; i++)
                projectiles[i].OnUpdate(deltaTime);
        }

        public void SpawnAgent(Agent agent, Vector3 pos, Quaternion rot)
        {
            agents.Add(agent);

            agentSpawned?.Invoke(agent, pos, rot);
        }

        public void SpawnProjectile(Projectile projectile, Vector3 pos, Quaternion rot)
        {
            projectiles.Add(projectile);

            projectileSpawned?.Invoke(projectile, pos, rot);
        }

        public void DeleteProjectile(Projectile projectile)
        {
            projectiles.Remove(projectile);

            projectileDeleted?.Invoke(projectile);
        }
    }
}