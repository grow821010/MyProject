using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    GameObject currentFloor;
    [SerializeField] int HP;
    [SerializeField] GameObject HPBar;
    [SerializeField] Text scoreText;
    int score;
    float scoreTime;
    Animator anim;
    SpriteRenderer render;
    AudioSource deathSound;
    [SerializeField] GameObject replayButton;
    // Start is called before the first frame update
    void Start()
    {
        HP = 10;
        score = 0;
        scoreTime = 0f;
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            render.flipX = false;
            anim.SetBool("run", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            render.flipX = true;
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 5f * Time.deltaTime, 0);
        }
        UpdateScore();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Normal")
        {
            if (other.contacts[0].normal == new Vector2(0f, 1f))
            {
                currentFloor = other.gameObject;
                modifyHP(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "Nails")
        {
            if (other.contacts[0].normal == new Vector2(0f, 1f))
            {
                currentFloor = other.gameObject;
                modifyHP(-3);
                anim.SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "Ceiling")
        {
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            modifyHP(-3);
            anim.SetTrigger("hurt");
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DeathLine")
        {
            Death();
        }
    }

    void modifyHP(int num)
    {
        HP += num;
        if (HP > 10)
        {
            HP = 10;
        }
        else if (HP <= 0)
        {
            HP = 0;
            Death();

        }
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        for (int i = 0; i < HPBar.transform.childCount; i++)
        {
            if (HP > i)
            {
                HPBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HPBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }

    void UpdateScore()
    {
        scoreTime += Time.deltaTime;
        if (scoreTime > 2.5f)
        {
            score++;
            scoreTime = 0f;
            scoreText.text = "地下" + score.ToString() + "層";
        }

    }

    void Death()
    {
        deathSound.Play();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }

    public void replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}
