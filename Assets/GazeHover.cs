using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeHover : MonoBehaviour
{
    public float sightLength = 100f;
    public GameObject targetObject;
    public BubbleTrigger loadingBar;
    public float changeAmount = .005f;


    void FixedUpdate()
    {
        RaycastHit seen;
        Ray raydirection = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(raydirection, out seen, sightLength))
        {
            if (seen.collider.tag == "Trigger" && loadingBar.progress < 1f)
            {
                loadingBar.progress += changeAmount;
            }
            else if (seen.collider.tag != "Trigger" && loadingBar.progress > 0f)
            {
                loadingBar.progress -= changeAmount;
            }

        }
    }

}
