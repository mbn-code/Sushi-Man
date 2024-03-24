using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    internal int Score = 0;
    internal void AddPoint()
    {
        Score += 1;
    }
}
