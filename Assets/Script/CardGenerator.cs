using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Card.UI
{
    public class CardGenerator : MonoBehaviour
    {
        #region Singleton
        public static CardGenerator instance;
        #endregion

        [SerializeField, TabGroup("Card Genarator")] private RectTransform cardField;
        [SerializeField, TabGroup("Card Genarator")] private GameObject cardButtonPrefabs;


        //cache
        private float flipcardDuration = 1;
        private int setRows;
        private int setColumns;

        private void Awake()
        {
            
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else if (instance != null)
            {
                Destroy(gameObject);
            }


            int[,] cardLayouts = new int[,] 
            {
                {2,2},
                {3,3},
                {4,4},
                {5,6},
            };

            int randomIndex = Random.Range(0, cardLayouts.GetLength(0));
            int rows = cardLayouts[randomIndex, 0];
            int columns = cardLayouts[randomIndex, 1];

            setRows = rows;
            setColumns = columns;
            GenerateCard(setRows, setColumns);
        }

        #region Public
        public int OnSetRows() => setRows;
        public int OnSetColumns() => setColumns;
        public void GenerateCard(int rows, int columns)
        {
            foreach (Transform card in cardField)
            {
                Destroy(card.gameObject);
            }


            float cardWidth = cardField.rect.width / columns;
            float cardHeight = cardField.rect.height / rows;

            float cardPositionX = -(cardField.rect.width / 2 - cardWidth / 2);
            float cardPositionY = cardField.rect.height / 2 - cardHeight / 2;

            int counter = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    GameObject newCard = Instantiate(cardButtonPrefabs, cardField);
                    newCard.name = "" + counter;
                    counter++;
                    RectTransform newCardRect = newCard.GetComponent<RectTransform>();

                    newCardRect.sizeDelta = new Vector2(cardWidth, cardHeight);

                    float cardSortX = (cardPositionX + column) * cardWidth;
                    float cardSortY = (cardPositionY - row) * cardHeight;
                    newCardRect.anchoredPosition = new Vector2(cardSortX, cardSortY);
                }
            }

        }
        public void PlayFlipAnimation()
        {
            StartCoroutine(FlipDurationTime());
        }

        #endregion


        #region Private
        private IEnumerator FlipDurationTime()
        {
            Quaternion startRotaion = cardButtonPrefabs.transform.rotation;
            Quaternion endRotation = Quaternion.Euler(0, 180, 0);

            float time = 0;

            if (time < flipcardDuration)
            {
                cardButtonPrefabs.transform.rotation = Quaternion.Slerp(startRotaion, endRotation, time / flipcardDuration);
                time += flipcardDuration;
                yield return null;
            }

            cardButtonPrefabs.transform.rotation = endRotation;

        }

        #endregion
    }
}

