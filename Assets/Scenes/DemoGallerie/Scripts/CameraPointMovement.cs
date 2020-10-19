using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityTemplateProjects;

public class CameraPointMovement : MonoBehaviour
{
    public GameObject camera2;
    public Material litPOS, normalPOS;
    public Material litframe, normalframe;
    public Material normaltv;
    public GameObject AllMovePos,AllPictures,AllADs;
    public GameObject PictureInfo, AdInfo;

    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(camera2.transform.position, camera2.transform.forward, Color.red);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit,1000))
        {
            if (hit.transform.gameObject.tag == "MovePos")
            {
                Changematerial(hit.transform.gameObject);
                hit.transform.gameObject.GetComponent<MeshRenderer>().material = litPOS;
                
                if (Input.GetMouseButtonDown(0))
                {
                    this.GetComponent<NavMeshAgent>().SetDestination(hit.transform.position);
                }
            }

            if (hit.transform.gameObject.tag == "Picture")
            {
                Changematerial(hit.transform.gameObject);
                hit.transform.gameObject.GetComponent<MeshRenderer>().material = litframe;

                if (Input.GetMouseButtonDown(0))
                {
                    camera2.GetComponent<FirstPersonCamera>().enabled = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    PictureInfo.GetComponent<PictureInfo>().SetNewText(hit.transform.gameObject.name);
                    PictureInfo.SetActive(true);
                }
            }
            if (hit.transform.gameObject.tag == "AD")
            {
                Changematerial(hit.transform.gameObject);
                hit.transform.gameObject.GetComponent<MeshRenderer>().material = litframe;

                if (Input.GetMouseButtonDown(0))
                {
                    camera2.GetComponent<FirstPersonCamera>().enabled = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    //PictureInfo.GetComponent<PictureInfo>().SetNewText(hit.transform.gameObject.name);
                    AdInfo.SetActive(true);
                }
            }
        }
    }

    public void Changematerial(GameObject hit)
    {
        for (int i = 0; i < AllMovePos.transform.childCount; i++)
        {
            if (AllMovePos.transform.GetChild(i) == hit)
            {
                break;
            }
            AllMovePos.transform.GetChild(i).GetComponent<MeshRenderer>().material = normalPOS;
        }

        for (int i = 0; i < AllPictures.transform.childCount; i++)
        {
            if (AllPictures.transform.GetChild(i) == hit)
            {
                break;
            }
            AllPictures.transform.GetChild(i).GetComponent<MeshRenderer>().material = normalframe;
        }

        for (int i = 0; i < AllADs.transform.childCount; i++)
        {
            if (AllADs.transform.GetChild(i) == hit)
            {
                break;
            }
            AllADs.transform.GetChild(i).GetComponent<MeshRenderer>().material = normaltv;
        }
    }

    public void EnableCameraMov()
    {
        camera2.GetComponent<FirstPersonCamera>().enabled = true;
    }
}
