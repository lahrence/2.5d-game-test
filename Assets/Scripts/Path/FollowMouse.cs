using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask mask;
    [SerializeField] Object marker;
    [SerializeField] GameObject player;
    Unit unit;
    GameObject instancedMarker;
    public bool success;
    public Vector3 instanceLocation;

    void Awake() {
        unit = player.GetComponent<Unit>();
    }

    void Update() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, mask)) {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            Vector3 position = raycastHit.point;
            position.y += 0.5f;
            position.x = Mathf.Round(position.x) - 0.5f;
            position.z = Mathf.Round(position.z) - 0.5f;
            transform.position = position;

            instanceLocation.y += 0.5f;
            if (Input.GetMouseButtonDown(0) && success) {
                Destroy(instancedMarker);
                print("Destroyed: " + instancedMarker);

                instancedMarker = (GameObject)Instantiate(marker, instanceLocation, Quaternion.identity);
                print("Instanced: " + instancedMarker);
            }

            if (Input.GetMouseButtonDown(0)) {
                print("Tracking: " + unit.isTracking);
                print("Success Mouse: " + success);
            }
        } else {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}