using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using Card.UI;
using Sirenix.OdinInspector;


namespace Card.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField, TabGroup("Card Button")] private List<Button> buttonList = new List<Button>();
        [SerializeField, TabGroup("Card Button")] private Sprite backgroungCardImage;
        [SerializeField, TabGroup("Card Button")] private Button restartButton;
        [SerializeField, TabGroup("Card Button")] private Button exitButton;

        [SerializeField, TabGroup("Image_Matching")] private Sprite[] matchingCardImages;
        [SerializeField, TabGroup("Image_Matching")] private List<Sprite> matchingImages = new List<Sprite>();

        [SerializeField, TabGroup("UI")] private TMP_Text scoreText;
        [SerializeField, TabGroup("UI")] private TMP_Text heighScoreText;
        [SerializeField, TabGroup("UI")] private TMP_Text countTurnText;
        [SerializeField, TabGroup("UI")] private Slider timeSlider;
        [SerializeField, TabGroup("UI")] private GameObject gameOver;

        [SerializeField, TabGroup("Sound")] private AudioSource correctSound;
        [SerializeField, TabGroup("Sound")] private AudioSource wrongSound;
        [SerializeField, TabGroup("Sound")] private AudioSource gameOverSound;



        //cache
        private float time = 15;
        private int rows;
        private int columns;
        private int countSelect;
        private int countCorrectSelect = 0;
        private int gameSelect;
        private int firstSelectIndex, secondSelectIndex;
        private string firstSelectname, secondSelectname;
        private bool firstSelect, secondSelect;
        private const string CurrentScore = "currentScore";

        private void Awake()
        {
            matchingCardImages = Resources.LoadAll<Sprite>("Sprite/Animal_1");
        }

        private void Start()
        {
            OnResetNewCards();
            StartCoroutine(StartCountDown());
            gameSelect = matchingImages.Count / 2;
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

            if (buttonList.Count == 0) return;

            foreach (Button button in buttonList)
            {
                if (index == loop / 2)
                {
                    index = 0;
                }
                matchingImages.Add(matchingCardImages[index]);
                index++;
            }
            Shuffle(matchingImages);
        }
        private void PickAcard()
        {
            if (!firstSelect)
            {
                CardGenerator.instance.PlayFlipAnimation();
                firstSelect = true;
                firstSelectIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
                firstSelectname = matchingImages[firstSelectIndex].name;
                buttonList[firstSelectIndex].image.sprite = matchingImages[firstSelectIndex];
            }
            else if (!secondSelect)
            {
                CardGenerator.instance.PlayFlipAnimation();
                secondSelect = true;
                secondSelectIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
                secondSelectname = matchingImages[secondSelectIndex].name;
                buttonList[secondSelectIndex].image.sprite = matchingImages[secondSelectIndex];

                countSelect++;
                countTurnText.text = "Turns" + ":" + countSelect;
                StartCoroutine(CheckeIfMatching());

            }
        }
        private void CheckIftheGameIsFinished()
        {
            if (countCorrectSelect == gameSelect)
            {
                gameSelect = 0;
                StartCoroutine(RestartGame());
            }
        }

        private void UpdateDisPlay()
        {
            int setCurrenScore = PlayerPrefs.GetInt(CurrentScore);
            heighScoreText.text = "Heigh score " + ":" + setCurrenScore;
        }

        private void Shuffle(List<Sprite> spriteList)
        {
            for (int i = 0; i < spriteList.Count; i++)
            {
                Sprite tempSprite = spriteList[i];
                int randomIndex = Random.Range(0, spriteList.Count);
                spriteList[i] = spriteList[randomIndex];
                spriteList[randomIndex] = tempSprite;
            }
        }

        private void OnResetNewCards()
        {
            matchingCardImages = Resources.LoadAll<Sprite>("Sprite/Animal_1");
            GetButtons();
            AddImageMatching();
            AddListeners();
            UpdateDisPlay();
            scoreText.text = "Matching" + ":" + countCorrectSelect;
            countSelect = 0;
            countTurnText.text = "Turns" + ":" + countSelect;
        }

        private IEnumerator CheckeIfMatching()
        {
            yield return new WaitForSeconds(0.5f);
            if (firstSelectname == secondSelectname)
            {
                correctSound.Play();
                buttonList[firstSelectIndex].image.color = new Color(0, 0, 0, 0);
                buttonList[secondSelectIndex].image.color = new Color(0, 0, 0, 0);

               
                buttonList[firstSelectIndex].interactable = false;
                buttonList[secondSelectIndex].interactable = false;

                countCorrectSelect++;
                scoreText.text = "Matching" + ":" + countCorrectSelect;
                PlayerPrefs.SetInt(CurrentScore, countCorrectSelect);

                yield return new WaitForSeconds(0.5f);
                CheckIftheGameIsFinished();

            }
            else if (firstSelectname != secondSelectname)
            {
                wrongSound.Play();
                buttonList[firstSelectIndex].image.sprite = backgroungCardImage;
                buttonList[secondSelectIndex].image.sprite = backgroungCardImage;
            }

            firstSelect = secondSelect = false;
        }

        private IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(0.1f);
            rows = CardGenerator.instance.OnSetRows();
            columns = CardGenerator.instance.OnSetColumns();
            CardGenerator.instance.GenerateCard(rows, columns);
            
            yield return new WaitForSeconds(0.1f);
            OnResetNewCards();

        }

        private IEnumerator StartCountDown()
        {
            float currentTime = time;
            timeSlider.maxValue = currentTime; 

            while (currentTime > 0)
            {
                timeSlider.value = currentTime;
                yield return new WaitForSeconds(1f);
                currentTime--;
            }
            timeSlider.value = 0;
            OnTimeUp();
        }

        private void OnTimeUp()
        {
            gameOver.SetActive(true);
            gameOverSound.Play();
            exitButton.onClick.AddListener(() => Exit());
            Debug.Log("Game over");
        }

        private void Exit()
        {
            Application.Quit();
        }


        #endregion
    }

}
