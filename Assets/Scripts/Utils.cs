using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct MinMax<T> {
    public T min;
    public T max;
}

public static class Utils {
    static List<T> GetBehaviorsList<T>(Collider[] colls)
    {
        var foundList = new List<T>();
        for (int i = 0; i < colls.Length; i++)
        {
            var foundColl = colls[i].gameObject.GetComponentInParent<T>();
            if (foundColl != null) foundList.Add(foundColl);
        }

        return foundList;
    }

    public static float RandomValueWithDeviation(float value, float deviation)
    {
        if (deviation == 0) return value;

        var portion = value * deviation;

        return value + Random.Range(-portion, portion);
    }

    public static List<T> FindColliders<T>(Vector3 pos, float radius)
    {
        var colls = Physics.OverlapSphere(pos, radius);

        return GetBehaviorsList<T>(colls);
    }

    public static List<T> FindColliders<T>(Vector3 pos, float radius, int layerMask)
    {
        var colls = Physics.OverlapSphere(pos, radius, layerMask);

        return GetBehaviorsList<T>(colls);
    }

    public interface IWithTransform
    {
        Transform transform { get; }
    }

    public static List<T> SortByProximity<T>(List<T> items, Vector3 origin)
    where T : MonoBehaviour
    {
        var distances = new List<float>();

        for (int i = 0; i < items.Count; i++)
        {
            var dist = Vector3.Distance(origin, items[i].transform.position);

            distances.Add(dist);
        }

        var j = -1;

        return items.OrderBy(item =>
        {
            j += 1;

            return distances[j];
        }).ToList();
    }

    public static List<float> IntervalList(float min, float max, int steps)
    {
        var list = new List<float>();
        var length = max - min;
        var stepLength = length / (steps - 1);

        for (int i = 0; i < steps; i++)
        {
            var val = min + i * stepLength;

            list.Add(val);
        }

        return list;
    }

    public static List<WaitForSeconds> GetWaitForSecondsList(float min, float max, int steps)
    {
        var values = IntervalList(min, max, steps);
        var list = new List<WaitForSeconds>();

        for (int i = 0; i < steps; i++)
        {
            list.Add(new WaitForSeconds(values[i]));
        }

        return list;
    }
}