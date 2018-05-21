using Players;
using UnityEditor;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void SaveNameAction(string name);

    public static event SaveNameAction OnSaveName;

    public delegate void OnHitAction(Collider2D hit);

    public static event OnHitAction OnHit;

    public delegate void OnExitHitAction(Collider2D hit);

    public static event OnExitHitAction OnExitHit;

    public delegate void OnDeathAction(PlayerType playerType);

    public static event OnDeathAction OnDeath;

    public delegate void OnCollectAction(PlayerType playerType);

    public static event OnCollectAction OnCollect;

    public delegate void OnEnablePowerUpAction(PlayerType playerType);

    public static event OnEnablePowerUpAction OnEnablePowerUp;

    public delegate void OnPowerUpAction(PlayerType playerType);

    public static event OnPowerUpAction OnPowerUp;

    public static void CallOnSave(string toSave)
    {
        if (OnSaveName != null) OnSaveName(toSave);
    }

    public static void CallOnHit(Collider2D hit)
    {
        if (OnHit != null) OnHit(hit);
    }

    public static void CallExitHit(Collider2D hit)
    {
        if (OnExitHit != null) OnExitHit(hit);
    }

    public static void CallOnDeath(PlayerType playerType)
    {
        if (OnDeath != null) OnDeath(playerType);
    }

    public static void CallOnCollect(PlayerType playerType)
    {
        if (OnCollect != null) OnCollect(playerType);
    }

    public static void CallOnPowerUp(PlayerType playerType)
    {
        if (OnPowerUp != null) OnPowerUp(playerType);
    }

    public static void CallEnablePowerUp(PlayerType playerWhoFiredPowerUp)
    {
        if (OnEnablePowerUp != null) OnEnablePowerUp(playerWhoFiredPowerUp);
    }
}