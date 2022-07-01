using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMeditation : MonoBehaviour
{

    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Hand")
        {
            //start
            Debug.Log("Entered trigger");
            gameObject.SetActive(false);
        }
    }


}
