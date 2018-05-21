using UnityEngine;

public class LoadNextMap : MonoBehaviour
{
    // Use this for initialization
    private void OnTriggerExit2D(Collider2D hit)
    {
        EventManager.CallExitHit(hit);
    }
}