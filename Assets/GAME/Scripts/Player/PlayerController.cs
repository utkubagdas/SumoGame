using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Local
    private float _power = 1;
    public float Power => _power;
    public float ScaleBoost { get; private set; }
    public bool Pushed { get; set; }
    public bool Eliminated { get; set; }
    #endregion
    
    #region Public
    public PlayerMovementController PlayerMovementController;
    #endregion
    
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallCollider"))
        {
            EventManager.OnLevelFail.Invoke();
            SetDelay(3f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        OpponentController opponentController = other.collider.GetComponent<OpponentController>();

        if (opponentController != null && !Pushed)
        {
            if (Power < opponentController.Power) return;
            
            opponentController.PushedFromPlayer = true;
            opponentController.Pushed = true;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = other.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the opponent
            dir.y = 0;
            Rigidbody attachedRigidbody = other.collider.attachedRigidbody;
            opponentController.OpponentMovementController.SetControlable(false);
            attachedRigidbody.AddForce(dir * 20, ForceMode.Impulse);
            StartCoroutine(ResetVelocityAfterDelay(1.5f, attachedRigidbody, opponentController));
        }
        else if (other.collider.CompareTag("WeakPoint") && !Pushed)
        {
            OpponentController opponentController2 = other.collider.GetComponentInParent<OpponentController>();
            opponentController2.PushedFromPlayer = true;
            opponentController2.Pushed = true;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = other.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the opponent
            dir.y = 0;
            Rigidbody attachedRigidbody = opponentController2.GetComponent<Rigidbody>();
            opponentController2.OpponentMovementController.SetControlable(false);
            attachedRigidbody.AddForce(dir * 25, ForceMode.Impulse);
            StartCoroutine(ResetVelocityAfterDelay(1.5f, attachedRigidbody, opponentController2));
        }
    }

    private IEnumerator ResetVelocityAfterDelay(float delayTime, Rigidbody rb, OpponentController opponentController)
    {
        yield return new WaitForSeconds(delayTime);
        //rb.velocity = Vector3.zero;
        if (Power >= opponentController.Power && opponentController.IsFall)
        {
            _power += opponentController.Power;
            BoostScale(opponentController.ScaleBoost / 5f);
            PlayerMovementController.BoostMovementSpeed(opponentController.OpponentMovementController.MovementSpeed / 10f);
        }

        if (!opponentController.IsFall)
        {
            opponentController.OpponentMovementController.SetControlable(true);
        }
        opponentController.Pushed = false;
    }
    
    private void SetDelay(float time)
    {
        StartCoroutine(SetDelayCo(time));
    }

    private IEnumerator SetDelayCo(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("GameScene");
    }
    
}
