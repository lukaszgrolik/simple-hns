using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehaviors
{
    public static class PlayerControllerUtils
    {
        static public void DropRandom(AgentController agentCtrl)
        {
            var itemDataList = new List<DataDefinition.Item>(){
                DataInstance.Items.handAxe,
                DataInstance.Items.shortStaff,
                DataInstance.Items.shortSword,
                DataInstance.Items.skullCap,
                DataInstance.Items.smallShield,
            };
            var itemData = itemDataList.Random();
            var item = new GameCore.Item(
                itemData: itemData
            );

            var circlePos = Random.insideUnitCircle * 3;
            var pos = agentCtrl.transform.position + new Vector3(circlePos.x, 0, circlePos.y);

            agentCtrl.GameplayManager.Game.itemSystem.Drop(item, pos);
        }

        static public void PickClosest(AgentController agentCtrl)
        {
            var droppedItems = Utils.FindColliders<DroppedItemMB>(agentCtrl.transform.position, 2, agentCtrl.GameplayManager.DroppedItemLayerMask);
            if (droppedItems.Count > 0)
            {
                var droppedItems_closest = Utils.SortByProximity(droppedItems, agentCtrl.transform.position);

                agentCtrl.Agent.equipment.Pick(droppedItems_closest[0].DroppedItem);
            }
        }

        static public void EquipTestItem_WithGoodStats(AgentController agentCtrl)
        {
            if (agentCtrl.Agent.equipment.IsSlotFree(GameCore.AgentEquipment.WornItemSlot.Test1) == false) return;

            var item = new GameCore.Item(
                itemData: new DataDefinition.Item(
                    name: "Good item",
                    category: new DataDefinition.ItemCategory(
                        name: "Some category"
                    )
                ),
                attrs: new List<GameCore.AgentAttribute>(){
                    new GameCore.AgentAttribute_PlusLife(20),
                    new GameCore.AgentAttribute_IncreasedMovementSpeed(10),
                    new GameCore.AgentAttribute_IncreasedAttackRate(20),
                    new GameCore.AgentAttribute_EnhancedDamage(50),
                    new GameCore.AgentAttribute_LifeStolenPerHit(1),
                    new GameCore.AgentAttribute_LifeStolenPerKill(3),
                    new GameCore.AgentAttribute_LifeRegen(6),
                }
            );

            agentCtrl.Agent.equipment.EquipItem(GameCore.AgentEquipment.WornItemSlot.Test1, item);
        }

        static public void EquipTestItem_WithSuperStats(AgentController agentCtrl)
        {
            if (agentCtrl.Agent.equipment.IsSlotFree(GameCore.AgentEquipment.WornItemSlot.Test2) == false) return;

            var item = new GameCore.Item(
                itemData: new DataDefinition.Item(
                    name: "Super item",
                    category: new DataDefinition.ItemCategory(
                        name: "Some category"
                    )
                ),
                attrs: new List<GameCore.AgentAttribute>(){
                    new GameCore.AgentAttribute_PlusLife(200),
                    new GameCore.AgentAttribute_IncreasedMovementSpeed(100),
                    new GameCore.AgentAttribute_IncreasedAttackRate(200),
                    new GameCore.AgentAttribute_EnhancedDamage(500),
                    new GameCore.AgentAttribute_LifeStolenPerHit(5),
                    new GameCore.AgentAttribute_LifeStolenPerKill(15),
                    new GameCore.AgentAttribute_LifeRegen(60),
                }
            );

            agentCtrl.Agent.equipment.EquipItem(GameCore.AgentEquipment.WornItemSlot.Test2, item);
        }

        static public void UnequipTestItem(AgentController agentCtrl, GameCore.AgentEquipment.WornItemSlot slot)
        {
            if (agentCtrl.Agent.equipment.IsSlotFree(slot)) return;

            agentCtrl.Agent.equipment.UnequipItem(slot);
        }
    }
}
