using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

   [SerializeField] float density = 60;
   public GameObject RockPrefab;

   private Terrain terrain;
   // Use this for initialization
   void Start()
   {
      terrain = GetComponentInParent<Terrain>();
      PlaceRocks();
   }

   private void PlaceRocks()
   {
      Vector3 terrainSize = terrain.terrainData.size;
      float fieldSizeX = terrainSize.x / density;
      float fieldSizeZ = terrainSize.z / density;

      for (int wx = 0; wx < density; wx++)
      {
         for (int wz = 0; wz < density; wz++)
         {
            Vector3 rockPos = new Vector3();
            rockPos.x = UnityEngine.Random.Range((fieldSizeX * wx) + 1, (fieldSizeX * (wx + 1)) - 1);
            rockPos.z = UnityEngine.Random.Range((fieldSizeZ * wz) + 1, (fieldSizeZ * (wz + 1)) - 1);
            //rockPos.y = terrain.terrainData.GetHeight((int)rockPos.x, (int)rockPos.z) + terrain.transform.position.y;
            rockPos.y = terrain.SampleHeight(rockPos) + terrain.transform.position.y;
            Quaternion quaternion = Quaternion.Euler(UnityEngine.Random.Range(-40, 40), UnityEngine.Random.Range(-40, 40), UnityEngine.Random.Range(-40, 40));

            GameObject FreshRock = Instantiate(RockPrefab, rockPos, quaternion, GameObject.Find("Enviroment").transform); //Enviroment must be placed at same y position
         }
      }
   }

   // Update is called once per frame
   void Update()
   {

   }
}
