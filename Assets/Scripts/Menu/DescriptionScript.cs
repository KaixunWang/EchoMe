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
        AddButtonEventTrigger(nextButton);
        AddButtonEventTrigger(previousButton);
        showItem(currentIndex);
        updateButtons();

    }
    private void AddButtonEventTrigger(GameObject buttonObject)
    {
        EventTrigger eventTrigger = buttonObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = buttonObject.AddComponent<EventTrigger>();
        }

        // PointerEnter
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((data) => OnButtonPointerEnter(buttonObject));
        eventTrigger.triggers.Add(pointerEnterEntry);

        // PointerExit
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((data) => OnButtonPointerExit(buttonObject));
        eventTrigger.triggers.Add(pointerExitEntry);
    }

    // Update is called once per frame
    void Update()
    {
        // showItem(currentIndex);
        // updateButtons();
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
        // restoreButtonColors();
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
        Button buttonComponent = button.GetComponent<Button>();
        ColorBlock colorBlock = buttonComponent.colors;
        colorBlock.normalColor = Color.cyan;
        buttonComponent.colors = colorBlock;
    }
    public void OnButtonPointerExit(GameObject button)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        Button buttonComponent = button.GetComponent<Button>();
        ColorBlock colorBlock = buttonComponent.colors;
        colorBlock.normalColor = Color.white;
        buttonComponent.colors = colorBlock;
    }
    // private void restoreButtonColors()
    // {
    //     previousButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    //     nextButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    //     ColorBlock previousColorBlock = previousButtonComponent.colors;
    //     previousColorBlock.normalColor = Color.white;
    //     previousButtonComponent.colors = previousColorBlock;
    //     ColorBlock nextColorBlock = nextButtonComponent.colors;
    //     nextColorBlock.normalColor = Color.white;
    //     nextButtonComponent.colors = nextColorBlock;
    // }
}
