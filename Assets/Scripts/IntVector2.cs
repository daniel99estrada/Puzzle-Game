using UnityEngine;

[System.Serializable]
public class IntVector2
{
    public int x;
    public int y;

    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public IntVector2(Vector2 vector)
    {
        x = Mathf.RoundToInt(vector.x);
        y = Mathf.RoundToInt(vector.y);
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x + b.x, a.y + b.y);
    }
}
