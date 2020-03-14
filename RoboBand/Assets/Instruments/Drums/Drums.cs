using UnityEngine;

public class Drums : MonoBehaviour {

    public void Back()
    {
        GameObject.Find("Drums").transform.localScale = new Vector3(1, 1);
        GameObject.Find("Canvas").GetComponent<HUD>().edit = false;
        gameObject.SetActive(false);
    }
}
