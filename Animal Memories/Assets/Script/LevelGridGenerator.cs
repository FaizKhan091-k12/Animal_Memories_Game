using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using DG.Tweening;

public class LevelGridGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform gridParent;
    public int pairCount = 6; // 6 pairs = 12 cells
    [SerializeField] List<GameObject> cells = new List<GameObject>();
    [SerializeField] private float testRotation_T;
    public float rotaionSpeed;
    public bool isGridGenerated = false;
    public float delay;
    public GameObject buttonBlocker;

 
    
    private void OnEnable()
    {
        GenerateGrid();
        buttonBlocker.SetActive(true);
    }

    private void OnDisable()
    {
        DestroyAllCells();
        isGridGenerated = false;
        cells.Clear();
    }

    private void Update()
    {
        if (isGridGenerated)
        {
            StartClueFlipAfterDelay(delay);
          
        }

       
    }

    public void StartClueFlipAfterDelay(float delay)
    {
        StartCoroutine(DelayedClueFlip(delay));
    }

    IEnumerator DelayedClueFlip(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClueFlip();
    }

    private void ClueFlip()
    {
        testRotation_T += Time.deltaTime * rotaionSpeed;
        testRotation_T = Mathf.Clamp01(testRotation_T);
        if (testRotation_T > .5f)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                cells[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }

               
        }
        for (int i = 0; i < cells.Count; i++)
        {
             
            cells[i].transform.GetChild(0).localEulerAngles = Vector3.Lerp(new Vector3(0,-180,0), Vector3.zero, testRotation_T);
     
        }

        if (testRotation_T > .99f)
        {
            cells[0].transform.GetChild(0).localEulerAngles = new Vector3(0, -180, 0);
            cells[1].transform.GetChild(0).localEulerAngles = new Vector3(0, -180, 0);
            cells[2].transform.GetChild(0).localEulerAngles = new Vector3(0, -180, 0);
            cells[3].transform.GetChild(0).localEulerAngles = new Vector3(0, -180, 0);
            isGridGenerated = false;
            Invoke(nameof(ButtonBlockerDisabled), 1f);
        }
        
    }

    public void ButtonBlockerDisabled()
    {
        buttonBlocker.SetActive(false);
    }


    void GenerateGrid()
    {
        List<int> values = new List<int>();

        // Create pairs
        for (int i = 0; i < pairCount; i++)
        {
            values.Add(i);
            values.Add(i); 
        }

        // Shuffle
        for (int i = 0; i < values.Count; i++)
        {
            int temp = values[i];
            int randomIndex = Random.Range(i, values.Count);
            values[i] = values[randomIndex];
            values[randomIndex] = temp;
        }

        // Spawn cells
        for (int i = 0; i < values.Count; i++)
        {
            GameObject cell = Instantiate(cellPrefab, gridParent);
            cells.Add(cell);
            for (int j = 0; j < cells.Count; j++)
            {
                cells[j].transform.localScale = Vector3.zero;
                StartCoroutine(CellsScaleUp());

            }
            cell.name = "Cell_" + i;

            // Optional: Assign value for match checking
            CellData cellData = cell.GetComponent<CellData>();
            if (cellData != null)
                cellData.value = values[i];

            // Optional: Show number for debug
            Text cellText = cell.GetComponentInChildren<Text>();
            if (cellText != null)
                cellText.text = values[i].ToString(); // For testing
        }

        isGridGenerated = true;
    }

    IEnumerator CellsScaleUp()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            yield return new WaitForSeconds(0.2f);
            cells[i].transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            
        }
    }
    
    void DestroyAllCells()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }
}