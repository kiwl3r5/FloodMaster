using System;
using Script.Player;
using UnityEngine;
using UnityEngine.Serialization;

public class ManholeManager : MonoBehaviour, IInteractable
{
    public float maxCapacity = 10;
    public float currentCapacity;
    public float capacity;

    public bool isFull = false;
    [SerializeField] private GameObject cloggedTrash;
    [SerializeField] private GameObject vortex;
    [SerializeField] private GameObject vortexStop;
    [SerializeField] private float vortexPosOffsetY = 0.1f;
    private void Start()
    {
        capacity = maxCapacity;
        cloggedTrash.transform.localScale = new(currentCapacity/10, currentCapacity/10, currentCapacity/10);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var vortexTransform = vortex.transform.position;
        var vortexNewPosition = new Vector3(vortexTransform.x, FloodSystem.Instance.water.transform.position.y+vortexPosOffsetY, vortexTransform.z);
        vortex.transform.position = vortexNewPosition;
        vortexStop.transform.position = vortexNewPosition;
        if (currentCapacity >= capacity)
        {
            vortex.SetActive(false);
            vortexStop.SetActive(true);
        }
        else
        {
            vortex.SetActive(true);
            vortexStop.SetActive(false);
        }
        
    }

    public void CloggedTrashSize()
    {
        cloggedTrash.transform.localScale = new(currentCapacity/10, currentCapacity/10, currentCapacity/10);
    }
    
    public void CheckFull()
    {
        if (currentCapacity >= capacity)
        {
            isFull = true;
        }
        else
        {
            isFull = false;
        }
    }

    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    public bool Interact(Interactor interactor)
    {
        Debug.Log("ClearingSewer");
        return true;
    }
}
