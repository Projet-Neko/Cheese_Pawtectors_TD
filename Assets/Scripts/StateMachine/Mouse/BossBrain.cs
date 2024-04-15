using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBrain : MouseBrain
{
    protected override void Update()
    {
        base.Update();

        if ((Entity as Mouse).IsBoss)
        {
            // Lorsqu’elle est vaincue, elle se divise en deux en possédant 50% de points de vie en moins.
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
