using System.Collections.Generic;
using UnityEngine;

public class LoadNextMap : MonoBehaviour
{
    private List<Collider2D> control = new List<Collider2D>();

    private void OnTriggerExit2D(Collider2D hit)
    {
        if (hit.gameObject.tag.Equals("collectible") || hit.gameObject.tag.Equals("platform"))
        {
            return;
        }

        if (!control.Contains(hit))
        {
            EventManager.EventManager.CallNextMapExitHit(hit);
            control.Add(hit);
        }

        if (control.Count >= 6)
        {
            control.RemoveRange(0, 2);
        }
    }
}