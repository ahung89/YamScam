using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailController : MonoBehaviour {
    public static TailController Instance;

    public bool controllingTail;
    public GameObject tailEnd;

    private Rigidbody2D tailEndRigidbody;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        controllingTail = false;
        tailEndRigidbody = tailEnd.GetComponent<Rigidbody2D>();
    }

    public LayerMask tailCollisionMask;

	void Update () {
        if (controllingTail)
        {
            tailEndRigidbody.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public bool HandleMouseDown()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tailCollisionMask.value);

        if (hit.collider != null)
        {
            controllingTail = true;
        }

        return controllingTail;
    }

    public void HandleMouseUp()
    {
        controllingTail = false;
    }
}
