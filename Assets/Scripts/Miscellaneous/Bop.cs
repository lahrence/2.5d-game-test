using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bop : MonoBehaviour
{
    [SerializeField] private float rate = 0.1f;
    [SerializeField] private float maxLimit = 1;
    [SerializeField] private float minLimit = 0;
    [SerializeField] private float offset;
    private float position;
    private bool up;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            position + offset,
            transform.position.z);
        if (up)
            position += rate;
        else if (!up)
            position -= rate;
        if (up && position > maxLimit)
            up = false;
        else if (!up && position < minLimit)
            up = true;
    }
}
