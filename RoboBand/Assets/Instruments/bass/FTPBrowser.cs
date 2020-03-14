using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * <summary> a class provides FTPbrowsing</summary>
 */
public class FTPBrowser : MonoBehaviour {
    /**
     * <value> Panel, LoadTabs - pointers to Gameobjects</value>
     * <value> content - array of files on the server</value>
     * <value> Text - list of objects for each file on the server</value>
     * <value> TextPrefab - prefab (text + two buttons + script)</value>
     */
    public GameObject Panel;
    public GameObject LoadTabs;
    private string[] content;
    public List<GameObject> Text;
    public GameObject TextPrefab;

    /**
     * <value> Start - a method callled once, to load files</value>
     */
    public void Start()
    {
        Reload();
    }

    /**
     * Reload - a method to load content from server
     */
    public void Reload()
    {
        //get content 
        content = Panel.GetComponent<Connection>().con;

        Text = new List<GameObject>();
        foreach (string file in content)
        {
            if (file.Length != 0)
            {
                //add prefab for each file 
                Text.Add(Instantiate(TextPrefab, this.transform));
                Text[Text.Count - 1].GetComponent<Text>().text = file;
                Text[Text.Count - 1].GetComponent<TextBehaviour>().name = file;
                Text[Text.Count - 1].transform.localPosition = new Vector3(0, (Text.Count) * (-30), 0);

            }
        }
    }

    /**
     * Destroy - a method to delete content from app
     */
    private void Destroy()
    {
        for (int i = Text.Count - 1; i >= 0; i--)
        {
            Destroy(Text[i]);
        }
    }

    /**
     * <value> Update - a method is calling once per frame</value>
     */
    public void Update()
    {
        //if content difference from con
        if (Panel.GetComponent<Connection>().con.Length != Text.Count)
        {
            Destroy();
            Reload();
        }
    }

    /**
     * <value> DeleteFromFTP - a method to call function to delete file from ftp server </value>
     * <param name="fileName"> file name </param>
     */
    public void DeleteFromFTP(string fileName)
    {
        Panel.GetComponent<Connection>().DeleteFromFTP(fileName);
    }

    /**
     * <value> GetNameFromFTP - a method to get file from ftp server </value>
     * <param name="fileName"> file name </param>
     */
    public void GetNameFromFTP(string fileName)
    {
        Panel.GetComponent<Connection>().LoadFromFTP(fileName);
        Panel.GetComponent<Connection>().EditName(fileName);
        Panel.SetActive(false);     
    }
}
