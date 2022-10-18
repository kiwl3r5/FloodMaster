using System.Collections;
using System.Collections.Generic;
using Script.Player;
using UnityEngine;

public class DeathObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.Instance.OnTakeDamage(100);
        }
    }
}
