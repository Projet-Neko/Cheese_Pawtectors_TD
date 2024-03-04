using UnityEngine;

public class MoveCameraAction
{
    private const float _speed = 0.1f;

    public void UpdateMovement(Vector2 InputDelta)
    {
        InputDelta = -InputDelta;                                                               // Invert the delta
        InputDelta *= _speed;                                                                   // Scale the delta
        Vector3 offset = new Vector3(InputDelta.x, InputDelta.y, 0);                            // Create the offset
        Camera.main.transform.position += offset;                                               // Move the camera
    }
}