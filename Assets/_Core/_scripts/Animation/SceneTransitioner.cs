using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTransitioner : MonoBehaviour
{
    [Header("Animation Settings")]
    public float animationDuration = 0.75f;

    [Header("References")]
    public Image maskCover;
    public Image icon;
    public Image logo;

    private Sequence tween;
    private string nextScene;

    private Action onCompleteCallback;
    private Action onRewindCallback;
    public static SceneTransitioner Instance;

    public static bool IsShown { get; private set; }
    public static bool IsPlaying { get; private set; }

    void Awake()
    {
        // Optional kalau mau jadi global manager
        // DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        tween = DOTween.Sequence()
            .SetUpdate(true)
            .SetAutoKill(false)
            .Pause()

            // Mask Fill
            .Append(maskCover.DOFillAmount(0, animationDuration).From())

            // Icon Scale + Rotation
            .Join(icon.transform.DOScale(0.01f, animationDuration * 0.6f).From())
            .Join(icon.transform.DOPunchRotation(new Vector3(0, 0, 90),
                                                 animationDuration * 0.9f, 6))

            // Logo Scale
            .Insert(animationDuration * 0.4f,
                logo.transform.DOScale(0.01f,
                animationDuration * 0.5f)
                .From()
                .SetEase(Ease.OutBack))

            .OnPlay(() =>
            {
                gameObject.SetActive(true);
                IsPlaying = true;
            })
            .OnRewind(OnRewind)
            .OnComplete(OnComplete);

        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        tween?.Kill();
    }

    // =========================
    // PUBLIC FUNCTION
    // =========================

    public void Show(bool doShow, Action callback = null)
    {
        DoShow(doShow, callback);
    }

    public void LoadScene(string sceneName)
    {
        nextScene = sceneName;

        Show(true, () =>
        {
            StartCoroutine(LoadAsync());
        });
    }

    // =========================
    // INTERNAL LOGIC
    // =========================

    void DoShow(bool doShow, Action callback = null)
    {
        IsShown = doShow;

        if (doShow)
        {
            maskCover.fillClockwise = true;

            onRewindCallback = null;
            onCompleteCallback = callback;

            tween.Restart();
        }
        else
        {
            maskCover.fillClockwise = false;

            onCompleteCallback = null;
            onRewindCallback = callback;

            if (tween.Elapsed() <= 0)
            {
                tween.Pause();
                OnRewind();
            }
            else
            {
                tween.PlayBackwards();
            }
        }
    }

    void OnComplete()
    {
        Time.timeScale = 1f;
        IsPlaying = false;

        onCompleteCallback?.Invoke();
    }

    void OnRewind()
    {
        IsPlaying = false;
        gameObject.SetActive(false);

        onRewindCallback?.Invoke();
    }
    IEnumerator LoadAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        while (!op.isDone)
            yield return null;

        // Ambil scene yang baru aktif
        Scene newScene = SceneManager.GetActiveScene();

        // Pindahkan object ini ke scene baru
        SceneManager.MoveGameObjectToScene(gameObject, newScene);

        // Sekarang animasi keluar
        Show(false);
    }
}