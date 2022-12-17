using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovementController : MonoBehaviour
{
    #region Local
    private float _startYPos;
    private float _distToGround;
    private bool _groundChecked;
    private RaycastHit _hit;
    private float _raycastDistance = 100f;
    #endregion
    
    #region Serialized
    [SerializeField] private CursorController CursorController;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private CapsuleCollider Collider;
    #endregion
    
    #region Property
    public bool IsFall { get; set; }
    public bool IsControlable { get; private set; }
    #endregion
    
    #region Public
    public float MovementSpeed = 8;
    #endregion
    
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
        //get the point which is located at 'capCol.radius' higher than transform's bottom
        var position1 = transform.position;
        Vector3 centerOfSphere1 = position1 + Vector3.up * Collider.radius;
        //and get th point which is located at
        // ('capCol.height'-'capCol.radius') higher than transform's bottom
        Vector3 centerOfSphere2 = position1 + Vector3.up * (Collider.height - Collider.radius);

        if (Physics.CapsuleCast(centerOfSphere1, centerOfSphere2, Collider.radius, Vector3.down, _raycastDistance, GroundLayer))
        {
            
        }
        else if(!_groundChecked)
        {
            _groundChecked = true;
            IsFall = true;
            SetControlable(false);
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
        MovementSpeed += boostAmount;
    }

    private void LevelStart()
    {
        SetControlable(true);
        IsFall = false;
    }
    
    public void SetControlable(bool active)
    {
        IsControlable = active;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 dir = Vector3.down;
        Gizmos.DrawRay(transform.position, dir);
    }
}
