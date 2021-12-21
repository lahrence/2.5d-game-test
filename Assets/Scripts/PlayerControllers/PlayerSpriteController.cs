using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    private Camera cam;
    PlayerController playerController;
    [SerializeField] private GameObject player;

    void Start() 
    {
        playerController = player.GetComponent<PlayerController>();
        cam = playerController.cameraObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}