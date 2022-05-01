using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentDrop
    {
        public AgentDrop(AgentHealth health)
        {
            health.died += OnAgentDied;
        }

        void OnAgentDied(Agent agent)
        {
            DropItems(agent);
        }

        void DropItems(Agent agent)
        {
            var n = 0;
            var rand = Random.value;

                 if (rand < .1f) n = 3;
            else if (rand < .25f) n = 2;
            else if (rand < .5f) n = 1;

            for (int i = 0; i < n; i++)
            {
                var itemSystem = agent.game.itemSystem;
                var agentPos = agent.game.GetPosition(agent);

                DropItem(itemSystem, agentPos);
            }
        }

        void DropItem(ItemSystem itemSystem, Vector3 agentPos)
        {
            var itemDataList = new List<DataDefinition.Item>(){
                DataInstance.Items.handAxe,
                DataInstance.Items.shortStaff,
                DataInstance.Items.shortSword,
                DataInstance.Items.skullCap,
                DataInstance.Items.smallShield,
            };
            var itemData = itemDataList.Random();
            var itemAttrs = GenerateDroppedItemAttrs();
            var item = new Item(
                itemData: itemData,
                attrs: itemAttrs
            );

            var circlePos = Random.insideUnitCircle * 1;
            var pos = agentPos + new Vector3(circlePos.x, 0, circlePos.y);

            itemSystem.Drop(item, pos);
        }

        List<AgentAttribute> GenerateDroppedItemAttrs()
        {

            var attrsConstructors = new List<System.Func<AgentAttribute>>(){
                () => new AgentAttribute_PlusLife(Random.Range(1, 20)),
                () => new AgentAttribute_IncreasedMovementSpeed(Random.Range(1, 4) * 5),
                () => new AgentAttribute_IncreasedAttackRate(Random.Range(1, 6) * 5),
                () => new AgentAttribute_EnhancedDamage(Random.Range(1, 100)),
                () => new AgentAttribute_LifeRegen(Random.Range(1, 60)),
                () => new AgentAttribute_LifeStolenPerHit(Random.Range(1, 3)),
                () => new AgentAttribute_LifeStolenPerKill(Random.Range(1, 10)),
            };
            var amount = Random.Range(1, attrsConstructors.Count + 1);
            var selectedAttrs = attrsConstructors.RandomMany(amount);

            List<AgentAttribute> resAgentAttrs = new List<AgentAttribute>();
            for (int i = 0; i < selectedAttrs.Count; i++)
            {
                var attr = selectedAttrs[i]();

                resAgentAttrs.Add(attr);
            }

            return resAgentAttrs;
        }
    }
}