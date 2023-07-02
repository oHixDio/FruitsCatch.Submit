using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("-StartUI-")]
    [SerializeField] Canvas startCanvas;
    [SerializeField] Image[] startImage;
    [SerializeField] Text enterText;
    [SerializeField] Text titleText;
    [SerializeField] Text buttonText;
    [SerializeField] Text countDownText;
    [SerializeField] float countDownTimer = 3f;
    [SerializeField] float countDownTimeLimit = 0f;
    float outImageAmount = 0.005f;
    float outTextAmount = 0.05f;

    [Header("-MenuUI-")]
    [SerializeField] Image[] resultImage;
    [SerializeField] Image[] resultStars;
    [SerializeField] Text[] resultText;
    [SerializeField] Canvas resultCanvas;

    [Header("-Scoring-")]
    [SerializeField] Text scoreText = default;
    int score = 000000;
    int maxScore = 999999;
    int minScore = 0;

    [Header("-Item System-")]
    [SerializeField] List<GameObject> itemList = new List<GameObject>();
    [SerializeField] GameObject[] stars;
    [SerializeField] GameObject heart;
    [SerializeField] float itemSpownTimeLimit = 5f;
    [SerializeField] float itemSpownTime = 1f;
    [SerializeField] float heartSpownTimeLimit = 15f;
    [SerializeField] float heartSpownTime = 1f;
    int hasStar = 0;
    float posX = 0;
    float posY = 0;
    float itemSpownTimer;
    float heartSpownTimer;
    Vector2 randamPos;

    [Header("-Timer System-")]
    [SerializeField] float playGameTime = 30f;
    [SerializeField] Text timeText;
    float playGameTimeLimit = 0f;

    [Header("-HP System-")]
    [SerializeField] GameObject[] hearts;
    [SerializeField] Text playerHPText;
    int playerHp = 3;
    int maxPlayerHP = 3;
    int minPlayerHP = 0;
    bool isDamage = false;

    [Header("-Etc..-")]
    [SerializeField] GameObject enemy;
    [SerializeField] Button[] button;
    AudioSource audioSource;
    AudioManager audioManager;
    public bool isCount = false;
    public bool isStart = false;
    public bool isPlaymode = false;
    public bool isPause = false;
    public bool isResult = false;
    public bool isBack = false;
    public bool isOver = false;
    public bool isEnter = false;
    int notSpace = 0;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    void Start()
    {
        scoreText.text = "SCORE：00000";
        playerHPText.text = "PlayerHP: " + playerHp + " / " + maxPlayerHP;
        timeText.text = playGameTime.ToString("F2");
    }
    void Update()
    {
        if (isPlaymode)
        {

            
            CurrentHP();
            SpownItems();
            PlayGameTimer();
        }

        CurrentScore();

        if (!isEnter)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isEnter = true;
            }
        }
        PushSpece();

    }
    void LateUpdate()
    {

        StartCoroutine("CountDownTimer");
        StartCoroutine("OutImageAndText");
        IsGameEnd();
        StartCoroutine("PauseReason");
        StartCoroutine("Result");
        StartCoroutine("TitleBack");
    }

    //スコア関係のメソッド（４
    public void UpScore(int val)
    {
        score += val;

        if (score > maxScore)
        {
            score = maxScore;
        }
    }
    public void DownScore(int val)
    {
        score -= val;

        if (score < minScore)
        {
            score = minScore;
        }
    }
    void CurrentScore()
    {
        //Scoreの表示
        if (score > 9999)
        {
            scoreText.text = "SCORE：" + score;
        }
        else if (score > 999)
        {
            scoreText.text = "SCORE：0" + score;
        }
        else if (score > 99)
        {
            scoreText.text = "SCORE：00" + score;
        }
        else if (score > 9)
        {
            scoreText.text = "SCORE：000" + score;
        }
        else
        {
            scoreText.text = "SCORE：0000" + score;
        }

        //スターの数
        if (score < 1000)
        {
            hasStar = 0;
        }
        else if (score < 3000)
        {
            hasStar = 1;
        }
        else if (score < 5000)
        {
            hasStar = 2;
        }
        else if (score < 7500)
        {
            hasStar = 3;
        }
        else if (score < 10000)
        {
            hasStar = 4;
        }
        else if (score >= 10000)
        {
            hasStar = 5;
        }
        
        //スターの表示管理
        SetActivStar();
    }
    void SetActivStar()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(false);
        }
        for (int i = 0; i < hasStar; i++)
        {
            stars[i].SetActive(true);
        }
    }

    //HP関係のメソッド（３
    public void UpHP()
    {
        playerHp++;

        if(playerHp > maxPlayerHP)
        {
            playerHp = maxPlayerHP;
        }

        playerHPText.text = "PlayerHP: " + playerHp + " / " + maxPlayerHP;

        isDamage = true;
    }
    public void DownHP()
    {
        playerHp--;

        if (playerHp <= minPlayerHP)
        {
            playerHp = minPlayerHP;
            BoolController(31);
        }

        playerHPText.text = "PlayerHP: " + playerHp + " / " + maxPlayerHP;

        isDamage = true;
    }
    void CurrentHP()
    {
        if (isDamage)
        {
            
            SetActivHeart();
        }
        
    }
    void SetActivHeart()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }
        for (int i = 0; i < playerHp; i++)
        {
            hearts[i].SetActive(true);
        }
    }

    //Timer関係のメソッド（２
    void SpownItems()
    {
        posX = Random.Range(-12.5f, 12.6f);
        posY = 10f;
        randamPos = new Vector2(posX, posY);

        itemSpownTimer += Time.deltaTime;
        if (itemSpownTimer > itemSpownTimeLimit)
        {
            Instantiate(itemList[Random.Range(0, itemList.Count)], randamPos, Quaternion.identity);
            itemSpownTimeLimit = Random.Range(0, itemSpownTime);
            itemSpownTimer = 0;
        }

        heartSpownTimer += Time.deltaTime;
        if (heartSpownTimer > heartSpownTimeLimit)
        {
            Instantiate(heart, randamPos, Quaternion.identity);
            audioManager.SetAndPlayAudio(9);
            heartSpownTimeLimit = Random.Range(10, heartSpownTime);
            heartSpownTimer = 0;
        }
    }
    void PlayGameTimer()
    {
        if (playGameTime > playGameTimeLimit)
        {
            playGameTime -= Time.deltaTime;
            timeText.text = playGameTime.ToString("F2");
        }
        else
        {
            playGameTime = 0;
            timeText.text = playGameTime.ToString("F2");
            BoolController(31);
        }
    }

    //MenuUI関係のメソッド（4
    IEnumerator OutImageAndText()
    {
        if (isStart)
        {
            if (startImage[0].fillAmount > 0)
            {
                for(int i = 0;i < startImage.Length; i++)
                {
                    startImage[i].fillAmount -= outImageAmount;
                }
                titleText.color = new Color(0, 0, 0, 0);
                buttonText.color = new Color(0, 0, 0, 0);

                yield return new WaitForSeconds(2f);

                startCanvas.gameObject.SetActive(false);
            }
            else
            {
                yield return new WaitForSeconds(1f);
                BoolController(01);
                BoolController(10);
            }
            
        }
        
    }
    IEnumerator CountDownTimer()
    {
        if (isCount)
        {
            countDownText.gameObject.SetActive(true);
            if (countDownTimer >= countDownTimeLimit)
            {
                countDownTimer -= Time.deltaTime;
                countDownText.text = countDownTimer.ToString("F0");
            }
            else
            {
                countDownText.text = "START!!";
                audioManager.SetAndPlayAudio(8);
                GameModeChanger();
                audioSource.Play();
                BoolController(00);
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            countDownText.gameObject.SetActive(false);
        }
        
    }
    void GameModeChanger()
    {
        if(!isPlaymode)
        {
            BoolController(21);
        }
        else
        {
            BoolController(20);
        }
    }
    public bool SetPlayMode()
    {
        return isPlaymode;
    }

    //Result関係のメソッド(4
    void IsGameEnd()
    {
        if(isPause)
        {
            GameModeChanger();
            audioSource.Stop();
            audioManager.SetAndPlayAudio(6);
            BoolController(41);
            BoolController(30);
        }
        
    }
    IEnumerator PauseReason()
    {
        if(isResult)
        {
            
            if (playGameTime == 0)
            {
                countDownText.text = "TIME UP !!";
                countDownText.gameObject.SetActive(true);
            }
            else if (playerHp == 0)
            {
                countDownText.text = "DIED...";
                countDownText.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(3f);

            BoolController(40);
            BoolController(61);
        }
    }
    IEnumerator Result()
    {
        if(isOver)
        {
            resultCanvas.gameObject.SetActive(true);
            resultText[2].text = score.ToString();

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < resultImage.Length; i++)
            {
                Debug.Log("Result");
                resultImage[i].fillAmount += outImageAmount;
            }

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < resultText.Length; i++)
            {
                resultText[i].color += new Color(0, 0, 0, outTextAmount);
            }
            for (int i = 0; i < hasStar; i++)
            {
                resultStars[i].fillAmount += outImageAmount;
            }

            yield return new WaitForSeconds(2f);

            TitleButtonTrue();
            BoolController(60);
        }
        
    }
    IEnumerator TitleBack()
    {
        if(isBack)
        {

            for (int i = 0; i < resultImage.Length; i++)
            {
                Debug.Log("Title");
                resultImage[i].fillAmount -= outImageAmount;
            }
            for (int i = 0; i < resultText.Length; i++)
            {
                resultText[i].color = new Color(0, 0, 0, 0);
            }
            for (int i = 0; i < hasStar; i++)
            {
                resultStars[i].fillAmount -= outImageAmount;
            }

            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene(0);

        }
    }

    private void PushSpece()
    {
        if(isEnter && notSpace == 0)
        {
            for (int i = 0; i < startImage.Length; i++)
            {
                startImage[i].fillAmount += outImageAmount;
            }
            titleText.color += new Color(0, 0, 0, outTextAmount);
            buttonText.color += new Color(0, 0, 0, outTextAmount);
            enterText.color -= new Color(0, 0, 0, outTextAmount);
            StartButtonTrue();
            Debug.Log("Hi");
        }
        
    }
    public void NotSpace()
    {
        notSpace = 1;
    }
    public void TitleButtonTrue()
    {
        button[0].interactable = true;
        
    }
    public void TitleButtonfalse()
    {
        button[0].interactable = false;
    }
    public void StartButtonTrue()
    {
        button[1].interactable = true;
    }
    public void StartButtonfalse()
    {
        button[1].interactable = false;
    }
    public void BoolController(int _LnumRft)
    {
        switch (_LnumRft)
        {
            case 00:
                isCount = false;
                break;
            case 01:
                isCount = true;
                break;
            case 10:
                isStart = false;
                break;
            case 11:
                isStart = true;
                break;
            case 20:
                isPlaymode = false;
                break;
            case 21:
                isPlaymode = true;
                break;
            case 30:
                isPause = false;
                break;
            case 31:
                isPause = true;
                break;
            case 40:
                isResult = false;
                break;
            case 41:
                isResult = true;
                break;
            case 50:
                isBack = false;
                break;
            case 51:
                isBack = true;
                break;
            case 60:
                isOver = false;
                break;
            case 61:
                isOver = true;
                break;

        }
    }

}
