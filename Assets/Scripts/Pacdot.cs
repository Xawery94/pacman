using UnityEngine;

public class Pacdot : MonoBehaviour {

    private AudioManager am;

    private void Start()
    {
        am = GameObject.FindObjectOfType<AudioManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
	{
        if (other.name == "pacman")
		{
            GameManager.pacdot_length++;
            am.pacdot_play();
            GameManager.score += 10;
		    GameObject[] pacdots = GameObject.FindGameObjectsWithTag("pacdot");
            Destroy(gameObject);
		}
	}
}
