﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ant_Behavior : MonoBehaviour
{
    private GameObject player;
    private GameObject antBase;
    private GameObject PickUp;
    private GameObject Puente;
    public bool followPlayer;
    NavMeshAgent agent;
    public Transform PickPosition;
    public float pickingRange;
    public float wanderRange;
    public float wanderminChangeTime;
    public float wandermaxChangeTime;
    public bool objectPicked;
    public bool movingToFood;
    public bool movingToBase;
    public bool movingToPoint;
    public bool movingToBigFood;
    public bool pickBigFood;
    public bool flyingAnt;


    Vector3 initialPosition;
    float initWanderTime;
    float wanderTime;

    // Start is called before the first frame update
    void Start()
    {
        pickingRange = 2.0f;
        wanderRange = 4f;
        wanderminChangeTime = 4.5f;
        wandermaxChangeTime = 7.0f;
        initialPosition = this.transform.position;
        initWanderTime = Time.time;
        wanderTime = 0.5f;

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        antBase = GameObject.FindGameObjectWithTag("Base");
        pickBigFood = movingToBigFood = movingToFood = objectPicked = followPlayer = false;
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
            movingToBase = true;
            agent.destination = antBase.transform.position;
        }
        else if (movingToBase && Vector3.Distance(antBase.transform.position, this.transform.position) <= pickingRange)
        {

            if (pickBigFood)
            {
                //player.GetComponent<Player_Movement>().ThrowBigFood(antBase.transform.position);
            }
            else
            {
                ThrowFood(antBase.transform.position);
            }

            movingToBase = false;
        }
        else if (movingToBigFood && Vector3.Distance(PickUp.transform.position, this.transform.position) <= pickingRange)
        {
            //PickUp.transform.position = PickPosition.position;
            //PickUp.GetComponent<Rigidbody>().freezeRotation = true;
            //objectPicked = true;
            //movingToFood = false;
            //FollowPlayer(true);
            movingToBigFood = false;


        }
        else if (pickBigFood && !movingToBigFood)
        {
            int counter = 0;
            foreach (var ant in player.GetComponent<Player_Movement>().ants)
            {
                if (ant.GetComponent<Ant_Behavior>().movingToBigFood == false && ant.GetComponent<Ant_Behavior>().pickBigFood == true)
                {
                    counter++;
                }
            }
            if (counter >= PickUp.GetComponent<BigFood_Behavior>().antsNeeded)
            {
                player.GetComponent<Player_Movement>().AntsPickedBigFood(PickUp);
            }

        }
        else if (movingToPoint && Vector3.Distance(agent.destination, this.transform.position) <= pickingRange/2)
        {
            Puente.GetComponent<PointPuente_Behavior>().DoPuenteWithAnt(this.gameObject);
        }
        else if(!movingToFood && !movingToPoint && !movingToBigFood && !pickBigFood && !movingToBase)
        {
            if (Time.time-initWanderTime >= wanderTime)
            {
                agent.destination = new Vector3(Random.Range(initialPosition.x - wanderRange, initialPosition.x + wanderRange),
                    this.transform.position.y,
                    Random.Range(initialPosition.z - wanderRange, initialPosition.z + wanderRange));
                initWanderTime = Time.time;
                wanderTime = Random.Range(wanderminChangeTime, wandermaxChangeTime);
            }

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

    public void PickBigFood(GameObject food)
    {
        PickUp = food;
        FollowPlayer(false);
        movingToBigFood = true;
        pickBigFood = true;
        agent.destination = food.transform.position;
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

    public void ThrowFood(Vector3 target)
    {
        objectPicked = false;
        PickUp.transform.position = PickPosition.position;
        Vector3 throwPosition = Vector3.Normalize(target - this.transform.position);
        PickUp.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        PickUp.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        PickUp.GetComponent<Rigidbody>().freezeRotation = false;
        PickUp.GetComponent<Rigidbody>().AddForce(new Vector3(throwPosition.x, 5, throwPosition.z) * 60);
    }

    public void GoTo(Vector3 _destination)
    {
        agent.destination = _destination;
    }
}
