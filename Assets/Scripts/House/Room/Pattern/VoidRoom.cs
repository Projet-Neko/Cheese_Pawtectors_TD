using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Overwritten;

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
