using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation
{
    North,
    East,
    South,
    West
}

public class Room : MonoBehaviour
{
    protected bool[] _openings = new bool[4];
    protected Orientation _orientation = Orientation.North;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void ChangeOrientation()
    {
        if (_orientation == Orientation.West)
            _orientation = Orientation.North;
        else
            ++_orientation;

        bool temp = _openings[3];
        for (int i = 3; i > 0; --i)
            _openings[i] = _openings[i - 1];
        _openings[0] = temp;
    }
}
