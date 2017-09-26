using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapMenu : MonoBehaviour {

    public void Back()
    {
        MenuManager.Instance.PopPanel();
    }
}
