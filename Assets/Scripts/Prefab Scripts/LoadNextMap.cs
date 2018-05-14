using UnityEngine;

public class LoadNextMap : MonoBehaviour
{
    // Use this for initialization
    private void OnTriggerExit2D(Collider2D hit)
    {
        Debug.Log("TriggerEXIT 2D");
        EventManager.CallExitHit(hit);
    }
}