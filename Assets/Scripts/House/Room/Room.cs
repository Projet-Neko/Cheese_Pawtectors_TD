using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Room : MonoBehaviour
{
    protected bool[] _openings = new bool[4];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void RotationClockwise()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z -= 90;

        if (rotation.z < 0)
            rotation.z += 360;

        transform.eulerAngles = rotation;

        bool temp = _openings[3];
        for (int i = 3; i > 0; --i)
            _openings[i] = _openings[i - 1];
        _openings[0] = temp;
    }

    protected void RotationAnticlockwise()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z += 90;

        if (rotation.z >= 360)
            rotation.z -= 360;

        transform.eulerAngles = rotation;

        bool temp = _openings[0];
        for (int i = 0; i < 3; ++i)
            _openings[i] = _openings[i + 1];
        _openings[3] = temp;
    }
}
