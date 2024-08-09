using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Card")]
    [SerializeField] private List<Button> buttonList = new List<Button>();
    [SerializeField] private Sprite backgroungCardImage;

    private void Start()
    {
        GetButtons();
        AddListeners();
    }

    #region Private
    private void GetButtons()
    {
        GameObject[] cardButton = GameObject.FindGameObjectsWithTag("Cards");

        if (cardButton.Length == 0) return;
       
        buttonList.Clear();

        foreach (GameObject card in cardButton)
        {
            Button button = card.GetComponent<Button>();

            if (button != null)
            {
                buttonList.Add(button);
                button.image.sprite = backgroungCardImage;
            } 
            else if (button == null)
            {
                Debug.Log("Doesn't have any button");
            }
        }
    }

    private void PickAcard()
    {
        string currentButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(currentButton);
    }

    private void AddListeners()
    {
        foreach (Button button in buttonList)
        {
            button.onClick.AddListener(() => PickAcard());
        }
    }
    #endregion
}
