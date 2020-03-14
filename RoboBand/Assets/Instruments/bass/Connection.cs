using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.FB;

/**
 * <summary> a class provides FTPconnection</summary>
 */
public class Connection : MonoBehaviour {

    /**
     * <value> adress, fileName - pointer to inputfields</value>
     * <value> tabs - a result string container</value>
     * <value> con (content) - array of fileNames from FTP server</value>
     * <value> Name - filename </value>
     */
    private const string NO_CONNECTION = "No connection";

    public GameObject adress;
    public GameObject LoadButton;
    public string tabs;
    public string[] con;
    private InputField fileName;
    private string Name;
    public string adress_string;
    



    /** 
    * <value> OnDisable - a method is called every time the object becomes disactive // Just for double defend from loading incorrect tabs</value>
    */
    private void OnDisable()
    {
        tabs = "";
    }

    /** 
    * <value> OnEnable - a method is called every time the object becomes active // sets default values to inputfield and copies results</value>
    */
    private void OnEnable()
    {
        Name = "";
        adress_string = GameObject.Find("Load_Tabs").GetComponent<Bass>().Get_adress();
        string path = "ftp://" + adress_string + "/CNC_Prg/";
        //load catalog from default adress
        
        tabs = GameObject.Find("Load_Tabs").GetComponent<Bass>().GetTabs();
        Load_Catalog(path);

    }

    public void EditeAdress()
    {
        if (adress.GetComponentInChildren<Text>().text != NO_CONNECTION)
        {
            adress_string = adress.GetComponentInChildren<Text>().text;
            GameObject.Find("Load_Tabs").GetComponent<Bass>().Set_adress(adress_string);
            string path = "ftp://" + adress_string + "/CNC_Prg/";
            //load catalog from default adress
            Load_Catalog(path);
        }
        else
        {
            adress.GetComponentInChildren<Text>().text = NO_CONNECTION;
        }
    }

    /**
     * <value> Cancel - a method is called when user presses 'Cancel' button// returns previous menu</value>
     */
    public void Cancel()
    {
        gameObject.SetActive(false);

    }

    /**
     * <value> Connect_Upload - a method is called when user presses 'Cancel' button// conects to the server and uploads file</value>
     */
    public void Connect_Upload()
    {   
        //ftp server path
        string path = "ftp://" + adress_string + "/CNC_Prg/";
        //user hasn't chosen name of file to save
        if (Name.Length == 0) con[0] = "Set Name To File, Please";
        else
        {
            string fileName = Name; //.PRG - AS program format

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path + Name);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            try
            {
                // Copy the contents of the file to the request stream.
                byte[] fileContents = Encoding.UTF8.GetBytes(tabs);

                request.ContentLength = (long)fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);

                path = "ftp://" + adress_string + "/CNC_Prg/";

                request = (FtpWebRequest)WebRequest.Create(path);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse responseCommon = (FtpWebResponse)request.GetResponse();

                request.Abort();
            }
            catch (Exception)
            {            
               // GameObject.Find("Load_Tabs").GetComponent<Bass>().Set_adress(adress_string);
                adress.GetComponentInChildren<Text>().text = NO_CONNECTION;
                throw;
            }
        }
     //   reload catalog
        Load_Catalog(path);

    }

    /**
    * <value> SaveTabs - a method is called for debug saves result to .txt file</value>
    */
    public void SaveTabs(string tabs, string name = "")
    {
        //there is some result
        if (tabs.Length != 0)
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
                    fileName = string.Concat("/bass_part_", name, i.ToString(), ".PRG");
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
     * <value> Load_Catalog - a separate method to load fileList from ftp</value>
     * <param name="path"> adress, string</param>
     */
    private void Load_Catalog(string path)
    {
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
        request.Method = WebRequestMethods.Ftp.ListDirectory;
        
        try
        {
            FtpWebResponse responseCommon = (FtpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(responseCommon.GetResponseStream());
            //all files in one string
            string catalog = sr.ReadToEnd();
            //splited string to con (content)
            con = catalog.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            sr.Close();
            responseCommon.Close();
            request.Abort();
        }
        catch (Exception)
        {
            //GameObject.Find("Load_Tabs").GetComponent<Bass>().Set_adress(adress_string);
            adress.GetComponentInChildren<Text>().text = NO_CONNECTION;
            throw;
        }

    }

    /**
     * <value> EditName - a method to change file name</value>
     * <param name="value"> string, new Name</param>
     */
    public void EditName(string value) => Name = value;

    /**
     * <value> LoadFromFTP - a method to load file from ftp </value>
     * <param name="value"> file name</param>
     */
    public void LoadFromFTP(string value)
    {
        //formulates adress
        string path = "ftp://" + adress_string + "/CNC_Prg/" + value;
        //sets connection
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
        request.Method = WebRequestMethods.Ftp.DownloadFile;
        try { 
            FtpWebResponse responseCommon = (FtpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(responseCommon.GetResponseStream());
            //read tabs from file
            tabs = sr.ReadToEnd();
            //sent tabs to previous menu
            LoadButton.GetComponent<Bass>().GetTabsFromFTP(tabs);
            //close connection
            sr.Close();
            responseCommon.Close();
            request.Abort();
        }
        catch (Exception)
        {
            adress.GetComponentInChildren<Text>().text = NO_CONNECTION;
            //GameObject.Find("Load_Tabs").GetComponent<Bass>().Set_adress(adress_string);
            throw;
        }
    }

    /**
     * <value> DeleteFromFTP - a method to delete file from server </value>
     * <param name="value"> file name</param>
     */
    public void DeleteFromFTP(string value)
    {
        //formulates adress
        string path = "ftp://" + adress_string + "/CNC_Prg/" + value;
        //sets connection
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
        //delete
        request.Method = WebRequestMethods.Ftp.DeleteFile;
        try
        {
            FtpWebResponse responseCommon = (FtpWebResponse)request.GetResponse();
            //close connection
            responseCommon.Close();
            request.Abort();
        }
        catch (Exception)
        {
            //GameObject.Find("Load_Tabs").GetComponent<Bass>().Set_adress(adress_string);
            adress.GetComponentInChildren<Text>().text = NO_CONNECTION;
            throw;
        }
        //reload catalog
        Load_Catalog(("ftp://" + adress_string + "/CNC_Prg/"));
    }
    
}
