using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogBehavior : MonoBehaviour
{
    public GameObject player;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) <= distance)
        {
            player.GetComponent<Player_Movement>().fogs.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
