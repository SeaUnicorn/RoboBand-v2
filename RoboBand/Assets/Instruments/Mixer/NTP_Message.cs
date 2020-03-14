using UnityEngine;

public class NTP_Message : MonoBehaviour
{
    public struct message
    {
        // Total: 384 bits or 48 bytes.
        public byte li_vn_mode;      // Eight bits. li, vn, and mode.
                                     // li.   Two bits.   Leap indicator.
                                     // vn.   Three bits. Version number of the protocol.
                                     // mode. Three bits. Client will pick mode 3 for client.

        public byte stratum;         // Eight bits. Stratum level of the local clock.
        public byte poll;            // Eight bits. Maximum interval between successive messages.
        public byte precision;       // Eight bits. Precision of the local clock.

        public uint rootDelay;      // 32 bits. Total round trip delay time.
        public uint rootDispersion; // 32 bits. Max error aloud from primary clock source.
        public uint refId;          // 32 bits. Reference clock identifier.

        public uint refTm_s;        // 32 bits. Reference time-stamp seconds.
        public uint refTm_f;        // 32 bits. Reference time-stamp fraction of a second.

        public uint origTm_s;       // 32 bits. Originate time-stamp seconds.
        public uint origTm_f;       // 32 bits. Originate time-stamp fraction of a second.

        public uint rxTm_s;         // 32 bits. Received time-stamp seconds.
        public uint rxTm_f;         // 32 bits. Received time-stamp fraction of a second.

        public uint txTm_s;         // 32 bits and the most important field the client cares about. Transmit time-stamp seconds.
        public uint txTm_f;         // 32 bits. Transmit time-stamp fraction of a second.
    } 
    public message Read(byte[] mess)
    {
        message result = new message();
        //TODO
        return result;
    }
    
}
