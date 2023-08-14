using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] Player_1 = new GameObject[3];
    [SerializeField] GameObject[] Player_2 = new GameObject[3];
    [SerializeField] GameObject Wall;
    [SerializeField] AudioClip MovementAudio;
    [SerializeField] AudioClip WallBreakAudio;
    [SerializeField] AudioClip[] WallHitAudio = new AudioClip[3];

    [SerializeField] TextMeshProUGUI instructionPlayer1;
    [SerializeField] TextMeshProUGUI instructionPlayer2;
    [SerializeField] GameObject confirm;
    [SerializeField] TextMeshProUGUI instructions;
    [SerializeField] TextMeshProUGUI tip;

    public TextMeshProUGUI WelcomeText;

    private AudioSource audioSource;

    private int NextCheckIndex1 = 1;
    private int NextCheckIndex2 = 1;
    private int WallHealth;
    private bool NextTutorial;
    private bool Tutorial1Ready;
    private bool Player1Done;
    private bool Player2Done;
    public bool GetReady;

    // Start is called before the first frame update
    void Start()
    {
        NextTutorial = false;
        Tutorial1Ready = false;
        Player1Done = false;
        Player2Done = false;
        GetReady = false;
        NextCheckIndex1 = 1;
        NextCheckIndex2 = 1;
        WallHealth = 10;
        Player_1[1].SetActive(false);
        Player_1[2].SetActive(false);
        Player_2[1].SetActive(false);
        Player_2[2].SetActive(false);
        confirm.SetActive(false);

        WelcomeText.text = instructions.text;

        audioSource = GetComponent<AudioSource>();

        Time.timeScale = 0.1f;
        instructionPlayer1.enabled = true;
        instructionPlayer2.enabled = true;
        instructions.enabled = true;
        StartCoroutine(ReadTime());
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1 && Tutorial1Ready == false)
        {
            Player_1[0].SetActive(true);
            Player_2[0].SetActive(true);
            WallHealth = 10;
            StartCoroutine(ReadTime());
            Tutorial1Ready = true;
        }

        if(Player1Done && Player2Done)
        {
            Time.timeScale = 0.1f;
            StartCoroutine(ReadTime());
            instructions.text = "WELL DONE!!! NOW, THE WALL SHOULD BE BREAKABLE... \n SHOOT IT DOWN & TAKE DOWN YOUR ENEMY TO START THE REAL BATTLE \n \n REMEMBER: THEY FIGHT BACK!!!";
            instructions.enabled = true;
            GetReady = true;
            Player1Done = false;
            Player2Done = false;
        }

        if (NextTutorial && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            NextTutorial = false;
            confirm.SetActive(false);
            instructions.enabled = false;
        }


    }

    public void NextCheckPointPlayer1()
    {
        Player_1[NextCheckIndex1 - 1].SetActive(false);
        audioSource.PlayOneShot(MovementAudio);

        if(NextCheckIndex1 < 3)
        {
            Player_1[NextCheckIndex1].SetActive(true);
        }

        NextCheckIndex1++;

        if (NextCheckIndex1 == 4)
        {
            Player1Done = true;
            return;
        }
    }

    public void NextCheckPointPlayer2()
    {
        Player_2[NextCheckIndex2 - 1].SetActive(false);
        audioSource.PlayOneShot(MovementAudio);

        if (NextCheckIndex2 < 3)
        {
            Player_2[NextCheckIndex2].SetActive(true);
        }

        NextCheckIndex2++;

        if (NextCheckIndex2 == 4)
        {
            Player2Done = true;
            return;
        }
    }

    public void WallHit()
    {
        WallHealth--;
        audioSource.PlayOneShot(WallHitAudio[Random.Range(0, 3)], 0.3f);
        //Debug.Log("WALL HIT");
        if(WallHealth <= 0)
        {
            audioSource.PlayOneShot(WallBreakAudio, 0.5f);
            GameBehaviour.Instance.UISpawn();
            tip.enabled = true;
            Invoke(nameof(TipEnabled), 3f);
            instructionPlayer1.enabled = false;
            instructionPlayer2.enabled = false;
            instructions.text = WelcomeText.text;
            Destroy(Wall);
        }
    }

    IEnumerator ReadTime()
    {
        yield return new WaitForSeconds(0.3f);
        NextTutorial = true;
        confirm.SetActive(true);
    }

    void TipEnabled()
    {
        tip.enabled = false;
    }
}
