using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip JumpClip;
    public AudioClip DeathClip;
    public AudioClip ClearClip;
    private int jumpCount =0;
    private float jumpForce = 500f;

    private bool isGrounded = false;
    private bool isDead = false;

    private SpriteRenderer playerRenderer;
    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private AudioSource playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            return;
        }

        if (Input.GetButtonDown("Jump")&& jumpCount<1)
        {
            //점프횟수증가
            jumpCount++;
            //AddForce로 점프하기
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
 
            //오디오소스재생
            playerAudio.Play();
            //
            Debug.LogFormat("점프카운터는{0}다.", jumpCount);
        }
        animator.SetBool("Grounded", isGrounded);
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        playerAudio.clip = DeathClip;
        playerAudio.Play();
        playerRigidbody.velocity = Vector2.zero;
        isDead = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.tag == "Dead" && isDead ==false)
        {
            //Debug.LogFormat("현재라이프{0}", Static.life);
            Die();
            //Static.life -= 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y >0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded=false;
    }
}
