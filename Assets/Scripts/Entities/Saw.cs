using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour {

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "BadYam")
        {
            Destroy(other.gameObject);
        }
        else if (other.tag == "Yam")
        {
            Destroy(other.gameObject);
            // Trigger effect/deduct health?
        }
    }
}
