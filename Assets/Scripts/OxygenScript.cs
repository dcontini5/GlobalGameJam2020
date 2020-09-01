using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenScript : MonoBehaviour
{
    [SerializeField]
    private GameObject HUD;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CollideWithPlayer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            CollideWithPlayer();
        }
    }

    private void CollideWithPlayer()
    {
        HUD.GetComponent<PlayerHUDScript>().FillOxygen();
    }
}
