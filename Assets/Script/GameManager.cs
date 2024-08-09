using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Card")]
    [SerializeField] private List<Button> buttonList = new List<Button>();
    [SerializeField] private Sprite backgroungCardImage;

    [Header("Image_Matching")]
    [SerializeField] private Sprite[] matchingCardImages;
    [SerializeField] private List<Sprite> matchingImages = new List<Sprite>();

    //cache
    private int countSelect;
    private int countCorrectSelect;
    private int gameSelect;
    private int firstSelectIndex, secondSelectIndex;
    private string firstSelectname, secondSelectname;
    private bool firstSelect, secondSelect;

    private void Awake()
    {
        matchingCardImages = Resources.LoadAll<Sprite>("Sprite/Animal_1");
    }

    private void Start()
    {
        GetButtons();
        AddListeners();
        AddImageMatching();
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
        if (!firstSelect)
        {
            firstSelect = true;
            firstSelectIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstSelectname = matchingImages[firstSelectIndex].name;
            buttonList[firstSelectIndex].image.sprite = matchingImages[firstSelectIndex];


        } else if (!secondSelect)
        {
            secondSelect = true;
            secondSelectIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondSelectname = matchingImages[secondSelectIndex].name;
            buttonList[secondSelectIndex].image.sprite = matchingImages[secondSelectIndex];

            if (firstSelectname == secondSelectname)
            {
                Debug.Log("Matching!!");
            } else if (firstSelectname != secondSelectname)
            {
                Debug.Log("unmatching!!");
            }
        }
    }

    private void AddListeners()
    {
        foreach (Button button in buttonList)
        {
            button.onClick.AddListener(() => PickAcard());
        }
    }

    private void AddImageMatching()
    {
        int loop = buttonList.Count;
        int index = 0;

        for (int i = 0; i < loop; i++)
        {
            if (index == loop / 2)
            {
                index = 0;
            }
            matchingImages.Add(matchingCardImages[index]);
            index++;
        }
    }
    #endregion
}
