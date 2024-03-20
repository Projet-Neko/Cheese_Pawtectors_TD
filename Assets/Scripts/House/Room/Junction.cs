using UnityEngine;

public class Junction : MonoBehaviour
{
    private bool _connected = false;    // True if the Junction is connected to another Junction
    private bool _valided = false;      // True if the Junction is belong to the valided path

    public bool Connected { get => _connected; }
    public bool Valided { get => _valided; }

    private void OnCollisionEnter(Collision collision)
    {
        _connected = true;

        Junction junction = collision.gameObject.GetComponent<Junction>();
        if (junction && junction.Valided)
        {
            _valided = true;
            // Notify room that the path is valided
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _connected = false;
    }
}
