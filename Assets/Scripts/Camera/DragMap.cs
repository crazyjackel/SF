using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private SaveFile m_save;

    void Start()
    {
        Scroll = myCamera.orthographicSize * ScrollInverseSpeed;
        if (m_save.LevelSelect == SceneManager.GetActiveScene().name)
        {
            this.transform.position = m_save.CameraPos ?? this.transform.position;
            dist = transform.position.z;  // Distance camera is above map
            Scroll = m_save.CameraScroll ?? myCamera.orthographicSize * ScrollInverseSpeed;
        }
    }

    void Update()
    {
        Scroll = Mathf.Clamp(Scroll - Input.mouseScrollDelta.y, 3 * ScrollInverseSpeed, 20 * ScrollInverseSpeed);
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

        myCamera.orthographicSize = Scroll / ScrollInverseSpeed;
        m_save.CameraPos = this.transform.position;
        m_save.CameraScroll = Scroll;
    }
}