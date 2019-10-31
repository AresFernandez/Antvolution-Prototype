using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour
{

    NavMeshAgent agent;
    Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
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
                GoTo(hit.point);

            }
        }
    }

    void GoTo(Vector3 _destination)
    {
        agent.destination = _destination;
    }

}
