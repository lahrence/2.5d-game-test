using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOccludeDetection : MonoBehaviour {
    [SerializeField] Transform targetObject;
    [SerializeField] LayerMask wallMask;
    [SerializeField] Camera cameraObject;

    public bool isWallOccluded;

    void Update() {
        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);
        
        isWallOccluded = hitObjects.Length > 0;
    }
}