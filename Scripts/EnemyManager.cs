using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [Header("Geter")]
    [SerializeField] GameObject spikeBall;
    [SerializeField] GameObject player;
    AudioManager audioManager;
    GameManager gameManager;

    [Header("Enemy System")]
    [SerializeField] float enemySpeed = 0;
    float playerPosX;
    float enemyPosX;
    float twicePosX;

    [Header("Spown System")]
    [SerializeField] int spownRange = 0;
    int timeLimit = 5;
    float timer = 0;
    int intTimer = 0;

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (gameManager.SetPlayMode())
        {
            SpownTimer();
        }
        
    }
    void LateUpdate()
    {
        if(gameManager.SetPlayMode())
        {
            MoveEnemy();
        }
        
    }

    //Enemy“®ìŠÖŒW‚Ìƒƒ\ƒbƒhi‚Q
    void GetPlayerPositionX()
    {
        playerPosX = player.transform.position.x;
        enemyPosX = this.transform.position.x;

        twicePosX = playerPosX - enemyPosX;
    }
    void MoveEnemy()
    {
        GetPlayerPositionX();

        this.transform.Translate(new Vector2(enemySpeed, 0) * twicePosX * Time.deltaTime);
    }

    //SpikeBallŠÖŒW‚Ìƒƒ\ƒbƒhi‚Q
    void SpownSpikeBall()
    {
        Instantiate(spikeBall, transform.position, Quaternion.identity);
        audioManager.SetAndPlayAudio(4);
    }
    void SpownTimer()
    {
        timer += Time.deltaTime;
        intTimer = (int)timer;

        if(intTimer > timeLimit)
        {
            timer = 0;
            SpownSpikeBall();
            timeLimit = Random.Range(3, spownRange);
        }

    }

}
