using System.Collections;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField]
    Material[] materials;
    float scrollSpeed = 0.1f;
    float scrollAcceleration = 0.1f;
    float scrollTime = 0;
    bool isEnded = false;

    public void Start()
    {
        StartScroll(); 
    }
    public void StartScroll()
    {
        StartCoroutine(Scroll());
    }
    public void EndScroll()
    {
        isEnded = true;
    }

    IEnumerator Scroll()
    {
        float currentSpeed = 0;
        while (!isEnded)
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
            foreach (var mat in materials)
            {
                mat.SetFloat("_ScrollTime",scrollTime );
            }
            yield return null;
        }
        while (currentSpeed > 0)
        {
            scrollTime += currentSpeed * Time.deltaTime;
            foreach (var mat in materials)
            {
                mat.SetFloat("_ScrollTime", scrollTime);
            }
            currentSpeed -= scrollAcceleration * Time.deltaTime;
            yield return null;
        }
        isEnded = false;
    }

}
