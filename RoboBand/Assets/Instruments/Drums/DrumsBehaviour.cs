using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumsBehaviour : MonoBehaviour {

    public GameObject PanelDrums;

    /**
    * <value> a method called once per frame // checks if user pressed the Drums (Pearl)</value>

    */
    public void OnMouseDown()
    {
        //if you press on mixer...
        if (!GameObject.Find("Canvas").GetComponent<HUD>().edit && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Drums was pushed");
            //it becomes bigger
            gameObject.transform.localScale *= 1.2f;
            GameObject.Find("Canvas").GetComponent<HUD>().edit = true;
            //editor panel becomes active
            PanelDrums.SetActive(true);
        }

    }
}
