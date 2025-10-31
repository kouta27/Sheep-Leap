using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private IrisOut irisOut;
    static public float totalScoreForRanking;

    public void OnclickStart()
    {
        //���ۑ����Ă��܂������̂������p
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        SceneManager.LoadScene("Stage0");
        if (irisOut == null)
        {
            //�Q�[���}�l�[�W���[�̃I�u�W�F�N�g�ɂ��Ă���irisOut��T��
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
