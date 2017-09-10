using UnityEngine;

public class ElevatedYam : Yam {

    public float speed;

    private void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        float rot = spawner.transform.eulerAngles.z * Mathf.Deg2Rad - Mathf.PI / 2f;
        rb2d.velocity = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot)) * speed;
    }
}
