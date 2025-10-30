using System.Collections;
using UnityEngine;

public class UIAnimatorController : MonoBehaviour
{
    [SerializeField] private TitleManager TitleManager;
    [SerializeField] IrisOut irisOut;
    public void PushAnimation()
    {
        GetComponent<Animator>().SetTrigger("Push");
    }

    public void RankingScene()
    {
        TitleManager.Onclickranking();
    }

    public void GameScene()
    {
        //3ïbå„Ç…ÉVÅ[ÉìëJà⁄
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
