using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorRoom : Room
{
    // Start is called before the first frame update
    void Start()
    {
        _openings[0] = false;
        _openings[1] = true;
        _openings[2] = false;
        _openings[3] = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
