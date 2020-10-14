using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraPointMovement : MonoBehaviour
{
    public GameObject camera2;
    public Material lit, normal;

    void Update()
    {
        RaycastHit hit;
        Debug.Log("Ray");
        Debug.DrawRay(camera2.transform.position, camera2.transform.forward, Color.red);
        if (Physics.Raycast(camera2.transform.position, camera2.transform.forward, out hit,1000))
        {
            if (hit.transform.gameObject.tag == "MovePos")
            {
                hit.transform.gameObject.GetComponent<MeshRenderer>().material = lit;
                StopCoroutine(changematerial(hit.transform.gameObject));
                StartCoroutine(changematerial(hit.transform.gameObject));
                if (Input.GetMouseButtonDown(0))
                {
                    this.GetComponent<NavMeshAgent>().SetDestination(hit.transform.position);
                }
            }
        }
    }

    public IEnumerator changematerial(GameObject hit)
    {
        
        yield return new WaitForSeconds(0.1f);
        hit.transform.gameObject.GetComponent<MeshRenderer>().material = normal;
    }
}
