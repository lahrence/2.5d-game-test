using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] Transform player;
    [SerializeField] float smoothTime;
    Vector3 followTransform;
    Vector3 currentVelocity;

    void Update() {
        followTransform.x = player.transform.position.x - 10;
        followTransform.y = transform.position.y;
        followTransform.z = player.transform.position.z - 10;
        transform.position = Vector3.SmoothDamp(transform.position, followTransform, ref currentVelocity, smoothTime, Mathf.Infinity);
    }
}
