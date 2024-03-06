using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.MovedAndRemoved;

        _openings[0] = false;
        _openings[1] = true;
        _openings[2] = true;
        _openings[3] = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
