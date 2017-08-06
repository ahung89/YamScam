using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject laserPrefab;
    public GameObject muzzleFlashPrefab;

    private GameManager gameManager;

    private void Awake ()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void OnLevelWasLoaded()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.transform == null || (hit.collider.tag != "BadYam" && hit.collider.tag != "Yam"))
            {
                GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                laser.GetComponent<Laser>().SetMissDir(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            }
            else if (hit.transform != null && hit.collider.tag == "BadYam")
            {
                GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);                laser.GetComponent<Laser>().SetTarget(hit.collider.gameObject);
                laser.GetComponent<Laser>().SetTarget(hit.collider.gameObject);
                //Destroy(hit.collider.gameObject);
            }
            else if (hit.transform != null && hit.collider.tag == "Yam")
            {
                //Destroy(hit.collider.gameObject);
                GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                laser.GetComponent<Laser>().SetTarget(hit.collider.gameObject);
                gameManager.IncrementGoodYamLost();
            }
        }
    }
}
