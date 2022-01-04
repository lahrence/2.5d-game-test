using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour {
    public Camera cameraObject;
    [SerializeField] GameObject sprite;
    [SerializeField] CharacterController controller;
    Transform cam;
    public Boolean turnLock = true;

    Animator animator;

    // Player Movement
    public float speed = 1.5f;

    float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;

    public string currentState;

    public string playerState;

    public float isCollided = 1f;
    CapsuleCollider capsuleCollider;
    Unit unit;
    bool isTracking;
    public float speedModifier;

    AnimationManager animationManager;

    void Start() {
        cam = cameraObject.transform;
        animator = sprite.GetComponent<Animator>();
        animationManager = sprite.GetComponent<AnimationManager>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        unit = GetComponent<Unit>();
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");
    }

    void Update() {
        // Get controller input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float modifier = Input.GetAxisRaw("Multiplier");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Camera and Player local rotation assignment
        float cameraLocalRotationY = cam.eulerAngles.y;
        float playerLocalRotationY = transform.eulerAngles.y;

        // Player movement
        if (direction.magnitude >= 0.1f && !unit.isTracking) {
            //rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraLocalRotationY;
            if (turnLock) targetAngle = Mathf.Round(targetAngle/45.0f) * 45.0f;
            float angle = Mathf.SmoothDampAngle(playerLocalRotationY, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //movement
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float totalOffset = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            if (modifier < 0.1f) speedModifier = totalOffset > 1 ? 1 : totalOffset;
            else speedModifier = totalOffset > 1 ? 1.5f : totalOffset * (modifier + 0.5f);

            Vector3 moveVector = moveDir.normalized * speed * speedModifier;
            moveVector.x = Mathf.Round(moveVector.x) * 0.01f;
            moveVector.y = Mathf.Round(moveVector.y) * 0.01f;
            moveVector.z = Mathf.Round(moveVector.z) * 0.01f;

            controller.SimpleMove(moveDir.normalized * speed * speedModifier);
            currentState = animationManager.PlayAnimation("walk", "topHat");
        } else if (direction.magnitude < 0.1f && !unit.isTracking) {
            controller.SimpleMove(new Vector3(0, 0, 0));
            //print(speedModifier);
            currentState = animationManager.PlayAnimation("idle", "topHat");

        }
        
        if (Input.GetButtonDown("Interact")) {
            print("A!");
        }
    }

    void OnTriggerEnter(Collider other) {
        isCollided = 0f;
    }

    void OnTriggerExit(Collider other) {
        isCollided = 1f;
    }
}