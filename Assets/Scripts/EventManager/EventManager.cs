using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void SaveNameAction(string name);

    public static event SaveNameAction OnSaveName;

    public delegate void OnHitAction(Collider2D hit);

    public static event OnHitAction OnHit;
    
    public delegate void OnExitHitAction(Collider2D hit);

    public static event OnExitHitAction OnExitHit;

    public static void CallOnSave(string toSave)
    {
        if (OnSaveName != null)
        {
            OnSaveName(toSave);
        }
    }

    public static void CallOnHit(Collider2D hit)
    {
        if (OnHit != null)
        {
            OnHit(hit);
        }
    }

    public static void CallExitHit(Collider2D hit)
    {
        if (OnExitHit != null)
        {
            OnExitHit(hit);
        }
    }
}