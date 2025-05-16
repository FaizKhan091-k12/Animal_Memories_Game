using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float clueTime = 2f;

    private CardFlipper firstCard, secondCard;
    public bool IsLocked { get; set; }

    public bool clueflipDone;
    
    public bool level1;
    public bool level2;
    public bool level3;
    public bool level4;
    public bool level5;

   public List<bool> whichLevelIsThis = new List<bool>();
    
    private void Awake()
    {
        whichLevelIsThis.Add(level1);
        whichLevelIsThis.Add(level2);
        whichLevelIsThis.Add(level3);
        whichLevelIsThis.Add(level4);
        whichLevelIsThis.Add(level5);
        instance = this;
    }

    void Start()
    {
   
        StartCoroutine(CluePhaseThenPlay());
    }

    IEnumerator CluePhaseThenPlay()
    {
        IsLocked = true;

        CardFlipper[] cards = FindObjectsOfType<CardFlipper>();

        // Flip all cards to front (show animal)
        foreach (var card in cards)
        {
         
            card.FlipToClue();
        }

        yield return new WaitForSeconds(clueTime);

        // Flip all cards back (show frame)
        foreach (var card in cards)
        {
            
            card.FlipBack();
        }

        yield return new WaitForSeconds(0.3f);
        IsLocked = false;
    }

    public void CardClicked(CardFlipper card)
    {
        if (firstCard == null)
        {
            firstCard = card;
            IsLocked = false; // allow second click
        }
        else if (secondCard == null)
        {
            secondCard = card;

            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        IsLocked = true;
        yield return new WaitForSeconds(0.5f);

        if (firstCard.Value == secondCard.Value)
        {
            Debug.Log($"Matched: {firstCard.Value} & {secondCard.Value} at {Time.time}");
            firstCard.isMatched = true;
            secondCard.isMatched = true;
            CheckAllMatched(); // 👈 Call this next
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();
        }

        firstCard.isFlipped = false;
        secondCard.isFlipped = false;

        firstCard = null;
        secondCard = null;

        yield return new WaitForSeconds(0.3f);
        IsLocked = false;
    }
    
    void CheckAllMatched()
    {
        CardFlipper[] cards = FindObjectsOfType<CardFlipper>();
        foreach (var card in cards)
        {
            if (!card.isMatched)
            {
                return; // At least one is not matched yet
            }
        }

        if (level1)
        {
            Debug.Log("🎉 Level 1 Completed");
            LevelBehaviour.instance.UnlockNextLevel(1);
        }

        if (level2)
        {
            Debug.Log("🎉 Level 2 Completed");
        }

        if (level3)
        {
            Debug.Log("🎉 Level 3 Completed");
        }

        if (level4)
        {
            Debug.Log("🎉 Level 4 Completed");
        }

        if (level5)
        {
            Debug.Log("🎉 Level 5 Completed");
        }

        Debug.Log("🎉 All cards matched!");
        // Optional: Trigger win screen, sound, etc.
    }   
}