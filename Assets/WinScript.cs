using System.Collections;
using System.Collections.Generic;
using System;
using Script.Manager;
using Script.Player;
using UnityEngine;

public class WinScript : MonoBehaviour, IInteractable
{
    public static WinScript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.minimapProgress.maxValue = Vector3.Distance (PlayerLocomotion.Instance.transform.position, transform.position);
    }

    private void Update()
    {
        GameManager.Instance.endLvDistance = Vector3.Distance (PlayerLocomotion.Instance.transform.position, transform.position);
        GameManager.Instance.minimapProgress.value = GameManager.Instance.minimapProgress.maxValue-GameManager.Instance.endLvDistance;
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
        GameManager.Instance.GameWinUI(true);
        return false;
    }

    public bool Interact1(Interactor interactor)
    {
        return false;
    }
}
