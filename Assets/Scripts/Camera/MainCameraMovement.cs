using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{
    public float moveSpeed;
    public float zoomSpeed;

    private float minX = -360.0f;
    private float maxX = 360.0f;

    private float minY = -50.0f;
    private float maxY = -10.0f;

    public float sensX = 0.0f;
    public float sensY = 0.0f;

    float rotationY = -45.0f;
    float rotationX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * moveSpeed * Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButton(2))
        {
            rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
            transform.localEulerAngles = new Vector3(0, rotationX, 0);
            Camera.main.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (transform.position.y >= 1)
        {
            transform.Translate(0, scroll * zoomSpeed, 0, Space.World);
        }
        else if(transform.position.y < 1 && scroll < 0)
        {
            transform.Translate(0, scroll * zoomSpeed, 0, Space.World);
        }
        else
        {
            Debug.Log("TOO LOW");
        }
    }
}
