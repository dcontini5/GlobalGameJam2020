using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private float maxDistance;

    [SerializeField]
    private GameObject crashSite;

    private List<GameObject> mTiles;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        mTiles = new List<GameObject>();

        tilePrefab.GetComponent<TileGeneration>().Initalize(crashSite);

        GenerateMap();
    }

    void GenerateMap()
    {
        Vector3 cameraPos = camera.transform.position;
        startPos = cameraPos;

        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;

        float tileWidth = tileSize.x;
        float tileDepth = tileSize.z;

        int maxTilesX = (int)(maxDistance - (tileWidth / 2) / tileWidth);
        int maxTilesZ = (int)(maxDistance - (tileDepth / 2) / tileDepth);

        for (int xTileIndex = -maxTilesX; xTileIndex <= maxTilesX; xTileIndex++)
        {
            for (int zTileIndex = -maxTilesZ; zTileIndex <= maxTilesZ; zTileIndex++)
            {
                Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + cameraPos.x + xTileIndex * tileWidth, this.gameObject.transform.position.y, this.gameObject.transform.position.z + cameraPos.z + zTileIndex * tileDepth);

                Vector2 tileDist = new Vector2(tilePosition.x, tilePosition.z);

                if((tileDist - new Vector2(cameraPos.x, cameraPos.z)).magnitude > maxDistance)
                {
                    continue;
                }

                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;

                mTiles.Add(tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = camera.transform.position;

        bool createNew = false;

        for (int i = 0; i < mTiles.Count; i++)
        {
            Vector3 tilePosition = mTiles[i].transform.position;

            Vector2 tileDist = new Vector2(tilePosition.x, tilePosition.z);

            if ((tileDist - new Vector2(cameraPos.x, cameraPos.z)).magnitude > maxDistance)
            {
                Destroy(mTiles[i]);

                mTiles.RemoveAt(i);
                i--;
                createNew = true;
            }
        }

        if (createNew)
        {
            Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;

            float tileWidth = tileSize.x;
            float tileDepth = tileSize.z;

            float xCurrent = startPos.x + (int)((cameraPos.x - startPos.x) / tileWidth) * tileWidth;
            float zCurrent = startPos.z + (int)((cameraPos.z - startPos.z) / tileDepth) * tileDepth;

            int maxTilesX = (int)((maxDistance - (tileWidth / 2)) / tileWidth);
            int maxTilesZ = (int)((maxDistance - (tileDepth / 2)) / tileDepth);


            //

            //float tileWidth = tileSize.x;
            //float tileDepth = tileSize.z;

            //float xCurrent = startPos.x + (int)(cameraPos.x - startPos.x / tileWidth) * tileWidth;
            //float zCurrent = startPos.z + (int)(cameraPos.z - startPos.z / tileDepth) * tileDepth;

            //int maxTilesX = (int)(maxDistance - (tileWidth / 2) / tileWidth);
            //int maxTilesZ = (int)(maxDistance - (tileDepth / 2) / tileDepth);

            for (int xTileIndex = -maxTilesX; xTileIndex <= maxTilesX; xTileIndex++)
            {
                for (int zTileIndex = -maxTilesZ; zTileIndex <= maxTilesZ; zTileIndex++)
                {
                    Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xCurrent + xTileIndex * tileWidth, this.gameObject.transform.position.y, this.gameObject.transform.position.z + zCurrent + zTileIndex * tileDepth);

                    Vector2 tileDist = new Vector2(tilePosition.x, tilePosition.z);

                    if ((tileDist - new Vector2(cameraPos.x, cameraPos.z)).magnitude > maxDistance)
                    {
                        continue;
                    }

                    bool moveOn = false;

                    for (int i = 0; i < mTiles.Count; i++)
                    {
                        if (mTiles[i].transform.position == tilePosition)
                        {
                            moveOn = true;
                        }
                    }

                    if (moveOn)
                    {
                        continue;
                    }

                    GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;

                    mTiles.Add(tile);
                }
            }
        }
    }
}
