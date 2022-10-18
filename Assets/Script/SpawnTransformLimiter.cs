using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTransformLimiter : MonoBehaviour
{
    [SerializeField] private bool isInFront;
    [SerializeField] private Vector3 minPosition;
    [SerializeField] private Vector3 maxLocalPosition;

    /*private void Awake()
    {
        throw new NotImplementedException();
    }*/

    private void Update()
    {
        switch (isInFront)
        {
            case false when gameObject.transform.position.z < minPosition.z:
                gameObject.transform.position = new Vector3(gameObject.transform.parent.position.x,0,minPosition.z);
                return;
            case false:
            {
                if (gameObject.transform.position.z >= minPosition.z && gameObject.transform.localPosition.z >= maxLocalPosition.z)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.parent.position.x,0,minPosition.z);
                }

                break;
            }
            case true when gameObject.transform.position.z > minPosition.z:
                gameObject.transform.position = new Vector3(gameObject.transform.parent.position.x,0,minPosition.z);
                return;
            case true:
            {
                if (gameObject.transform.position.z <= minPosition.z && gameObject.transform.localPosition.z <= maxLocalPosition.z)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.parent.position.x,0,minPosition.z);
                }

                break;
            }
        }
    }
}
