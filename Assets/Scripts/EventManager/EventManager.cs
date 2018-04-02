using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void SaveNameAction(string name);

    public static event SaveNameAction OnSaveName;

    public static void CallOnSave(string toSave)
    {
        if (OnSaveName != null)
        {
            OnSaveName(toSave);
        }
    }
}