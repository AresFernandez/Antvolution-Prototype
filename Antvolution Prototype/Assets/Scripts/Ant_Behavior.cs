using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ant_Behavior : MonoBehaviour
{
    private GameObject player;
    private GameObject PickUp;
    private GameObject Puente;
    public bool followPlayer;
    NavMeshAgent agent;
    public Transform PickPosition;
    public float pickingRange = 2.0f;
    public bool objectPicked;
    public bool movingToFood;
    public bool movingToPoint;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        movingToFood = objectPicked = followPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (objectPicked)
        {
            PickUp.transform.position = PickPosition.position;
        }

        if (followPlayer)
        {
            agent.destination = player.transform.position;
        }else if (movingToFood && Vector3.Distance(PickUp.transform.position, this.transform.position) <= pickingRange)
        {
            PickUp.transform.position = PickPosition.position;
            PickUp.GetComponent<Rigidbody>().freezeRotation = true;
            objectPicked = true;
            movingToFood = false;
            FollowPlayer(true);
        }else if (movingToPoint && Vector3.Distance(agent.destination, this.transform.position) <= pickingRange)
        {
            Puente.GetComponent<PointPuente_Behavior>().DoPuenteWithAnt(this.gameObject);
        }
    }

    public void FollowPlayer(bool b)
    {
        followPlayer = b;
    }

    public void GoToPointPuente(RaycastHit hit)
    {
        Puente = hit.transform.gameObject;
        FollowPlayer(false);
        agent.destination = hit.transform.position;
        movingToPoint = true;
    }

    public void PickFood(GameObject food)
    {
        PickUp = food;
        if (Vector3.Distance(food.transform.position, this.transform.position) <= pickingRange)
        {
            food.transform.position = PickPosition.position;
            food.GetComponent<Rigidbody>().freezeRotation = true;
            objectPicked = true;
        }
        else
        {
            FollowPlayer(false);
            movingToFood = true;
            agent.destination = food.transform.position;
        }
    }

    public void ThrowFood(RaycastHit hit)
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
