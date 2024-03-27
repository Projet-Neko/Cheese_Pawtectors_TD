using System;
using UnityEngine;

public class Junction : MonoBehaviour
{
    private Junction _junctionConnected = null;  // The Junction that this Junction is connected to

    public event Func<Junction, bool> OnCheckPath;

    private void OnCollisionEnter(Collision collision)
    {
        _junctionConnected = collision.gameObject.GetComponent<Junction>();
    }

    private void OnCollisionExit(Collision collision)
    {
        _junctionConnected = null;
    }

    // Check if the junction is connected to another junction and if the next room is in a valid path
    public bool Validation()
    {
        return _junctionConnected && _junctionConnected.OnCheckPath != null && _junctionConnected.OnCheckPath.Invoke(_junctionConnected);
    }
}
