using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnclickStart()
    {
        //���ۑ����Ă��܂������̂������p
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        SceneManager.LoadScene("Stage0");
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
