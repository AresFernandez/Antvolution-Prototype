using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPuente_Behavior : MonoBehaviour
{
    public int antsNeeded = 2;
    public int actualAnts;

    public GameObject Player;
    public GameObject Base;
    public GameObject WaterAnt1;
    public GameObject WaterAnt2;
    public GameObject PuenteObstacle;

    // Start is called before the first frame update
    void Start()
    {
        actualAnts = 0;
    }

    public void DoPuenteWithAnt(GameObject other)
    {
        if (other.tag == "Ant")
        {
            if (other.GetComponent<Ant_Behavior>().movingToPoint == true)
            {
                if (actualAnts == 0)
                {
                    Player.GetComponent<Player_Movement>().ants.Remove(other);
                    Destroy(other.gameObject);
                    Base.GetComponent<Base_Behavior>().maxAnts--;
                    WaterAnt1.SetActive(true);
                    actualAnts++;
                }else if (actualAnts == 1)
                {
                    Player.GetComponent<Player_Movement>().ants.Remove(other);
                    Destroy(other.gameObject);
                    Base.GetComponent<Base_Behavior>().maxAnts--;
                    WaterAnt2.SetActive(true);
                    PuenteObstacle.SetActive(false);
                    actualAnts++;
                    this.gameObject.SetActive(false);
                }
                else
                {
                    other.GetComponent<Ant_Behavior>().movingToPoint = false;
                    other.GetComponent<Ant_Behavior>().FollowPlayer(true);
                }
            }
        }
    }
}
