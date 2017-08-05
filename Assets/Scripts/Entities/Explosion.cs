using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	void Awake () {
        Destroy(gameObject, .5f);
	}
}
