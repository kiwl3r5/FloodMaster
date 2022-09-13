using Script.Manager;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private Outline _objectiveOutline;

    private void Awake()
    {
        _objectiveOutline = GetComponent<Outline>();
    }

    private void Update()
    {
        _objectiveOutline.enabled = GameManager.Instance.isWinUnlock;
    }
}
