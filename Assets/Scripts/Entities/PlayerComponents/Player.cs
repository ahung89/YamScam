using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance;

    public GameObject leftEye;
    public GameObject rightEye;
    public Weapon currentWeapon;

    private void Awake ()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TailController.Instance == null || !TailController.Instance.HandleMouseDown())
            {
                currentWeapon.HandleMouseDown();
            }
        }

        if (Input.GetMouseButtonUp(0) && TailController.Instance != null)
        {
            TailController.Instance.HandleMouseUp();
        }
    }
}
