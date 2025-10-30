using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�t�F�N�g���ꌳ�Ǘ�����}�l�[�W���[�i�V���O���g���j
/// </summary>
public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [System.Serializable]
    public class EffectEntry
    {
        public EffectType type;
        public GameObject prefab;
    }

    [Header("�o�^����G�t�F�N�g�ꗗ")]
    [SerializeField] private List<EffectEntry> effects = new();

    private Dictionary<EffectType, GameObject> effectDict;

    private void Awake()
    {
        // �V���O���g���̏�����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // �����o�^
        effectDict = new Dictionary<EffectType, GameObject>();
        foreach (var e in effects)
        {
            if (e.prefab != null && !effectDict.ContainsKey(e.type))
                effectDict.Add(e.type, e.prefab);
        }
    }

    /// <summary>
    /// �w�肵���G�t�F�N�g��Prefab���擾�i�������Ȃ��j
    /// </summary>
    public GameObject GetEffectPrefab(EffectType type)
    {
        if (effectDict.TryGetValue(type, out var prefab))
            return prefab;

        Debug.LogWarning($"EffectManager: {type} �͓o�^����Ă��܂���B");
        return null;
    }

    /// <summary>
    /// �w�肵���ʒu�ɃG�t�F�N�g���Đ�����i�����j������j
    /// </summary>
    public GameObject PlayEffect(EffectType type, Vector3 position, Quaternion rotation = default,
                                 float destroyAfterSeconds = 0f, Transform parent = null)
    {
        if (!effectDict.TryGetValue(type, out var prefab))
        {
            Debug.LogWarning($"EffectManager: {type} �͓o�^����Ă��܂���B");
            return null;
        }

        var effect = Instantiate(prefab, position, rotation, parent);

        if (destroyAfterSeconds > 0f)
        {
            Destroy(effect, destroyAfterSeconds);
        }
        else
        {
            // �p�[�e�B�N���̏ꍇ�A�����Ŏ����𐄒肵�Ĕj��
            var ps = effect.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }

        return effect;
    }

    /// <summary>
    /// �蓮�ŃG�t�F�N�g���폜
    /// </summary>
    public void StopEffect(GameObject effect)
    {
        if (effect != null)
            Destroy(effect);
    }
}


public enum EffectType
{
    Explosion,
    Heal,
    Sparkle,
    jumpdown,
    jumpup,
    GetEffect,
}