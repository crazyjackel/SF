using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMap : MonoBehaviour
{
    private float dist;
    [SerializeField]
    private Vector3 MouseStart;
    [SerializeField]
    private float Scroll;
    [SerializeField]
    private float ScrollInverseSpeed = 5;
    [SerializeField]
    private Camera myCamera;

    void Start()
    {
        dist = transform.position.z;  // Distance camera is above map
        Scroll = myCamera.orthographicSize * ScrollInverseSpeed;
    }

    void Update()
    {
        Scroll -= Input.mouseScrollDelta.y;
        if (Input.GetMouseButtonDown(1))
        {
            MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            MouseStart = Camera.main.ScreenToWorldPoint(MouseStart);
            MouseStart.z = transform.position.z;

        }
        else if (Input.GetMouseButton(1))
        {
            var MouseMove = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            MouseMove = myCamera.ScreenToWorldPoint(MouseMove);
            MouseMove.z = transform.position.z;
            transform.position = transform.position - (MouseMove - MouseStart);
        }

        myCamera.orthographicSize = Mathf.Clamp(Scroll / ScrollInverseSpeed, 3, 20);
    }
}