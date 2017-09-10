using UnityEngine;

public struct BeastKilledEvent
{
    public GameObject beast;

    public BeastKilledEvent(GameObject beast)
    {
        this.beast = beast;
    }
}

public struct GoodYamLostEvent { }