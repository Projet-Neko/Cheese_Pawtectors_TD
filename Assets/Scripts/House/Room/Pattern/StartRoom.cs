using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Protected;

        _openings[0] = false;
        _openings[1] = true;
        _openings[2] = false;
        _openings[3] = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
