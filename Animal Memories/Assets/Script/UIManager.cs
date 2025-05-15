using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    
    [Header("UI Refrences")] 
    public Button playButton;
    public Button audioButton;
    public Button backToMainMenuButton;
    public ScrollRect scrollRect;
    public Sprite unmuteImage;
    public Sprite muteImage;
    public bool isMuted;

    [Header("Elements Refrences")]
    public AudioSource audioSource;
    
    [Header("References Values")]
    public float maxVolume;

    [Header("LevelSelector Menu")] 
    public GameObject levelsParent;
    private GameObject[] levels;

    public float left;
    public float right;
    private void Start()
    {

        GetTheChildCount();
        
        playButton.transform.localScale = Vector3.zero;
        playButton.onClick.AddListener(PlayButtonClick);
        audioButton.onClick.AddListener(AudioButtonClick);
        backToMainMenuButton.onClick.AddListener(BackToMainMenuButtonClick);
        Invoke(nameof(PlayButtonInitialize), 0.2f);
        
    }

    private void GetTheChildCount()
    {
        int childCount = levelsParent.transform.childCount;
        levels = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            levels[i] = levelsParent.transform.GetChild(i).gameObject;
        }
    }




    void PlayButtonClick()
    {
        playButton.transform.localScale = Vector3.zero;
        playButton.transform.DOScale(new Vector3(1,1,1),0.2f).SetEase(Ease.OutBack);
        ResetScroll();
        PlayPopClip();
        Invoke(nameof(ActivateLevelSelectPanel), 0.25f);
       
    }

    void BackToMainMenuButtonClick()
    {
      
        backToMainMenuButton.transform.localScale = Vector3.one;
        backToMainMenuButton.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutBack);
        PlayPopClip();
        Invoke(nameof(ActivateMainMenuPanel), 0.25f);
    }



    public void ResetScroll()
    {

        scrollRect.horizontalNormalizedPosition = 0f; // Left (if needed)
    }

    void ActivateMainMenuPanel()
    {
        PlayButtonInitialize();
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
    }
    void ActivateLevelSelectPanel()
    {
        
        backToMainMenuButton.transform.localScale = Vector3.zero;
        backToMainMenuButton.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].transform.localScale = Vector3.zero;
        }

        StartCoroutine(AllLevelsVisible());

    }

   

    IEnumerator AllLevelsVisible()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            yield return new WaitForSeconds(0.15f);
            PlayLevelPopClip();
            levels[i].transform.DOScale(Vector3.one, .2f).SetEase(Ease.OutBack);
            
        }
    }

    private void PlayPopClip()
    {
        if (isMuted) return;
        AudioManager.instance.PlayPopClip();
    }

    void PlayLevelPopClip()
    {
        if(isMuted) return;
        AudioManager.instance.PlayLevelPopClip();
    }

    public void AudioButtonClick()
    {
        isMuted = !isMuted;
        
        if (!isMuted)
        {
            audioButton.transform.localScale = Vector3.zero;
            audioButton.transform.DOScale(new Vector3(-1,1,1),0.2f).SetEase(Ease.OutBack);
            audioSource.volume = maxVolume;
            audioButton.image.sprite = unmuteImage;
            AudioManager.instance.PlayPopClip();
            
        }
        else
        {
            PlayPopClip();
            audioButton.transform.localScale = Vector3.zero;
            audioButton.transform.DOScale(new Vector3(-1,1,1),0.2f).SetEase(Ease.OutBack);
            audioSource.volume = 0f;
            audioButton.image.sprite = muteImage;
        }
    }

    void PlayButtonInitialize()
    {
        playButton.transform.localScale = Vector3.zero;
        playButton.transform.DOScale(new Vector3(1,1,1),0.5f).SetEase(Ease.OutBack);
        PlayPopClip();
    }
    
    
}

