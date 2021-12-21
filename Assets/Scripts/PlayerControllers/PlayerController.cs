using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cameraObject;
    [SerializeField] GameObject sprite;
    [SerializeField] CharacterController controller;
    Transform cam;

    Animator animator;

    // Player Movement
    float speed = 4f;

    [SerializeField] float height = 1f;
    float turnSmoothTime = 0.1f;
    float smoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 smoothVelocity = Vector3.zero;

    string currentState;

    string playerState;

    void Start()
    {
        cam = cameraObject.transform;
        animator = sprite.GetComponent<Animator>();
    }

    void Update()
    {
        // Get controller input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Camera and Player local rotation assignment
        float cameraLocalRotationY = cam.eulerAngles.y;
        float playerLocalRotationY = transform.eulerAngles.y;

        // Calculate the camera rotation relative to the player
        float cameraOrbit = (cameraLocalRotationY <= playerLocalRotationY ?
                             playerLocalRotationY - cameraLocalRotationY :
                             360 + playerLocalRotationY - cameraLocalRotationY);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraLocalRotationY;
            float angle = Mathf.SmoothDampAngle(playerLocalRotationY, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            float totalOffset = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            float speedModifier = totalOffset > 1 ? 1 : totalOffset;

            controller.Move(moveDir.normalized * speed * speedModifier * Time.deltaTime);
            transform.position = Vector3.SmoothDamp(transform.position,
                                                    new Vector3(transform.position.x, height, transform.position.z),
                                                    ref smoothVelocity, smoothTime);
            playerState = AnimationController.animations["walk"];
        }
        else
        {
            playerState = AnimationController.animations["idle"];
        }
        currentState = AnimationController.SpriteAnimationPerspective(cameraOrbit, playerState, animator, currentState);
    }
}