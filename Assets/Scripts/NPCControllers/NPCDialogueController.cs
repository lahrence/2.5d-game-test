using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NPCDialogueController
{
    public static GameObject NPCDetect(GameObject[] npcObjects, GameObject player)
    {
        float tempDistance = 0f;
        GameObject closestNPC = null;
        foreach (GameObject npc in npcObjects) {
            float distance = Vector3.Distance(npc.transform.position, player.transform.position);
            if (tempDistance == 0f) tempDistance = distance;
            if (distance <= tempDistance && distance <= 1.5f) closestNPC = npc;
        }
        return(closestNPC);
    }
}