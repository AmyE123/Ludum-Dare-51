using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField, TextArea(minLines:1, maxLines:10)]
    private string _description;

    [SerializeField]
    private LevelLayout _levelPrefab;

    public string LevelName => _name;
    public string Description => _description;
    public GameObject LevelPrefab => _levelPrefab == null ? null : _levelPrefab.gameObject;
}
