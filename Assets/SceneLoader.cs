using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject canvasMenu;
    public GameObject loaderUI;
    public Slider progressSlider;

    // Start is called before the first frame update
    void Start()
    {
        canvasMenu.SetActive(false);
        //gameObject.SetActive(false);
        StartCoroutine(LoadScene_Coroutine(1));
    }

    public IEnumerator LoadScene_Coroutine(int index){
        progressSlider.value = 0;
        loaderUI.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;
        while(!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progress;
            if(progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
