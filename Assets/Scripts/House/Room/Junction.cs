using UnityEngine;

public class Junction : MonoBehaviour
{
    private bool _connected = false;

    public bool Connected { get => _connected; }

    private void OnCollisionEnter(Collision collision)
    {
        _connected = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        _connected = false;
    }
}
