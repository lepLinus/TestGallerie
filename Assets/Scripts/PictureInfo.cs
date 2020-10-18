using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PictureInfo : MonoBehaviour
{
    public TextMeshProUGUI Header, MainText;
    public Image Priv;
    public Sprite Example;

    public void SetNewText(string picturename)
    {
        Header.text = "Freedom - Modern Art";
        MainText.text = "This is only an Example Text. \nThe Picture can be discribed here and Social Media and Website can be linked.";
        Priv.sprite = Example;
    }
}
