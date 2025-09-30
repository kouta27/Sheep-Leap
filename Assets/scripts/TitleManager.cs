using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnclickStart()
    {
        SceneManager.LoadScene("Stage1");
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
