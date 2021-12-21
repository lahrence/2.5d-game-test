using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteController : MonoBehaviour
{
    private Camera cam;
    NPCController npcController;
    [SerializeField] private GameObject npc;

    void Start() 
    {
        npcController = npc.GetComponent<NPCController>();
        cam = npcController.cameraObject;
    }

    void Update()
    {
        transform.LookAt(cam.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}