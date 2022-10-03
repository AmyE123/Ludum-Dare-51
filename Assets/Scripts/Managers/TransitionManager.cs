using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager _instance;

    [SerializeField]
    private bool _startVisible;

    [SerializeField]
    private Image[] _bars;

    [SerializeField]
    private CanvasGroup _grp;

    [SerializeField]
    private float _transitionSpeed = 0.6f;

    [SerializeField]
    private float _holdTime = 0.1f;

    private string _sceneToLoad;

    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);

        foreach (Image img in _bars)
            img.fillAmount = _startVisible ? 1 : 0;

        _grp.blocksRaycasts = _grp.interactable = _startVisible;
    }

    public static void LoadScene(string sceneName)
    {
        if (_instance == null)
            _instance = FindObjectOfType<TransitionManager>();

        _instance.StartTransition(sceneName);
    }

    private void StartTransition(string sceneName)
    {
        foreach (Image img in _bars)
            img.DOFillAmount(1, _transitionSpeed).SetEase(Ease.Linear);

        _grp.alpha = 1;
        _grp.blocksRaycasts = true;

        MusicManager.ResetSelf();

        StopAllCoroutines();
        StartCoroutine(WaitForTransitionRoutine(sceneName));
    }

    private IEnumerator WaitForTransitionRoutine(string sceneName)
    {
        yield return new WaitForSeconds(_transitionSpeed + 0.05f + _holdTime);
        SceneManager.LoadScene(sceneName);

        foreach (Image img in _bars)
            img.DOFillAmount(0, _transitionSpeed).SetEase(Ease.Linear);

        yield return new WaitForSeconds(_transitionSpeed / 2);
        _grp.blocksRaycasts = false;
    }
}
