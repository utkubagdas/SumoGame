using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    #region Local
    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;
    #endregion
    
    #region Public
    public Transform CursorForward;
    #endregion
    
    private void Start ()
    {
        _sensitivity = 0.4f;
        _rotation = Vector3.zero;
    }
     
    private void Update()
    {
        if(_isRotating)
        {
           SetRotate();
        }

        if (Input.GetMouseButtonDown(0))
        {
            InputDownSettings();
        }

        if (Input.GetMouseButtonUp(0))
        {
            InputUpSettings();
        }
    }

    private void SetRotate()
    {
        // offset
        _mouseOffset = (Input.mousePosition - _mouseReference);
             
        // apply rotation
        _rotation.y = +(_mouseOffset.x + _mouseOffset.y) * _sensitivity;
             
        // rotate
        transform.Rotate(_rotation);
             
        // store mouse
        _mouseReference = Input.mousePosition;
    }

    private void InputDownSettings()
    {
        // rotating cursor
        _isRotating = true;
         
        // store mouse
        _mouseReference = Input.mousePosition;
    }

    private void InputUpSettings()
    {
        // rotating cursor
        _isRotating = false;
    }
    
}
