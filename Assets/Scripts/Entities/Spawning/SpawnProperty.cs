[System.Serializable]
public struct SpawnProperty {
    public YamProperty property;
    public float value;

    public SpawnProperty(YamProperty property, float value)
    {
        this.property = property;
        this.value = value;
    }
}

public enum YamProperty
{
    FlySpeedX, FlySpeedY, ElevateSpeedX, ElevateSpeedY
}
