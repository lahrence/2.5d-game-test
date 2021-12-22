using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{

    public Camera cameraObject;
    [SerializeField] GameObject sprite;
    NPCDialogueController npcDialogueController;
    Transform cam;

    Animator animator;

    string currentState;
    string npcState;

    void Start()
    {
        cam = cameraObject.transform;
        animator = sprite.GetComponent<Animator>();
        npcState = AnimationController.animations["idle"];
    }

    void Update()
    {
        float cameraLocalRotation = cam.eulerAngles.y;
        float npcLocalRotation = transform.eulerAngles.y;

        // Calculate the camera rotation relative to the npc
        float cameraOrbit = (cameraLocalRotation <= npcLocalRotation ?
                             npcLocalRotation - cameraLocalRotation :
                             360 + npcLocalRotation - cameraLocalRotation);

        // Change sprite perspective
        currentState = AnimationController.SpriteAnimationPerspective(cameraOrbit, npcState, animator, currentState, 1f);
    }
}