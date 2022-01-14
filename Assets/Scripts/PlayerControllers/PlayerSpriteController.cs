using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSpriteController : MonoBehaviour {
    PlayerController playerController;

    AnimationManager animationManager;
    Animator animator;

    float cameraOrbit;
    [HideInInspector] public string currentState;

    Camera cam;
    [SerializeField] private GameObject player;
    Unit unit;
    bool isTracking;
    bool isMoving;
    [SerializeField] GameObject selector;
    FollowMouse followMouse;
    Vector3 position;
    public event EventHandler<OnAngleChangeEventArgs> OnAngleChange;
    public class OnAngleChangeEventArgs : EventArgs {
        public float angle;
        public string currentState;
    }

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
        
        Vector3 relativePos = position - player.transform.position;

        transform.rotation = Quaternion.Euler(0f,
                                              cameraLocalRotationY,
                                              0f);
        if (!isTracking && !isMoving && !followMouse.controllerMode) {
            float checkPlayerPositionX = Mathf.Ceil(player.transform.position.x) - 0.5f;
            float checkPlayerPositionZ = Mathf.Ceil(player.transform.position.z) - 0.5f;
            if (!(checkPlayerPositionX == position.x &&
                  checkPlayerPositionZ == position.z)) {
                float rotation = Quaternion.LookRotation(relativePos, Vector3.up).eulerAngles.y - 45;
                cameraOrbit = (rotation - cameraLocalRotationY < 0
                               ? 360 + rotation - cameraLocalRotationY + 45
                               : rotation - cameraLocalRotationY + 45);
            }
        } else {
            cameraOrbit = (cameraLocalRotationY <= playerLocalRotationY
                           ? playerLocalRotationY - cameraLocalRotationY
                           : playerLocalRotationY - cameraLocalRotationY + 360);
        }
        OnAngleChange?.Invoke(this, new OnAngleChangeEventArgs {angle = cameraOrbit, currentState = currentState});
    }
}
