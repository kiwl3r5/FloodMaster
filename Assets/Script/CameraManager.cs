using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
    public float sensitivity;
    
    private static CameraManager _instance;
    public static CameraManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        _cinemachineFreeLook.m_YAxis.m_MaxSpeed = sensitivity;
        _cinemachineFreeLook.m_XAxis.m_MaxSpeed = sensitivity * 100;
    }
}
