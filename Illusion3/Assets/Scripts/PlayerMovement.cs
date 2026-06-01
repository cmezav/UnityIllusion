using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float sensibilidad = 2f;

    private CharacterController controller;
    private float rotacionVertical = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movimiento = transform.right * x + transform.forward * z;
        controller.Move(movimiento * velocidad * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        rotacionVertical -= mouseY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -80f, 80f);

        transform.Rotate(Vector3.up * mouseX);

        Transform cam = GetComponentInChildren<Transform>();
        if (cam != null)
        {
            foreach (Transform hijo in transform)
            {
                if (hijo.GetComponent<Camera>() != null)
                {
                    hijo.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
                    break;
                }
            }
        }
    }
}