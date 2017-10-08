using UnityEngine;

public class Yam : MonoBehaviour {
    public GameObject explosion;
    public bool destroyed = false;
    public bool isBad = false;
    public bool tracked = false;
    public GameObject targetBeast;

    protected Rigidbody2D rb2d;
    protected GameObject spawner;

    public void Init(GameObject spawner, GameObject targetBeast)
    {
        this.spawner = spawner;
        this.targetBeast = targetBeast;
    }

    [SubscribeGlobal]
    public void HandleBeastKilled(BeastKilledEvent e)
    {
        if (targetBeast == e.beast)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        destroyed = true;
        Destroy(gameObject);

        if (this.isBad) 
        {
            PlayerDataManager.YamScore++;
        }
    }
}
