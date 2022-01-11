using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour {
    PlayerController playerController;

    AnimationManager animationManager;
    Animator animator;

    public float cameraOrbit;
    public string currentState;

    private Camera cam;
    [SerializeField] private GameObject player;
    Unit unit;
    bool isTracking;
    bool isMoving;
    [SerializeField] GameObject selector;
    FollowMouse followMouse;
    Vector3 position;

    void Start() {
        animationManager = GetComponent<AnimationManager>();
        animator = GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
        cam = playerController.cameraObject;
        unit = player.GetComponent<Unit>();
        followMouse = selector.GetComponent<FollowMouse>();
    }

    void Update() {
        isTracking = unit.isTracking;
        isMoving = playerController.isMoving;
        position = followMouse.position;
        float cameraLocalRotationY = cam.transform.eulerAngles.y;
        float playerLocalRotationY = player.transform.eulerAngles.y;
        cameraOrbit = (cameraLocalRotationY <= playerLocalRotationY
                       ? playerLocalRotationY - cameraLocalRotationY
                       : playerLocalRotationY - cameraLocalRotationY + 360);
        
        Vector3 relativePos = position - transform.position;

        transform.rotation = Quaternion.Euler(0f,
                                              cameraLocalRotationY,
                                              0f);
        if (!isTracking && !isMoving) {
            float rotation = Quaternion.LookRotation(relativePos, Vector3.up).eulerAngles.y;
            if (player.transform.position.x != position.x &&
                player.transform.position.z != position.z) {
                cameraOrbit = 315 - (rotation <= transform.rotation.y
                                     ? transform.rotation.y - rotation
                                     : transform.rotation.y - rotation + 360);
            }
        }
    }
}
