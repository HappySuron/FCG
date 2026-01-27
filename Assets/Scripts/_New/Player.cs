using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Valuables")]
    int maxHandCount = 10;

    [Header("CardDeck")]
    public List<GameObject> curPlayersDeck;

    [Header("SceneReferences")]
    public GameObject chosenCardToPlay;

    public Transform handArea;

    public TextMeshProUGUI deckCount;
    public TextMeshProUGUI handCount;
    public GameObject vfxPrefab;

    public void FirstSetUp(){
        
        if (DeckLoader.Instance != null)
        {
            curPlayersDeck = DeckLoader.Instance.LoadDeck("MyFirstDeck");
        }
        else{
            Debug.LogError("No DeckLoader was found");
        }
        UpdateDeckHandCount();
        DrawCards(3);

    }

    private void Start(){
        FirstSetUp();
    }



    private void Awake()
    {
        // Если в сцене уже есть TooltipManager, можно добавить проверку
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void DrawCards(int amount)
    {
        if (curPlayersDeck.Count ==0){
            //Debug.LogError("NO CARD TO DRAW CONSIDER AS LOSE");
            return;
        }
        // Проверка, чтобы в руке не было больше 10 карт
        if (handArea.childCount >= maxHandCount)
        {
            Debug.Log("Невозможно взять карты, так как в руке уже 10 карт.");
            return; // Прерываем выполнение метода, если карт уже 10
        }

        // Если карт меньше 10, начинаем добавлять карты в руку
        for (int i = 0; i < amount; i++)
        {
            if (handArea.childCount >= 10) // Дополнительная проверка на всякий случай
            {
                Debug.Log("В руке не может быть больше 10 карт.");
                break; // Прерываем цикл, если достигнут лимит карт в руке
            }

            GameObject newCard = Instantiate(curPlayersDeck[0], handArea);
            CardBase card = newCard.GetComponent<CardBase>();
            card.SetupCard();
            card.isCardinHand = true;
            card.isPlayerAlly = true;
            curPlayersDeck.RemoveAt(0); // Используем RemoveAt вместо Remove
            UpdateDeckHandCount();
        }
    }

    public void PlayCardToARow(GameObject row){
        if (chosenCardToPlay != null)
        {
            CardBase cardBase = chosenCardToPlay.GetComponent<CardBase>();

            if (cardBase != null)
            {
                // Проверка на соответствие типа карты
                if (row.GetComponent<RowClick>().playerAlliesRow){
                    if (cardBase.cardType == row.GetComponent<RowClick>().thisRowType || cardBase.cardType == CardType.Special)
                    {
                        chosenCardToPlay.transform.SetParent(row.GetComponent<RowClick>().cardContainer, false);
                        //chosenCardToPlay.transform.localPosition = Vector3.zero;

                        // chosenCardToPlay.transform.localScale = Vector3.one * 1.5f;

                        // // Плавное уменьшение масштаба до нормального размера
                        // chosenCardToPlay.transform.DOScale(Vector3.one*1.3f, 0.3f).SetEase(Ease.OutQuad);

                        chosenCardToPlay.GetComponent<CardBase>().EnterAnimation();

                        chosenCardToPlay.GetComponent<CardBase>().ApplyEffect();
                        chosenCardToPlay.GetComponent<CardBase>().isCardinHand = false;
                        Debug.Log("Карта добавлена в ряд: " + gameObject.name);
                        chosenCardToPlay = null;
                        UpdateDeckHandCount();
                        GameManager.Instance.UpdateAllRowStats();
                        GameManager.Instance.goFurther = true;
                    }
                    else
                    {
                        Debug.LogWarning($"Карта типа {cardBase.cardType} не может быть добавлена в ряд типа {row.GetComponent<RowClick>().thisRowType}!");
                    }
                }
                else{
                    Debug.LogWarning($"Это ряд оппонента");
                }
            }
            else
            {
                Debug.LogError("У выбранной карты отсутствует компонент CardBase!");
            }
        }
        else
        {
            Debug.LogWarning("Нет выбранной карты для добавления в ряд!");
        }

    }


    public void UpdateDeckHandCount(){
        deckCount.text = curPlayersDeck.Count.ToString();
        handCount.text = handArea.childCount.ToString();
    }


    public void AddCardToARow(GameObject row, GameObject prefab) 
    {
        // Создаем экземпляр карты из префаба
        GameObject cardInstance = Instantiate(prefab);
        
        // Получаем компонент CardBase из созданной карты
        CardBase cardBase = cardInstance.GetComponent<CardBase>();

        if (cardBase != null)
        {
            cardBase.SetupCard();
            // Проверка на соответствие типа карты
            if (row.GetComponent<RowClick>().playerAlliesRow)
            {
                // Проверяем, можно ли добавить карту в ряд, исходя из типа карты
                if (cardBase.cardType == row.GetComponent<RowClick>().thisRowType || cardBase.cardType == CardType.Special)
                {
                    // Добавляем карту в контейнер ряда
                    cardBase.transform.SetParent(row.GetComponent<RowClick>().cardContainer, false);
                    //cardInstance.transform.localPosition = Vector3.zero;

                    // Увеличиваем масштаб карты при добавлении
                    // cardInstance.transform.localScale = Vector3.one * 1.5f;

                    // // Плавное уменьшение масштаба до нормального размера
                    // cardInstance.transform.DOScale(Vector3.one*1.3f, 0.3f).SetEase(Ease.OutQuad);

                    // Применяем эффекты карты и меняем её статус
                    cardBase.GetComponent<CardBase>().EnterAnimation();
                    cardBase.ApplyEffect();
                    cardBase.isCardinHand = false;
                    
                    Debug.Log("Карта добавлена в ряд: " + row.name);

                    // Обновляем количество карт в руке и силу рядов
                    UpdateDeckHandCount();
                    GameManager.Instance.UpdateAllRowStats();
                }
                else
                {
                    Debug.LogWarning($"Карта типа {cardBase.cardType} не может быть добавлена в ряд типа {row.GetComponent<RowClick>().thisRowType}!");
                }
            }
            else
            {
                Debug.LogWarning("Это ряд оппонента, и карта не может быть размещена в этом ряду.");
            }
        }
        else
        {
            Debug.LogError("У выбранной карты отсутствует компонент CardBase!");
        }
    }

}
