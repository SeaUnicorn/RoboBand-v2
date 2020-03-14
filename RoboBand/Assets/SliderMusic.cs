using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMusic : MonoBehaviour {

    /**
     *<value> OnValueChanged - a method gets handle position and sets time of music (in sec)</value>
     * <param name="value">float handle value</param>
     */
    public void OnValueChanged(float value)
    {
        //drag slider => set music time
        GameObject.Find("Audio Source").GetComponent<AudioSource>().time = value / 1000.0f;
    }
    /**
     * <value> Update - a method is called once per frame // scrolls slider area depends on the music playtime</value>
     */
    public void Update()
    {
        if (GameObject.Find("Audio Source").GetComponent<AudioSource>().isPlaying) GetComponent<Slider>().value = GameObject.Find("Audio Source").GetComponent<AudioSource>().time * 1000.0f;



    }
}
