using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject VolumGlob;
    public Slider Slider;
    public AudioSource Main;

    public void SetRes(int index)
    {
        if (index == 1)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else if (index == 2)
        {
            Screen.SetResolution(3840, 2160, true);
        }
        else
        {
            Screen.SetResolution(1280, 720, true);
        }
    }
    public void SetQul(int index)
    {
        if (index == 1)
        {
            VolumGlob.SetActive(true);
        }
        else
        {
            VolumGlob.SetActive(false);
        }
    }

    public void Update()
    {
        Main.volume = Slider.value;
    }
}
