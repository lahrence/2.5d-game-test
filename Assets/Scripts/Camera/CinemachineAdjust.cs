using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineAdjust : MonoBehaviour {
    public CinemachineFreeLook cinemachine;
    [SerializeField] Camera cameraObject;
    WallOccludeDetection wallOccludeDetection;

    public bool allowOrbit = true;

    void Start() {
        wallOccludeDetection = cameraObject.GetComponent<WallOccludeDetection>();
    }

    void LateUpdate() {
        if (allowOrbit) {
            float currentRotation = cinemachine.m_XAxis.Value;
            float rotateValue = cinemachine.m_XAxis.Value;
            if (Input.GetButtonDown("Left Bumper")) {
                rotateValue = cinemachine.m_XAxis.Value + 45f;
            } else if (Input.GetButtonDown("Right Bumper")) {
                rotateValue = cinemachine.m_XAxis.Value - 45f;
            }

            rotateValue = Mathf.Round(rotateValue / 45.0f) * 45.0f;

            cinemachine.m_XAxis.Value = Quaternion.Lerp(Quaternion.Euler(0, currentRotation , 0),
                                                         Quaternion.Euler(0, rotateValue, 0),
                                                         0.825f).eulerAngles.y;
        }
    }
}