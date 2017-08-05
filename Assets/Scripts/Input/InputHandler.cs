using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    private GameManager gameManager;

    private void Awake ()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.transform == null) return;

            if (hit.collider.tag == "BadYam")
            {
                Destroy(hit.collider.gameObject);
            }
            else if (hit.collider.tag == "Yam")
            {
                Destroy(hit.collider.gameObject);
                gameManager.IncrementGoodYamLost();
            }
        }
	}
}
