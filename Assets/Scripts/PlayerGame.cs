using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGame : MonoBehaviour
{
    private Camera _mainCamera;

    public float SpeedRot;
    public float SpeedPos;

    private float _xRot;
    private float _yRot;
    private float _xPos;
    private float _zPos;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        
        _mainCamera = GetComponentInChildren<Camera>();
        _mainCamera.transform.LookAt(_mainCamera.transform.forward);
       
    }

    private void Update()
    {
        Cursor.visible = true;
        _xRot += Input.GetAxis("Mouse X") * SpeedRot;
        _yRot += Input.GetAxis("Mouse Y") * SpeedRot;
        _xPos = Input.GetAxisRaw("Horizontal");
        _zPos = Input.GetAxisRaw("Vertical");       
        MoveRot();
    }

    public Vector3 ShootDistance()
    {
        var mousePosition = Input.mousePosition;
        //var ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        var ray = _mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit.point;
    }

    private void MoveRot()
    {
        var cameraRot = Quaternion.Euler(-_yRot, _xRot, 0f);
        var playerRot = Quaternion.Euler(0f, _xRot, 0f);
        _mainCamera.transform.rotation = cameraRot;
        transform.rotation = playerRot;

        var distance = new Vector3(_xPos, 0f, _zPos);
        distance = transform.TransformDirection(distance);
        transform.position += distance * SpeedPos * Time.deltaTime;
    }
}
