using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject laserPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject leftEye;
    public GameObject rightEye;
    public float joinDistance = 3;

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
                GameObject left = Instantiate(laserPrefab, leftEye.transform.position, Quaternion.identity);
                GameObject right = Instantiate(laserPrefab, rightEye.transform.position, Quaternion.identity);

                Vector2 missDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                Vector2 joinPos = (Vector2)transform.position + missDir * joinDistance;

                left.GetComponent<Laser>().SetMissDir(missDir, joinPos, true, right);
                right.GetComponent<Laser>().SetMissDir(missDir, joinPos, false);
            }
            else if (hit.transform != null && hit.collider.tag == "BadYam")
            {
                GameObject left = Instantiate(laserPrefab, leftEye.transform.position, Quaternion.identity);
                GameObject right = Instantiate(laserPrefab, rightEye.transform.position, Quaternion.identity);

                Vector2 missDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                Vector2 joinPos = (Vector2)transform.position + missDir * joinDistance;

                left.GetComponent<Laser>().SetTarget(hit.collider.gameObject, joinPos, true, right);
                right.GetComponent<Laser>().SetTarget(hit.collider.gameObject, joinPos, false);
            }
            else if (hit.transform != null && hit.collider.tag == "Yam")
            {
                GameObject left = Instantiate(laserPrefab, leftEye.transform.position, Quaternion.identity);
                GameObject right = Instantiate(laserPrefab, rightEye.transform.position, Quaternion.identity);

                Vector2 missDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                Vector2 joinPos = (Vector2)transform.position + missDir * joinDistance;

                left.GetComponent<Laser>().SetTarget(hit.collider.gameObject, joinPos, true, right);
                right.GetComponent<Laser>().SetTarget(hit.collider.gameObject, joinPos, false);
            }
        }
    }
}
