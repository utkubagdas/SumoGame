using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectableBoost : MonoBehaviour, ICollectable
{
    [SerializeField] private float MovementSpeedBoostAmount;
    [SerializeField] private float ScaleBoostAmount;
    
    public void CollectForAI(OpponentController opponentController)
    {
        opponentController.BoostScale(ScaleBoostAmount);
        opponentController.OpponentMovementController.BoostMovementSpeed(MovementSpeedBoostAmount);
    }

    public void CollectForPlayer(PlayerController playerController)
    {
        playerController.BoostScale(ScaleBoostAmount);
        playerController.PlayerMovementController.BoostMovementSpeed(MovementSpeedBoostAmount / 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        OpponentController opponentController = other.GetComponent<OpponentController>();
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null)
        {
            CollectForPlayer(playerController);
            EventManager.OnCollectBoost.Invoke(this);
            Destroy(gameObject);
        }

        if (opponentController != null)
        {
            CollectForAI(opponentController);
            EventManager.OnCollectBoost.Invoke(this);
            Destroy(gameObject);
        }
    }
}
