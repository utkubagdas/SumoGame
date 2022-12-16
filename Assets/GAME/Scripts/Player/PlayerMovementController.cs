using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private CursorController CursorController;
    private float _startYPos;
    
    [SerializeField] private float _movementSpeed = 2f;

    public float MovementSpeed => _movementSpeed;

    private void Start()
    {
        _startYPos = transform.position.y; // get the initial y value to clamp it
    }

    private void Update()
    {
        var position = transform.position;
        var tempPos = position;
        tempPos.y = _startYPos; // clamp the y so that the height does not distort
        position = tempPos;
        position = Vector3.MoveTowards(position, CursorController.CursorForward.position, //move to the front of the cursor
            MovementSpeed * Time.deltaTime);
        transform.position = position;
    }

    public void BoostMovementSpeed(float boostAmount)
    {
        _movementSpeed += boostAmount;
    }
}
