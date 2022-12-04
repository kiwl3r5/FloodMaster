using System;
using System.Collections;
using System.Collections.Generic;
using MidniteOilSoftware;
using Script.Manager;
using Script.Player;
using UnityEngine;

public class SupplyDropBox : MonoBehaviour,IInteractable
{
    private void FixedUpdate()
    {
        if (GameManager.Instance.isEmptyPool)
        {
            ObjectPoolManager.DespawnGameObject(gameObject);
            ObjectPoolManager.EmptyPool();
            return;
        }
    }

    [Header("Interact")]
    [SerializeField] private bool eKeyEnable;
    [SerializeField] private string prompt;
    [SerializeField] private bool fKeyEnable;
    [SerializeField] private string prompt1;
    public string InteractionPrompt => prompt;
    public string InteractionPrompt1 => prompt1;
    public bool IsEKeyEnable => eKeyEnable;
    public bool IsFKeyEnable => fKeyEnable;

    public bool Interact(Interactor interactor)
    { 
        GachaFatlump.Instance.StartSupplyGacha();
        ObjectPoolManager.DespawnGameObject(gameObject);
        return false;
    }

    public bool Interact1(Interactor interactor)
    {
        return false;
    }
}
