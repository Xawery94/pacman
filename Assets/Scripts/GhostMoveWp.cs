using System.Collections;
using UnityEngine;

public class GhostMoveWp : MonoBehaviour
{
    int cur = 0;
    public Transform[] waypoints;
    public PlayerController pacman;
    public float speed;
    enum State { Chase, Run };
    State state;
    private Vector3 startPos;
    private GameManager gm;
    private AudioManager am;
    private float timeToWhite;
    private float toggleInterval;
    private bool isWhite = false;

    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        am = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    void FixedUpdate()
    {
        if (GameManager.gameState == GameManager.GameState.Init)
        {
            cur = 0;
        }
        else if (GameManager.gameState == GameManager.GameState.Game)
        {
            if (gm == null) Debug.Log("Ghost cant find GameManager!");
            if (transform.position != waypoints[cur].position)
            {
                Vector2 p = Vector2.MoveTowards(transform.position,
                                                waypoints[cur].position,
                                                speed);
                GetComponent<Rigidbody2D>().MovePosition(p);
            }
            else
            {
                cur = (cur + 1) % waypoints.Length;
            }

            Vector2 dir = waypoints[cur].position - transform.position;
            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y);
        }
    }

    public void Terrify()
    {
        state = State.Run;
        timeToWhite = gm.scareLength - 2f;
        GetComponent<Animator>().SetBool("Run_White", false);
        RunAway();
    }

    public void Calm()
    {
        if (state != State.Run) return;

        timeToWhite = 0;
        GetComponent<Animator>().SetBool("Run_White", false);
        GetComponent<Animator>().SetBool("Run_Blue", false);
        state = State.Chase;
    }

    public void ToggleBlueWhite()
    {
        isWhite = !isWhite;
        GetComponent<Animator>().SetBool("Run_White", isWhite);
    }

    public void RunAway()
    {
        GetComponent<Animator>().SetBool("Run_Blue", true);
        StartCoroutine("TurnWhite", timeToWhite);
    }

    IEnumerator TurnWhite(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<Animator>().SetBool("Run_White", true);
    }

    public void InitializeGhost()
    {
        startPos = getStartPosAccordingToName();
    }
    
    public void InitializeGhost(Vector3 pos)
    {
        transform.position = getStartPosAccordingToName();
    }

     private Vector3 getStartPosAccordingToName()
     {
        cur = 0;
        switch (gameObject.name)
         {
             case "blinky":
                 return new Vector3(15f, 17f, 0f);

             case "pinky":
                 return new Vector3(15f, 17f, 0f);

             case "inky":
                 return new Vector3(15f, 17f, 0f);

             case "clyde":
                 return new Vector3(15f, 17f, 0f);
         }

         return new Vector3();
     }
     

    void OnTriggerEnter2D(Collider2D other)
    {
         if (other.name == "pacman")
         {
             if (state == State.Run)
             {
                am.ghostEat_play();
                Calm();
                InitializeGhost(startPos);
                pacman.UpdateScore();
             }
             else
             {
                am.death_play();
                gm.LoseLife();
             }
             
        }
    }
    
}
