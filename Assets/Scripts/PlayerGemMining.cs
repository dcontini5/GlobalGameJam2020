using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerGemMining : MonoBehaviour
{
    #region Variables

    private List<GameObject> m_ListOfClosestGems = new List<GameObject>();
    private GameObject m_ClosestVisibleGem = null;

    [SerializeField]
    private float m_MaxFOVAngle = 50.0f;

    private bool m_IsMiningGem = false;


    #endregion //Variables

    #region Properties
    public bool IsMiningGem
    {
        get
        {
            return m_IsMiningGem;
        }
        set
        {
            m_IsMiningGem = value;
        }
    }


    #endregion //Properties

    #region Methods

    public void MineGem()
    {
        if(m_ClosestVisibleGem != null)
        {
            RockInfo rockInfo = m_ClosestVisibleGem.GetComponent<RockInfo>();

            if(rockInfo)
            {
                rockInfo.IsBeingMined = m_IsMiningGem;
            }
        }
    }

    public void RemoveFromList(GameObject objectToRemove)
    {
        if(m_ListOfClosestGems.Contains(objectToRemove))
        {
            m_ListOfClosestGems.Remove(objectToRemove);
        }
        
    }

    public void SelectClosestObject()
    {
        float shortestDistance = float.MaxValue;
        float distanceFromGem = 0.0f;
        Vector3 positionOfPlayer = Vector3.zero;
        Vector3 positionOfGem = Vector3.zero;
        m_ClosestVisibleGem = null;

        for (int i = 0; i < m_ListOfClosestGems.Count; i++)
        {
           if(m_ListOfClosestGems[i] != null)
            {
                positionOfGem = m_ListOfClosestGems[i].transform.position;

                positionOfPlayer = transform.position;

                //Check to see of the gem is behind the player
                Vector3 toGemVec = positionOfGem - positionOfPlayer;
                toGemVec.Normalize();
                float dotProd = Vector3.Dot(toGemVec, transform.forward);

                //This is infront of the player
                if (dotProd > 0.0f)
                {
                    //Calculate distance between the gem and the player to find the closest one
                    positionOfGem.y = 0.0f; //Simply to XZ plane
                    positionOfPlayer.y = 0.0f; //Simply to XZ Plane

                    distanceFromGem = Vector3.Distance(positionOfGem, positionOfPlayer);

                    if (distanceFromGem < shortestDistance)
                    {
                        shortestDistance = distanceFromGem;

                        //Check to see if object is within field of view
                        float angle = Vector3.Angle(transform.parent.forward, toGemVec);
                        Debug.Log(angle);
                        if (angle <= m_MaxFOVAngle)
                        {
                            m_ClosestVisibleGem = m_ListOfClosestGems[i];
                        }
                    }
                }
            }
            
           
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        SelectClosestObject();

        MineGem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Contains("Ore") && other.gameObject.tag != "Ore")
        {
            m_ListOfClosestGems.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Ore") && other.gameObject.tag != "Ore")
        {
            if(m_ListOfClosestGems.Contains(other.gameObject))
            {
                m_ListOfClosestGems.Remove(other.gameObject);
            }
        }
    }

    #endregion //Methods
}
