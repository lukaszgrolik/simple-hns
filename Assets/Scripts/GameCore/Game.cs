using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        private readonly List<GameCore.Agent> agents = new List<GameCore.Agent>();
        private readonly List<GameCore.Projectile> projectiles = new List<GameCore.Projectile>();

        public class ProjectileSpawnedEvent : UnityEvent<Projectile, Vector3, Quaternion> {}
        public readonly ProjectileSpawnedEvent projectileSpawned = new ProjectileSpawnedEvent();

        public class AgentSpawnedEvent : UnityEvent<Agent, Vector3, Quaternion> {}
        public readonly AgentSpawnedEvent agentSpawned = new AgentSpawnedEvent();

        public Game()
        {

        }

        abstract public Vector3 GetPosition(ITransformScript script);
        abstract public Vector3 GetProjectileSpawnPosition(GameCore.Agent agent);

        public void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < agents.Count; i++)
                agents[i].OnUpdate(deltaTime);
        }

        public void SpawnAgent(Agent agent, Vector3 pos, Quaternion rot)
        {
            agents.Add(agent);

            agentSpawned.Invoke(agent, pos, rot);
        }

        public void SpawnProjectile(Projectile projectile, Vector3 pos, Quaternion rot)
        {
            projectiles.Add(projectile);

            projectileSpawned.Invoke(projectile, pos, rot);
        }
    }
}