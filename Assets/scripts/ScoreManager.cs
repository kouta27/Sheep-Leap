using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("スコア設定")]
    public int score = 0;
    public int baseScorePerNote = 100;　//ノーツの基本得点
    public int bonusPerCombo = 10;　//コンボ1ごとに加算するボーナス点

    [Header("UI参照")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    [Header("演出設定")]
    public float comboPopScale = 1.4f;　//コンボ時に拡大する倍率
    public float popDuration = 0.15f;　//ポップの速さ
    public Color comboFlashColor = Color.red;　//ミスの時にフラッシュする色
    public float flashDuration = 0.25f;

    int combo = 0;
    int maxCombo = 0;
    Color comboOriginalColor;
    Vector3 comboOriginalScale;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        if (comboText != null)
        {
            comboOriginalColor = comboText.color;
            comboOriginalScale = comboText.transform.localScale;
        }
    }

    void Start()
    {
        UpdateUI();
    }

    //ノーツがヒットしたとき
    public void OnNoteHit(int noteBaseScore = -1)
    {
        if (noteBaseScore < 0) noteBaseScore = baseScorePerNote;

        combo++;
        if (combo > maxCombo) maxCombo = combo;

        //スコア計算（基本点＋コンボボーナス）
        int add = noteBaseScore + combo * bonusPerCombo;
        score += add;

        UpdateUI();

        //コンボのポップ演出
        if (comboText != null)
            StartCoroutine(ComboPop());
    }

    //ミスしたとき
    public void OnMiss()
    {
        combo = 0;
        UpdateUI();

        if (comboText != null)
            StartCoroutine(ComboFlash());
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";

        if (comboText != null)
        {
            if (combo <= 0) comboText.text = "Combo: 0";
            else comboText.text = $"Combo: {combo}";
        }
    }

    IEnumerator ComboPop()
    {
        Transform t = comboText.transform;
        Vector3 start = comboOriginalScale;
        Vector3 target = start * comboPopScale;
        float elapsed = 0f;

        //拡大してすぐ元に戻す
        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float f = elapsed / popDuration;
            t.localScale = Vector3.Lerp(start, target, Mathf.Sin(f * Mathf.PI));
            yield return null;
        }
        t.localScale = start;
    }

    IEnumerator ComboFlash()
    {
        //ミス時に色を一瞬変える
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flashDuration;
            comboText.color = Color.Lerp(comboFlashColor, comboOriginalColor, t);
            yield return null;
        }
        comboText.color = comboOriginalColor;
        comboText.transform.localScale = comboOriginalScale;
    }
}
