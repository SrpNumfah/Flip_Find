using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttonList = new List<Button>();
    private void Start()
    {
        GetButtons();
    }

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
            } 
            else if (button == null)
            {
                Debug.Log("Doesn't have any button");
            }
        }
    }
}
