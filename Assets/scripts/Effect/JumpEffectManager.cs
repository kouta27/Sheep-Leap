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
        // --- �V���O���g���p�^�[���̎��� ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[�����܂����ł��j������Ȃ��悤�ɂ���
        }
        else
        {
            Destroy(gameObject); // ���ɑ��݂���ꍇ�͎��g��j��
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
                // �W�����v�G�t�F�N�g�Ȃ�
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
