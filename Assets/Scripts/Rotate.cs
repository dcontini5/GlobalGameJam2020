using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rotate : MonoBehaviour
{

    public float rotationSpeed;
    public Vector3 rotation;
    public GameObject OxigenPS;
    public static GameObject HUD = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(rotation , Time.deltaTime * rotationSpeed);



    }


    public void DestroyMe()
    {
        Instantiate(OxigenPS, transform.position, transform.rotation);
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("adjawldkamwldmjawldk" + collision.gameObject.tag);   
        if(collision.gameObject.tag == "Player")
        {

            //Instantiate(OxigenPS, transform.position, transform.rotation);
            Destroy(gameObject);
            HUD.GetComponent<PlayerHUDScript>().CollectOxygen();
        }


    }

}
