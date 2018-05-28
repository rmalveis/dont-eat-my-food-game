using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    public Transform Player;
    private float _timer = 10f;
    private float _height;


    // Use this for initialization
    void Start()
    {
        _height = transform.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.position.y < transform.position.x + 0.25 * _height)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            transform.position = new Vector3(transform.position.x, Player.position.y, 0);
            _timer = 1;
        }
    }
}