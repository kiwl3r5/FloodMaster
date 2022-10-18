using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFoce : MonoBehaviour
{
    public float thrust = 6;
    public Rigidbody rb;
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.AddForce(0,0,thrust);
    }
}
