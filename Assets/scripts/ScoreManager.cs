using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("�X�R�A�ݒ�")]
    public int score = 0;
    public int baseScorePerNote = 100;�@//�m�[�c�̊�{���_
    public int bonusPerCombo = 10;�@//�R���{1���Ƃɉ��Z����{�[�i�X�_

    [Header("UI�Q��")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    [Header("���o�ݒ�")]
    public float comboPopScale = 1.4f;�@//�R���{���Ɋg�傷��{��
    public float popDuration = 0.15f;�@//�|�b�v�̑���
    public Color comboFlashColor = Color.red;�@//�~�X�̎��Ƀt���b�V������F
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

    //�m�[�c���q�b�g�����Ƃ�
    public void OnNoteHit(int noteBaseScore = -1)
    {
        if (noteBaseScore < 0) noteBaseScore = baseScorePerNote;

        combo++;
        if (combo > maxCombo) maxCombo = combo;

        //�X�R�A�v�Z�i��{�_�{�R���{�{�[�i�X�j
        int add = noteBaseScore + combo * bonusPerCombo;
        score += add;

        UpdateUI();

        //�R���{�̃|�b�v���o
        if (comboText != null)
            StartCoroutine(ComboPop());
    }

    //�~�X�����Ƃ�
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

        //�g�債�Ă������ɖ߂�
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
        //�~�X���ɐF����u�ς���
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
