using System;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float clueTime = 2f;

    private CardFlipper firstCard, secondCard;
    public bool IsLocked { get; set; }

    public bool clueflipDone;

    private void Awake()
    {
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

        Debug.Log("🎉 All cards matched!");
        // Optional: Trigger win screen, sound, etc.
    }   
}