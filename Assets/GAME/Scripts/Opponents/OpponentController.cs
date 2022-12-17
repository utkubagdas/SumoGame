using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MonoBehaviour
{
    #region Property
    public bool Pushed { get; set; }
    public bool IsFall { get; private set; }
    private float _power = 1;
    public float Power => _power;
    public float ScaleBoost { get; private set; }
    public bool Eliminated { get; set; }
    public bool PushedFromPlayer { get; set; }
    
    public bool IsDead { get; set; }
    public List<OpponentController> OpponentControllers { get; } = new List<OpponentController>();
    #endregion
    
    #region Public
    public OpponentMovementController OpponentMovementController;
    #endregion


    private void Start()
    {
        ScaleBoost = transform.localScale.x;
    }

    public void SetAllOpponentList(List<OpponentController> opponentControllers)
    {
        OpponentControllers.AddRange(opponentControllers);
    }

    public void BoostScale(float boostAmount)
    {
        var transform1 = transform;
        var localScale = transform1.localScale;
        localScale += Vector3.one * boostAmount;
        transform1.localScale = localScale;
        ScaleBoost = localScale.x;
        _power += 0.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OutCollider") && Pushed && !Eliminated)
        {
            Eliminated = true;
            OpponentMovementController.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition;
            EventManager.OnFallOpponent.Invoke();
            IsFall = true;
            if (PushedFromPlayer)
            {
                EventManager.OnFallOpponentFromPlayer.Invoke();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        OpponentController opponentController = other.collider.GetComponent<OpponentController>();
        PlayerController playerController = other.collider.GetComponent<PlayerController>();
        
        
        if (opponentController != null && !Pushed)
        {
            if (Power < opponentController.Power) return;

            PushedFromPlayer = false;
            opponentController.Pushed = true;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = other.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the opponent
            dir.y = 0;
            opponentController.OpponentMovementController.SetControlable(false);
            Rigidbody attachedRigidbody;
            (attachedRigidbody = other.collider.attachedRigidbody).AddForce(dir * 10, ForceMode.Impulse);

           
            StartCoroutine(ResetVelocityAfterDelay(1.5f, attachedRigidbody, opponentController));
            
        }
        
        if (playerController != null && !Pushed)
        {
            if (Power <= playerController.Power) return;
            
            playerController.Pushed = true;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = other.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the opponent
            dir.y = 0;
            Rigidbody attachedRigidbody;
            playerController.PlayerMovementController.SetControlable(false);
            (attachedRigidbody = other.collider.attachedRigidbody).AddForce(dir * 10, ForceMode.Impulse);
        
           
            StartCoroutine(ResetVelocityAfterDelayPlayer(1.5f, attachedRigidbody, playerController));
            
        }
    }

    private IEnumerator ResetVelocityAfterDelay(float delayTime, Rigidbody rb, OpponentController opponentController)
    {
        yield return new WaitForSeconds(delayTime);
        rb.velocity = Vector3.zero;
        if (Power > opponentController.Power && opponentController.IsFall && !opponentController.Eliminated)
        {
            opponentController.Eliminated = true;
            _power += opponentController.Power;
            BoostScale(opponentController.ScaleBoost / 2f);
            OpponentMovementController.BoostMovementSpeed(opponentController.OpponentMovementController.MovementSpeed / 2f);
        }

        if (!opponentController.IsFall)
        {
            opponentController.OpponentMovementController.SetControlable(true);
        }
        opponentController.Pushed = false;
    }
    
    private IEnumerator ResetVelocityAfterDelayPlayer(float delayTime, Rigidbody rb, PlayerController playerController)
    {
        yield return new WaitForSeconds(delayTime);
        rb.velocity = Vector3.zero;
        if (Power > playerController.Power && playerController.PlayerMovementController.IsFall && !playerController.Eliminated)
        {
            playerController.Eliminated = true;
            _power += playerController.Power;
            BoostScale(playerController.ScaleBoost / 2f);
            OpponentMovementController.BoostMovementSpeed(playerController.PlayerMovementController.MovementSpeed / 2f);
        }

        if (!playerController.PlayerMovementController.IsFall)
        {
            playerController.PlayerMovementController.SetControlable(true);
        }
        playerController.Pushed = false;
    }
}
