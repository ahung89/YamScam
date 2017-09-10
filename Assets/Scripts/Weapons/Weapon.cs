using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public LayerMask collisionMask;

    public virtual void HandleMouseDown() { }

    public virtual void HandleMouseUp() { }
}
