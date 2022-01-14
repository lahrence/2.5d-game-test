using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Camera cameraObject;
    [SerializeField] GameObject sprite;
    [SerializeField] CharacterController controller;
    PlayerSpriteController playerSC;
    Transform cam;
    public bool turnLock = true;

    Animator animator;

    // Player Movement
    public float speed = 1.5f;

    float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;

    CapsuleCollider capsuleCollider;
    Unit unit;
    bool isTracking;
    public float speedModifier;
    public bool isMoving;

    AnimationManager animationManager;

    void Start() {
        cam = cameraObject.transform;
        animator = sprite.GetComponent<Animator>();
        animationManager = sprite.GetComponent<AnimationManager>();
        playerSC = sprite.GetComponent<PlayerSpriteController>();
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

        if (direction.magnitude >= 0.1f && !unit.isTracking) {
            // Rotation
            float targetAngle = (Mathf.Atan2(direction.x, direction.z)
                                 * Mathf.Rad2Deg
                                 + cameraLocalRotationY);
            if (turnLock) targetAngle = Mathf.Round(targetAngle/45.0f) * 45.0f;
            float angle = Mathf.SmoothDampAngle(playerLocalRotationY,
                                                targetAngle,
                                                ref turnSmoothVelocity,
                                                turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Movement
            Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f)
                               * Vector3.forward);
            float totalOffset = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            if (modifier < 0.1f) {
                speedModifier = totalOffset > 1 ? 1 : totalOffset;
            }
            else {
                speedModifier = (totalOffset > 1
                                 ? 1.5f
                                 : totalOffset * (modifier + 0.5f));
            }
            isMoving = true;
            
            controller.SimpleMove(moveDir.normalized * speed * speedModifier);
            playerSC.currentState = animationManager.PlayAnimation("walk",
                                                                   "topHat");
        } else if (direction.magnitude < 0.1f && !unit.isTracking) {
            controller.SimpleMove(new Vector3(0, 0, 0));
            playerSC.currentState = animationManager.PlayAnimation("idle",
                                                                   "topHat");
            isMoving = false;
        } else {
            isMoving = false;
        }
        transform.position = new Vector3(transform.position.x, 0.75f, transform.position.z);
    }
}
