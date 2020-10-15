using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public bool left;
    public Sprite avatar1;
    public Sprite avatar2;
    [TextArea(3,10)]
    public string[] sentences;

}
