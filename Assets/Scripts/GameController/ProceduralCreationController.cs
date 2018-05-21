using UnityEngine;

public class ProceduralCreationController : MonoBehaviour
{
    public Transform Player1;
    public Transform Player2;
    public GameObject[] Platforms;

    private GameObject _currentMapLeft;
    private GameObject _nextMapLeft;
    private GameObject _oldMapLeft;
    private GameObject _currentMapRight;
    private GameObject _nextMapRight;
    private GameObject _oldMapRight;

    private readonly Vector3 _leftMapInitialPosition = new Vector3(-14.62f, 14.11f, 0);
    private readonly Vector3 _rightMapInitialPosition = new Vector3(2.93f, 14.11f, 0);

    private static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (var trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    void HandleHitOnMapScroll(Collider2D hit)
    {
        if (hit.gameObject.transform != Player1 && hit.gameObject.transform != Player2)
        {
            return;
        }

        var old = _oldMapLeft;
        var next = _nextMapLeft;
        var curr = _currentMapLeft;
        var player = Player1;

        if (hit.gameObject.transform == Player2)
        {
            old = _oldMapRight;
            next = _nextMapRight;
            curr = _currentMapRight;
            player = Player2;
        }

        curr.transform.Translate(new Vector3(0, player.position.y * 0.01f, 0));
        next.transform.Translate(new Vector3(0, player.position.y * 0.01f, 0));
        if (old != null)
        {
            old.transform.Translate(new Vector3(0, player.position.y * 0.01f, 0));
        }
    }

    private void HandleExitHitNextMap(Collider2D hit)
    {
        if (hit.gameObject != _nextMapLeft && hit.gameObject != _nextMapRight)
        {
            return;
        }

        var nextMap = GetNextMap("ProceduralObjects/PO-3");

        if (hit.gameObject == _nextMapRight)
        {
            if (_oldMapRight != null)
            {
                Destroy(_oldMapRight);
                _oldMapRight = null;
            }

            _oldMapRight = _currentMapRight;
            _currentMapRight = _nextMapRight;
            var rightMapInitialPosition = new Vector3(2.93f,
                _currentMapRight.GetComponent<SpriteRenderer>().bounds.size.y +
                _currentMapRight.gameObject.transform.position.y, 0);

            _nextMapRight = CreateNextMap(nextMap, rightMapInitialPosition, "Right");
        }
        else
        {
            if (_oldMapLeft != null)
            {
                Destroy(_oldMapLeft);
                _oldMapLeft = null;
            }

            _oldMapLeft = _currentMapLeft;
            _currentMapLeft = _nextMapLeft;
            Vector3 leftMapInitialPosition = new Vector3(2.93f,
                _currentMapLeft.GetComponent<SpriteRenderer>().size.y + _currentMapLeft.gameObject.transform.position.y,
                0);
            _nextMapLeft = CreateNextMap(nextMap, _leftMapInitialPosition, "Left");
        }
    }

    // Use this for initialization
    void Start()
    {
        var leftFirstMapPosition = new Vector3(-14.62f, -2.93f, 0);
        var leftSecondMapPosition = new Vector3(-14.62f, 12.95f, 0);
        var rightFirstMapPosition = new Vector3(2.93f, -2.93f, 0);
        var rightSecondMapPosition = new Vector3(2.93f, 12.95f, 0);

        var nextMap = GetNextMap();
        var nextMap2 = GetNextMap("ProceduralObjects/PO-2");

        _currentMapLeft = CreateNextMap(nextMap, leftFirstMapPosition, "Left");
        _currentMapRight = CreateNextMap(nextMap, rightFirstMapPosition, "Right");
        _nextMapLeft = CreateNextMap(nextMap2, leftSecondMapPosition, "Left");
        _nextMapRight = CreateNextMap(nextMap2, rightSecondMapPosition, "Right");

        EventManager.OnHit += HandleHitOnMapScroll;
        EventManager.OnExitHit += HandleExitHitNextMap;
    }

    private void OnDestroy()
    {
        EventManager.OnHit -= HandleHitOnMapScroll;
    }

    private GameObject CreateNextMap(GameObject nextMap, Vector3 mapPosition, string sideName)
    {
        var obj = Instantiate(nextMap, mapPosition, Quaternion.identity);
        obj.name += "-" + sideName;

        SetLayerRecursively(obj, LayerMask.NameToLayer(sideName + "Player"));
        return obj;
    }

    private GameObject GetNextMap(string prefabName = "ProceduralObjects/PO-1")
    {
        return Resources.Load(prefabName) as GameObject;
    }
}