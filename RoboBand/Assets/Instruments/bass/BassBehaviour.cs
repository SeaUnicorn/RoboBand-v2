using UnityEngine;

/**
 * <summary> a class provides behaviour of the bass guitar on the main scene</summary> 
 */
public class BassBehaviour : MonoBehaviour {
    /**
     * <value> PanelBass - a editor menu </value>
     */
    public GameObject PanelBass;

    /**
     * <value> a method called once per frame // checks if user press the guitar (fender)</value>
     */
    public void OnMouseOver()
    {
        //if you press on guitar...
        if ( !GameObject.Find("Canvas").GetComponent<HUD>().edit && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Bass was pushed");
            //it becomes bigger
            gameObject.transform.localScale *= 1.5f;
            GameObject.Find("Canvas").GetComponent<HUD>().edit = true;
            //editor panel becomes active
            PanelBass.SetActive(true);
            
        }  
    }
}
