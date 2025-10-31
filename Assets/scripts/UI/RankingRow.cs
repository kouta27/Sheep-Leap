using UnityEngine;
using TMPro;

public class RankingRow : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetRow(string displayName, int score)
    {
        if (nameText) nameText.text = displayName;
        if (scoreText) scoreText.text = score.ToString();
    }
}
