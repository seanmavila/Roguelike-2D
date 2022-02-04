using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public int playerFoodPoints = 100;
    public static GameManager instance = null;
    public BoardManager boardScript;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private GameObject exitImage;
    private GameObject buttonPanel;
    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;
    private bool menuOpen = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;
        exitImage = GameObject.Find("Exit Menu");
        buttonPanel = GameObject.Find("Button Panel");
        exitImage.SetActive(false);
        buttonPanel.SetActive(false);


        levelImage = GameObject.Find("Level Image");
        levelText = GameObject.Find("Level Text").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);


        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {      
        levelText.text = "Afer " + level + " days, you have perished...";
        levelImage.SetActive(true);
        buttonPanel.SetActive(true);
        exitImage.SetActive(false);
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuOpen = !menuOpen;
            exitImage.SetActive(menuOpen);
        }
        
        if (playersTurn || enemiesMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
