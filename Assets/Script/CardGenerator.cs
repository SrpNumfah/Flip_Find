using UnityEngine;

namespace Card.UI
{
    public class CardGenerator : MonoBehaviour
    {
        [SerializeField] private RectTransform cardField;
        [SerializeField] private GameObject cardButtonPrefabs;
       

        private void Awake()
        {
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

            GenerateCard(rows, columns);
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

            float cardPositionX = -(cardField.rect.width / 2 - cardWidth / 2);
            float cardPositionY = cardField.rect.height / 2 - cardHeight / 2;

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    GameObject newCard = Instantiate(cardButtonPrefabs, cardField);
                    RectTransform newCardRect = newCard.GetComponent<RectTransform>();

                    newCardRect.sizeDelta = new Vector2(cardWidth, cardHeight);

                    float cardSortX = (cardPositionX + column) * cardWidth;
                    float cardSortY = (cardPositionY - row) * cardHeight;
                    newCardRect.anchoredPosition = new Vector2(cardSortX,cardSortY);
                }
            }

        }
        #endregion
    }
}

