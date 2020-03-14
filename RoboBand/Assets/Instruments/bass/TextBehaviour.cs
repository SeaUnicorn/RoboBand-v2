using UnityEngine;

/**
 * <summary> a class to operate with text objects (reflects files on the ftp server) </summary>
 */
public class TextBehaviour : MonoBehaviour {

    /**
     * <value> name - name of the file</value>
     * <value> Panel - pointer to Panel</value>
     * <value> LoadButton, DeleteButton -pointers to buttons</value>
     * <value> CMD - file content</value>
     */
    public string name;
    public GameObject LoadButton;
    public GameObject DeleteButton;
    public GameObject Panel;
    public string CMD;

    /**
     * <value> OnMouseOver - a method is called if the pointer is over the object</value>
     */
    public void OnMouseOver()
    {
        //if right button was pressed
        if(Input.GetMouseButton(1))
        {
            //buttons appear/disapear
            LoadButton.SetActive(!LoadButton.activeSelf);
            DeleteButton.SetActive(!DeleteButton.activeSelf);
        }
    }

    /**
     * <value> Load - a method is called idf LoadButton was pressed // load file from server</value>
     */
    public void Load()
    {
        //buttons disapear
        LoadButton.SetActive(!LoadButton.activeSelf);
        DeleteButton.SetActive(!DeleteButton.activeSelf);
        //method to load file was called
        this.transform.GetComponentInParent<FTPBrowser>().GetNameFromFTP(name);
    }
    /**
     * <value> Delete - a method is called if DeleteButton was pressed // delete file from server</value>
     */
    public void Delete()
    {
        //buttons disapear
        LoadButton.SetActive(!LoadButton.activeSelf);
        DeleteButton.SetActive(!DeleteButton.activeSelf);
        //method to delete file was called
        this.transform.GetComponentInParent<FTPBrowser>().DeleteFromFTP(name);
    }
}
