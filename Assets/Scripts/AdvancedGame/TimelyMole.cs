using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassicGame;

public class TimelyMole : Mole
{
    public float TimeGain = 3.0f;

    public override void Die()
    {
        base.Die();
        GameManager.TimeRemaining += TimeGain;
    }
}
