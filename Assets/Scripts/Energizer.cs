using UnityEngine;

public class Energizer : MonoBehaviour {

    private GameManager gm;
    private AudioManager am;

	void Start ()
	{
	    gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
	    am = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        if (gm == null) {
            Debug.Log("Energizer did not find Game Manager!");
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "pacman")
        {
            am.energizer_play();
            gm.ScareGhosts();
            Destroy(gameObject);
        }
    }
}
