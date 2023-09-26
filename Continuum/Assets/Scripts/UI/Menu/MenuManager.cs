using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset inputs;

    [Header("Canvases")]
    [SerializeField] private Canvas startPane;
    [SerializeField] private Canvas nav;
    [SerializeField] private Canvas levelSelect;
    [SerializeField] private Canvas options;
    [SerializeField] private Canvas keybinds;
    [SerializeField] private Canvas keybindsK;
    [SerializeField] private Canvas keybindsG;

    [Header("Buttons")]
    [SerializeField] private Button start;
    [SerializeField] private Button tutorial;
    [SerializeField] private Button optionsBack;
    [SerializeField] private Button kbBack;
    [SerializeField] private Button kbdKbBack;
    [SerializeField] private Button gpdKbBack;

    [Header("Objects")]
    [SerializeField] private GameObject anyToStart;
    [SerializeField] private GameObject ButtonCont;
    private CheckpointManager checkpointManager;
    [SerializeField] private GameObject cmPrefab;

    public Animator backgroundAnim;
    public Animator titleAnim;

    private readonly float bgAnimTime = 2f;

    private void Awake()
    {
        Time.timeScale = 1.0f;

        if (startPane) startPane.gameObject.SetActive(true);
        if (nav) nav.gameObject.SetActive(false);
        if (levelSelect) levelSelect.gameObject.SetActive(false);
        if (options) options.gameObject.SetActive(false);
        if (keybinds) keybinds.gameObject.SetActive(false);
        if (keybindsK) keybindsK.gameObject.SetActive(false);
        if (keybindsG) keybindsG.gameObject.SetActive(false);

        PlayerData data = SaveManager.LoadData();

        //Checkpoint Object Create
        if (GameObject.FindGameObjectWithTag("CheckpointManager"))
        {
            checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
        }
        else
        {
            GameObject cm = Instantiate(cmPrefab);
            cm.name = "CheckpointManager";
            checkpointManager = cm.GetComponent<CheckpointManager>();
        }

        //checkpointManager.savedLevel = -1;

        //Data load
        if (data != null)
        {
            //loading checkpoint data
        }
    }

    private void Update()
    {
        if(anyToStart.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                StartButton();
            }
        }
    }

    private void OnEnable()
    {
        inputs.FindAction("Cancel").performed += Back_performed;
    }

    private void OnDisable()
    {
        inputs.FindAction("Cancel").performed += Back_performed;
    }


    public void Back_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (nav.gameObject.activeSelf) CloseButton();
            if (levelSelect.gameObject.activeSelf) ToggleLevelSelect();
            if (options.gameObject.activeSelf) ToggleOptions();
            if (keybinds.gameObject.activeSelf) ToggleKeybindings();
            if (keybindsK.gameObject.activeSelf) ToggleKeyboardKeybindings();
            if (keybindsG.gameObject.activeSelf) ToggleGamepadKeybindings();
        }
    }

    public void StartButton()
    {
        StartCoroutine(OpenMenu());
    }

    public void CloseButton()
    {
        StartCoroutine(CloseMenu());
        SoundManager.PlaySound(SoundManager.Sound.snd_footstep);
    }

    public IEnumerator OpenMenu()
    {
        backgroundAnim.SetTrigger("start");
        titleAnim.SetTrigger("start");

        anyToStart.gameObject.SetActive(false);
        ButtonCont.SetActive(false);

        yield return new WaitForSeconds(bgAnimTime);

        nav.gameObject.SetActive(true);

        if (!nav.GetComponentInChildren<TypeTextHandler>().on)
        {
            yield return new WaitForSeconds(3.5f);
        }

        ButtonCont.SetActive(true);
        start.Select();

        yield break;
    }

    public IEnumerator CloseMenu()
    {
        backgroundAnim.SetTrigger("start");
        

        nav.gameObject.SetActive(false);

        yield return new WaitForSeconds(bgAnimTime);

        titleAnim.SetTrigger("start");
        anyToStart.gameObject.SetActive(true);

        yield break;
    }

    public void Continue()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_footstep);

        //Level Select
        switch (checkpointManager.savedLevel)
        {
            case 1:
                ChangeScene("Level_1");
                break;
            case 2:
                ChangeScene("Level_2");
                break;
            case 3:
                ChangeScene("Level_3");
                break;
            case 4:
                ChangeScene("Level_4");
                break;
            default:
                ChangeScene("Level_Tutorial");
                break;
        }

    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleLevelSelect()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_footstep);

        if (nav.gameObject.activeSelf)
        {
            nav.gameObject.SetActive(false);
            levelSelect.gameObject.SetActive(true);
            tutorial.Select();
        }
        else if (levelSelect.gameObject.activeSelf)
        {
            levelSelect.gameObject.SetActive(false);
            nav.gameObject.SetActive(true);
            start.Select();
        }
    }

    public void ToggleOptions()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_footstep);

        if (nav.gameObject.activeSelf)
        {
            nav.gameObject.SetActive(false);
            options.gameObject.SetActive(true);
            optionsBack.Select();
        }
        else if (options.gameObject.activeSelf)
        {
            options.gameObject.SetActive(false);
            nav.gameObject.SetActive(true);
            start.Select();
        }
    }

    public void ToggleKeybindings()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_footstep);

        if (options.gameObject.activeSelf)
        {
            options.gameObject.SetActive(false);
            keybinds.gameObject.SetActive(true);
            kbBack.Select();
        }
        else if (keybinds.gameObject.activeSelf)
        {
            keybinds.gameObject.SetActive(false);
            options.gameObject.SetActive(true);
            optionsBack.Select();
        }
    }

    public void ToggleKeyboardKeybindings()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_footstep);

        if (keybinds.gameObject.activeSelf)
        {
            keybinds.gameObject.SetActive(false);
            keybindsK.gameObject.SetActive(true);
            kbdKbBack.Select();
        }
        else if (keybindsK.gameObject.activeSelf)
        {
            keybindsK.gameObject.SetActive(false);
            keybinds.gameObject.SetActive(true);
            kbBack.Select();
        }
    }

    public void ToggleGamepadKeybindings()
    {
        SoundManager.PlaySound(SoundManager.Sound.snd_footstep);

        if (keybinds.gameObject.activeSelf)
        {
            keybinds.gameObject.SetActive(false);
            keybindsG.gameObject.SetActive(true);
            gpdKbBack.Select();
        }
        else if (keybindsG.gameObject.activeSelf)
        {
            keybindsG.gameObject.SetActive(false);
            keybinds.gameObject.SetActive(true);
            kbBack.Select();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
