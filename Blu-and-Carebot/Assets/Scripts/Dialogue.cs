// Author: Fatima Nadeem / Croft

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class Dialogue
/*  This class holds a character and its dialogue in the form of 1-10 sentences.
*/

{    
    public string characterName;
    public Sprite characterImage;
    
    [TextArea(1, 10)]
    public string[] sentences;
}
