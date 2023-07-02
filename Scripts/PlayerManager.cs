using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] int speed = 1;

    AudioManager audioManager;
    GameManager gameManager;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator anim;

    float velX;

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        anim = rb.GetComponent<Animator>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (gameManager.SetPlayMode())
        {
            PlayerMove();
            
        }
        else
        {
            velX = 0;
            rb.velocity = new Vector2(0,0);

        }
        Anime();
    }
    void FixedUpdate()
    {
        if(gameManager.SetPlayMode())
        {
            PlayerControll();
        }
        
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<ItemManager>().DamageThis();
        }
        if(collision.gameObject.tag == "Heart")
        {
            collision.gameObject.GetComponent<ItemManager>().HealPlayer();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<ItemManager>().DamageThis();
            gameManager.DownHP();
        }
        if(collision.gameObject.tag == "Heart")
        {
            collision.gameObject.GetComponent<ItemManager>().HealPlayer();
            gameManager.UpHP();
        }
    }

    void Anime()
    {
        //歩行アニメーション
        anim.SetFloat("speed", Mathf.Abs(velX));
    }

    void PlayAudio()
    {
        audioManager.SetAndPlayAudio(1);
    }

    void PlayerMove()
    {
        velX = Input.GetAxis("Horizontal");
        
        //向き変更
        if (velX > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (velX < 0f)
        {
            spriteRenderer.flipX = true;
        }

    }

    void PlayerControll()

    {
        rb.velocity = new Vector2(velX * speed, rb.velocity.y);
    }
}
