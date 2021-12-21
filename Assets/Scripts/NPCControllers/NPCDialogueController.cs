using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueController : MonoBehaviour
{
    public bool PlayerDetect(GameObject player)
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance <= 1.5;
    }
}