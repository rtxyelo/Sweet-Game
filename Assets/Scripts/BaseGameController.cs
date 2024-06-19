using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameController : MonoBehaviour
{
    public virtual int CurrentLevelScore { get; }
    public virtual int GameTime { get; }
}
