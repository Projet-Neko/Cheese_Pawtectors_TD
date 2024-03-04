using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidRoom : Room
{
    // Start is called before the first frame update
    void Start()
    {
        _openings[0] = false;
        _openings[1] = false;
        _openings[2] = false;
        _openings[3] = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
