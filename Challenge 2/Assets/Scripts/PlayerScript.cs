using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    Animator anim;
    private bool facingRight = true;

    private int score;
    private int lives;
    private int level;

    public float speed;

    /*text*/
    public Text scoretext;
    public Text scoretextShadow;
    public Text wintext;
    public Text wintextShadow;
    public Text livestext;
    public Text livestextShadow;
    public Text leveltext;
    public Text leveltextShadow;

    /*music*/
    public AudioClip musicClipOne;
    public AudioClip musicClipWin;
    public AudioClip musicClipThree;
    public AudioSource musicSource;
    public AudioClip sfxClipJump;
    public AudioClip sfxClipCoin;
    public AudioClip sfxClipEnemy;
    public AudioSource sfxSource;

    private Rigidbody2D rb2d;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        musicSource.clip = musicClipOne;
        musicSource.Play();

        score = 0;
        level = 1;
        lives = 3;

        rb2d = GetComponent<Rigidbody2D>();

        scoretext.text = "Score: " + score.ToString();
        scoretextShadow.text = "Score: " + score.ToString();

        wintext.text = " ";
        wintextShadow.text = " ";

        livestext.text = "Lives: " + lives.ToString();
        livestextShadow.text = "Lives: " + lives.ToString();

        leveltext.text = "Level " + level.ToString();
        leveltextShadow.text = "Level " + level.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.drag = 1;

        float hozMovement = Input.GetAxis("Horizontal");

        rb2d.AddForce(new Vector2(hozMovement * speed, 0));


        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        /*End Animation*/

        if ((Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)))
        {
            rb2d.AddForce(new Vector2(0, -1), ForceMode2D.Impulse);
        }

        

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        /*score system*/
        if(other.gameObject.CompareTag ("Coin"))
        {
            score += 1;
            scoretext.text = "Score: " + score.ToString();
            scoretextShadow.text = "Score: " + score.ToString();
            other.gameObject.SetActive (false);

            sfxSource.clip = sfxClipCoin;
            sfxSource.Play();
        }

        if (score == 4 && level == 1)
        {
            lives = 3;
            level = 2;
            transform.position = new Vector2(62.5f, 1f);
            anim.SetInteger("State", 0);
            rb2d.velocity = new Vector2(0.0f, 0.0f);

            livestext.text = "Lives: " + lives.ToString();
            livestextShadow.text = "Lives: " + lives.ToString();

            leveltext.text = "Level " + level.ToString();
            leveltextShadow.text = "Level "+ level.ToString();

        }

        if (score == 8)
        {
            anim.SetInteger("State", 0);
            wintext.text = "You win! Game created by Ryan Fisher!";
            wintextShadow.text = "You win! Game created by Ryan Fisher!";
            musicSource.clip = musicClipWin;
            musicSource.Play();
            musicSource.loop = false;
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(this);
        }



        /*lives system */
        if (other.gameObject.CompareTag("Enemy"))
        {
            lives = lives - 1;
            livestext.text = "Lives: " + lives;
            livestextShadow.text = "Lives: " + lives;
            other.gameObject.SetActive(false);
            sfxSource.clip = sfxClipEnemy;
            sfxSource.Play();
        }

        if (lives == 0)
        {
            anim.SetInteger("State", 0);
            wintext.text = "You Lose! Better luck next time!";
            wintextShadow.text = "You Lose! Better luck next time!";
            musicSource.clip = musicClipThree;
            musicSource.Play();
            musicSource.loop = false;
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(this);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        /*jump system*/
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        if (collision.collider.tag == "Ground")
        {
            if((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow)))
            {
                rb2d.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
                sfxSource.clip = sfxClipJump;
                sfxSource.Play();
            }
            if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow)))
            {
                rb2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }

            /*Animation*/
            if (Mathf.Abs(hozMovement) > 0)
            {
                anim.SetInteger("State", 1);
            }
            if (Mathf.Abs(hozMovement) == 0)
            {
                anim.SetInteger("State", 0);
            }

            if (vertMovement > 0)
            {
                anim.SetInteger("State", 2);
            }

            if (score == 8 || lives == 0)
            {
                anim.SetInteger("State", 0);
            }
        }
    }
}
