using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageAndSpUI : MonoBehaviour
{
    [SerializeField] private Image[] diffStage;
    [SerializeField] private Image supplyAlert;
    [SerializeField] private float supplyAlertDelay;
    private int currenUiDiffStage = 1;
    
    private static StageAndSpUI _instance;
    public static StageAndSpUI Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        if (LevelScaling.Instance.levelScale == 1 && currenUiDiffStage != 1)
        {
            currenUiDiffStage = 1;
            diffStage[0].transform.LeanMoveLocalX(-320, 0.5f).setEaseInExpo();
            diffStage[1].transform.LeanMoveLocalX(-320, 0.5f).setEaseInExpo();
            diffStage[2].transform.LeanMoveLocalX(-320, 0.5f).setEaseInExpo();
        }
        if (LevelScaling.Instance.levelScale == 2 && currenUiDiffStage != 2)
        {
            currenUiDiffStage = 2;
            diffStage[0].transform.LeanMoveLocalX(0, 0.5f).setEaseOutExpo();
            diffStage[1].transform.LeanMoveLocalX(-320, 0.5f).setEaseInExpo();
            diffStage[2].transform.LeanMoveLocalX(-320, 0.5f).setEaseInExpo();
        }
        if (LevelScaling.Instance.levelScale == 3 && currenUiDiffStage != 3)
        {
            currenUiDiffStage = 3;
            diffStage[0].transform.LeanMoveLocalX(-320, 0.5f).setEaseInExpo();
            diffStage[1].transform.LeanMoveLocalX(0, 0.5f).setEaseOutExpo();
            diffStage[2].transform.LeanMoveLocalX(-320, 0.5f).setEaseInExpo();
        }
        if (LevelScaling.Instance.levelScale == 4 && currenUiDiffStage != 4)
        {
            currenUiDiffStage = 4;
            diffStage[0].transform.LeanMoveLocalX(-320, 0.5f).setEaseInExpo();
            diffStage[1].transform.LeanMoveLocalX(-320, 0.5f).setEaseInExpo();
            diffStage[2].transform.LeanMoveLocalX(0, 0.5f).setEaseOutExpo();
        }
    }

    public IEnumerator SupplyAlert() // down330  up0
    {
        supplyAlert.transform.LeanMoveLocalY(0, 0.5f).setEaseOutExpo();
        yield return new WaitForSeconds(supplyAlertDelay);
        supplyAlert.transform.LeanMoveLocalY(330, 0.5f).setEaseInExpo();
    }
    
}
