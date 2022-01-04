using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour {
    private Camera cam;
    PlayerController playerController;
    [SerializeField] private GameObject player;

    AnimationManager animationManager;
    Animator animator;
    public float cameraOrbit;

    void Start() {
        animationManager = GetComponent<AnimationManager>();
        animator = GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
        cam = playerController.cameraObject;
    }

    void Update() {
        float cameraLocalRotationY = transform.eulerAngles.y;
        float playerLocalRotationY = player.transform.eulerAngles.y;
        cameraOrbit = (cameraLocalRotationY <= playerLocalRotationY ?
                             playerLocalRotationY - cameraLocalRotationY :
                             360 + playerLocalRotationY - cameraLocalRotationY);

        transform.rotation = Quaternion.Euler(0f, cam.transform.rotation.eulerAngles.y, 0f);
    }
}