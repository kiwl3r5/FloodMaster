using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterExtended : MonoBehaviour
{

    // Update is called once per frame
    private void Update()
    {
        transform.position= new Vector3(transform.position.x,FloodSystem.Instance.floodPoint/100*1.2f,transform.position.z);
    }
}
