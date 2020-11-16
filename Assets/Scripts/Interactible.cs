using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public bool isdoor;
    bool isopen = false;
    public bool isarcade;
    public GameObject ArcadeMenu;
    // Start is called before the first frame update
    public void Interact()
    {

        if (isdoor && !isopen)
        {
            isopen = true;
            this.transform.parent.GetComponent<Animation>().Play("Door");
        }
        if (isarcade)
        {
            ArcadeMenu.SetActive(true);
        }
        
    }
}
