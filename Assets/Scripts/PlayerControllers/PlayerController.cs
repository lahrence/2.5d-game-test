using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public Camera cameraObject;
    [SerializeField] GameObject sprite;
    [SerializeField] CharacterController controller;
    Transform cam;
    public float lockAngle;

    Animator animator;

    // Player Movement
    float speed = 3f;

    float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;

    string currentState;

    string playerState;

    float isCollided = 1f;
    CapsuleCollider capsuleCollider;

    void Start()
    {
        cam = cameraObject.transform;
        animator = sprite.GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");

        // Get controller input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float modifier = Input.GetAxisRaw("Multiplier");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Camera and Player local rotation assignment
        float cameraLocalRotationY = cam.eulerAngles.y;
        float playerLocalRotationY = transform.eulerAngles.y;

        // Calculate the camera rotation relative to the player
        float cameraOrbit = (cameraLocalRotationY <= playerLocalRotationY ?
                             playerLocalRotationY - cameraLocalRotationY :
                             360 + playerLocalRotationY - cameraLocalRotationY);

        float speedModifier = 1f;

        // Player movement
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraLocalRotationY;
            lockAngle = Mathf.Round(targetAngle/45.0f) * 45.0f;
            float angle = Mathf.SmoothDampAngle(playerLocalRotationY, lockAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            float totalOffset = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            if (modifier < 0.1f) speedModifier = totalOffset > 1 ? 1 : totalOffset;
            else speedModifier = totalOffset > 1 ? 1.5f : totalOffset * (modifier + 0.5f);

            controller.SimpleMove(moveDir.normalized * speed * speedModifier);

            playerState = AnimationController.animations["topHat"]["walk"];
        }
        else
        {
            playerState = AnimationController.animations["topHat"]["idle"];
        }
        
        currentState = AnimationController.SpriteAnimationPerspective(cameraOrbit, playerState, animator, currentState, speedModifier * isCollided);
        
        if (NPCDialogueController.NPCDetect(npcObjects, gameObject) != null && Input.GetAxisRaw("Interact") >= 0.1f) {
            print("A!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isCollided = 0f;
    }

    void OnTriggerExit(Collider other)
    {
        isCollided = 1f;
    }
}