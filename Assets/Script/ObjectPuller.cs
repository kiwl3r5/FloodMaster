using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPuller : MonoBehaviour
{

    public LayerMask whatIsManhole;
    [SerializeField] private float pullRange = 4;
    [SerializeField] private float pullForce = 3;
    //public bool manholeInRange;
    
    //public CollectiblesObj collectiblesObj;
    public ManholeManager manholeManager;
    //public Vector3 manholePosition;

    //public List<GameObject> attractedTo;
    
    private readonly Collider[] _ManholeColliders = new Collider[3];
    public int numFound;

    private void FixedUpdate()
    {
        numFound = Physics.OverlapSphereNonAlloc(transform.position, pullRange, _ManholeColliders,
            whatIsManhole);
            
        if (numFound>0)
        {
            manholeManager = _ManholeColliders[0].GetComponent<ManholeManager>();
            var direction = manholeManager.transform.position - transform.position;

            if (manholeManager != null)
            {
                if (!manholeManager.isFull) gameObject.GetComponent<Rigidbody>().AddForce (pullForce * direction);
            }
        }
        else
        {
            if (manholeManager != null) manholeManager = null;
        }
    }

    /*private void Start()
    {
        //collectiblesObj = GetComponent<CollectiblesObj>();
        var objects = GameObject.FindGameObjectsWithTag("manhole");
        //var objectCount = objects.Length;
        foreach (var obj in objects)
        {
            attractedTo.Add(obj);
        }
        manholeManager = attractedTo[0].GetComponent<ManholeManager>();
    }*/
    

    /*private void FixedUpdate ()
    {
        manholeInRange = Physics.CheckSphere(transform.position, pullRange, whatIsManhole);
        attractedTo = attractedTo.OrderBy(
            x => Vector3.Distance(this.transform.position,x.transform.position)
        ).ToList();
        manholeManager = attractedTo[0].GetComponent<ManholeManager>();
        if (!manholeInRange || manholeManager.isFull)
        {
            return;
        }
        Vector3 direction = attractedTo[0].transform.position - transform.position;
        gameObject.GetComponent<Rigidbody>().AddForce (pullForce * direction);
         
    }*/

    private void OnDrawGizmosSelected()
    {
        var position = transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(position, pullRange);
    }
}
