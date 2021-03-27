using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MinMax<T> {
    public T min;
    public T max;
}

public static class Utils {
    public static List<T> FindColliders<T>(Vector3 pos, float radius) {
        var colls = Physics.OverlapSphere(pos, radius);

        var foundList = new List<T>();
        for (int i = 0; i < colls.Length; i++) {
            var foundColl = colls[i].gameObject.GetComponentInParent<T>();
            if (foundColl != null) foundList.Add(foundColl);
        }

        return foundList;
    }

    public static List<float> IntervalList(float min, float max, int steps) {
        var list = new List<float>();
        var length = max - min;
        var stepLength = length / (steps - 1);

        for (int i = 0; i < steps; i++) {
            var val = min + i * stepLength;

            list.Add(val);
        }

        return list;
    }

    public static List<WaitForSeconds> GetWaitForSecondsList(float min, float max, int steps) {
        var values = IntervalList(min, max, steps);
        var list = new List<WaitForSeconds>();

        for (int i = 0; i < steps; i++) {
            list.Add(new WaitForSeconds(values[i]));
        }

        return list;
    }
}