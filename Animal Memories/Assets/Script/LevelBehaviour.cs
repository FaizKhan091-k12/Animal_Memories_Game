using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class LevelBehaviour : MonoBehaviour
{
    public Button[] levelButtons;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public float fadeDuration = 1f;
    public GameObject gamePanel;
    public GameObject[] uiPanels;
    public GameObject[] allLevels;
    public GameObject[] levelStartButton;
    public GameObject backButton;

    public GameObject[] InGameWindows;

    void Start()
    {
        UpdateButtons();
        TurnOffInGameWindows();
    }

    private void TurnOffInGameWindows()
    {
        for (int i = 0; i < InGameWindows.Length; i++)
        {
            InGameWindows[i].SetActive(false);
        }
    }

    public void UnlockNextLevel(int currentLevel)
    {
        int highestUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (currentLevel >= highestUnlocked && currentLevel < levelButtons.Length)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();

            // Start fading effect for the next level
            StartCoroutine(FadeOutLockAndEnableButton(currentLevel));
        }
        else
        {
            UpdateButtons(); // fallback in case no new level is unlocked
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.Save();
        UpdateButtons();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void UpdateButtons()
    {
        int unlockedUpTo = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            Button button = levelButtons[i];

            bool isUnlocked = (i + 1) <= unlockedUpTo;

            button.interactable = isUnlocked;

            // Button base alpha
            Color color = button.image.color;
            color.a = isUnlocked ? 1f : 0.5f;
            button.image.color = color;

            // Children alpha (text/icons)
            foreach (Graphic g in button.GetComponentsInChildren<Graphic>())
            {
                Color c = g.color;
                c.a = isUnlocked ? 1f : 0.5f;
                g.color = c;
            }

            // Lock icon logic
            Transform lockTransform = button.transform.Find("LockIcon");
            if (lockTransform != null)
            {
                Image lockImage = lockTransform.GetComponent<Image>();
                if (lockImage != null)
                {
                    lockImage.sprite = isUnlocked ? unlockedSprite : lockedSprite;
                    lockImage.enabled = true;

                    Color lockColor = lockImage.color;
                    lockColor.a = isUnlocked ? 0f : 1f;
                    lockImage.color = lockColor;
                }
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator FadeOutLockAndEnableButton(int index)
    {
        Button button = levelButtons[index];
        Transform lockTransform = button.transform.Find("LockIcon");

        if (lockTransform != null)
        {
            Image lockImage = lockTransform.GetComponent<Image>();
            if (lockImage != null)
            {
                // 🔁 Step 1: Swap locked → unlocked sprite before fading
                lockImage.sprite = unlockedSprite;
                lockImage.color = new Color(1f, 1f, 1f, 1f); // fully visible
                lockImage.enabled = true;

                // ⏳ Step 2: Fade the unlocked sprite
                float elapsed = 0f;
                Color startColor = lockImage.color;

                while (elapsed < fadeDuration)
                {
                    elapsed += Time.deltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                    lockImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                    yield return null;
                }

                lockImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
                lockImage.enabled = false;
            }
        }

        // ✅ Step 3: Unlock the button and update UI
        PlayerPrefs.SetInt("UnlockedLevel", index + 1);
        PlayerPrefs.Save();
        UpdateButtons();
    }


    public void LevelToActivate(int levelIndex)
    {
        TurnOffInGameWindows();
        uiPanels[0].SetActive(false);
        uiPanels[1].SetActive(false);
        gamePanel.SetActive(true);
        
        for (int i = 0; i < allLevels.Length; i++)
        {
            allLevels[i].SetActive(false);
            allLevels[levelIndex].SetActive(true);
            allLevels[levelIndex].transform.GetChild(0).gameObject.SetActive(true);
            backButton.transform.localScale = Vector3.zero;
            backButton.transform.DOScale(Vector3.one, .12f).SetEase(Ease.OutBack);

            for (int j = 0; j < levelStartButton.Length; j++)
            {
                levelStartButton[j].transform.localScale = Vector3.one;
                
            }
        }
  
    }

    public void StartCurrentLevel(int levelIndex)
    {
        
        if (levelIndex == 0)
        {
            levelStartButton[levelIndex].transform.localScale = Vector3.zero;
            levelStartButton[levelIndex].transform.DOScale(Vector3.one, .12f).SetEase(Ease.OutBack);
            Invoke(nameof(LevelOneActivities), .2f);
        }

        if (levelIndex == 1)
        {
            levelStartButton[levelIndex].transform.localScale = Vector3.zero;
            levelStartButton[levelIndex].transform.DOScale(Vector3.one, .12f).SetEase(Ease.OutBack);
            Invoke(nameof(LevelTwoActivities), .2f);
        }
       
    }
    
    void LevelOneActivities()
    {
        levelStartButton[0].transform.localScale = Vector3.zero;
        levelStartButton[0].transform.parent.gameObject.SetActive(false);
        InGameWindows[0].SetActive(true);
  
    }
    void LevelTwoActivities()
    {
        levelStartButton[1].transform.localScale = Vector3.zero;
        levelStartButton[1].transform.parent.gameObject.SetActive(false);
        InGameWindows[1].SetActive(true);
    }
    public void BackButtonClicked()
    {
        backButton.transform.localScale = Vector3.one;
        backButton.transform.DOScale(Vector3.zero, .12f).SetEase(Ease.OutBack);
        
        UIManager.instance.PlayButtonClick();
        Invoke(nameof(BackButtonDelays),.2f);
       
    }

    void BackButtonDelays()
    {
        uiPanels[0].SetActive(false);
        uiPanels[1].SetActive(true);
        gamePanel.SetActive(false);
        for (int i = 0; i < allLevels.Length; i++)
        {
            allLevels[i].SetActive(false);
           
        }
    }


    
    
}
