using Godot;

public class LinearPath : Path
{

    private Vector2 start;
    private Vector2 end;

    public LinearPath(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
    }

    public float getLength()
    {
        return (end - start).Length();
    }

    public Vector2 getPosition(float t)
    {
        return start + (end - start) * t;
    }
}