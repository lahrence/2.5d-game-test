using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMarker : MonoBehaviour {
    GameObject player;
    Unit unit;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        unit = player.GetComponent<Unit>();
    }
}