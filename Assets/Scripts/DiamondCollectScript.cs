using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondCollectScript : MonoBehaviour
{
    [SerializeField]
    private int diamondRequired;

    private int diamondGathered = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "( 0 / " + diamondRequired.ToString() + " )";
    }

    // Update is called once per frame
    public void CollectDiamond()
    {
        diamondGathered++;

        if (diamondGathered >diamondRequired)
        {
            diamondGathered = diamondRequired;
        }

        GetComponent<Text>().text = "( " + diamondGathered.ToString() + " / " + diamondRequired.ToString() + " )";
    }

    public bool CollectedAll()
    {
        return diamondGathered >= diamondRequired;
    }
}
