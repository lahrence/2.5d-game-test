using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

    public Camera cameraObject;
    [SerializeField] GameObject sprite;
    [SerializeField] string npcName = "topHat";
    [SerializeField] CharacterController controller;
    Transform cam;

    Animator animator;

    string currentState;
    string npcState;

    void Start() {
        cam = cameraObject.transform;
        animator = sprite.GetComponent<Animator>();
        //npcState = AnimationController.animations[npcName]["idle"];
    }

    void Update() {

        // Change sprite perspective
        //currentState = AnimationController.SpriteAnimationPerspective(npcState, animator, currentState, 1f);

        controller.SimpleMove(new Vector3(0, 0, 0));
    }
}
