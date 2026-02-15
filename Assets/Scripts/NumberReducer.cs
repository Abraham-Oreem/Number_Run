using UnityEngine;
using DG.Tweening;
using TMPro;

public class NumberReducer : MonoBehaviour
{
    private int displayNumber = 10;
    [SerializeField] private Transform targetTransform;
    [SerializeField] public float tweenDuration = 0.15f;
    [SerializeField] private TextMeshProUGUI textDisp;

    [SerializeField] public int damage = 15;

    private Tween currentTween;

    private void Start()
    {
        displayNumber = damage;
        textDisp.text = "-" + displayNumber.ToString();
    }

    public void DamageEffect(Player p)
    {
        if (displayNumber <= 0) return;

        currentTween?.Kill();
        float newZ = 0;

        currentTween = targetTransform
            .DOScaleZ(newZ, tweenDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                textDisp.text = "-" + displayNumber.ToString();
            })
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                textDisp.gameObject.SetActive(false);
            });
    }



    public void StopTween(Player p)
    {
        currentTween?.Kill();
        this.gameObject.SetActive(false); 
        textDisp.gameObject.SetActive(false);
    }
}
