using UnityEngine;
using Crosstales.FB;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Mixer : MonoBehaviour
{
    private string path;
    public AudioClip music;
    public AudioSource audiosource = new AudioSource();
    public Slider slider;
    public TMPro.TextMeshProUGUI text;
    
    public float MixerTime;
    public IPEndPoint clientEndPoint;



    public UdpClient server = new UdpClient();


    public IPAddress serverIP = IPAddress.Parse("192.168.10.18");
    static int port = 123;


    public void Start()
    {
        MixerTime = Time.realtimeSinceStartup * 1000.0f;
        server.Client.Bind(new IPEndPoint(IPAddress.Any, port));
        
        byte[] time = Encoding.UTF8.GetBytes(MixerTime.ToString());

        server.Send(time, time.Length, "255.255.255.255", port);


       

    }

    public void FixedUpdate()
    {
        MixerTime += Time.deltaTime * 1000.0f;
        GameObject.Find("Timer").GetComponent<Text>().text = MixerTime.ToString();
        if(MixerTime >= 16000 && MixerTime%16000 == 0)
        {
            byte[] time = Encoding.UTF8.GetBytes(MixerTime.ToString());
            server.Send(time, time.Length, "255.255.255.255", port);
        }

    }
    public void Back()
    {
        GameObject.Find("Mixer").transform.localScale = new Vector3(1, 1);
        GameObject.Find("Canvas").GetComponent<HUD>().edit = false;
        gameObject.SetActive(false);
    }
    public void LoadMusic()
    {
        path =  FileBrowser.OpenSingleFile("Choose a music file", "", "wav");
        //the only way to get file in runtime            
        WWW request = GetAudioFromFile("file://" + path);
        //waiting for file be loaded (necessary)
        while (!request.isDone)
        { } // that can be a waiting animation
        //finaly set the music file
        music = request.GetAudioClip(true, false, AudioType.WAV);

        GameObject.Find("Audio Source").GetComponent<AudioSource>().clip = music;
        GameObject.Find("Audio Source").GetComponent<AudioSource>().clip.LoadAudioData();

        slider.maxValue = GameObject.Find("Audio Source").GetComponent<AudioSource>().clip.length * 1000.0f;
        MusicInfo();

    }



    public void MusicInfo()
    {
        StringBuilder info = new StringBuilder();
        string[] name = path.Split('/');
        info.AppendLine(name[name.Length - 1]);
        info.Append("lenght = ");
        info.Append(slider.maxValue);
        info.Append(" ms");
        text.text = info.ToString();
    }
    /**
  * <value> GetAudioFromFile - method creates a www request to file</value>
  * <para>//the only way to get the file in runtime</para>
  * <param name="pathMusic"> simulation of URL </param>
  * <returns WWW> request to the file (not web)</returns>
  */
    private WWW GetAudioFromFile(string pathMusic)
    {
        return new WWW(pathMusic);
    }

    /**
    *<value> PauseMusic - method allows to pause or unpause music from current time//Togglebutton "Play/Pause"</value> 
    *<param name="isOnOff" > a boolean flag that reflects if music is turned on or not</param>
    */
    public void PlayPauseMusic()
    {
        //if music has been playing yet and it wasn't paused (flag)
        if (GameObject.Find("Audio Source").GetComponent<AudioSource>().isPlaying)
        {
            //pause music
            GameObject.Find("Audio Source").GetComponent<AudioSource>().Pause();
            //changing the picture
            //***
        }

        else
        {//if music has not been playing yet and it was paused (flag)
            if (!GameObject.Find("Audio Source").GetComponent<AudioSource>().isPlaying)
            {
                //if music has not been playing yet
                PlayMusic();
                //pause music
                GameObject.Find("Audio Source").GetComponent<AudioSource>().UnPause();
                //changing the picture
                //***
            }
        }
    }

    /**
  *<value> PlayMusic - method enables music playing (an start to play)</value>
  */
    public void PlayMusic()
    {
        //if music has not been playing yet
        if (GameObject.Find("Audio Source").GetComponent<AudioSource>().isPlaying != true)
        {
            GameObject.Find("Audio Source").GetComponent<AudioSource>().enabled = true;
            GameObject.Find("Audio Source").GetComponent<AudioSource>().Play();
        }
    }



}
