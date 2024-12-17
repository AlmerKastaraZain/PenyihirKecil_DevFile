using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] List<string> lines;
    [SerializeField] List<int> turns;
    [SerializeField] List<AudioClip> clips;

    public List<string> Lines
    {
        get { return lines; }
    }

    public List<int> Turns
    {
        get { return turns; }
    }

    public List<AudioClip> Clips
    {
        get { return clips; }
    }
}
