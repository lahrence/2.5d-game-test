using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CursorManager;

public class FollowMouse : MonoBehaviour {
    public bool isVisible = true;

    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask unwalkableMask;
    [SerializeField] Object marker;
    [SerializeField] GameObject player;
    Unit unit;
    GameObject instancedMarker;
    [HideInInspector] public bool success;
    [HideInInspector] public Vector3 instanceLocation;

    float horizontal;
    float vertical;
    [HideInInspector] public Vector3 direction;

    public int PathLimit = 128;
    [HideInInspector] public int pathLength = 0;
    bool pathFound;
    [HideInInspector] public bool isTracking;
    [SerializeField] GameObject gridObject;
    GridInitial grid;
    Outline outline;
    [SerializeField] Material mainMaterial;
    Vector3 smoothDampVelocity;
    BoxCollider boxCollider;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public bool controllerMode = false;

    void Awake() {
        unit = player.GetComponent<Unit>();
		grid = gridObject.GetComponent<GridInitial>();
        outline = GetComponent<Outline>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");
        Vector3 mouseDirection = new Vector3(mouseX, 0f, mouseY).normalized;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,
                            out RaycastHit raycastHitGround,
                            float.MaxValue,
                            groundMask)) {
            position = raycastHitGround.point;
            position.y = Mathf.Round(position.y);

            position.x = Mathf.Ceil(position.x) - 0.5f;
            position.z = Mathf.Ceil(position.z) - 0.5f;
            float worldBorderX = grid.gridWorldSize.x / 2 - 0.5f;
            float worldBorderY = grid.gridWorldSize.y / 2 - 0.5f;
            if (position.x > worldBorderX) {
                position.x = worldBorderX;
            } else if (position.x < -worldBorderX) {
                position.x = -worldBorderX;
            }

            if (position.z > worldBorderY) {
                position.z = worldBorderY;
            } else if (position.z < -worldBorderY) {
                position.z = -worldBorderY;
            }
        }
        transform.position = Vector3.SmoothDamp(transform.position, position, ref smoothDampVelocity, 0.025f, Mathf.Infinity);
        
        if ((mouseDirection.magnitude >= 0.1f || Input.GetMouseButtonDown(0)) && isVisible && direction.magnitude < 0.1f) {
            //gameObject.GetComponent<MeshRenderer>().enabled = true;
        } else if (direction.magnitude >= 0.1f) {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        } else if (!isVisible && mouseDirection.magnitude >= 0.1f) {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        if (direction.magnitude >= 0.1f) {
            controllerMode = true;
        }
    }

    void LateUpdate() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.x = Mathf.Ceil(mousePosition.x) - 0.5f;
        mousePosition.z = Mathf.Ceil(mousePosition.z) - 0.5f;

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        bool isWalkable = grid.NodeFromWorldPoint(transform.position).walkable;

        Vector3 min = boxCollider.center - boxCollider.size * 0.5f;
        Vector3 max = boxCollider.center + boxCollider.size * 0.5f;
        RaycastHit dump;

        Vector3[] corners = new Vector3[4] {
            mainCamera.WorldToScreenPoint(transform.TransformPoint(new Vector3(min.x, max.y, min.z))),
            mainCamera.WorldToScreenPoint(transform.TransformPoint(new Vector3(min.x, max.y, max.z))),
            mainCamera.WorldToScreenPoint(transform.TransformPoint(new Vector3(max.x, max.y, max.z))),
            mainCamera.WorldToScreenPoint(transform.TransformPoint(new Vector3(max.x, max.y, min.z)))
        };

        Ray[] rayList = new Ray[4];
        for (int i = 0; i < 4; i++) {
            rayList[i] = mainCamera.ScreenPointToRay(corners[i]);
        }

        bool[] hitList = new bool[4];
        for (int i = 0; i < 4; i++) {
            hitList[i] = Physics.Raycast(rayList[i], out dump,
                                         float.MaxValue,
                                         unwalkableMask);
        }
        if (Physics.Raycast(ray,
                            out RaycastHit raycastHit,
                            float.MaxValue, groundMask)) {
            if (!isWalkable
                || Vector3.Distance(player.transform.position,
                                    transform.position) > PathLimit) {
                if (isVisible) {
                    mainMaterial.color = Color.Lerp(mainMaterial.color, new Color(0.6f,0.05f,0.05f, 0.25f), Mathf.PingPong(Time.time, 1));
                    mainMaterial.SetColor("_EmissionColor", Color.Lerp(mainMaterial.GetColor("_EmissionColor"), new Color(0.6f,0.05f,0.05f), Mathf.PingPong(Time.time, 1)));
                }
                Default();
            } else if (!hitList[0]
                       && !hitList[1]
                       && !hitList[2]
                       && !hitList[3]) {
                if (isVisible) {
                    mainMaterial.color = Color.Lerp(mainMaterial.color, new Color(1,1,1,0.25f), Mathf.PingPong(Time.time, 1));
                    mainMaterial.SetColor("_EmissionColor", Color.Lerp(mainMaterial.GetColor("_EmissionColor"), Color.white, Mathf.PingPong(Time.time, 1)));
                }
                Pointer();
            } else {
                if (isVisible) {
                    mainMaterial.color = Color.Lerp(mainMaterial.color, new Color(1,1,1,0.25f), Mathf.PingPong(Time.time, 1));
                    mainMaterial.SetColor("_EmissionColor", Color.Lerp(mainMaterial.GetColor("_EmissionColor"), Color.white, Mathf.PingPong(Time.time, 1)));
                }
                Pointer();
            }
        }
        if (Input.GetMouseButtonDown(0)
            && success
            && direction.magnitude < 0.1f
            && isVisible) {
            if (Vector3.Distance(player.transform.position, transform.position) <= PathLimit) {
                controllerMode = false;
                Destroy(instancedMarker);
                instanceLocation.y -= 0.01f;
                instancedMarker = (GameObject)Instantiate(marker,
                                                        instanceLocation,
                                                        Quaternion.Euler(0, 0, 0));
            }
        } else if (!unit.isTracking) {
            Destroy(instancedMarker);
        }
    }
}
