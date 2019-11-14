using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour
{
    public Transform PickPosition;
    public Vector3 AveragePickPosition;
    public float pickingRange = 2.0f;
    public float groupRange = 4.0f;
    public GameObject GroupParticles;
    public GameObject MoveParticles;
    public GameObject ImTooFar;
    public GameObject WeNeedMoreAnts;
    public GameObject WeCantReachThat;
    NavMeshAgent agent;
    Camera playerCam;
    GameObject PickUp;
    GameObject BigFood;
    bool objectPicked;
    public List<GameObject> ants;
    public List<GameObject> bigFoodAnts;
    public bool antsPickedBigFood;

    // Start is called before the first frame update
    void Start()
    {
        ants = new List<GameObject>();
        bigFoodAnts = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        playerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        antsPickedBigFood = objectPicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (objectPicked)
        {
            PickUp.transform.position = PickPosition.position;
        }

        if (antsPickedBigFood)
        {
            AveragePickPosition = new Vector3(0, 0, 0);

            foreach (var ant in bigFoodAnts)
            {
                AveragePickPosition += ant.GetComponent<Ant_Behavior>().PickPosition.position;
            }

            AveragePickPosition /= bigFoodAnts.Count;
            BigFood.transform.position = AveragePickPosition;
            BigFood.GetComponent<Rigidbody>().freezeRotation = true;
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
                    if (hitObject.GetComponent<FlyingObjective>().flyingObjective == false) //Si está en el suelo
                    {
                        if (ants.Count > 0) //Si tienes alguna hormiga siguiendote
                        {
                            Instantiate(MoveParticles, new Vector3(hitObject.transform.position.x, hitObject.transform.position.y - 0.25f, hitObject.transform.position.z), GroupParticles.transform.rotation, hitObject.transform);
                            //La primera hormiga libre coge la comida
                            foreach (var ant in ants)
                            {
                                if (ant.GetComponent<Ant_Behavior>().objectPicked == false
                                    && ant.GetComponent<Ant_Behavior>().movingToFood == false
                                    && ant.GetComponent<Ant_Behavior>().movingToBigFood == false
                                    && ant.GetComponent<Ant_Behavior>().movingToPoint == false
                                    && ant.GetComponent<Ant_Behavior>().pickBigFood == false)
                                {
                                    ant.GetComponent<Ant_Behavior>().PickFood(hitObject);
                                    break;
                                }
                            }
                        }
                        else if (Vector3.Distance(hitObject.transform.position, this.transform.position) <= pickingRange) //Si no te sigue nadie
                        {
                            //Yo cojo la comida
                            hitObject.transform.position = PickPosition.position;
                            PickUp = hitObject;
                            PickUp.GetComponent<Rigidbody>().freezeRotation = true;
                            objectPicked = true;
                        }
                        else //Si estoy solo y muy lejos de la comida
                        {
                            ImTooFar.SetActive(true); //Dialogo estoy muy lejos
                        }
                    }
                    else //Si está en altura
                    {
                        bool weHaveFlyingAnt = false;
                        if (ants.Count > 0) //Si tienes alguna hormiga siguiendote
                        {
                            
                            //La primera hormiga libre coge la comida
                            foreach (var ant in ants)
                            {
                                if (ant.GetComponent<Ant_Behavior>().objectPicked == false
                                    && ant.GetComponent<Ant_Behavior>().movingToFood == false
                                    && ant.GetComponent<Ant_Behavior>().movingToBigFood == false
                                    && ant.GetComponent<Ant_Behavior>().movingToPoint == false
                                    && ant.GetComponent<Ant_Behavior>().pickBigFood == false
                                    && ant.GetComponent<Ant_Behavior>().flyingAnt == true)
                                {
                                    ant.GetComponent<Ant_Behavior>().PickFood(hitObject);
                                    Instantiate(MoveParticles, new Vector3(hitObject.transform.position.x, hitObject.transform.position.y - 0.25f, hitObject.transform.position.z), GroupParticles.transform.rotation, hitObject.transform);
                                    weHaveFlyingAnt = true;
                                    break;
                                }
                            }
                        }

                        if (!weHaveFlyingAnt)
                        {
                            WeCantReachThat.SetActive(true);
                        }
                    }
                    
                }
                else if (hitObject.tag == "BigFood") //Si clicas en comida
                {
                    int counter = ants.Count;
                    foreach (var ant in ants) //Contamos las hormigas que te siguen que estén libres
                    {
                        if (ant.GetComponent<Ant_Behavior>().objectPicked
                            || ant.GetComponent<Ant_Behavior>().movingToFood
                            || ant.GetComponent<Ant_Behavior>().movingToBigFood
                            || ant.GetComponent<Ant_Behavior>().movingToPoint
                            || ant.GetComponent<Ant_Behavior>().pickBigFood)
                        {
                            counter--;
                        }
                    }

                    if (counter >= hitObject.GetComponent<BigFood_Behavior>().antsNeeded) //Si tienes el minimo de hormigas para coger la comida
                    {
                        counter = 0;
                        Instantiate(MoveParticles, new Vector3(hitObject.transform.position.x, hitObject.transform.position.y - 0.25f, hitObject.transform.position.z), GroupParticles.transform.rotation, hitObject.transform);
                        //Las primeras hormigas libres cogen la comida
                        foreach (var ant in ants)
                        {
                            if (ant.GetComponent<Ant_Behavior>().objectPicked == false
                                && ant.GetComponent<Ant_Behavior>().movingToFood == false
                                && ant.GetComponent<Ant_Behavior>().movingToBigFood == false
                                && ant.GetComponent<Ant_Behavior>().movingToPoint == false
                                && ant.GetComponent<Ant_Behavior>().pickBigFood == false)
                            {
                                ant.GetComponent<Ant_Behavior>().PickBigFood(hitObject);
                                counter++;
                                if (counter >= hitObject.GetComponent<BigFood_Behavior>().antsNeeded)
                                {
                                    break;
                                }
                            }
                        }

                    }
                    else //Si no hay suficientes hormigas para coger la comida grande
                    {
                        WeNeedMoreAnts.SetActive(true); //Dialogo no hay suficientes hormigas
                    }
                }
                else if (hitObject.tag == "Player") //Si clico en mí mismo (player)
                {
                    Instantiate(GroupParticles, new Vector3(this.transform.position.x, this.transform.position.y - 0.15f, this.transform.position.z), GroupParticles.transform.rotation, this.transform);
                    ants.Clear();
                    //Agrupo las hormigas cercanas y que me sigan
                    foreach (var ant in GameObject.FindGameObjectsWithTag("Ant"))
                    {

                        if (Vector3.Distance(ant.transform.position,transform.position)<=groupRange 
                            || ant.GetComponent<Ant_Behavior>().movingToFood
                            || ant.GetComponent<Ant_Behavior>().movingToBigFood
                            || ant.GetComponent<Ant_Behavior>().movingToPoint
                            || ant.GetComponent<Ant_Behavior>().pickBigFood)
                        {
                            ant.GetComponent<Ant_Behavior>().movingToFood = false;
                            ant.GetComponent<Ant_Behavior>().movingToBigFood = false;
                            ant.GetComponent<Ant_Behavior>().movingToPoint = false;
                            ant.GetComponent<Ant_Behavior>().pickBigFood = false;
                            ant.GetComponent<Ant_Behavior>().FollowPlayer(true);
                            ants.Add(ant);
                        }
                        else 
                        {
                            //ant.GetComponent<Ant_Behavior>().FollowPlayer(false);
                        }
                    }

                }
                else if (hitObject.tag == "Point") //Si clicas en Punto Puente
                {
                    //Si tienes alguna hormiga siguiendote y el puente necesita hormigas
                    if (ants.Count > 0 && hitObject.GetComponent<PointPuente_Behavior>().actualAnts < hitObject.GetComponent<PointPuente_Behavior>().antsNeeded) 
                    {
                        Instantiate(MoveParticles, new Vector3(hitObject.transform.position.x, hitObject.transform.position.y + 0.25f, hitObject.transform.position.z), GroupParticles.transform.rotation, hitObject.transform);
                        //La primera hormiga libre coge la comida
                        foreach (var ant in ants)
                        {
                            if (ant.GetComponent<Ant_Behavior>().objectPicked == false
                                && ant.GetComponent<Ant_Behavior>().movingToFood == false
                                && ant.GetComponent<Ant_Behavior>().movingToBigFood == false
                                && ant.GetComponent<Ant_Behavior>().movingToPoint == false
                                && ant.GetComponent<Ant_Behavior>().pickBigFood == false)
                            {
                                ant.GetComponent<Ant_Behavior>().GoToPointPuente(hit);
                                break;
                            }
                        }
                    }
                }
                else // Si no clico ni en comida ni en player
                {
                    if (antsPickedBigFood)
                    {
                        antsPickedBigFood = false;
                        BigFood.transform.position = AveragePickPosition;
                        Vector3 throwPosition = Vector3.Normalize(hit.point - AveragePickPosition);
                        BigFood.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                        BigFood.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                        BigFood.GetComponent<Rigidbody>().freezeRotation = false;
                        BigFood.GetComponent<Rigidbody>().AddForce(new Vector3(throwPosition.x, 5, throwPosition.z) * 60);
                    }
                    else
                    {
                        if (hitObject.tag == "Base")
                        {
                            if (ants.Count > 0) //Si tienes alguna hormiga siguiendote
                            {
                                Instantiate(MoveParticles, new Vector3(hitObject.transform.position.x, hitObject.transform.position.y + 0.25f, hitObject.transform.position.z), GroupParticles.transform.rotation, hitObject.transform);
                                //Dejan de seguirme
                                foreach (var ant in ants)
                                {
                                    if (ant.GetComponent<Ant_Behavior>().objectPicked == false &&
                                        ant.GetComponent<Ant_Behavior>().movingToFood == false &&
                                        ant.GetComponent<Ant_Behavior>().movingToBigFood == false &&
                                        ant.GetComponent<Ant_Behavior>().movingToPoint == false &&
                                        ant.GetComponent<Ant_Behavior>().pickBigFood == false)
                                    {
                                        ant.GetComponent<Ant_Behavior>().FollowPlayer(false);
                                    }
                                }

                                //Si me siguen las añado de nuevo a mi lista
                                ants.Clear();
                                foreach (var ant in GameObject.FindGameObjectsWithTag("Ant"))
                                {
                                    if (ant.GetComponent<Ant_Behavior>().followPlayer)
                                    {
                                        ants.Add(ant);
                                    }
                                }

                            }
                        }

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


    }

    void GoTo(Vector3 _destination)
    {
        agent.destination = _destination;
    }

    public void AntsPickedBigFood(GameObject _BigFood)
    {
        antsPickedBigFood = true;
        BigFood = _BigFood;
        bigFoodAnts.Clear();
        foreach (var ant in ants)
        {
            if (ant.GetComponent<Ant_Behavior>().pickBigFood == true)
            {
                bigFoodAnts.Add(ant);
                ant.GetComponent<Ant_Behavior>().FollowPlayer(true);
                ant.GetComponent<Ant_Behavior>().pickBigFood = false;
            }
        }
    }

}
