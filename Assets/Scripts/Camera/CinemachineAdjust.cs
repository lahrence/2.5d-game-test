using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineAdjust : MonoBehaviour
{
    public CinemachineFreeLook cinemachine;
    float targetAngleX;
    bool doSnapCamera = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate() {
        targetAngleX = Mathf.Round(cinemachine.m_XAxis.Value / 45.0f) * 45.0f;
        if (doSnapCamera) {
        cinemachine.m_XAxis.Value = Quaternion.Lerp(Quaternion.Euler(0, cinemachine.m_XAxis.Value, 0),
                                                    Quaternion.Euler(0, targetAngleX, 0), 
                                                    5 * Time.deltaTime).eulerAngles.y;
        }
    }
}
