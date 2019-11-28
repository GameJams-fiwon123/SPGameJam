using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
	public Animator anim;

	public SpriteRenderer sprRenderer1, sprRenderer2;
	public Sprite[] sprites;
	int indexSprite = 0;

	// Start is called before the first frame update
	void Start() {
		sprRenderer1.sprite = sprites[0];
		indexSprite++;
	}

	public void Next() {
		if (indexSprite < sprites.Length) {
			sprRenderer2.sprite = sprites[indexSprite];
			anim.Play("BackgroundManager");
		}
	}

	public void NextAnimation() {
		sprRenderer1.sprite = sprites[indexSprite];
		indexSprite++;
	}
}
