using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;


public class NTPserver : ScriptableObject
{
    public static int port = 123;
    public UdpClient server = new UdpClient();
    public NTP_Message message; 
    public void BroadcastServer()
    {
        server.Client.Bind(new IPEndPoint(IPAddress.Any, port));
        message = new NTP_Message();
    }

    public void SendTimeMessage(byte[] mess)
    {
        server.Send(mess, mess.Length, "255.255.255.255", port);
    }
    public void ReadMessage(NTP_Message message)
    {

    }

    public void ServerClose()
    {
        server.Close();
    }
}