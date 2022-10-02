using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    float _lerpSpeed;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Transform _tiltTransform;

    [SerializeField]
    Transform _target;

    [Header("Win Information")]
    [SerializeField]
    Vector3 _winCameraPos;
    
    [SerializeField]
    Vector3 _winCameraTilt;

    [SerializeField]
    Vector3 _winCameraOffset;

    [SerializeField]
    float _winTransitionTime;

    [SerializeField]
    Ease _winEase = Ease.OutExpo;

    private bool _isWinMode;


    private GameObject _topLeft;
    private GameObject _bottomRight;

    void Start()
    {
        // Not sure I like this
        // _topLeft = GameObject.FindGameObjectWithTag("TopLeft");
        // _bottomRight = GameObject.FindGameObjectWithTag("BottomRight");
        transform.position = _target.position;
    }

    public void SwapToWinMode()
    {
        if (_isWinMode)
            return;

        _isWinMode = true;
        _camera.transform.DOLocalMove(_winCameraPos, _winTransitionTime).SetEase(_winEase);
        _tiltTransform.transform.DOLocalRotate(_winCameraTilt, _winTransitionTime).SetEase(_winEase);
        _tiltTransform.transform.DOLocalMove(_winCameraOffset, _winTransitionTime).SetEase(_winEase);
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
            return;

        Vector3 targetPos = _target.position;

        if (_topLeft != null)
        {
            targetPos.x = Mathf.Max(_topLeft.transform.position.x, targetPos.x);
            targetPos.z = Mathf.Min(_topLeft.transform.position.z, targetPos.z);
        }

        if (_bottomRight != null)
        {
            targetPos.x = Mathf.Min(_bottomRight.transform.position.x, targetPos.x);
            targetPos.z = Mathf.Max(_bottomRight.transform.position.z, targetPos.z);
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, _lerpSpeed * Time.deltaTime);        
    }

    public void Init(Transform tgt)
    {
        _target = tgt;
    }

    public Vector3 CameraForward
    {
        get
        {
            Vector3 fwd = transform.forward;
            fwd.y = 0;
            return fwd.normalized;
        }
    }

    public Vector3 CameraRight
    {
        get
        {
            return transform.right;
        }
    }
}
