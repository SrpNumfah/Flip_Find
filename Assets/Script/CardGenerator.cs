using UnityEngine;

namespace Card.UI
{
    public class CardGenerator : MonoBehaviour
    {
        [SerializeField] private RectTransform cardField;
        [SerializeField] private GameObject cardButtonPrefabs;

        private void Start()
        {
            int[,] cardLayout = new int[,]
            {
                {2,2},
                {3,3},
                {5,6}
            };

            int randomCardLayoutIndex = Random.Range(0,cardLayout.GetLength(0));
            int randomCardLayoutRows = cardLayout[randomCardLayoutIndex, 0];
            int randomCardLayoutColumns = cardLayout[randomCardLayoutIndex, 1];

            Debug.Log(randomCardLayoutIndex.ToString());
            GenerateCard(randomCardLayoutRows, randomCardLayoutColumns);
        }
        #region Private
        private void GenerateCard(int rows, int columns)
        {
            foreach (Transform card in cardField)
            {
                Destroy(card.gameObject);
            }

            float cardWidth = cardField.rect.width / columns;
            float cardHeight = cardField.rect.height / rows;

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    GameObject newCard = Instantiate(cardButtonPrefabs, cardField);
                    RectTransform newCardRect = newCard.GetComponent<RectTransform>();

                    newCardRect.sizeDelta = new Vector2(cardWidth, cardHeight);

                    newCardRect.anchoredPosition = new Vector2((column - columns / 2f + 0.5f) * cardHeight, (row - rows / 2f + 0.5f) * cardWidth);
                }
            }

        }
        #endregion
    }
}

