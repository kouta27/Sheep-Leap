using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    public Text rankingText;

    const int maxRank = 10;
    List<int> scores = new List<int>();

    void Start()
    {
        Debug.Log(TitleManager.totalScoreForRanking);//このTitleManager.totalScoreForRankingが前プレイした人のトータルスコア

        LoadRanking();

        //今回のトータルスコアを取得
        int latestScore = GameManager.instance.GetTotalScore();

        //ランキングに追加してソート
        scores.Add(latestScore);
        scores.Sort((a, b) => b.CompareTo(a)); // 降順
        if (scores.Count > maxRank)
            scores.RemoveAt(maxRank);

        SaveRanking();
        UpdateRankingUI();
    }

    void LoadRanking()
    {
        scores.Clear();
        for (int i = 0; i < maxRank; i++)
        {
            int s = PlayerPrefs.GetInt("Rank" + i, 0);
            scores.Add(s);
        }
    }

    void SaveRanking()
    {
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt("Rank" + i, scores[i]);
        }
        PlayerPrefs.Save();
    }

    void UpdateRankingUI()
    {
        rankingText.text = "";
        for (int i = 0; i < scores.Count; i++)
        {
            rankingText.text += $"{i + 1}位：{scores[i]}\n";
        }
    }

    public void OnClickTitle()
    {
        if (GameManager.instance != null)
        SceneManager.LoadScene("Title");
    }
}
