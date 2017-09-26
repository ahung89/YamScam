using UnityEngine;

public class IconStrip : MonoBehaviour {

    public IconStripType type;

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

    public enum IconStripType { LostYams, Lives }

    [SubscribeGlobal]
    public void HandleBeastKilledEvent(BeastKilledEvent e)
    {
        if (type == IconStripType.Lives)
        {
            Mark();
        }
    }

    [SubscribeGlobal]
    public void GoodYamLostEvent (GoodYamLostEvent e)
    {
        if (type == IconStripType.LostYams)
        {
            Mark();
        }
    }
}
