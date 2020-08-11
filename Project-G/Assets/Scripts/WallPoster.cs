using UnityEngine;

public class WallPoster : MonoBehaviour 
{
	private SpriteRenderer _poster;
	public Sprite[] Posters;
	
	private void Start() {
		int posterChance = Random.Range(0,100);
		if (posterChance < 25) {     // 25% chance for putting a poster on the wall
			_poster = GetComponent<SpriteRenderer>();
			int posterIndex = Random.Range(0, Posters.Length);
			_poster.sprite = Posters[posterIndex];
			_poster.sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;
		}
	}
}
