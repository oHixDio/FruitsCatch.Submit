using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private int point = 0;

    GameManager gameManager;
    AudioManager audioManager;
    new CircleCollider2D collider = default;


    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        collider = GetComponent<CircleCollider2D>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void GetItem()
    {
        audioManager.SetAndPlayAudio(3);
        gameManager.UpScore(this.point);
        Destroy(this.gameObject);
    }

    public void DamageThis()
    {
        audioManager.SetAndPlayAudio(5);
        gameManager.DownScore(this.point);
        Destroy(this.gameObject);
    }

    public void HealPlayer()
    {
        audioManager.SetAndPlayAudio(10);
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stage")
        {
            StartCoroutine("Bounds");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bar")
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Bounds()
    {
        yield return new WaitForSeconds(1f);

        collider.isTrigger = true;
    }
}
