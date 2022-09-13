using Script.Manager;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private static EndTrigger _instance;
    public static EndTrigger Instance { get { return _instance; } }

    public GameObject winArea;
    private MeshRenderer _winAreaMeshRenderer;
    private Collider _winAreaCollider;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        winArea = gameObject;
        _winAreaMeshRenderer = winArea.GetComponent<MeshRenderer>();
        _winAreaCollider = winArea.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        HideWinArea(GameManager.Instance.isWinUnlock);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (GameManager.Instance.sceneNum < 2)
        {
            GameManager.Instance.LevelCompleteUI(true);
        }
        else
        {
            GameManager.Instance.GameWinUI(true);
        }
    }

    private void HideWinArea(bool isHide)
    {
        _winAreaMeshRenderer.enabled = isHide;
        _winAreaCollider.enabled = isHide;
    }
}
