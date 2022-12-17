using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerMovementController PlayerMovementController;
    private float _power = 1;
    public float Power => _power;
    public float ScaleBoost { get; private set; }
    [HideInInspector] public bool Pushed;
    //public bool IsFall { get; private set; }
    public bool Eliminated { get; set; }
    
    
    private void Start()
    {
        ScaleBoost = transform.localScale.x;
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

    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 dir = Vector3.down;
        Gizmos.DrawRay(transform.position, dir);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OutColliderForPlayer"))
        {
            PlayerMovementController.IsFall = true;
        }
        
        if (other.CompareTag("FallCollider"))
        {
            PlayerMovementController.IsFall = true;
            //SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            EventManager.OnLevelFail.Invoke();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        OpponentController opponentController = other.collider.GetComponent<OpponentController>();

        if (opponentController != null && !Pushed)
        {
            Debug.Log("push");
            opponentController.Pushed = true;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = other.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the opponent
            dir.y = 0;
            Rigidbody attachedRigidbody = other.collider.attachedRigidbody;
            opponentController.OpponentMovementController.NavMeshAgent.enabled = false;
            attachedRigidbody.isKinematic = false;
            attachedRigidbody.AddForce(dir * 20, ForceMode.Impulse);
            StartCoroutine(ResetVelocityAfterDelay(1.5f, attachedRigidbody, opponentController));
        }
        else if (other.collider.CompareTag("WeakPoint") && !Pushed)
        {
            Debug.Log("weakpoint");
            OpponentController opponentController2 = other.collider.GetComponentInParent<OpponentController>();
            opponentController2.Pushed = true;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = other.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the opponent
            dir.y = 0;
            Rigidbody attachedRigidbody = opponentController2.GetComponent<Rigidbody>();
            opponentController2.OpponentMovementController.NavMeshAgent.enabled = false;
            attachedRigidbody.isKinematic = false;
            attachedRigidbody.AddForce(dir * 40, ForceMode.Impulse);
            StartCoroutine(ResetVelocityAfterDelay(1.5f, attachedRigidbody, opponentController2));
        }
    }

    private IEnumerator ResetVelocityAfterDelay(float delayTime, Rigidbody rb, OpponentController opponentController)
    {
        yield return new WaitForSeconds(delayTime);
        rb.velocity = Vector3.zero;
        if (Power >= opponentController.Power && opponentController.IsFall && !opponentController.Eliminated)
        {
            opponentController.Eliminated = true;
            _power += opponentController.Power;
            BoostScale(opponentController.ScaleBoost / 2f);
            PlayerMovementController.BoostMovementSpeed(opponentController.OpponentMovementController.MovementSpeed / 2f);
            EventManager.OnFallOpponent.Invoke();
            EventManager.OnFallOpponentFromPlayer.Invoke();
        }

        if (!opponentController.IsFall)
        {
            rb.isKinematic = true;
            opponentController.OpponentMovementController.NavMeshAgent.enabled = true;
        }
        opponentController.Pushed = false;
    }

    
}
