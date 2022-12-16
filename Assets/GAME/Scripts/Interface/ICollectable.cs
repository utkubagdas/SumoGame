using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    void CollectForAI(OpponentController opponentController);
    void CollectForPlayer(PlayerController playerController);
}
