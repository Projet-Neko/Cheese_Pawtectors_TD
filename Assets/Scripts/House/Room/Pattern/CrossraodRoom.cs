using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossraodRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.MovedAndRemoved;

        _openings[0] = true;
        _openings[1] = true;
        _openings[2] = true;
        _openings[3] = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
