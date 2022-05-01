using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
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

        public AgentAttributesCollection(List<AgentAttribute> attrs = null)
        {
            // @todo validate duplicates?

            if (attrs != null) AddClonedAttributes(attrs);
        }

        void AddClonedAttributes(List<AgentAttribute> attrs)
        {
            for (int i = 0; i < attrs.Count; i++)
            {
                attributes.Add(attrs[i].Clone());
            }
        }

        public override string ToString()
        {
            var lines = new List<string>();

            for (int i = 0; i < attributes.Count; i++)
            {
                var attr = attributes[i];

                lines.Add($"#{i + 1} {attr.Label}: {attr.Value}");
            }

            return string.Join(" | ", lines);
        }

        public void Add(AgentAttributesCollection attrsColl)
        {
            for (int i = 0; i < attrsColl.attributes.Count; i++)
            {
                var attrToAdd = attrsColl.attributes[i];

                AgentAttribute foundAttr = null;

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
                    attributes.Add(attrToAdd.Clone());
                }
            }
        }

        public void Remove(AgentAttributesCollection valuesToSubtractColl)
        {
            var attrsToRemove = new List<AgentAttribute>();

            for (int i = 0; i < valuesToSubtractColl.attributes.Count; i++)
            {
                var valueToSubtract = valuesToSubtractColl.attributes[i];

                AgentAttribute foundAttr = null;

                for (int j = 0; j < attributes.Count; j++)
                {
                    if (attributes[j].GetType() == valueToSubtract.GetType())
                    {
                        foundAttr = attributes[j];
                        break;
                    }
                }

                if (foundAttr != null)
                {
                    if (valueToSubtract.Value >= foundAttr.Value)
                    {
                        attrsToRemove.Add(foundAttr);
                    }
                    else
                    {
                        foundAttr.Decrement(valueToSubtract);
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
            AddClonedAttributes(attrsColl.attributes);
        }

        static public AgentAttributesCollection Sum(AgentAttributesCollection attrsCollA, AgentAttributesCollection attrsCollB)
        {
            var coll = new AgentAttributesCollection(
                attrs: attrsCollA.attributes
            );
            coll.Add(attrsCollB);

            return coll;
        }
    }
}