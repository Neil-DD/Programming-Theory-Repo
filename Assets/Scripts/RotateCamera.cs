using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");

        transform.Rotate(Vector3.up, HorizontalInput * rotateSpeed * Time.deltaTime);
    }
}
