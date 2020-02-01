using System;
using UnityEngine;

[Serializable]
public struct Sentences
{
    public string name;
    [TextArea(3, 10)]
    public string sentence;
    public AudioClip clip;
}

[System.Serializable]
public class Dialogue
{
    public string title;    
    public Sentences[] sentences;
}
