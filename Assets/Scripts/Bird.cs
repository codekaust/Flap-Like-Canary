using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    public float upforce=250f;
    
	public float birdGravity=1;

	public bool isDead = false;
    private Rigidbody2D rb2d;
    private Animator anim;

	public static KinectData player;
	private bool canFlap = false;
	private bool played;
	private bool playerFound;

	public AudioClip flap;
	public AudioClip hit;
	public AudioSource birdFlap;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator> ();
		playerFound = false;
		player = new KinectData();
		player.Start ();
		birdFlap.clip = flap;
		played = false;
    }

    // Update is called once per frame
    void Update () {
		playerFound = player.IsPlayerFound();

		if (GameControl.instance.totalTimePassed > 2.5) {
			rb2d.gravityScale = birdGravity;
			if (isDead == false && playerFound)
			{           
				if (!canFlap && player.GetLeftHandY() > player.GetHeadY() && player.GetRightHandY() > player.GetHeadY()) {
					canFlap = true;
				}

				if(Input.GetMouseButtonDown(0) || (canFlap && (player.GetLeftHandY() < player.GetHipY() && player.GetRightHandY() < player.GetHipY()))){
					birdFlap.Play();
					rb2d.velocity = Vector2.zero;
					rb2d.AddForce(new Vector2(0, upforce));
					anim.SetTrigger("FlapParam");
					canFlap = false;
				}
			}
		}	
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if ( collision.gameObject.tag.Equals("Column") || collision.gameObject.tag.Equals("Ground"))
        {
			if (!played) {
				birdFlap.clip = hit;
				birdFlap.Play ();
				played = true;
			}
            rb2d.velocity = Vector2.zero;
            isDead = true;
            anim.SetTrigger("Die");
            GameControl.instance.BirdDied();
        }
    }
}
