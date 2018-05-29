using Players;
using UnityEngine;

namespace EventManager
{
    public class EventManager : MonoBehaviour
    {
        public delegate void SaveNameAction(string name);

        public static event SaveNameAction OnSaveName;

        public delegate void OnNextMapExitHitAction(Collider2D hit);

        public static event OnNextMapExitHitAction OnNextMapExitHit;

        public delegate void OnDeathAction(PlayerType playerType);

        public static event OnDeathAction OnDeath;

        public delegate void OnCollectAction(PlayerType playerType);

        public static event OnCollectAction OnCollect;

        public delegate void OnEnablePowerUpAction(PlayerType playerType);

        public static event OnEnablePowerUpAction OnEnablePowerUp;

        public delegate void OnPowerUpAction(PlayerType playerType);

        public static event OnPowerUpAction OnPowerUp;

        public delegate void OnHideMapAction(Collider2D hit);

        public static event OnHideMapAction OnHideMap;

        public static void CallOnSave(string toSave)
        {
            if (OnSaveName != null) OnSaveName(toSave);
        }

        public static void CallNextMapExitHit(Collider2D hit)
        {
            if (OnNextMapExitHit != null) OnNextMapExitHit(hit);
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

        public static void CallOnHideMap(Collider2D hit)
        {
            if (OnHideMap != null) OnHideMap(hit);
        }
    }
}