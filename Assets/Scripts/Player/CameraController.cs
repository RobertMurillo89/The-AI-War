using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float Damping;

    [Header("----- ZoomComponents -----")]
    public float zoomSpeed;
    public float minOrthographicSize;
    public float maxOrthographicSize;


    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        // Adjust zoom based on input (e.g., mouse scroll wheel)
        float zoomChange = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        Camera.main.orthographicSize -= zoomChange;

        // Clamp the orthographic size to prevent it from getting too large or too small
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minOrthographicSize, maxOrthographicSize);
    }

    void LateUpdate()
    {
        // Apply offset only to X and Y
        Vector3 movePosition = new Vector3(Target.position.x + Offset.x, Target.position.y + Offset.y, transform.position.z);

        // Keep the camera's Z position constant
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, Damping);
    }
}
