using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public GameObject target;
    FollowMouse followMouse;
    [SerializeField] CharacterController controller;
    float speed = 125f;
    Vector3[] path;
    public int targetIndex;
    float margin = 0.075f;
    string playerState;
    public bool isTracking = false;
    PlayerController pc;
    AnimationManager animationManager;
    PlayerSpriteController playerSC;
    [SerializeField] GameObject sprite;

    void Awake() {
        followMouse = target.GetComponent<FollowMouse>();
        animationManager = sprite.GetComponent<AnimationManager>();
        playerSC = sprite.GetComponent<PlayerSpriteController>();
        pc = GetComponent<PlayerController>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)
            && followMouse.direction.magnitude < 0.1f) {
            PathRequestManager.RequestPath(transform.position,
                                           target.transform.position,
                                           OnPathFound);
        }

        if (followMouse.direction.magnitude >= 0.1f) {
            isTracking = false;
        }
        followMouse.isTracking = isTracking;
        if (isTracking) {
            pc.speedModifier = 1.5f;
            playerSC.currentState = animationManager.PlayAnimation("walk",
                                                                   "topHat");
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        followMouse.success = pathSuccessful;
        if (pathSuccessful && Vector3.Distance(target.transform.position, transform.position) <= followMouse.PathLengthLimit) {
            path = newPath;
            followMouse.pathLength = newPath.Length;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        Vector3 currentWaypoint = (path.Length > 0 ?
                                   path[0] :
                                   transform.position);
        isTracking = true;
        while (true) {
            if (!isTracking) {
                targetIndex = 0;
                path = new Vector3[0];
                yield break;
            }
            float testVectorX = currentWaypoint.x - transform.position.x;
            float testVectorY = currentWaypoint.z - transform.position.z;
            if ((testVectorX < margin && testVectorX > -margin)
                && (testVectorY < margin && testVectorY > -margin)) {
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
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  rotation,
                                                  Time.deltaTime * 25f);

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
