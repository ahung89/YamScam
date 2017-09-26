using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public static MenuManager Instance;

    public List<GameObject> nonStartingMenus;

    private Stack<GameObject> panelStack;

    private void Awake ()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
        panelStack = new Stack<GameObject>();
        nonStartingMenus.ForEach(Destroy);
    }

    public void PushPanel (GameObject panelPrefab, bool zoom = false)
    {
        GameObject panel = Instantiate(panelPrefab, transform);

        if (panelStack.Count > 0)
        {
            SetPanelEnabled(panelStack.Peek(), false);
        }

        panelStack.Push(panel);
    }

    public void PopPanel(bool zoom = false)
    {
        GameObject poppedPanel = panelStack.Pop();
        Destroy(poppedPanel);

        if (panelStack.Count > 0)
        {
            SetPanelEnabled(panelStack.Peek(), true);
        }
    }

    void SetPanelEnabled(GameObject panel, bool enabled)
    {
        panel.GetComponent<CanvasGroup>().interactable = enabled;
    }
}
