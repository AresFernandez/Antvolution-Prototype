using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour
{
    public Transform PickPosition;
    public float pickingRange = 2.0f;
    public float groupRange = 4.0f;
    NavMeshAgent agent;
    Camera playerCam;
    GameObject PickUp;
    bool objectPicked;
    public List<GameObject> ants;

    // Start is called before the first frame update
    void Start()
    {
        ants = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        playerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        objectPicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (objectPicked)
        {
            PickUp.transform.position = PickPosition.position;
        }


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

        if (Input.GetMouseButtonDown(1)) // Botón derecho
        {
            Vector3 point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Ray ray = playerCam.ScreenPointToRay(point);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.tag == "Food") //Si clicas en comida
                {
                    if (ants.Count>0) //Si tienes alguna hormiga siguiendote
                    {
                        //La primera hormiga libre coge la comida
                        foreach (var ant in ants)
                        {
                            if (ant.GetComponent<Ant_Behavior>().objectPicked == false)
                            {
                                ant.GetComponent<Ant_Behavior>().PickFood(hitObject);
                                break;
                            }
                        }                   
                    }
                    else if (Vector3.Distance(hitObject.transform.position,this.transform.position)<=pickingRange) //Si no te sigue nadie
                    {
                        //Yo cojo la comida
                        hitObject.transform.position = PickPosition.position;
                        PickUp = hitObject;
                        PickUp.GetComponent<Rigidbody>().freezeRotation = true;
                        objectPicked = true;
                    }
                }
                else if (hitObject.tag == "Player") //Si clico en mí mismo (player)
                {
                    ants.Clear();
                    //Agrupo las hormigas cercanas y que me sigan
                    foreach (var ant in GameObject.FindGameObjectsWithTag("Ant"))
                    {

                        if (Vector3.Distance(ant.transform.position,transform.position)<=groupRange)
                        {
                            ant.GetComponent<Ant_Behavior>().FollowPlayer(true);
                            ants.Add(ant);
                        }
                        else
                        {
                            ant.GetComponent<Ant_Behavior>().FollowPlayer(false);
                        }
                    }

                }
                else if (hitObject.tag == "Point") //Si clicas en Punto Puente
                {
                    //Si tienes alguna hormiga siguiendote y el puente necesita hormigas
                    if (ants.Count > 0 && hitObject.GetComponent<PointPuente_Behavior>().actualAnts < hitObject.GetComponent<PointPuente_Behavior>().antsNeeded) 
                    {
                        //La primera hormiga libre coge la comida
                        foreach (var ant in ants)
                        {
                            if (ant.GetComponent<Ant_Behavior>().movingToPoint == false && ant.GetComponent<Ant_Behavior>().objectPicked == false)
                            {
                                ant.GetComponent<Ant_Behavior>().GoToPointPuente(hit);
                                break;
                            }
                        }
                    }
                }
                else // Si no clico ni en comida ni en player
                {
                    foreach (var ant in ants) //Si alguna hormiga tiene cogida comida la tira
                    {
                        if (ant.GetComponent<Ant_Behavior>().objectPicked)
                        {
                            ant.GetComponent<Ant_Behavior>().ThrowFood(hit);
                        }
                    }
                    if (objectPicked) // Si yo tengo comida cogida la tiro
                    {
                        objectPicked = false;
                        PickUp.transform.position = PickPosition.position;
                        Vector3 throwPosition = Vector3.Normalize(hit.point - this.transform.position);
                        PickUp.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                        PickUp.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                        PickUp.GetComponent<Rigidbody>().freezeRotation = false;
                        PickUp.GetComponent<Rigidbody>().AddForce(new Vector3(throwPosition.x, 5, throwPosition.z) * 60);
                    }
                }


            }
        }


    }

    void GoTo(Vector3 _destination)
    {
        agent.destination = _destination;
    }

}
