using UnityEngine;
using DG.Tweening;

public class MoodBackgroundAnimations : MonoBehaviour
{

    [Header("Options")]
    public float MaxRotationOffset = 5;
    public float MinRotationOffset = 24;
    public float MinDuration = 4;
    public float MaxDuration = 8;

    [Header("Refrences")]
    public RectTransform[] Leaf;
    
    
    private Tween[] tweens;

    void Start()
    {

        tweens = new Tween[Leaf.Length];

        for (int i = 0; i < Leaf.Length; i++)
        {
            RectTransform rt = Leaf[i];

            float direction = Random.value > 0.5f ? 1f : -1f;
            float rotDiff = Random.Range(MinRotationOffset, MaxRotationOffset) * direction;

            float duration = Random.Range(MinDuration, MaxDuration);

            float targetZ = rt.localEulerAngles.z + rotDiff;

            tweens[i] = rt.DOLocalRotate(
                new Vector3(0, 0, targetZ),
                duration
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
        }

    }

    // Update is called once per frame
    void OnDestroy()
    {
        foreach (var t in tweens)
        {
            t.Kill();
        }
    }
}
