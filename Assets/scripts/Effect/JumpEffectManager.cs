using UnityEngine;

public class JumpEffectManager : MonoBehaviour
{
    [SerializeField]
    Animator chargeGaugeAnimator, chargeEffectAnimator;
    [SerializeField]
    GameObject jumpEffectObject, superJumpEffect, chargeEffect;

    public static JumpEffectManager Instance { get; private set; }
    private JumpEffectType currentJumpEffectType = JumpEffectType.None;

    private void Awake()
    {
        // --- シングルトンパターンの実装 ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されないようにする
        }
        else
        {
            Destroy(gameObject); // 既に存在する場合は自身を破棄
            return;
        }
    }

    public void StartChargeEffect()
    {
        chargeEffect.SetActive(true);
        chargeGaugeAnimator.SetTrigger("OneCharge");
        currentJumpEffectType = JumpEffectType.Normal;
    }

    public void JumpEffect()
    {
        switch (currentJumpEffectType)
        {
            case JumpEffectType.None:
                // ジャンプエフェクトなし
                break;
            case JumpEffectType.Normal:
                jumpEffectObject.SetActive(true);
                chargeGaugeAnimator.SetTrigger("Jump");
                chargeEffect.SetActive(false);
                currentJumpEffectType = JumpEffectType.None;
                break;
            case JumpEffectType.Super:
                superJumpEffect.SetActive(true);
                chargeGaugeAnimator.SetTrigger("Jump");
                chargeEffectAnimator.SetTrigger("Jump");
                chargeEffect.SetActive(false);
                currentJumpEffectType = JumpEffectType.None;
                break;
        }
    }

    public void UpCharegeLevel()
    {
        if (currentJumpEffectType < JumpEffectType.Super)
        {
            currentJumpEffectType++;
        }
        switch (currentJumpEffectType)
        {
            case JumpEffectType.Super:

                chargeEffectAnimator.SetTrigger("Two");
                chargeGaugeAnimator.SetTrigger("TwoCharge");
                break;
        }
    }
}

public enum JumpEffectType
{
    None,
    Normal,
    Super
}
