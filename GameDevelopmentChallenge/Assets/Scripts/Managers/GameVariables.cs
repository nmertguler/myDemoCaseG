using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariables : Singleton<GameVariables>
{
    [Space(5)]
    public Transform soldierHolder;
    public Camera mainCamera;
    public GiveXp giveXp;
    public CardSelectController cardSelectController;
    public AttackPattern attackPattern;
}
