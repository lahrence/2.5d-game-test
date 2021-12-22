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

    [SerializeField] float height = 1f;
    float turnSmoothTime = 0.05f;
    float smoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 smoothVelocity = Vector3.zero;

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

            controller.Move(moveDir.normalized * speed * speedModifier * Time.deltaTime);
            transform.position = Vector3.SmoothDamp(transform.position,
                                                    new Vector3(transform.position.x, height, transform.position.z),
                                                    ref smoothVelocity, smoothTime);
            playerState = AnimationController.animations["topHat"]["walk"];
        }
        else
        {
            playerState = AnimationController.animations["topHat"]["idle"];
        }
        currentState = AnimationController.SpriteAnimationPerspective(cameraOrbit, playerState, animator, currentState, speedModifier * isCollided);
    }

    void OnTriggerEnter(Collider other)
    {
        isCollided = 0f;
    }

    void OnTriggerExit(Collider other)
    {
        isCollided = 1f;
    }

    //private int WallCollide() {
        //Physics.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0f, Vector3.forward, .1f, )
    //}
}