using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RankingSystem;

public class RankingInputUI : MonoBehaviour, IRankingReceiver
{
    [Header("References")]
    [SerializeField] private Ranking ranking;            // Ranking component in the scene
    [SerializeField] private TMP_InputField nameField;   // Player name input
    [SerializeField] private TMP_InputField scoreField;  // Optional manual score input
    [SerializeField] private Button submitButton;        // Submit button
    [SerializeField] private TMP_Text statusText;        // Status feedback
    [SerializeField] private GameManager gameManager;    // ← Add your GameManager here
    [SerializeField] private GameObject inputUIObject,submitButtonObject;   // ← Add the UI GameObject here
    [SerializeField] private Animator animator;        // ← Add Animator for UI transitions

    private bool isPosting = false;

    private void Awake()
    {
        if (submitButton != null)
            submitButton.onClick.AddListener(OnClickSubmit);
        SetStatus("");
    }

    private void OnDestroy()
    {
        if (submitButton != null)
            submitButton.onClick.RemoveListener(OnClickSubmit);
    }

    private void OnClickSubmit()
    {
        if (isPosting) return;

        string playerName = (nameField != null) ? nameField.text.Trim() : "";
        if (string.IsNullOrEmpty(playerName))
        {
            SetStatus("Please input your name.");
            return;
        }

        int scoreValue = 0;

        scoreValue = GameManager.instance.GetTotalScore(); 

        // Lock UI and post
        isPosting = true;
        if (submitButton) submitButton.interactable = false;
        SetStatus("Posting...");
        //インプットフィールドとボタンを非活性化
        
        if(inputUIObject!=null) inputUIObject.SetActive(false);
        if(submitButtonObject!=null) submitButtonObject.SetActive(false);
        if(animator!=null) animator.SetTrigger("Can");
        // Post to ranking server
        ranking.PostRanking(playerName, scoreValue, this);
    }

    private void SetStatus(string msg)
    {
        if (statusText != null) statusText.text = msg;
    }

    // ---- IRankingReceiver ----
    public void OnRankingPostSuccess()
    {
        isPosting = false;
        if (submitButton) submitButton.interactable = true;
        SetStatus("Post succeeded!");
    }

    public void OnRankingPostError()
    {
        isPosting = false;
        if (submitButton) submitButton.interactable = true;
        SetStatus("Post failed. Please try again.");
    }

    // Unused (required by interface)
    public void OnRankingLoadSuccess(RankingData[] datas) { }
    public void OnRankingLoadError() { }
}
