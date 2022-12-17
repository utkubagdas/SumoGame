using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private CursorController CursorController;
    private float _startYPos;
    
    [SerializeField] private float _movementSpeed = 0.1f;

    [HideInInspector] public bool IsFall;
    
    public bool IsControlable { get; private set; }

    public float MovementSpeed => _movementSpeed;
    
    private float _distToGround;
    private RaycastHit _hit;
    private float _raycastDistance = 100f;
    [SerializeField] private LayerMask GroundLayer;
    private bool _groundChecked;

    private void OnEnable()
    {
        EventManager.OnLevelStart.AddListener(LevelStart);
    }

    private void OnDisable()
    {
        EventManager.OnLevelStart.RemoveListener(LevelStart);
    }

    private void Start()
    {
        _startYPos = transform.position.y; // get the initial y value to clamp it
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, _raycastDistance, GroundLayer)) 
        {
            
        }
        else if (!_groundChecked)
        {
            _groundChecked = true;
            IsFall = true;
            IsControlable = false;
            Debug.Log("isfall true");
        }
        
        if (!IsControlable)
            return;
        
        if (IsFall)
            return;
        
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

    private void LevelStart()
    {
        IsControlable = true;
        IsFall = false;
    }
    
    public void SetControlable(bool active)
    {
        IsControlable = active;
    }
}
