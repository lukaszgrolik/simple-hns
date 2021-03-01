using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
    public bool tilt = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        var forward = Camera.main.transform.forward;

        if (tilt) {
            transform.forward = forward;
        }
        else {
            transform.forward = new Vector3(forward.x, 0, forward.z);
        }
    }
}
