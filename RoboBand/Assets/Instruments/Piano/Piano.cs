using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : MonoBehaviour {

    public void Back()
    {
        GameObject.Find("Piano").transform.localScale = new Vector3(1, 1);
        GameObject.Find("Canvas").GetComponent<HUD>().edit = false;
        gameObject.SetActive(false);
    }
}
