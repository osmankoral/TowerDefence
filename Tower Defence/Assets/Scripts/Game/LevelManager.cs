using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] level;

    public void LevelStart(int _level)
    {
        Instantiate(level[_level], transform);
    }

}