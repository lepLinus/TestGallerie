using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraPointMovement : MonoBehaviour
{
    public GameObject camera2;
    public Material lit, normal;
    public GameObject AllMovePos;

    void Update()
    {
        RaycastHit hit;
        Debug.Log("Ray");
        Debug.DrawRay(camera2.transform.position, camera2.transform.forward, Color.red);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit,1000))
        {
            if (hit.transform.gameObject.tag == "MovePos")
            {
                Changematerial(hit.transform.gameObject);
                hit.transform.gameObject.GetComponent<MeshRenderer>().material = lit;
                
                if (Input.GetMouseButtonDown(0))
                {
                    this.GetComponent<NavMeshAgent>().SetDestination(hit.transform.position);
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
            AllMovePos.transform.GetChild(i).GetComponent<MeshRenderer>().material = normal;
        }
        //hit.transform.gameObject.GetComponent<MeshRenderer>().material = normal;
    }
}
