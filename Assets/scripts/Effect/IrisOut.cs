using UnityEngine;
using System.Collections;
public class IrisOut : MonoBehaviour
{
  [SerializeField]
    Material irisMaterial;
    float scrollSpeed = 0.1f;
    float scrollAcceleration = 0.1f;
    [SerializeField]
    float scrollTime = 0;
    public void StartIrisOut()
    {
        StartCoroutine(IrisOutProcess());
    }
    
    IEnumerator IrisOutProcess()
    {
        float currentSpeed = 0;
        while (scrollTime  <= 1)
        {
            if (currentSpeed < scrollSpeed)
            {
                currentSpeed += scrollAcceleration * Time.deltaTime;
                scrollTime += currentSpeed *  Time.deltaTime;
            }
            else
            {
                scrollTime += scrollSpeed *Time.deltaTime;
            }
            irisMaterial.SetFloat("_IrisTime", scrollTime );
            yield return null;
        }
    }

}