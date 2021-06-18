using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    protected Joystick joystick;
    protected Rigidbody2D rigid;
    private bool canMove = true;
    public float speed;
    public int maxHealth;
    public int health;
    private bool isDead = false;
    public AudioClip sa;

    private GameObject healthBar;

    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        rigid = GetComponent<Rigidbody2D>();
        healthBar = GameObject.Find("healthBar");
        health = maxHealth;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 localScale = healthBar.transform.localScale;
        float scaleX  = health;
        scaleX = scaleX / maxHealth;
        scaleX = scaleX * 4.144f;
        localScale.x = Mathf.Max(scaleX, 0);
        healthBar.transform.localScale = localScale;

        
    }

    // Update is called once per frame
    void Update()
    {

        if (canMove)
        {
            rigid.velocity = new Vector3(joystick.Horizontal * speed, joystick.Vertical * speed, 0);
        }
        

    }
    
    public void pushPlayer(float pushTime, Vector3 direction)
    {
        if(canMove)
        {
            StartCoroutine(pushPlayerNumerator(pushTime,direction));
        }
        
    }

    private IEnumerator pushPlayerNumerator(float pushTime,Vector3 direction)
    {
        canMove = false;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(direction*10000*Time.deltaTime);
        rigid.freezeRotation = true;
        yield return new WaitForSeconds(pushTime);
        canMove = true;
        rigid.velocity = Vector3.zero;
        
    }

    public void damagePlayer(int damage)
    {
        if(!isDead)
        {
            health -= damage;
            if (health <= 0)
            {
                isDead = true;
                SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
                for(int i = 0;i<sprites.Length;i++)
                {
                    sprites[i].enabled = false;
                }
                UIManager.instance.deadScreen();
            }
            Vector3 localScale = healthBar.transform.localScale;
            float scaleX = health;
            scaleX = scaleX / maxHealth;
            scaleX = scaleX * 4.144f;
            localScale.x = Mathf.Max(Mathf.Min(scaleX, 4.144f), 0);
            healthBar.transform.localScale = localScale;
        }
        
    }
}
