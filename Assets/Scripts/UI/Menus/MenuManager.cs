using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public static MenuManager Instance;

    public List<GameObject> nonStartingMenus;

    private void Awake ()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
        nonStartingMenus.ForEach(m => m.SetActive(true));
    }
}
