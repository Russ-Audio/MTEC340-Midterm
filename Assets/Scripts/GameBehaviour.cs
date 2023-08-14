using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameBehaviour : MonoBehaviour
{

    public static GameBehaviour Instance;

    [SerializeField] Player[] _players = new Player[2];
    [SerializeField] AudioClip roundOver;
    [SerializeField] AudioClip countDown;
    [SerializeField] TextMeshProUGUI pauseGUI;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject quitButton;

    [SerializeField] TextMeshProUGUI player1Health;
    [SerializeField] TextMeshProUGUI player2Health;
    [SerializeField] TextMeshProUGUI scoreBoard;
    [SerializeField] TextMeshProUGUI playInstruction;


    [SerializeField] GameObject[] Player_1 = new GameObject[2];

    private AudioSource audioSource;

    private int Health_1;
    private int Health_2;

    private int Score_1;
    private int Score_2;

    public GameState _state;
    public GameState storeState;

    private Scene scene;

    

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        ResetGame();

        audioSource = GetComponent<AudioSource>();
        _state = GameState.Start;
    }


    // Update is called once per frame
    void Update()
    {


        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            _state = GameState.Start;
        }

        if(Input.GetKeyDown(KeyCode.Space) && _state == GameState.GameOver)
        {
            StopCoroutine(ResetCoroutine());
            ResetGame();
        }

        if(Input.GetKeyDown(KeyCode.Space) && _state == GameState.Start)
        {
            SceneManager.LoadScene(1);
            _state = GameState.Warmup;

            foreach (Player p in _players)
            {
                p.Life = 1;
                p.Point = 2;
            }

            Health_1 = 1;
            Health_2 = 1;

        }

        if(_state != GameState.Start && _state != GameState.Pause && Input.GetKeyDown(KeyCode.P))
        {
            storeState = _state;
            pauseGUI.enabled = true;
            restartButton.SetActive(true);
            quitButton.SetActive(true);
            Time.timeScale = 0;
            _state = GameState.Pause;
        }
        else if(_state != GameState.Start && _state == GameState.Pause && Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1;
            pauseGUI.enabled = false;
            restartButton.SetActive(false);
            quitButton.SetActive(false);
            _state = storeState;
        }
    }

    public void PlayerDamage (int player, int damage)
    {
        Debug.Log("HIT" + " " + player);

        _players[player - 1].Life -= damage;

        if((player - 1) == 0)
        {
            Health_1 -= damage;
        }
        else
        {
            Health_2 -= damage;
        }

        player1Health.text = $"Health: {Health_1.ToString()}";
        player2Health.text = $"Health: {Health_2.ToString()}";

        if (_players[player - 1].Life <= 0)
        {
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                StartCoroutine(Ready());
                Player_1[player - 1].SetActive(false);
                audioSource.PlayOneShot(countDown, 0.7f);
                player1Health.enabled = false;
                player2Health.enabled = false;
                return;
            }

            if((player - 1) == 0)
            {
                Score_2++;
            }
            else
            {
                Score_1++;
            }

            if(Score_1 == 2)
            {
                StartCoroutine(GameEnding(1));
            }

            else if (Score_2 == 2)
            {
                StartCoroutine(GameEnding(2));
            }

            player1Health.enabled = false;
            player2Health.enabled = false;
            Player_1[player - 1].SetActive(false);

            scoreBoard.text = $"{Score_1.ToString()} - {Score_2.ToString()}\n<size=20%>Best of 3";

            scoreBoard.enabled = true;

            //play audio round ended
            GameOver(player);
        }
    }

    IEnumerator GameEnding(int playernumber)
    {
        yield return new WaitForSeconds(0.3f);
        scoreBoard.text = $"<size=50%>PLAYER {playernumber.ToString()} WINS";
    }

    void GameOver(int winner)
    {
        audioSource.PlayOneShot(roundOver, 0.6f);
        Time.timeScale = 0.2f;

        _state = GameState.RoundOver;

        _players[winner - 1].Point -= 1;

        Debug.Log($"Player Point = { _players[winner - 1].Point}");

        if(_players[winner - 1].Point <= 0)
        {
            Time.timeScale = 0.1f;
            _state = GameState.GameOver;
            StartCoroutine(ResetCoroutine());
            //Debug.Log("Game Ended");
        }
        else
        {
            StartCoroutine(NextRound());
            //Debug.Log("Game not Ended");
        }
    }

    IEnumerator Ready()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
        foreach (Player p in _players)
        {
            p.Life = 3;
            p.Point = 2;
        }

        Health_1 = 3;
        Health_2 = 3;

        Score_1 = 0;
        Score_2 = 0;

        player1Health.text = $"Health: {Health_1.ToString()}";
        player2Health.text = $"Health: {Health_2.ToString()}";

        player1Health.enabled = true;
        player2Health.enabled = true;
        playInstruction.enabled = true;
        Invoke(nameof(DisableInstruc), 7f);

        _state = GameState.Play;
    }

    void DisableInstruc()
    {
        playInstruction.enabled = false;
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        foreach (Player p in _players)
        {
            p.Life = 3;
        }

        Health_1 = 3;
        Health_2 = 3;

        player1Health.text = $"Health: {Health_1.ToString()}";
        player2Health.text = $"Health: {Health_2.ToString()}";

        player1Health.enabled = true;
        player2Health.enabled = true;

        scoreBoard.enabled = false;

        Time.timeScale = 1;
    }

    IEnumerator ResetCoroutine()
    {
        yield return new WaitForSeconds(1f);
        ResetGame();
    }

    public void UISpawn()
    {
        player1Health.text = $"Health: {Health_1.ToString()}";
        player2Health.text = $"Health: {Health_2.ToString()}";


        player1Health.enabled = true;
        player2Health.enabled = true;

    }

    public void ResetGame()
    {
        foreach (Player p in _players)
        {
            p.Life = 3;
            p.Point = 2;
        }
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Debug.Log("RESET");
        pauseGUI.enabled = false;
        restartButton.SetActive(false);
        quitButton.SetActive(false);
        player1Health.enabled = false;
        player1Health.enabled = false;
        scoreBoard.enabled = false;
        Score_1 = 0;
        Score_2 = 0;
    }

    public void SetPlayerRef(int PlayerIndex, GameObject Player)
    {
        Player_1[PlayerIndex - 1] = Player;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
