using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class LevelScaling : MonoBehaviour
{
    public int levelScale = 1;
    [SerializeField] private float floodPercentage;
    public float mapPercentage;

    private static LevelScaling _instance;
    public static LevelScaling Instance { get { return _instance; } }

    private void Awake()
    {
        mapPercentage = GameManager.Instance.minimapProgress.value / GameManager.Instance.minimapProgress.maxValue*100;
        _instance = this;
    }

    private void Update()
    {
        mapPercentage = GameManager.Instance.minimapProgress.value / GameManager.Instance.minimapProgress.maxValue*100;
        floodPercentage = FloodSystem.Instance.floodPoint;
        
        /* E = floodPercentage <= 25
         * F = floodPercentage is > 25 and <= 50
         * G = floodPercentage is > 50 and <= 75
         * H = floodPercentage > 75
         * A = mapPercentage <= 25
         * B = mapPercentage is > 25 and <= 50
         * C = mapPercentage is > 50 and <= 75
         * D = mapPercentage > 75
         */
        //# (1) #
        if (floodPercentage <= 25 && mapPercentage <= 25) //EA
        {
            levelScale = 1;
        }
        //# (2) #
        if (floodPercentage <= 25 && mapPercentage is > 25 and <= 50 //EB
            || floodPercentage <= 25 && mapPercentage is > 50 and <= 75 //EC
            || floodPercentage is > 25 and <= 50 && mapPercentage <= 25 //FA
            || floodPercentage is > 25 and <= 50 && mapPercentage is > 25 and <= 50 //FB
            || floodPercentage is > 25 and <= 50 && mapPercentage is > 50 and <= 75 //FC
            || floodPercentage is > 50 and <= 75 && mapPercentage <= 25 //GA
            || floodPercentage is > 50 and <= 75 && mapPercentage is > 25 and <= 50) //GB
        {
            levelScale = 2;
        }
        //# (3) #
        if (floodPercentage <= 25 && mapPercentage > 75 //ED
            || floodPercentage is > 25 and <= 50 && mapPercentage > 75 //FD
            || floodPercentage is > 50 and <= 75 && mapPercentage is > 50 and <= 75 //GC
            || floodPercentage > 75 && mapPercentage <= 25 //HA
            || floodPercentage > 75 && mapPercentage is > 25 and <= 50 //HB
            || floodPercentage is > 50 and <= 75 && mapPercentage > 75 )//GD
        {
            levelScale = 3;
        }
        //# (4) #
        if (floodPercentage > 75 && mapPercentage is > 50 and <= 75 //HC
            || floodPercentage > 75 && mapPercentage > 75) //HD
        {
            levelScale = 4;
        }
    }
}
