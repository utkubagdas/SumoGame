using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MonoBehaviour
{
    public Transform WeakPoint;
    public OpponentMovementController OpponentMovementController;
    public List<OpponentController> OpponentControllers { get; } = new List<OpponentController>();

    [HideInInspector] public bool Pushed;
    public void SetAllOpponentList(List<OpponentController> opponentControllers)
    {
        OpponentControllers.AddRange(opponentControllers);
    }

    public void BoostScale(float boostAmount)
    {
        transform.localScale += Vector3.one * boostAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OutCollider") && Pushed)
        {
            OpponentMovementController.NavMeshAgent.enabled = false;
        }
    }
}
