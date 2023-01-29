using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManager;
using UnityEngine.SceneManagement;

public class MenuButtonClicks : MonoBehaviour
{
    public Animator anim;
    public Image background;

    public GameObject characterSkills;

    public TMPro.TMP_Text itemTXT;
    public TMPro.TMP_Text itemUpgradeTXT;
    public TMPro.TMP_Text magicTXT;
    public TMPro.TMP_Text magicUpgradeTXT;

    public Slider BackgroundVolume;
    public Slider SFXVolume;

    public Image ItemImage;
    public Image MagicImage;

    public GameObject OptionMenu;

    public GameObject[] Skills;
    private bool canCloseMenu;

    public Button exitToMenuButton;
    public Button quitToDesktopButton;

    private CurrentUpgrades inventory;
    public InputHandlerFirstPerson input;

    public GameObject ItemLock;
    public GameObject MagicLock;

    // Start is called before the first frame update
    void Start()
    {
        BackgroundVolume.value = 1;
        SFXVolume.value = 1;
        inventory = GameManager.PlayerManager.pm.GetComponent<CurrentUpgrades>();
        OptionMenu.SetActive(false);
        anim = GetComponent<Animator>();
        input = InputHandlerFirstPerson.instance;
        for (int i = 0; i < Skills.Length; i++)
        {
            Skills[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var source in AudioManager.instance._sfxSources)
        {
            source.volume = SFXVolume.value;
        }

        AudioManager.instance._bgm.volume = BackgroundVolume.value;

        if(ItemImage.sprite == null)
        {
            ItemLock.SetActive(true);
        }
        else
        {
            ItemLock.SetActive(false);
        }

        if (MagicImage.sprite == null)
        {
            MagicLock.SetActive(true);
        }
        else
        {
            MagicLock.SetActive(false);
        }

        if (input.openMenuKey)
        {
            PlayerManager.pm.gameplayPaused = true;
            PlayerManager.pm.EnableCursor();
            Time.timeScale = 0;
            anim.SetBool("Menu", true);
            canCloseMenu = true;
        }
        if (input.escapeKey)
        {
            canCloseMenu = true;
            anim.SetBool("open", false);
        }

        if (input.escapeKey && canCloseMenu == true && OptionMenu.activeSelf == false)
        {
            PlayerManager.pm.gameplayPaused = false;
            PlayerManager.pm.DisableCursor();
            anim.SetBool("Menu", false);
            Time.timeScale = 1;

        }
        if (input.escapeKey && OptionMenu.activeSelf == true)
        {
            OptionMenu.SetActive(false);
        }
    }

    public void ButtonclickBlue()
    {

        if (canCloseMenu == false)
        {
            anim.SetBool("open", false);
            canCloseMenu = true;
        }
        else
        {
            canCloseMenu = false;
            anim.SetBool("open", true);

            characterSkills.SetActive(true);
        }
    }
    public void ButtonclickGreen()
    {
       //increase base damage

    }
    public void ButtonclickOrange()
    {
       //increase item level
    }
    public void ButtonclickRed()
    {
        //increase magic level
    }

    public void ButtonClickOptions()
    {
        OptionMenu.SetActive(true);
    }

    private IEnumerator Escape()
    {
        yield return new WaitForSeconds(1f);

        canCloseMenu = true;
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
