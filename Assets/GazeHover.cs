using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeHover : MonoBehaviour
{
    public float sightLength = 100f;
    private BubbleTrigger currentTrigger = null;

    void FixedUpdate()
    {
        RaycastHit seen;
        Ray raydirection = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(raydirection, out seen, sightLength))
        {
            if (seen.collider.tag == "Trigger") // hits trigger
            {
                BubbleTrigger seenTrigger = seen.collider.gameObject.GetComponent<BubbleTrigger>();
                if (currentTrigger == null) {
                    currentTrigger = seenTrigger;
                    currentTrigger.isRayHitting = true;
                }
                else if(currentTrigger == seenTrigger)
                {
                    //currentTrigger.rayHit = true;
                }
                else if(currentTrigger != seenTrigger) // moved to different trigger
                {
                    currentTrigger.isRayHitting = false;
                    currentTrigger = seenTrigger;
                }      
            }
            else
            {
                if (currentTrigger) { currentTrigger.isRayHitting = false; }
                currentTrigger = null;
            }
        }
        else
        {
            if (currentTrigger) { currentTrigger.isRayHitting = false; }
            currentTrigger = null;
        }
    }

}
