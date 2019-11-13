using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Base_Behavior : MonoBehaviour
{
    public int maxAnts;
    public int maxFlyingAnts;
    public float spawnTime = 2.0f;
    public int antsToWin = 5;

    //public Text antText;
    public TextMeshProUGUI antText;
    public GameObject ant;
    public GameObject flyingAnt;
    public GameObject YouWin;
    public GameObject BlackFade;

    float startTime;
    int antCount;
    int flyingAntCount;

    // Start is called before the first frame update
    void Start()
    {
        maxFlyingAnts = maxAnts = 0;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        antText.text = "Ants: " + (maxAnts+maxFlyingAnts);

        //Contador hormigas
        antCount = 0;
        flyingAntCount = 0;
        foreach (var ant in GameObject.FindGameObjectsWithTag("Ant"))
        {
            if (ant.GetComponent<Ant_Behavior>().flyingAnt)
            {
                flyingAntCount++;
            }
            else
            {
                antCount++;
            }
        }


        //Spawn hormigas
        if (Time.time - startTime >= spawnTime && antCount < maxAnts)
        {
            startTime = Time.time;
            Instantiate(ant, transform.position, transform.rotation);
        }
        else if (Time.time - startTime >= spawnTime && flyingAntCount < maxFlyingAnts)
        {
            startTime = Time.time;
            Instantiate(flyingAnt, transform.position, transform.rotation);
        }

        if (maxAnts + maxFlyingAnts >= antsToWin)
        {
            YouWin.SetActive(true);
            BlackFade.SetActive(true);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Food")
        {
            startTime = Time.time;
            maxAnts++;
            Destroy(other.gameObject);
        }
        if (other.tag == "BigFood")
        {
            startTime = Time.time;
            maxFlyingAnts++;
            Destroy(other.gameObject);
        }
    }
}
