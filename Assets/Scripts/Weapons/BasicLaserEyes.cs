using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLaserEyes : Weapon {
    public GameObject laserPrefab;
    public float joinDistance = 3;

    public override void HandleMouseDown ()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, collisionMask.value);
        Player player = Player.Instance;

        if (hit.transform == null)
        {
            GameObject left = Instantiate(laserPrefab, player.leftEye.transform.position, Quaternion.identity);
            GameObject right = Instantiate(laserPrefab, player.rightEye.transform.position, Quaternion.identity);

            Vector2 missDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position).normalized;
            Vector2 joinPos = (Vector2)player.transform.position + missDir * joinDistance;

            left.GetComponent<Laser>().SetMissDir(missDir, joinPos, true, right);
            right.GetComponent<Laser>().SetMissDir(missDir, joinPos, false);

            left.GetComponent<AudioSource>().Play();
        }
        else
        {
            GameObject left = Instantiate(laserPrefab, player.leftEye.transform.position, Quaternion.identity);
            GameObject right = Instantiate(laserPrefab, player.rightEye.transform.position, Quaternion.identity);

            Vector2 missDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position).normalized;
            Vector2 joinPos = (Vector2)player.transform.position + missDir * joinDistance;

            left.GetComponent<Laser>().SetTarget(hit.collider.gameObject, joinPos, true, right);
            right.GetComponent<Laser>().SetTarget(hit.collider.gameObject, joinPos, false);

            left.GetComponent<AudioSource>().Play();
        }
    }

}
