using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public abstract class AgentAttribute
    {
        // PlusLife,
        // EnhancedDamage,

        public abstract string Label { get; }

        private int value;
        public int Value => value;

        public AgentAttribute(int value)
        {
            this.value = value;
        }

        public void Increment(AgentAttribute agentAttribute)
        {
            this.value += agentAttribute.value;
        }

        public void Decrement(AgentAttribute agentAttribute)
        {
            this.value -= agentAttribute.value;
        }
    }

    // public abstract class AgentAttribute_IntValue : AgentAttribute
    // {

    // }

    public class AgentAttribute_PlusLife : AgentAttribute
    {
        public override string Label => "life";

        public AgentAttribute_PlusLife(int value) : base(value) {}
    }

    public class AgentAttribute_IncreasedMovementSpeed : AgentAttribute
    {
        public override string Label => "increased movement speed";

        public AgentAttribute_IncreasedMovementSpeed(int value) : base(value) {}
    }

    public class AgentAttribute_IncreasedAttackRate : AgentAttribute
    {
        public override string Label => "increased attack rate";

        public AgentAttribute_IncreasedAttackRate(int value) : base(value) {}
    }

    public class AgentAttribute_EnhancedDamage : AgentAttribute
    {
        public override string Label => "enhanced damage";

        public AgentAttribute_EnhancedDamage(int value) : base(value) {}
    }

    public class AgentAttribute_LifeRegen : AgentAttribute
    {
        public override string Label => "life regeneration";

        public AgentAttribute_LifeRegen(int value) : base(value) {}
    }

    public class AgentAttribute_LifeStolenPerHit : AgentAttribute
    {
        public override string Label => "life stolen per hit";

        public AgentAttribute_LifeStolenPerHit(int value) : base(value) {}
    }

    public class AgentAttribute_LifeStolenPerKill : AgentAttribute
    {
        public override string Label => "life stolen per kill";

        public AgentAttribute_LifeStolenPerKill(int value) : base(value) {}
    }

    public class AgentAttributesCollection
    {
        public readonly List<AgentAttribute> attributes = new List<AgentAttribute>();

        // private float plusLife;
        // private float enhancedDamage;
        // private float increasedAttackRate;
        // // private float increasedCastRate;
        // private float increasedMovementSpeed;
        // private float lifeStolenPerHit;
        // private float lifeStolenPerKill;
        // private float lifeRegeneration;
        // private float plusMagicFind;

        public AgentAttributesCollection(List<GameCore.AgentAttribute> attrs = null)
        {
            // @todo validate duplicates?

            if (attrs != null) attributes.AddRange(attrs);
        }

        public void Add(AgentAttributesCollection attrsColl)
        {
            for (int i = 0; i < attrsColl.attributes.Count; i++)
            {
                var attrToAdd = attrsColl.attributes[i];

                GameCore.AgentAttribute foundAttr = null;

                for (int j = 0; j < attributes.Count; j++)
                {
                    if (attributes[j].GetType() == attrToAdd.GetType())
                    {
                        foundAttr = attributes[j];
                        break;
                    }
                }

                if (foundAttr != null)
                {
                    foundAttr.Increment(attrToAdd);
                }
                else
                {
                    attributes.Add(attrToAdd);
                }
            }
        }

        public void Remove(AgentAttributesCollection attrsColl)
        {
            var attrsToRemove = new List<GameCore.AgentAttribute>();

            for (int i = 0; i < attrsColl.attributes.Count; i++)
            {
                var attrToRemove = attrsColl.attributes[i];

                GameCore.AgentAttribute foundAttr = null;

                for (int j = 0; j < attributes.Count; j++)
                {
                    if (attributes[i].GetType() == attrToRemove.GetType())
                    {
                        foundAttr = attributes[i];
                        break;
                    }
                }

                if (foundAttr != null)
                {
                    if (attrToRemove.Value >= foundAttr.Value)
                    {
                        attrsToRemove.Add(foundAttr);
                    }
                    else
                    {
                        foundAttr.Decrement(attrToRemove);
                    }
                }
            }

            for (int i = 0; i < attrsToRemove.Count; i++)
            {
                attributes.Remove(attrsToRemove[i]);
            }
        }

        public void Replace(AgentAttributesCollection attrsColl)
        {
            attributes.Clear();
            attributes.AddRange(attrsColl.attributes);
        }

        public AgentAttributesCollection SumWith(AgentAttributesCollection attrsColl)
        {
            var attrs = new List<AgentAttribute>(attributes);
            attrs.AddRange(attrsColl.attributes);

            var coll = new AgentAttributesCollection(
                attrs: attrs
            );

            return coll;
        }
    }

    public class AgentCardEquipment
    {
        private AgentCard agentCard;
        private AgentEquipment agentEquipment;

        public event System.Action combinedAttrsChanged;

        public AgentCardEquipment(
            AgentCard agentCard,
            AgentEquipment agentEquipment
        )
        {
            this.agentCard = agentCard;
            this.agentEquipment = agentEquipment;

            agentEquipment.itemEquipped += OnItemEquipped;
            agentEquipment.itemUnequipped += OnItemUnequipped;
        }

        void OnItemEquipped(Item item)
        {
            // Debug.Log($"OnItemEquipped: {item}");

            UpdateCombinedAttrs();
        }

        void OnItemUnequipped(Item item)
        {
            UpdateCombinedAttrs();
        }

        void UpdateCombinedAttrs()
        {
            agentCard.combinedAttrsCollection.Replace(agentCard.attrsCollection.SumWith(agentEquipment.wornItemsAttrsCollection));

            combinedAttrsChanged?.Invoke();
        }
    }

    public class AgentCard
    {
        private float life;

        public readonly AgentAttributesCollection attrsCollection = new AgentAttributesCollection();
        public readonly AgentAttributesCollection combinedAttrsCollection = new AgentAttributesCollection();
    }
}