using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
public enum GameState
{
    StartGame,
    PlayerTurn,
    Intermission,
    OpponentTurn,
    EndRound,
    GameOver
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Синглтон для глобального доступа


    public GameState currentGameState { get; private set; }  // Текущее состояние игры
    public int roundNumber { get; private set; } = 1;        // Номер текущего раунда


    private bool gameInProgress = false;


    [Header("SceneReferences")]

    public Image turnStaff;

    public TextMeshProUGUI targetTokensPlayerObj;
    public TextMeshProUGUI targetTokensOpponentObj;

    public TextMeshProUGUI curTokensPlayerObj;
    public TextMeshProUGUI curTokensOpponentObj;

    public List<GameObject> playerRows;

    public List<GameObject> opponentRows;

    public TextMeshProUGUI[] playerRowPowerDisplays;
    public TextMeshProUGUI[] opponentRowPowerDisplays;

    [Header("Arts")]

    public Sprite turnOp;
    public Sprite turnPl;
    public Sprite turnNo;

    [Header("Valuables")]
    public int targetTokensPlayer;
    public int targetTokensOpponent;

    public int curTokensPlayer;
    public int curTokensOpponent;

    public bool goFurther= false;

    [Header("Row Power Values")]
    public int[] playerRowPowers;
    public int[] opponentRowPowers;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;  // Устанавливаем синглтон
            DontDestroyOnLoad(gameObject);  // Сохраняем объект при смене сцен (если нужно)
        }
        else
        {
            Destroy(gameObject);  // Удаляем дублирующийся GameManager
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        gameInProgress = true;
        currentGameState = GameState.StartGame;
        roundNumber = 1; // Сбрасываем номер раунда при новой игре

        Debug.Log("Player draw 5");
        Debug.Log("Opponent draw 5");

        UpdateTargetValues(true, 25);
        UpdateTargetValues(false, 30);
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (gameInProgress)
        {
            Debug.Log($"Раунд {roundNumber} начинается!");
            yield return StartCoroutine(PlayerTurn());
            yield return StartCoroutine(Intermission());
            yield return StartCoroutine(OpponentTurn());
            yield return StartCoroutine(EndRound());

            roundNumber++; // Увеличиваем номер раунда после каждого цикла
        }

        currentGameState = GameState.GameOver;
    }

    private IEnumerator PlayerTurn()
    {
        currentGameState = GameState.PlayerTurn;
        Debug.Log($"Ход игрока (Раунд {roundNumber})");

        yield return StartCoroutine(FlipTurnStaff(turnPl));

        Player.Instance.DrawCards(1);
        //Debug.Log("Player draw 1");
        yield return new WaitUntil(() => goFurther);
        Debug.Log("Going further");
        goFurther = false;



        //yield return StartCoroutine(player.PlayCard());
        yield return new WaitForSeconds(1);
    }

    private IEnumerator Intermission()
    {
        
        currentGameState = GameState.Intermission;
        Debug.Log($"Междуход (Раунд {roundNumber})");

        yield return StartCoroutine(FlipTurnStaff(turnNo));


        //yield return new WaitUntil(() => goFurther);
        Debug.Log("Going further");





        goFurther = false;
        yield return new WaitForSeconds(2);
    }

    private IEnumerator OpponentTurn()
    {
        currentGameState = GameState.OpponentTurn;
        Debug.Log($"Ход противника (Раунд {roundNumber})");

        yield return StartCoroutine(FlipTurnStaff(turnOp));

        Opponent.Instance.PlayOpponentTurn();

        Opponent.Instance.DrawCards(1);
        //Debug.Log("Opponent draw 1");
        yield return new WaitUntil(() => goFurther);
        Debug.Log("Going further");
        goFurther = false;





        //yield return StartCoroutine(opponent.PlayCard());

        yield return new WaitForSeconds(1);
    }

    private IEnumerator EndRound()
    {

        currentGameState = GameState.EndRound;

        Debug.Log($"Конец раунда {roundNumber} - Подсчёт очков");
        
        yield return StartCoroutine(FlipTurnStaff(turnNo));
        yield return CountEndOfTurnTokensPlayer();
        yield return CountEndOfTurnTokensOpponent();

        if (curTokensPlayer>=targetTokensPlayer) {
            Debug.Log("WIN");
            gameInProgress = false;
        }
        if (curTokensPlayer>=targetTokensPlayer) {
            Debug.Log("LOSE");
            gameInProgress = false;
        }
        // тут логику проверки очков
        //yield return new WaitUntil(() => goFurther);

        Debug.Log("Going further");
        goFurther = false;
        //UpdateRowStatsAlly ();

        yield return new WaitForSeconds(2);
    }


    private IEnumerator FlipTurnStaff(Sprite newSprite)
    {
        // Animation for coin flip
        float flipDuration = 0.5f;  // Общая длительность анимации переворота

        // Первая половина переворота: уменьшение масштаба по X до 0 (эффект сжатия)
        turnStaff.transform.DOScaleX(0, flipDuration / 2f).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(flipDuration / 2f);  // Ждем завершения первой половины

        // Меняем спрайт, когда жезл "исчез"
        turnStaff.sprite = newSprite;

        // Вторая половина переворота: увеличение масштаба по X обратно до 1
        turnStaff.transform.DOScaleX(1, flipDuration / 2f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(flipDuration / 2f);  // Ждем завершения второй половины
    }

    public void UpdateTargetValues(bool isPlayer, int newTargetAmount)
    {
        if (isPlayer)
        {
            if (targetTokensPlayer != newTargetAmount)
            {
                targetTokensPlayer = newTargetAmount;
                targetTokensPlayerObj.text = targetTokensPlayer.ToString();
            }
            Debug.Log("Player target updated");
        }
        else
        {
            if (targetTokensOpponent != newTargetAmount)
            {
                targetTokensOpponent = newTargetAmount;
                targetTokensOpponentObj.text = targetTokensOpponent.ToString();
            }
            Debug.Log("Opponent target updated");
        }
    }

    public void UpdateRowStatsAlly() 
    {
        for (int i = 0; i < playerRows.Count; i++)
        {
            int totalPower = 0;

            foreach (Transform child in playerRows[i].transform)
            {
                CardBase card = child.GetComponent<CardBase>();
                if (card != null)
                {
                    totalPower += card.cardPowerCurrent;
                }
            }

            // Обновляем значение в массиве
            playerRowPowers[i] = totalPower;

            // Отображаем силу в UI
            if (i < playerRowPowerDisplays.Length)
            {
                playerRowPowerDisplays[i].text = totalPower.ToString();
            }
        }
    }


    public void UpdateRowStatsOpponent() 
    {
        for (int i = 0; i < opponentRows.Count; i++)
        {
            int totalPower = 0;

            foreach (Transform child in opponentRows[i].transform)
            {
                CardBase card = child.GetComponent<CardBase>();
                if (card != null)
                {
                    totalPower += card.cardPowerCurrent;
                }
            }

            // Обновляем значение в массиве
            opponentRowPowers[i] = totalPower;

            // Отображаем силу в UI
            if (i < opponentRowPowerDisplays.Length)
            {
                opponentRowPowerDisplays[i].text = totalPower.ToString();
            }
        }
    }

    public void UpdateAllRowStats(){
        UpdateRowStatsAlly();
        UpdateRowStatsOpponent();
    }



    public void ChangeCurTokens(bool isPlayer, int amount){
        if (isPlayer){
            curTokensPlayer += amount;
            // Убедитесь, что количество токенов не опустится ниже 0
            curTokensPlayer = Mathf.Max(0, curTokensPlayer);
            curTokensPlayerObj.text = curTokensPlayer.ToString();
        }
        else {
            curTokensOpponent += amount;
            // Убедитесь, что количество токенов не опустится ниже 0
            curTokensOpponent = Mathf.Max(0, curTokensOpponent);
            curTokensOpponentObj.text = curTokensOpponent.ToString();
        }
    }

public IEnumerator CountEndOfTurnTokensPlayer()
{
    int totalToAdd = 0;

    // Для каждого ряда делаем задержку
    for (int i = 0; i < playerRowPowers.Length; i++)
    {
        int rowPower = playerRowPowers[i];
        totalToAdd += rowPower;

        // Плавное добавление токенов с задержкой для каждого ряда
        yield return StartCoroutine(AddTokensOverTime(true, rowPower, i));
    }
    // Теперь добавим оставшиеся токены (если нужно) 
}

    public IEnumerator CountEndOfTurnTokensOpponent()
    {
        int totalToAdd = 0;

        // Для каждого ряда делаем задержку
        for (int i = 0; i < opponentRowPowers.Length; i++)
        {
            int rowPower = opponentRowPowers[i];
            totalToAdd += rowPower;

            // Плавное добавление токенов с задержкой для каждого ряда
            yield return StartCoroutine(AddTokensOverTime(false, rowPower, i));
        }

        // Теперь добавим оставшиеся токены (если нужно)
    }

    private IEnumerator AddTokensOverTime(bool isPlayer, int rowPower, int rowIndex)
    {
        int step = 1; // Сколько токенов добавлять за раз
        float delay = 0.2f; // Задержка между добавлениями токенов

        // Выводим в консоль, какой ряд сейчас обрабатываем
        Debug.Log($"Добавление токенов для ряда {rowIndex + 1}, мощность ряда: {rowPower}");

        // Добавляем токены по одному с задержкой
        for (int i = 0; i < rowPower; i += step)
        {
            ChangeCurTokens(isPlayer, step); // Добавляем токен
            yield return new WaitForSeconds(delay); // Задержка
        }
    }

}
