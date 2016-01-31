using UnityEngine;
using System.Collections;

public class WorldObjectRandomFacing : MonoBehaviour {

	protected void Start () {
	    if (Random.Range(0, 2) == 0) {
	        GetComponent<SpriteRenderer>().flipX = true;
	    }
	}
}
