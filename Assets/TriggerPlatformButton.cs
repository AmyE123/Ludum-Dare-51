using DG.Tweening;
using UnityEngine;

public class TriggerPlatformButton : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private bool _isPressed;
    private bool _hasMoved;

    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private Material _pressedMat;

    [SerializeField]
    private Transform _platform;

    [SerializeField]
    [Range(0, 10)]
    private int _platformLevel;

    [SerializeField]
    private int _platformRiseDelay;

    void Start()
    {
        _isPressed = false;
    }

    private void Update()
    {
        if(_isPressed)
        {
            RisePlatform();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PLAYER_TAG)
        {
            _isPressed = true;
            _renderer.material = _pressedMat;           
        }
    }

    private void RisePlatform()
    {
        if(!_hasMoved)
        {
            _platform.DOLocalMoveY(_platformLevel, _platformRiseDelay);
            _hasMoved = true;
        }       
    }
}
