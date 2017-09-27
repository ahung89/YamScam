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
        nonStartingMenus.ForEach(m => {
            ZoomablePanel zp = m.GetComponent<ZoomablePanel>();
            if (zp != null)
            {
                zp.GetComponent<Image>().raycastTarget = false;
                zp.enabled = false;
            }
        });
    }

    public void ZoomIntoPanel(ZoomablePanel panel)
    {
        panel.enabled = true;
        panelStack.Push(panel.gameObject);
    }

    public void PushPanel (GameObject panelPrefab)
    {
        GameObject panel = Instantiate(panelPrefab, transform);

        if (panelStack.Count > 0)
        {
            SetPanelEnabled(panelStack.Peek(), false);
        }

        panelStack.Push(panel);
    }

    public void PopPanel()
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
