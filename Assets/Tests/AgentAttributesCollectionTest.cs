using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using GameCore;

namespace AgentAttributesCollectionTest
{
    public class Add
    {
        [Test]
        public void adds_non_existing_attrs_and_sums_values_of_existing_ones()
        {
            var attrsCollectionA = new AgentAttributesCollection(
                new List<AgentAttribute>(){
                    new AgentAttribute_PlusLife(20),
                    new AgentAttribute_IncreasedMovementSpeed(10),
                }
            );
            var attrsCollectionB = new AgentAttributesCollection(
                new List<AgentAttribute>(){
                    new AgentAttribute_IncreasedMovementSpeed(5),
                    new AgentAttribute_LifeRegen(6),
                }
            );

            attrsCollectionA.Add(attrsCollectionB);

            Assert.AreEqual(3, attrsCollectionA.attributes.Count);
            Assert.AreEqual(typeof(AgentAttribute_PlusLife), attrsCollectionA.attributes[0].GetType());
            Assert.AreEqual(20, attrsCollectionA.attributes[0].Value);
            Assert.AreEqual(typeof(AgentAttribute_IncreasedMovementSpeed), attrsCollectionA.attributes[1].GetType());
            Assert.AreEqual(15, attrsCollectionA.attributes[1].Value);
            Assert.AreEqual(typeof(AgentAttribute_LifeRegen), attrsCollectionA.attributes[2].GetType());
            Assert.AreEqual(6, attrsCollectionA.attributes[2].Value);
        }
    }

    public class Remove
    {
        [Test]
        public void removes_attr_if_value_to_subtract_is_GTE_current_value()
        {
            var attrsCollection = new AgentAttributesCollection(
                new List<AgentAttribute>(){
                    new AgentAttribute_PlusLife(20),
                    new AgentAttribute_IncreasedMovementSpeed(10),
                    new AgentAttribute_LifeRegen(6),
                }
            );
            var attrValuesToSubtract = new AgentAttributesCollection(
                new List<AgentAttribute>(){
                    new AgentAttribute_IncreasedMovementSpeed(10),
                }
            );

            attrsCollection.Remove(attrValuesToSubtract);

            Assert.AreEqual(2, attrsCollection.attributes.Count);
            Assert.AreEqual(typeof(AgentAttribute_PlusLife), attrsCollection.attributes[0].GetType());
            Assert.AreEqual(20, attrsCollection.attributes[0].Value);
            Assert.AreEqual(typeof(AgentAttribute_LifeRegen), attrsCollection.attributes[1].GetType());
            Assert.AreEqual(6, attrsCollection.attributes[1].Value);
        }

        [Test]
        public void subtracts_and_does_not_remove_attr_if_value_to_subtract_is_LT_current_value()
        {
            var attrsCollection = new AgentAttributesCollection(
                new List<AgentAttribute>(){
                    new AgentAttribute_PlusLife(20),
                    new AgentAttribute_IncreasedMovementSpeed(10),
                    new AgentAttribute_LifeRegen(6),
                }
            );
            var attrValuesToSubtract = new AgentAttributesCollection(
                new List<AgentAttribute>(){
                    new AgentAttribute_IncreasedMovementSpeed(2),
                }
            );

            attrsCollection.Remove(attrValuesToSubtract);

            Assert.AreEqual(3, attrsCollection.attributes.Count);
            Assert.AreEqual(typeof(AgentAttribute_PlusLife), attrsCollection.attributes[0].GetType());
            Assert.AreEqual(20, attrsCollection.attributes[0].Value);
            Assert.AreEqual(typeof(AgentAttribute_IncreasedMovementSpeed), attrsCollection.attributes[1].GetType());
            Assert.AreEqual(8, attrsCollection.attributes[1].Value);
            Assert.AreEqual(typeof(AgentAttribute_LifeRegen), attrsCollection.attributes[2].GetType());
            Assert.AreEqual(6, attrsCollection.attributes[2].Value);
        }
    }

    public class Replace
    {
        [Test]
        public void removes_all_existing_attributes_and_adds_all_the_given_ones()
        {
            var attrsCollectionA = new AgentAttributesCollection(
                new List<AgentAttribute>(){
                    new AgentAttribute_PlusLife(20),
                    new AgentAttribute_IncreasedMovementSpeed(10),
                    new AgentAttribute_LifeRegen(6),
                }
            );
            var attrsCollectionB = new AgentAttributesCollection(
                new List<AgentAttribute>(){
                    new AgentAttribute_IncreasedMovementSpeed(10),
                }
            );

            attrsCollectionA.Replace(attrsCollectionB);

            Assert.AreEqual(1, attrsCollectionA.attributes.Count);
            Assert.AreEqual(typeof(AgentAttribute_IncreasedMovementSpeed), attrsCollectionA.attributes[0].GetType());
            Assert.AreEqual(10, attrsCollectionA.attributes[0].Value);
        }
    }

    public class Sum
    {
        [Test]
        public void creates_non_existing_attrs_and_adds_existing_ones()
        {
            var attrsCollectionA = new AgentAttributesCollection(
                new List<AgentAttribute>(){
                    new AgentAttribute_PlusLife(20),
                    new AgentAttribute_IncreasedMovementSpeed(10),
                }
            );
            var attrsCollectionB = new AgentAttributesCollection(
                new List<AgentAttribute>(){
                    new AgentAttribute_IncreasedMovementSpeed(5),
                    new AgentAttribute_LifeRegen(6),
                }
            );

            var resAttrsColl = AgentAttributesCollection.Sum(attrsCollectionA, attrsCollectionB);

            Assert.AreEqual(3, resAttrsColl.attributes.Count);
            Assert.AreEqual(typeof(AgentAttribute_PlusLife), resAttrsColl.attributes[0].GetType());
            Assert.AreEqual(20, resAttrsColl.attributes[0].Value);
            Assert.AreEqual(typeof(AgentAttribute_IncreasedMovementSpeed), resAttrsColl.attributes[1].GetType());
            Assert.AreEqual(15, resAttrsColl.attributes[1].Value);
            Assert.AreEqual(typeof(AgentAttribute_LifeRegen), resAttrsColl.attributes[2].GetType());
            Assert.AreEqual(6, resAttrsColl.attributes[2].Value);
        }
    }
}
