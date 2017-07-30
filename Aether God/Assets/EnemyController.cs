using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

   public GameObject formationPrefab;
   private List<Formation> formations;
	// Use this for initialization
	void Start () {
      formations = new List<Formation>();
      GameObject newFormation = Instantiate(formationPrefab, Vector3.zero, Quaternion.identity);
      formations.Add(newFormation.GetComponent<Formation>());

      foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
      {
         gameObject.GetComponent<Human>().JoinFormation(newFormation.GetComponent<Formation>());
      }

      newFormation.GetComponent<Formation>().MoveTo(GameObject.FindGameObjectWithTag("Enemy").transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
