﻿// Author: Fatima Nadeem / Croft

using UnityEngine;

[System.Serializable]
public class Dialogue
/*  This class holds a character and its dialogue in the form of 1-10 sentences.
*/

{    
    public string characterName;
    public Sprite characterImage; // 340x340
    
    [TextArea(1, 10)]
    public string[] sentences;
}
