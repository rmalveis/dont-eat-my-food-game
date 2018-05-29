using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    public Transform Player;
    private const float Amount = 5f;
    private float _timer = Amount;

    private float _bottomLimit;

    private void Start()
    {
        _bottomLimit = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;

        if (!(_timer <= 0)) return;

        if (Player.position.y >= _bottomLimit)
        {
            transform.position = new Vector3(transform.position.x, Player.position.y, 0);
        }

        _timer = Amount;
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        if (hit.gameObject.tag.Equals("collectible") || hit.gameObject.tag.Equals("platform"))
        {
            return;
        }

        EventManager.EventManager.CallOnHideMap(hit);
    }
}