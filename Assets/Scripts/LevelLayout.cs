using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLayout : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnPos;

    [SerializeField]
    private int _maxWaterHeight;

    public int MaxWaterHeight => _maxWaterHeight;

    public Vector3 SpawnPosition => _spawnPos == null ? Vector3.up : _spawnPos.position;
}
