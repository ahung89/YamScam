using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconStrip : MonoBehaviour {

    Icon[] icons;

    void Awake()
    {
        icons = GetComponentsInChildren<Icon>();
    }

	public void Mark()
    {
        bool marked = false;
        int i = icons.Length - 1;
        while (!marked && i >= 0)
        {
            marked = icons[i].Mark();
            i--;
        }
    }
}
