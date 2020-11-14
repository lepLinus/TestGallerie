using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Menu;


    public void CloseAll()
    {
        for (int i = 0; i< Menu.Count; i++)
        {
            Menu[i].SetActive(false);
        }
    }
}
