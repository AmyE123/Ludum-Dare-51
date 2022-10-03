using DG.Tweening;
using UnityEngine;

public class TriggerPlatformButton : MonoBehaviour
{
    public enum TriggerState { RisePlatform, LowerGates, ToggleObject }
    private const string PLAYER_TAG = "Player";

    private bool _isPressed;
    private bool _hasMoved;

    [SerializeField]
    private TriggerState _triggerState;

    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private Material _pressedMat;

    [SerializeField]
    private Transform _platform;

    [SerializeField]
    private int _platformLevel;

    [SerializeField]
    private int _platformMoveDelay;

    void Start()
    {
        _isPressed = false;
    }

    private void Update()
    {
        if(_isPressed)
        {
            if (_triggerState == TriggerState.RisePlatform) { RisePlatform(); }
            else if (_triggerState == TriggerState.LowerGates) { LowerGates(); }
            else { ToggleObject(); }
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
            _platform.DOLocalMoveY(_platformLevel, _platformMoveDelay);
            _hasMoved = true;
        }       
    }

    private void LowerGates()
    {
        if (!_hasMoved)
        {
            _platform.DOLocalMoveY(-20, _platformMoveDelay);
            _hasMoved = true;
        }
    }

    private void ToggleObject()
    {
        if (!_hasMoved)
        {
            _platform.gameObject.SetActive(!_platform.gameObject.activeInHierarchy);
            _hasMoved = true;
        }
    }
}
