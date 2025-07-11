using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DescriptionScript : MonoBehaviour
{
    public GameObject[] items;
    public GameObject nextButton;
    public GameObject previousButton;
    private int currentIndex = 0;
    private Button nextButtonComponent;
    private Button previousButtonComponent;
    // Start is called before the first frame update
    void Start()
    {
        nextButtonComponent = nextButton.GetComponent<Button>();
        previousButtonComponent = previousButton.GetComponent<Button>();
        nextButtonComponent.onClick.AddListener(showNextItem);
        previousButtonComponent.onClick.AddListener(showPreviousItem);
        showItem(currentIndex);
        updateButtons();

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void showItem(int index)
    {
        foreach (var item in items)
        {
            item.SetActive(false);
        }
        if (index >= 0 && index < items.Length)
        {
            items[index].SetActive(true);
        }
        updateButtons();
        restoreButtonColors();
    }
    private void showNextItem()
    {
        if (currentIndex < items.Length - 1)
        {
            currentIndex++;
            showItem(currentIndex);
        }
    }
    private void showPreviousItem()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            showItem(currentIndex);
        }
    }
    private void updateButtons()
    {
        nextButton.SetActive(currentIndex < items.Length - 1);
        previousButton.SetActive(currentIndex > 0);
    }
    public void OnButtonPointerEnter(GameObject button)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().color = Color.cyan;
    }
    public void OnButtonPointerExit(GameObject button)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }
    private void restoreButtonColors()
    {
        previousButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        nextButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }
}
