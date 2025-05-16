using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CardFlipper : MonoBehaviour
{

    public static CardFlipper instance;
    public GameObject visualRoot;
    public GameObject frontImage; // 🐶 Animal face
    public GameObject backImage; // 🪵 Wooden frame

    private int value;
    public int Value => value;

    public bool isFlipped = false;
    public bool isMatched = false;

    private Button button;
    private GameManager gameManager;
    private CellData cellData;

    private Quaternion faceDown = Quaternion.Euler(0, 180, 0); // rotated away
    private Quaternion faceUp = Quaternion.Euler(0, 0, 0); // facing camera


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        gameManager = FindObjectOfType<GameManager>();
        cellData = GetComponent<CellData>();
        value = cellData.value;

        visualRoot.transform.rotation = faceDown; // 🪵 Start rotated
        ShowVisuals(true); // 🪵 Show frame

        // Invoke(nameof(FlipUp),5f);

    }


    
    public void FlipToClue()
    {
     
        StartCoroutine(Flip(faceUp, .5f, true));
    }

    public void FlipBack()
    {
        isFlipped = false;
        StartCoroutine(Flip(faceDown, .5f, false)); // Show 🪵 frame
    }

    public void FlipUp()
    {


        StartCoroutine(Flip(faceUp, .5f, true, () =>
        {
            isFlipped = true;
            gameManager.CardClicked(this);
        }));


    }

    public void OnClick()
    {
        if (isFlipped || isMatched || gameManager.IsLocked) return;
        gameManager.IsLocked = true;
        FlipUp();
       
    }

    IEnumerator Flip(Quaternion targetRot, float duration, bool showFront, Action onComplete = null)
    {
      

        Quaternion start = visualRoot.transform.rotation;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / duration);
            visualRoot.transform.rotation = Quaternion.Lerp(start, targetRot, progress);
            yield return null;
        }

        visualRoot.transform.rotation = targetRot;

      

        ShowVisuals(showFront);

        onComplete?.Invoke();
    }



    void ShowVisuals(bool showFront)
    {
        
        if (frontImage) frontImage.SetActive(showFront);
        if (backImage) backImage.SetActive(!showFront);
    }
}
