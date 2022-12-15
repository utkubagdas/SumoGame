using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private CursorController CursorController;
    [SerializeField] private float MovementSpeed = 2f;
    private float _startYPos;

    private void Start()
    {
        _startYPos = transform.position.y;
    }

    private void Update()
    {
        var position = transform.position;
        var tempPos = position;
        tempPos.y = _startYPos;
        position = tempPos;
        position = Vector3.MoveTowards(position, CursorController.CursorForward.position,
            MovementSpeed * Time.deltaTime);
        transform.position = position;
    }
}
