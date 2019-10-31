using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour
{
    public Transform PickPosition;
    public float pickingRange = 2.0f;
    NavMeshAgent agent;
    Camera playerCam;
    GameObject PickUp;
    bool objectPicked;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        objectPicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Ray ray = playerCam.ScreenPointToRay(point);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObject = hit.transform.gameObject;


                GoTo(hit.point);

            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Ray ray = playerCam.ScreenPointToRay(point);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.tag == "Food")
                {
                    if (Vector3.Distance(hitObject.transform.position,this.transform.position)<=pickingRange)
                    {
                        hitObject.transform.position = PickPosition.position;
                        PickUp = hitObject;
                        PickUp.GetComponent<Rigidbody>().freezeRotation = true;
                        objectPicked = true;
                    }
                    else
                    {
                        GoTo(hit.point);
                    }
                }
                else if (objectPicked)
                {
                    objectPicked = false;
                    Vector3 throwPosition = Vector3.Normalize(hit.point - this.transform.position);
                    PickUp.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                    PickUp.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                    PickUp.GetComponent<Rigidbody>().freezeRotation = false;
                    PickUp.GetComponent<Rigidbody>().AddForce(new Vector3(throwPosition.x, 5,throwPosition.z) * 60);
                }

            }
        }

        if (objectPicked)
        {
            PickUp.transform.position = PickPosition.position;
        }
    }

    void GoTo(Vector3 _destination)
    {
        agent.destination = _destination;
    }

}
