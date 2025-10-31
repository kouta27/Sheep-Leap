using System.Collections;
using UnityEngine;

public class UIAnimatorController : MonoBehaviour
{
    [SerializeField] private TitleManager TitleManager;
    [SerializeField] IrisOut irisOut;
    private void Start()
    {
        if (TitleManager == null)
        {
            TitleManager = FindObjectOfType<TitleManager>();
        }
        if (irisOut == null)
        {
            //�Q�[���}�l�[�W���[�̃I�u�W�F�N�g�ɂ��Ă���irisOut��T��
            GameObject gamemanager = GameManager.instance.gameObject;
            irisOut = gamemanager.GetComponent<IrisOut>();
        }
    }
    public void PushAnimation()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SoundId.UI_Click);
        }
        GetComponent<Animator>().SetTrigger("Push");
    }

    public void RankingScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SoundId.UI_Click);
        }
        TitleManager.Onclickranking();
    }

    public void GameScene()
    {
        //3�b��ɃV�[���J��
        StartCoroutine(WaitAndLoadScene(2f) );
    }

    IEnumerator WaitAndLoadScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        TitleManager.OnclickStart();
    }

    public void IrisOut()
    {
        irisOut.StartIrisOut();
    }
}
