using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    [Range(0f,2f)]
        public float level;
    public AudioClip clip;
}
