using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardScript;

    private int level = 4;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
