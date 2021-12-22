using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineAdjust : MonoBehaviour
{
    public CinemachineFreeLook cinemachine;
    [SerializeField] Camera cameraObject;
    WallOccludeDetection wallOccludeDetection;
    // Start is called before the first frame update
    void Start()
    {
        wallOccludeDetection = cameraObject.GetComponent<WallOccludeDetection>();
    }

    // Update is called once per frame
    void LateUpdate() {
        float targetAngle = Mathf.Round(cinemachine.m_XAxis.Value / 45.0f) * 45.0f;
        cinemachine.m_XAxis.Value = Quaternion.Lerp(Quaternion.Euler(0, cinemachine.m_XAxis.Value, 0),
                                                    Quaternion.Euler(0, targetAngle, 0), 
                                                    5 * Time.deltaTime).eulerAngles.y;
    }
}
