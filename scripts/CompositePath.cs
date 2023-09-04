using System.Collections.Generic;
using Godot;

public class CompositePath : Path
{

    List<Path> paths;
    List<float> pathStarts;

    float totalLength = 0;


    public CompositePath(List<Path> paths)
    {
        this.paths = new List<Path>();
        pathStarts = new List<float>();

        foreach (var path in paths)
        {
            this.paths.Add(path);
            totalLength += path.getLength();
        }

        float partialLength = 0;
        foreach (var path in this.paths)
        {
            pathStarts.Add(partialLength / totalLength);
            partialLength += path.getLength();
        }
    }

    public Vector2 getPosition(float t)
    {
        int index = 0;
        for (int i = 0; i < paths.Count; i++)
        {
            index = i;
            if (t < pathStarts[i])
            {
                index--;
                break;
            }
        }

        var path = paths[index];
        float pathStart = pathStarts[index];
        float nextStart = index == paths.Count - 1 ? 1 : pathStarts[index + 1];

        return path.getPosition((t - pathStart) / (nextStart - pathStart));
    }

    public float getLength()
    {
        return totalLength;
    }
}