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
    }
}
