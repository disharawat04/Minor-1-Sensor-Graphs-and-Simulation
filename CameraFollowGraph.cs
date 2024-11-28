using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed = 5f; // Speed for keyboard movement
    public float mouseSensitivity = 100f; // Speed for mouse rotation
    public float scrollSpeed = 10f; // Speed for zooming with the mouse scroll wheel
    private float rotationX = 0f; // Camera rotation on the X-axis
    private float rotationY = 0f; // Camera rotation on the Y-axis

    void Update()
    {
        // Keyboard movement
        HandleKeyboardMovement();

        // Mouse rotation 
        HandleMouseRotation();

        // Mouse scrolling (zoom in and out)
        HandleMouseScroll();
    }

    private void HandleKeyboardMovement()
    {
        
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 

        Vector3 movement = new Vector3(horizontal, vertical, 0) * cameraSpeed * Time.deltaTime;

        transform.Translate(movement, Space.World);
    }

    private void HandleMouseRotation()
    {
 
        if (Input.GetMouseButton(1)) 
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            rotationX -= mouseY;
            rotationY += mouseX;

            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
    }

    private void HandleMouseScroll()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        Vector3 zoomMovement = transform.forward * scrollInput * scrollSpeed * Time.deltaTime;

        transform.position += zoomMovement;
    }
}
