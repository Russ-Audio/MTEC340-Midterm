using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource slowMusic;
    [SerializeField] AudioSource fastMusic;
    [SerializeField] AudioSource ambience;
    [SerializeField] AudioSource uiClick;

    public float fadeDuration = 1.0f;
    private float timer = 0.0f;
    private bool inCombat;
    private bool restarting;

    private Scene currentScene;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if(objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        //slowMusic = GameObject.Find("Music_1").GetComponent<AudioSource>();
        //fastMusic = GameObject.Find("Music_2").GetComponent<AudioSource>();
        //ambience = GameObject.Find("Ambience").GetComponent<AudioSource>();
        //uiClick = GameObject.Find("UI").GetComponent<AudioSource>();

        inCombat = false;
        //restarting = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        slowMusic.volume = 0f;
        fastMusic.volume = 0f;
        ambience.volume = 1f;
        slowMusic.Play();
        fastMusic.Play();
        ambience.Play();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            restarting = true;
        }

        if (restarting && SceneManager.GetActiveScene().buildIndex == 0)
        {
            slowMusic.Play();
            fastMusic.Play();
            ambience.Play();
            restarting = false;
        }
        */

        //Set Bool for Music Change
        if (SceneManager.GetActiveScene().buildIndex >= 2 && SceneManager.GetActiveScene().buildIndex < 5)
        {
            inCombat = true;
        }
        else
        {
            inCombat = false;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                uiClick.PlayOneShot(uiClick.clip);
            }
        }

            //CrossFade Between Music 1 & 2
            if (inCombat)
        {
            timer = 0.0f;
            if(timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float normalizedTime = timer / fadeDuration;
                float targetVolumeFadeIn = Mathf.Lerp(fastMusic.volume, 1.0f, normalizedTime);
                float targetVolumeFadeOut = Mathf.Lerp(slowMusic.volume, 0.0f, normalizedTime);
                slowMusic.volume = targetVolumeFadeOut;
                fastMusic.volume = targetVolumeFadeIn;
            }
        }
        else
        {
            timer = 0.0f;
            if(timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float normalizedTime = timer / fadeDuration;
                float targetVolumeFadeIn = Mathf.Lerp(slowMusic.volume, 1.0f, normalizedTime);
                float targetVolumeFadeOut = Mathf.Lerp(fastMusic.volume, 0.0f, normalizedTime);
                slowMusic.volume = targetVolumeFadeIn;
                fastMusic.volume = targetVolumeFadeOut;
            }
        }
    }


}
