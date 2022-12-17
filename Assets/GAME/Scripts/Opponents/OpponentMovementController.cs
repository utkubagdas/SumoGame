using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class OpponentMovementController : MonoBehaviour
{
    #region Property
    public bool IsControlable { get; private set; }
    #endregion
    
    #region Serialized
    [SerializeField] private LayerMask BoostLayer;
    #endregion
    
    #region Local
    private Transform _targetTransform;
    private Collider[] _hitColliders;
    private bool _hasTarget;
    private float _distance = Mathf.Infinity;
    #endregion
    
    #region Public
    public Rigidbody Rigidbody;
    public float MovementSpeed = 5f;
    #endregion
    
    private void OnEnable()
    {
        EventManager.OnLevelStart.AddListener(LevelStart);
    }

    private void OnDisable()
    {
        EventManager.OnLevelStart.RemoveListener(LevelStart);
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.LevelStarted || !IsControlable)
            return;

        var tempPos = transform.position;
        tempPos.y = 1.25f;
        transform.position = tempPos;
        
        FindNearestBoost();

        if (_targetTransform == null)
        {
            _distance = Mathf.Infinity;
        }
        
        if (_hasTarget)
        {
            if (_targetTransform != null)
            {
                var position = _targetTransform.position;
                var position1 = transform.position;
                var dir  = (position - position1).normalized * MovementSpeed;
                Rigidbody.velocity = dir;
                var lookPos = position - position1;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
            }
            else
            {
                _hasTarget = false;
            }
        }

        if (_targetTransform == null)
        {
            _hasTarget = false;
        }
    }
    
    //to find other opponents and chase them

    // private Transform FindNearestOpponent()
    // {
    //     foreach (var opponent in OpponentController.OpponentControllers)
    //     {
    //         var newDistance = Vector3.Distance(transform.position, opponent.transform.position);
    //         if (newDistance < _opponentDistance && opponent != OpponentController)
    //         {
    //             if (OpponentController.OpponentControllers.IndexOf(OpponentController) == 0)
    //             {
    //                 _targetTransform = player.transform;
    //             }
    //             else
    //             {
    //                 _targetTransform = opponent.transform;
    //             }
    //             _opponentDistance = newDistance;
    //         }
    //     }
    //     return _targetTransform;
    // }
    
    //to collect collectible boosts
    //in the example game they were mostly chasing boosts, so it's like this right now.
    
    private Transform FindNearestBoost()
    {
        _hitColliders = Physics.OverlapSphere(transform.position + GetComponent<CapsuleCollider>().center, 100f, BoostLayer);
        if (_hitColliders.Length <= 0)
            return null;

        foreach (var hit in _hitColliders)
        {
            var tempDistance = Vector3.Distance(transform.position, hit.transform.position);
            if (tempDistance < _distance)
            {
                _distance = tempDistance;
                _targetTransform = hit.transform;
                _hasTarget = true;
            }
        }
        return _targetTransform;
    }

    public void BoostMovementSpeed(float boostAmount)
    {
        MovementSpeed += boostAmount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + GetComponent<CapsuleCollider>().center + Vector3.zero * 1.0f, 20f);
    }

    private void LevelStart()
    {
        SetControlable(true);
    }
    public void SetControlable(bool active)
    {
        IsControlable = active;
    }
    
}
