using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private IrisOut irisOut;
    static public float totalScoreForRanking;

    public void OnclickStart()
    {
        //↓保存してしまったものを消す用
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        SceneManager.LoadScene("Stage0");
        if (irisOut == null)
        {
            //ゲームマネージャーのオブジェクトについているirisOutを探す
            GameObject gamemanager = GameManager.instance.gameObject;
            irisOut = gamemanager.GetComponent<IrisOut>();
        }
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

    public void OnclickTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
