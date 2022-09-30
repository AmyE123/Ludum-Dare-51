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

    public string LevelName => _name;
    public string Description => _description;
}
