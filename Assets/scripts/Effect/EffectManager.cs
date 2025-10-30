using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクトを一元管理するマネージャー（シングルトン）
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

    [Header("登録するエフェクト一覧")]
    [SerializeField] private List<EffectEntry> effects = new();

    private Dictionary<EffectType, GameObject> effectDict;

    private void Awake()
    {
        // シングルトンの初期化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 辞書登録
        effectDict = new Dictionary<EffectType, GameObject>();
        foreach (var e in effects)
        {
            if (e.prefab != null && !effectDict.ContainsKey(e.type))
                effectDict.Add(e.type, e.prefab);
        }
    }

    /// <summary>
    /// 指定したエフェクトのPrefabを取得（複製しない）
    /// </summary>
    public GameObject GetEffectPrefab(EffectType type)
    {
        if (effectDict.TryGetValue(type, out var prefab))
            return prefab;

        Debug.LogWarning($"EffectManager: {type} は登録されていません。");
        return null;
    }

    /// <summary>
    /// 指定した位置にエフェクトを再生する（自動破棄あり）
    /// </summary>
    public GameObject PlayEffect(EffectType type, Vector3 position, Quaternion rotation = default,
                                 float destroyAfterSeconds = 0f, Transform parent = null)
    {
        if (!effectDict.TryGetValue(type, out var prefab))
        {
            Debug.LogWarning($"EffectManager: {type} は登録されていません。");
            return null;
        }

        var effect = Instantiate(prefab, position, rotation, parent);

        if (destroyAfterSeconds > 0f)
        {
            Destroy(effect, destroyAfterSeconds);
        }
        else
        {
            // パーティクルの場合、自動で寿命を推定して破棄
            var ps = effect.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }

        return effect;
    }

    /// <summary>
    /// 手動でエフェクトを削除
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