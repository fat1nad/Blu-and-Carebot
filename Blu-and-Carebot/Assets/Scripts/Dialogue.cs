// Author: Fatima Nadeem / Croft

using UnityEngine;

[System.Serializable]

public class Dialogue
/*  This class holds a dialogue in the form of 2-10 sentences.
*/

{    
    [TextArea(2, 10)]
    public string[] sentences;
}
