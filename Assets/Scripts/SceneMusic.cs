using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(999)]
public class SceneMusic : MonoBehaviour
{
    [Header("This music will play when the scene is loaded")]
    [SerializeField] private AudioClip _music;

    void Start()
    {
        if (_music != null)
            MusicManager.PlayMusic(_music);
    }
}
