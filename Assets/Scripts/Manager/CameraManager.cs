using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera cinemachine;

    public void Init(Transform transform)
    {
        cinemachine = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        cinemachine.Follow = transform;
    }
}
