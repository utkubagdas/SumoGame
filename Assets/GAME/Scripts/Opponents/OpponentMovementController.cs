using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentMovementController : MonoBehaviour
{
    [SerializeField] private OpponentController OpponentController;
    public Transform _targetTransform;
    public NavMeshAgent NavMeshAgent;
    private float _movementSpeed = 3.5f;
    public float MovementSpeed => _movementSpeed;
    
    public bool _hasTarget;
    [SerializeField] private LayerMask BoostLayer;
    private Collider[] hitColliders;
    
    
    private void Update()
    {
        if (!NavMeshAgent.enabled)
            return;
        
        
        if (!_hasTarget)
        {
            FindNearestBoost();
        }
        else
        {
            if (_targetTransform != null)
            {
                NavMeshAgent.destination = _targetTransform.position;
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

    // private Transform FindNearestOpponent()
    // {
    //     foreach (var opponent in OpponentController.OpponentControllers)
    //     {
    //         var newDistance = Vector3.Distance(transform.position, opponent.transform.position);
    //         if (newDistance < _opponentDistance && opponent != OpponentController)
    //         {
    //             if (OpponentController.OpponentControllers.IndexOf(OpponentController) == 0)
    //             {
    //                 _targetTransform = FindObjectOfType<PlayerMovementController>().transform;
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
    
    private Transform FindNearestBoost()
    {
        hitColliders = Physics.OverlapSphere(transform.position + GetComponent<CapsuleCollider>().center, 50f, BoostLayer);
        if (hitColliders.Length <= 0)
            return null;
        _targetTransform = hitColliders[0].transform;
        _hasTarget = true;
        return _targetTransform;
    }

    public void BoostMovementSpeed(float boostAmount)
    {
        _movementSpeed += boostAmount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + GetComponent<CapsuleCollider>().center + Vector3.zero * 1.0f, 20f);
    }
}
