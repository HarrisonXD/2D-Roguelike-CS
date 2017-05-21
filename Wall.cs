using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour 
{
	//Sprite to be displayed when player damages wall
	public Sprite dmgSprite;
	//Walls hp
	public int hp = 4;
	//Audio clips that play when the player attacks a wall
	public AudioClip chopSound1;
	public AudioClip chopSound2;

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () 
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	//Call when wall is attacked by player
	public void DamageWall (int loss) 
	{
		//Plays one of the two chop sounds randomly
		SoundManager.instance.RandomizeSfx (chopSound1, chopSound2);
		//Displays the damaged wall sprite
		spriteRenderer.sprite = dmgSprite;
		//Subtract the attack from walls total hp
		hp -= loss;
		//Iff hp is less then or equal to 0 disable the gameObject
		if (hp <= 0)
			gameObject.SetActive (false);
	}
}
