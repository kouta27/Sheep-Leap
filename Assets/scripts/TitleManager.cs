using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private IrisOut irisOut;
    static public float totalScoreForRanking;

    public void OnclickStart()
    {
        //«•Û‘¶‚µ‚Ä‚µ‚Ü‚Á‚½‚à‚Ì‚ğÁ‚·—p
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        SceneManager.LoadScene("Stage0");
        irisOut.ClearIrisOut();
    }

    public void Onclickranking()
    {
        SceneManager.LoadScene("Ranking");
    }
    public void OnclickQuit()
    {
        Application.Quit();
    }
}
