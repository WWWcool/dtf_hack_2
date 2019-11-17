using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleSceneManager : MonoBehaviour
{
    private int currentLoadIndex = 0;

    private bool sceneStarting = true;
    private bool sceneEnding = false;
    private Image fade;
    [SerializeField] private float fadeSpeed = 0.5f;

    private void Start()
    {
        GameObject image = GameObject.Find("Fade");
        fade = image.GetComponent<Image>();
        currentLoadIndex = SceneManager.GetActiveScene().buildIndex;
        fade.enabled = false;
    }

    public void LoadNextScene()
    {
        currentLoadIndex++;
        if (currentLoadIndex >= SceneManager.sceneCountInBuildSettings)
        {
            currentLoadIndex = 0;
        }
        sceneEnding = true;
    }

    public void LoadPreviousScene()
    {
        currentLoadIndex = Mathf.Clamp(currentLoadIndex - 1, 0, SceneManager.sceneCountInBuildSettings - 1);
        sceneEnding = true;
    }

    private void Update()
    {
        if (sceneStarting) StartScene();
        if (sceneEnding) EndScene();
    }

    private void StartScene()
    {
        fade.enabled = true;
        fade.color = Color.Lerp(fade.color, Color.clear, fadeSpeed * Time.deltaTime);
        print($"color {fade.color} delta {Time.deltaTime}");
        if (fade.color.a <= 0.01f)
        {
            fade.color = Color.clear;
            fade.enabled = false;
            sceneStarting = false;
        }
    }
    private void EndScene()
    {
        sceneStarting = false;
        fade.enabled = true;
        fade.color = Color.Lerp(fade.color, Color.black, fadeSpeed * Time.deltaTime);
        print($"color {fade.color} delta {Time.deltaTime}");
        if (fade.color.a >= 0.95f)
        {
            sceneEnding = false;
            fade.color = Color.black;
            SceneManager.LoadScene(currentLoadIndex);
        }
    }

}
