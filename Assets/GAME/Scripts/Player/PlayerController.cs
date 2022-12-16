using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovementController PlayerMovementController;

    public void BoostScale(float boostAmount)
    {
        transform.localScale += Vector3.one * boostAmount;
    }

    private void OnCollisionEnter(Collision other)
    {
        OpponentController opponentController = other.collider.GetComponent<OpponentController>();

        if (opponentController != null)
        {
            opponentController.Pushed = true;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = other.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the opponent
            dir.y = 0;
            Rigidbody attachedRigidbody;
            (attachedRigidbody = other.collider.attachedRigidbody).AddForce(dir * 1000f);
            StartCoroutine(ResetVelocityAfterDelay(1.5f, attachedRigidbody, opponentController));
        }
    }

    private IEnumerator ResetVelocityAfterDelay(float delayTime, Rigidbody rb, OpponentController opponentController)
    {
        yield return new WaitForSeconds(delayTime);
        rb.velocity = Vector3.zero;
        opponentController.Pushed = false;
    }
}
