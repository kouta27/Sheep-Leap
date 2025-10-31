using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RankingSystem;
using System.Linq;

public class RankingListUI : MonoBehaviour, IRankingReceiver
{
    [Header("References")]
    [SerializeField] private Ranking ranking;           // Assign the Ranking component in the scene
    [SerializeField] private RectTransform content;     // Parent (VerticalLayoutGroup + ContentSizeFitter recommended)
    [SerializeField] private GameObject rowPrefab;      // Row prefab with RankingRow component
    [SerializeField] private Button refreshButton;      // Optional: Refresh button
    [SerializeField] private TMP_Text statusText;       // Optional: Status feedback

    [Header("Options")]
    [SerializeField] private bool sortDescendingByScore = true; // Sort option

    private void Awake()
    {
        if (refreshButton != null)
            refreshButton.onClick.AddListener(Refresh);
    }

    private void OnDestroy()
    {
        if (refreshButton != null)
            refreshButton.onClick.RemoveListener(Refresh);
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        ClearList();
        SetStatus("Loading...");
        ranking.GetRanking(this);
    }

    private void ClearList()
    {
        if (content == null) return;
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

    private void AddRow(int rankNo, RankingData data)
    {
        if (rowPrefab == null || content == null) return;

        var go = Instantiate(rowPrefab, content);
        var row = go.GetComponent<RankingRow>();
        if (row == null)
        {
            Debug.LogWarning("Row prefab does not have RankingRow component.");
            return;
        }

        // Example display: "1. PlayerName" and "12345"
        row.SetRow($"{rankNo}. {data.name}", data.score);
    }

    private void SetStatus(string msg)
    {
        if (statusText != null) statusText.text = msg;
    }

    // ---- IRankingReceiver ----
    public void OnRankingLoadSuccess(RankingData[] datas)
    {
        // Sort if needed
        RankingData[] list = datas ?? new RankingData[0];
        if (sortDescendingByScore)
            list = list.OrderByDescending(d => d.score).ToArray();

        // Build UI
        ClearList();
        for (int i = 0; i < list.Length; i++)
        {
            AddRow(i + 1, list[i]);
        }

        SetStatus(list.Length > 0 ? $"Loaded {list.Length} entries." : "No entries.");
    }

    public void OnRankingLoadError()
    {
        ClearList();
        SetStatus("Load failed.");
    }

    // Unused in this class but required by the interface
    public void OnRankingPostSuccess() { }
    public void OnRankingPostError() { }
}
