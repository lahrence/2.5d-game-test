using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public GameObject target;
    FollowMouse followMouse;
    [SerializeField] CharacterController controller;
    float speed = 125f;
    Vector3[] path;
    int targetIndex;
    float margin = 0.075f;
    string playerState;
    public bool isTracking = false;
    PlayerController playerController;
    AnimationManager animationManager;
    [SerializeField] GameObject sprite;

    void Awake() {
        followMouse = target.GetComponent<FollowMouse>();
        animationManager = sprite.GetComponent<AnimationManager>();
        playerController = GetComponent<PlayerController>();
    }

    void Update() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetMouseButtonDown(0)) {
            PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
        }

        if (direction.magnitude >= 0.1f) {
            isTracking = false;
        } else if (isTracking) {
            playerController.speedModifier = 1;
            playerController.currentState = animationManager.PlayAnimation("walk", "topHat");
        }

        //print(isTracking);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if (pathSuccessful) {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
        print("Success Original: " + pathSuccessful);
    }
    
    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];
        isTracking = true;
        while (true) {
            if (!isTracking) {
                targetIndex = 0;
                path = new Vector3[0];
                isTracking = false;
                yield break;
            }
            float testVectorX = currentWaypoint.x - transform.position.x;
            float testVectorY = currentWaypoint.z - transform.position.z;
            if ((testVectorX < margin && testVectorX > -margin) && (testVectorY < margin && testVectorY > -margin)) {
                targetIndex++;
                if ((targetIndex >= path.Length)) {
                    targetIndex = 0;
                    path = new Vector3[0];
                    isTracking = false;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            Vector3 lookPos = currentWaypoint - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 25f);

            Vector3 moveSpeed = transform.forward * speed * 0.02f;
            controller.SimpleMove(moveSpeed);
            yield return null;
        }
    }

    public void OnDrawGizmos() {
        if (path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(0.1f, 0.1f, 0.1f));

                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
