using UnityEngine;

public class WoodenYam : Yam {
    
    void Start()
    {
    }

    private void Update() 
    {
    }

    void OnCollisionEnter2D(Collision2D col) 
    {        
        if (col.gameObject.name == "WoodpeckerBody") 
        {
            Destroy(this.gameObject);
            // TODO: play a wooden yam cracking sound
        } 
        else 
        {
            // TODO: play a collision sound
        }
    }
}
