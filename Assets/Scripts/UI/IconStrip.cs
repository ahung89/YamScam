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
        int i = 0;
        while (!marked && i < icons.Length)
        {
            marked = icons[i].Mark();
            i++;
        }
    }
}
