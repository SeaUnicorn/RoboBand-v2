using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guitar : MonoBehaviour {

    public void Back()
    {
        GameObject.Find("Guitar").transform.localScale = new Vector3(1, 1);
        GameObject.Find("Canvas").GetComponent<HUD>().edit = false;
        gameObject.SetActive(false);
    }
}
