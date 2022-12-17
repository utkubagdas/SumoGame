using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class OpponentMovementController : MonoBehaviour
{
    [SerializeField] private OpponentController OpponentController;
    [SerializeField] private Rigidbody Rigidbody;
    private Transform _targetTransform;
    public NavMeshAgent NavMeshAgent;
    private float _movementSpeed = 5f;
    public float MovementSpeed => _movementSpeed;
    
    public bool _hasTarget;
    [SerializeField] private LayerMask BoostLayer;
    private Collider[] hitColliders;

    private float _distance = Mathf.Infinity;

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
        NavMeshAgent.speed = MovementSpeed;
    }

    private void Update()
    {
        if (!NavMeshAgent.enabled || !GameManager.Instance.LevelStarted)
            return;
        
        FindNearestBoost();

        if (_targetTransform == null)
        {
            _distance = Mathf.Infinity;
        }
        
        if (_hasTarget)
        {
            if (_targetTransform != null)
            {
                if(NavMeshAgent.enabled)
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
    
    private Transform FindNearestBoost()
    {
        hitColliders = Physics.OverlapSphere(transform.position + GetComponent<CapsuleCollider>().center, 50f, BoostLayer);
        if (hitColliders.Length <= 0)
            return null;

        foreach (var hit in hitColliders)
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
        _movementSpeed += boostAmount;
        NavMeshAgent.speed = _movementSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + GetComponent<CapsuleCollider>().center + Vector3.zero * 1.0f, 20f);
    }

    private void LevelStart()
    {
        NavMeshAgent.enabled = true;
        //Rigidbody.isKinematic = false;
    }
}
