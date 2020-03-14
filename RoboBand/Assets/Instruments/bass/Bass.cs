using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using Crosstales.FB;
using System;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Globalization;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * <summary> a class provides menu of editor</summary>
 */
public class Bass : MonoBehaviour {

    /**
     * <value> fS_x, fS_y, fS_z - inputfields (first string position) </value>
     * <value> firstPoint - contains information about coordinates and angle</value>
     * <value> path - a string-path to tabs</value>
     * <value> sound_tempo - duration of whole note (ms)</value>
     * <value> tabs - a container for information about tabs (parsed)</value>
     * <value> Text - a field to post result</value>
     * <value> CMD - a variable to accumulate result</value>
     * <value> ConnectionPanel - a pointer to GUI panel to set up ftp-connection</value>
     * <value> GuitarRotation - contains information about guitar global rotation</value>
     * <value> mode - a mode of playing</value>
     */
    public GameObject fS_x, fS_y, fS_z;
    public GameObject angle_ZX, angle_ZY;
    private char mode;

    public PointMode.point firstPoint;
    public G_CMD.guitar_Info g_info;
    private string path;
    private int sound_tempo;
    private List<PlayYard.noteInf> tabs = new List<PlayYard.noteInf>();
    public TMP_InputField Text;
    public string CMD;
    public GameObject ConnectionPanel;
    private Vector3 firstString;
    private Vector2 GuitarRotation;
    private List<GameObject> fields;
    private int active_element = 0;
    public string adress_string;
    EventSystem system;

    /**
     * <value> a method called once // sets the default values of first string coordinates</value>
     */
    private void Start()
    {

        fS_x.GetComponentInChildren<Text>().text = "-476";
        fS_y.GetComponentInChildren<Text>().text = "-143";
        fS_z.GetComponentInChildren<Text>().text = "485";
        
        firstString = new Vector3(-476, -143, 485);

        angle_ZX.GetComponentInChildren<Text>().text = "-66";
        angle_ZY.GetComponentInChildren<Text>().text = "0";

        GuitarRotation = new Vector2(-66, 0);
        fields = new List<GameObject>()
        {fS_x, fS_y, fS_z, angle_ZX, angle_ZY };
        mode = 'D';
        adress_string = "192.168.10.10";
        system = EventSystem.current;// EventSystemManager.currentSystem;
    }

    public string Get_adress() 
    {
        return adress_string;
    }

    public void Set_adress(string new_ftp_string)
    {
        adress_string = new_ftp_string;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (active_element == fields.Count - 1) active_element = 0;
            else active_element++;
            fields[active_element].GetComponent<InputField>().OnPointerClick(new PointerEventData(system));
        }
    }
    public string GetTabs()
    {
        return CMD;
    }

    /**
     * <value> a method called when user presses the button "load tab's"// loads .xml document and prepare G_Comands list</value>
     */
    public void LoadTabs()
    {
        //creates a new xml (for .xml)
        XmlDocument xml = new XmlDocument();
        //loads text (user choose a path/ in runtime)
        xml.Load(FileBrowser.OpenSingleFile("Choose tabs", "", ""));
        XmlElement xRoot = xml.DocumentElement;
        //find all elements with tag "sound"
        var elements = xml.GetElementsByTagName("sound");
        foreach (XmlNode child in elements)
        {
            // duration of whole in ms
            sound_tempo = Convert.ToInt32(60000 / (Convert.ToDouble((child.Attributes.GetNamedItem("tempo").Value)) / 4));
          
        }
        //find all elements with tag "note"
        XmlNodeList notes = xml.GetElementsByTagName("note");
        //first string init-tion
        firstPoint = new PointMode.point(firstString.x, firstString.y, firstString.z, 0, 0, 0);
        //parsing xml
        tabs = new List<PlayYard.noteInf>();
        foreach (XmlNode note in notes)
        {

            int fret = 0;
            int st = 0;
            float duration = 0;
            //sets the duration (only TUXGUITAR XML format; 3840 - duration of whole in tuxguitar unites)
            duration = Convert.ToSingle(note["duration"].InnerText) / 3840.0f;
            //in case of rest
            if (note.FirstChild.Name == "rest")
            {
                fret = 999;
                st = 9;
            }
            else
            {
                XmlNode notation = note["notations"]["technical"].CloneNode(true);
                fret = Convert.ToInt32(notation["fret"].InnerText);
                //inverts string number
                st = 5 - Convert.ToInt32(notation["string"].InnerText);
            }
            tabs.Add(new PlayYard.noteInf(duration, st, fret));
        }

        //accumulates guitar information
        g_info = new G_CMD.guitar_Info(tabs, GuitarRotation.x, GuitarRotation.y, mode, firstPoint, sound_tempo);
        
        CMD = gameObject.GetComponent<PlayYard>().Play_tabs_xml(g_info);
        //formates strings for AS
        CMD = CMD.Replace(',', '.');
        // show text in the textBox
        Text.text = CMD;
        
    }

    /**
     * <value> GetTabsFromFTP - a method to load tabs (g-commands) from ftp </value>
     * <param name="cmd"> text of PRG file</param>
     */
    public void GetTabsFromFTP(string cmd)
    {
        //reload variable
        CMD = cmd;
        // show text in the textBox
        Text.text = CMD;
        //save document 
        SaveTabs();
    }


    /**
     * <value> Edit - a methods edits firststring position</value>
    */
    public void Edit()
    {
        tabs = new List<PlayYard.noteInf>();
        CMD = "";
        Text.text = "";
        firstPoint.Edit(firstString.x, firstString.y, firstString.z);
    }

    /**
     * <value> EditXString - a methods takes value from inputefield and sets to container</value>
     * <param name="value"> string position</param>
     */
    public void EditXString(string value)
    {
        tabs = new List<PlayYard.noteInf>();
        CMD = "";
        Text.text = "";
        firstString = new Vector3(float.Parse(value, CultureInfo.InvariantCulture), firstString.y, firstString.z);
        active_element = fields.IndexOf(fS_x);
   
    }

    /**
     * <value> EditYString - a methods takes value from inputefield and sets to container</value>
     * <param name="value"> string position</param>
     */
    public void EditYString(string value)
    {
        tabs = new List<PlayYard.noteInf>();
        CMD = "";
        Text.text = "";
        firstString = new Vector3(firstString.x, float.Parse(value, CultureInfo.InvariantCulture), firstString.z);
        active_element = fields.IndexOf(fS_y);
    }

    /**
     * <value> EditZString - a methods takes value from inputefield and sets to container</value>
     * <param name="value"> string position</param>
     */
    public void EditZString(string value)
    {
        tabs = new List<PlayYard.noteInf>();
        CMD = "";
        Text.text = "";
        firstString = new Vector3(firstString.x, firstString.y, float.Parse(value, CultureInfo.InvariantCulture));
        active_element = fields.IndexOf(fS_z);
    }

    /**
     * <value> EditAngleZX - a methods takes value from inputefield and sets to container</value>
     * <param name="value"> ZX-angle value</param>
     */
    public void EditAngleZX(string value)
    {
        tabs = new List<PlayYard.noteInf>();
        CMD = "";
        Text.text = "";
        GuitarRotation = new Vector2(float.Parse(value, CultureInfo.InvariantCulture), GuitarRotation.y);
        active_element = fields.IndexOf(angle_ZX);
    }

    /**
     * <value> EditAngleZY - a methods takes value from inputefield and sets to container</value>
     * <param name="value"> ZY-angle value</param>
     */
    public void EditAngleZY(string value)
    {
        tabs = new List<PlayYard.noteInf>();
        CMD = "";
        Text.text = "";
        GuitarRotation = new Vector2(GuitarRotation.x, float.Parse(value, CultureInfo.InvariantCulture));
        active_element = fields.IndexOf(angle_ZY);
    }

    /**
     * <value> SetMode - a method sets 'P' (tapping) mode </value>
     * <param name="value"> toggle value</param>
     */
    public void SetMode(bool value)
    {
        if (value) mode = 'P';
        else mode = 'D';
    }

    /**
     * <value> SaveTabs - a method is called when user presses button 'Save tab's'// saves result to .txt file</value>
     */
    public void SaveTabs()
    {
        //there is some result
        if(Text.text.Length !=0)
        {
            //asks user about path
            string pathExp = FileBrowser.OpenSingleFolder("Choose folder", Application.dataPath);
            if (pathExp.Length != 0) // user choose the path
            {
                var fileName = "/bass_part.PRG"; //.PRG - AS program format
                var directory = pathExp + fileName;
                int i = 1;
                while (File.Exists(directory)) //if there is a program with the same name
                {
                    //a method creates a new name
                    fileName = string.Concat("/bass_part_", i.ToString(), ".PRG");
                    i++;
                    directory = pathExp + fileName;
                }
                var file = File.CreateText(directory);
                file.Write(Text.text);
                file.Close();
            }
        }
    }

    public void SaveTabs(string tabs)
    {
        //there is some result
        if (Text.text.Length != 0)
        {
            //asks user about path
            string pathExp = FileBrowser.OpenSingleFolder("Choose folder", Application.dataPath);
            if (pathExp.Length != 0) // user choose the path
            {
                var fileName = "/bass_part.PRG"; //.PRG - AS program format
                var directory = pathExp + fileName;
                int i = 1;
                while (File.Exists(directory)) //if there is a program with the same name
                {
                    //a method creates a new name
                    fileName = string.Concat("/bass_part_", i.ToString(), ".PRG");
                    i++;
                    directory = pathExp + fileName;
                }
                var file = File.CreateText(directory);
                file.Write(tabs);
                file.Close();
            }
        }
    }
    /**
     * <value> UploadTabsFTP - a method is called when user presses button 'Upload tab's'// opens the connection panel with connection options</value>
     */
    public void UploadTabsFTP()
    {
        ConnectionPanel.SetActive(true);
    }

    public void Back()
    {
        GameObject.Find("Bass").transform.localScale = new Vector3(1, 1);
        GameObject.Find("Canvas").GetComponent<HUD>().edit = false;
        gameObject.transform.parent.gameObject.SetActive(false);
       
    }

    public void SaveParam()
    {
        StringBuilder parameters = new StringBuilder();
        parameters.AppendLine("X    Y   Z");
        parameters.AppendLine(firstString.ToString());
        parameters.AppendLine("angleZX  angleZY");
        parameters.AppendLine(GuitarRotation.ToString());

        string pathExp = FileBrowser.OpenSingleFolder("Choose folder", Application.dataPath);
        if (pathExp.Length != 0) // user choose the path
        {
            var fileName = "/bass_parameters.txt"; 
            var directory = pathExp + fileName;
            int i = 1;
            while (File.Exists(directory)) //if there is a program with the same name
            {
                //a method creates a new name
                fileName = string.Concat("/bass_parameters_", i.ToString(), ".txt");
                i++;
                directory = pathExp + fileName;
            }
            var file = File.CreateText(directory);
            file.Write(parameters);
            file.Close();
        }
    }
}
