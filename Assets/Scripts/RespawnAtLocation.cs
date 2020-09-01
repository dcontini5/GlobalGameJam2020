using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RespawnAtLocation : MonoBehaviour
{
    //Public
    public GameObject[] ores;
    public int[] rarity;
    public float SpawnArea;
    public float minSpawnDistance;
    public float RayCastHeight;
    public float RayCastLenght;

    [SerializeField]
    GameObject HUD = null;


    // Start is called before the first frame update
    void Start()
    {
        if(HUD != null)
        {
            RockInfo.HUD = HUD;
            Rotate.HUD = HUD;
        }
    }

    // Update is called once per frame
    void Update()    {


        int i = 0;

        foreach (GameObject ore in ores)
        {
            int j = rarity[i];
            while (j > 0)
            {

                SpawnOre(ore);
                j--;

            }

            i++;

        }


    }


    private void SpawnOre(GameObject ore)
    {

        float a = Random.value * 2 * Mathf.PI;
        float r = SpawnArea * Mathf.Sqrt(Random.value) + minSpawnDistance;

        float x = this.transform.position.x + r * Mathf.Cos(a);
        float z = this.transform.position.z + r * Mathf.Sin(a);



        Vector3 pos = new Vector3(x, RayCastHeight, z);

        if (Physics.Raycast(pos, -Vector3.up, out RaycastHit hit, RayCastLenght, 1))
        {

            Instantiate(ore, hit.point, ore.transform.rotation);
            if(ore)
            {
                if(ore.GetComponent<RockInfo>())
                {
                    //ore.GetComponent<RockInfo>().HUD = HUD;

                    ore.GetComponent<RockInfo>().Player = transform.parent.gameObject;
                    
                }
            }

        }


    }
   
    void OnCollisionExit(Collision collision){


        Debug.Log("Object has left the sphere of view");
        if(collision.gameObject.tag.Contains("Ore"))
        {

            //collision.gameObject.SetActive(false);
            collision.gameObject.GetComponent<Renderer>().enabled = false;
            Renderer[] children = collision.gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer child in children) child.enabled = false;
            int i = 0;
            foreach (GameObject ore in ores)
            {

                if (ore.tag == collision.gameObject.tag) rarity[i]++;
                i++;

            }
                
            

        }




    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Object has entered the sphere of view");
        if (collision.gameObject.tag.Contains("Ore"))
        {

            //collision.gameObject.SetActive(true);
            collision.gameObject.GetComponent<Renderer>().enabled = true;
            Renderer[] children = collision.gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer child in children) child.enabled = true;
            int i = 0;
            foreach (GameObject ore in ores)
            {

                if (ore.tag == collision.gameObject.tag) rarity[i]--;
                i++;

            }
             
                

        }
        

    }



}
