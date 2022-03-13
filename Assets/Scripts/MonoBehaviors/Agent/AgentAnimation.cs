using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    class AgentAnimation
    {
        private AgentController agentController;
        private SpriteModel spriteModel;

        enum AnimState
        {
            Idle,
            Movement,
            Attack,
            Casting,
            Death,
        }

        private IEnumerator waitCoroutine;

        public AgentAnimation(AgentController agentController)
        {
            this.agentController = agentController;
        }

        public void OnSpawned(SpriteModel spriteModel)
        {
            this.spriteModel = spriteModel;

            var agent = agentController.Agent;

            spriteModel.Animator.Play(AnimState.Idle.ToString());

            agent.health.died += OnAgentDied;

            agent.movement.destinationChanged += OnAgentDestinationChanged;
            agent.movement.stopped += OnAgentStopped;

            agent.combat.meleeAttackStarted += OnAgentMeleeAttackStarted;
            agent.combat.castingStarted += OnAgentCastingStarted;

            // spriteModel.OnSpawned();

            // spriteModel.Animator.
        }

        void OnAgentDied(GameCore.Agent agent)
        {
            PlayClip(AnimState.Death);
        }

        void OnAgentDestinationChanged(Vector2 pos)
        {
            // Debug.Log("OnAgentDestinationChanged");
            PlayClip(AnimState.Movement);
        }

        void OnAgentStopped()
        {
            // Debug.Log("OnAgentStopped");
            PlayClip(AnimState.Idle);
        }

        void OnAgentMeleeAttackStarted()
        {
            // Debug.Log("OnAgentMeleeAttackStarted");
            PlayClip(AnimState.Attack);

            Wait(1 / 2f);
        }

        void OnAgentCastingStarted()
        {
            // Debug.Log("OnAgentCastingStarted");
            PlayClip(AnimState.Casting);

            Wait(1 / 2f);
        }

        void PlayClip(AnimState animState)
        {
            if (waitCoroutine != null)
            {
                agentController.StopCoroutine(waitCoroutine);
                waitCoroutine = null;
                // Debug.Log("existing corout stopped");
            }

            spriteModel.Animator.Play(animState.ToString());
        }

        void Wait(float val)
        {
            waitCoroutine = WaitCoroutine(val);
            agentController.StartCoroutine(waitCoroutine);
        }

        IEnumerator WaitCoroutine(float val)
        {
            // Debug.Log($"wait for: {val}");
            yield return new WaitForSeconds(val);

            // Debug.Log($"now idle");
            spriteModel.Animator.Play(AnimState.Idle.ToString());
        }
    }
}
