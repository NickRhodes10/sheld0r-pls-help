using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{    
    public Image FadingImage;
    public float fadeTime = 1;
    public float alpha;    

    public Button newGameButton;
    public Button loadGameButton;
    public Button quitButton;


    private void Awake()
    {
        PlayerSaving.SaveToken.PlayerSaveToken data = PlayerSaving.StatsSaveLoad.Load();
        if (data == null)
        {
            loadGameButton.interactable = false;
        }
        else
        {
            loadGameButton.interactable = true;
        }
    }
    void Start()
    {
        StartCoroutine(FadeImage());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator FadeImage()
    {
        for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
        {
            alpha = (i - 0) / (fadeTime - 0);
            FadingImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    private void Update()
    {
        if (alpha <= .01f)
        {
            FadingImage.gameObject.SetActive(false);
        }
    }
    

    public void NewGameButton()
    {
        PlayerPrefs.SetString("Save", "false");
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadGameButton()
    {
        // This do be where we will put the code for loading
        PlayerPrefs.SetString("Save", "true");
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync(2);
    }

    public void Quit()
    {
        Application.Quit();
    }



}
