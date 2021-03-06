﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralController : MonoBehaviour
{
    private class PlatformObject
    {
        public int Id { get; set; }
        public bool Visible;

        public GameObject Instance { get; private set; }
        public List<PlatformObject> Combinations { get; private set; }

        public PlatformObject(GameObject platform)
        {
            Id = int.Parse(platform.name);
            Instance = platform;
        }

        public void setCombinations(List<PlatformObject> platformCombinations)
        {
            Combinations = platformCombinations;
        }
    }

    public GameObject[] RightPlatforms;
    public GameObject[] LeftPlatforms;

    private readonly List<PlatformObject> _rightPlatforms = new List<PlatformObject>();
    private readonly List<PlatformObject> _leftPlatforms = new List<PlatformObject>();
    private System.Random rnd = new System.Random();

    private void HandleHideMap(Collider2D hit)
    {
        var obj = hit.gameObject.transform.parent.gameObject;
        var isLeft = obj.layer == LayerMask.NameToLayer("LeftPlayer");
        var platforms = isLeft ? _leftPlatforms : _rightPlatforms;
        var p = platforms.Find(o => o.Instance == obj);
        p.Instance.SetActive(false);
        p.Visible = false;
        var children = p.Instance.GetComponentsInChildren<Transform>(true);
        
        foreach (var child in children)
        {
            if (child.tag.Equals("collectible"))
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void HandleNextMapExitHitNextMap(Collider2D hit)
    {
        var obj = hit.gameObject.transform.parent.gameObject;
        var isLeft = obj.layer == LayerMask.NameToLayer("LeftPlayer");
        var platforms = isLeft ? _leftPlatforms : _rightPlatforms;
        var p = platforms.Find(o => o.Instance == obj);

        ShowNextMap(p);
    }

    private List<PlatformObject> GetPlatformCombinations(int id, List<PlatformObject> platforms)
    {
        var aux = "";
        switch (id)
        {
            case 1:
                aux += "3,4,5,6,7,8,9,11,12,14,15,16,17,18,19";
                break;
            case 2:
                aux += "3,4,5,8,9,12,14,15,16,17,18,19,20";
                break;
            case 3:
                aux += "6,7,11,12,13,14,15,17,20";
                break;
            case 4:
                aux += "3,2,1,6,7,8,9,11,14,16,17,18,19";
                break;
            case 5:
                aux += "1,2,3,4,6,7,8,9,10,11,12,14,15,16,17,18,19,20";
                break;
            case 6:
                aux += "1,2,3,4,5,8,9,10,12,13,14,15,16,18,20";
                break;
            case 7:
                aux += "1,2,3,4,5,8,9,10,12,15,16,17,18,19";
                break;
            case 8:
                aux += "1,2,3,5,9,12,14,15,16,18,20";
                break;
            case 9:
                aux += "1,2,3,4,5,8,10,12,13,14,15,16,17,18,20";
                break;
            case 10:
                aux += "1,2,3,4,5,6,7,8,9,11,12,13,14,15,16,17,18,19,20";
                break;
            case 11:
                aux += "1,2,3,5,8,9,12,14,15,16,18,20";
                break;
            case 12:
                aux += "1,3,4,5,10,12,13,14,15,16,17,18,20";
                break;
            case 13:
                aux += "1,2,3,4,5,6,7,8,9,11,12,14,15,16,17,18,19";
                break;
            case 14:
                aux += "1,2,3,4,5,6,7,8,9,10,11,12,13,15,16,17,18,19,20";
                break;
            case 15:
                aux += "1,2,3,4,5,8,9,10,11,12,14,16,17,18,19,20";
                break;
            case 16:
                aux += "1,2,3,4,5,6,7,8,9,11,12,14,17,18,19";
                break;
            case 17:
                aux += "1,2,3,4,5,6,7,8,9,10,11,12,14,17,18,19";
                break;
            case 18:
                aux += "1,2,3,4,5,6,7,8,9,10,11,12,14,16,17,19";
                break;
            case 19:
                aux += "1,2,3,4,5,6,7,8,9,10,11,12,14,16,17,18";
                break;
            default:
                aux += "1,2,3,4,5,8,9,10,15,16,17,18,19";
                break;
        }

        return platforms.Where(platform => Array.Exists(aux.Split(','), element => int.Parse(element) == platform.Id))
            .ToList();
    }

    private static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (var trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    // Use this for initialization
    void Start()
    {
        // Converting Arrays of GameObjects
        // To Lists of PlatformObjects
        for (var i = 0; i < RightPlatforms.Length; i++)
        {
            _leftPlatforms.Add(new PlatformObject(LeftPlatforms[i]));
            _rightPlatforms.Add(new PlatformObject(RightPlatforms[i]));
        }

        // Creating the relationships between the PlatformObjects.
        // Setting correct Labels to instances.
        for (var i = 0; i < _rightPlatforms.Count; i++)
        {
            _rightPlatforms[i].setCombinations(GetPlatformCombinations(_rightPlatforms[i].Id, _rightPlatforms));
            _leftPlatforms[i].setCombinations(GetPlatformCombinations(_leftPlatforms[i].Id, _leftPlatforms));
            SetLayerRecursively(_leftPlatforms[i].Instance, LayerMask.NameToLayer("LeftPlayer"));
            SetLayerRecursively(_rightPlatforms[i].Instance, LayerMask.NameToLayer("RightPlayer"));
        }


        EventManager.EventManager.OnNextMapExitHit += HandleNextMapExitHitNextMap;
        EventManager.EventManager.OnHideMap += HandleHideMap;

        // Crating the first Map:
        ShowFirstMaps();
    }

    private void OnDestroy()
    {
        EventManager.EventManager.OnNextMapExitHit -= HandleNextMapExitHitNextMap;
    }

    private void ShowFirstMaps()
    {
        var rightMap = _rightPlatforms.Find(p => p.Id == 13);
        var leftMap = _leftPlatforms.Find(p => p.Id == 13);

        var leftFirstMapPosition = new Vector3(-30.2f, -3.3f, 0);
        var rightFirstMapPosition = new Vector3(2.93f, -3.3f, 0);

        ActivateMap(rightMap, rightFirstMapPosition);
        ActivateMap(leftMap, leftFirstMapPosition);

        ShowNextMap(rightMap);
        ShowNextMap(leftMap);
    }

    private void ActivateMap(PlatformObject map, Vector3 position)
    {
        map.Instance.transform.position = position;
        map.Instance.SetActive(true);
        map.Visible = true;
    }

    void ShowNextMap(PlatformObject currentMap)
    {
        var aux = currentMap.Combinations.Where(c => !c.Visible).ToArray();

        var nextMap = aux[rnd.Next(0, aux.Length)];

        var v = currentMap.Instance.transform.position +
                new Vector3(0, currentMap.Instance.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y,
                    0);

        ActivateMap(nextMap, v);
    }
}