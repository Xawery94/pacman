using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int lives = 3;

	public enum GameState { Init, Game, Dead }
	public static GameState gameState;
    private GameObject pacman;
    private GameObject blinky;
    private GameObject pinky;
    private GameObject inky;
    private GameObject clyde;
    public Canvas ReadyCanvas;
    public Canvas GameOverCanvas;
    public Canvas WinCanvas;
    private AudioManager am;
    private float _timeToCalm;
    public static bool scared;
    public static int score;
    public static int pacdot_length;
    public float scareLength;
    public float initialDelay;
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if(this != _instance)   
                Destroy(this.gameObject);
        }

        AssignGhosts();
    }

	void Start () 
	{
        am = GameObject.FindObjectOfType<AudioManager>();

        ReadyCanvas.enabled = false;
        WinCanvas.enabled = false;
        StartCoroutine("ShowReadyScreen", initialDelay);
    }

    void OnLevelWasLoaded()
    {
        lives = 3;
        AssignGhosts();
        ResetVariables();
    }

    private void ResetVariables()
    {
        _timeToCalm = 0.0f;
        scared = false;
    }

    void Update()
    {
        pacdot_length = GameObject.FindGameObjectsWithTag("pacdot").Length;

        if (scared && _timeToCalm <= Time.time)
        {
            CalmGhosts();
        }

        if (pacdot_length <= 0)
        {
            WinCanvas.enabled = true;
            gameState = GameState.Init;
            AudioManager.win = true;
            am.win_play();
        }
	}

	public void ResetScene()
	{
        CalmGhosts();
		pacman.transform.position = new Vector3(15f, 11f, 0f);
		blinky.transform.position = new Vector3(15f, 20f, 0f);
		pinky.transform.position = new Vector3(14.5f, 17f, 0f);
		inky.transform.position = new Vector3(16.5f, 17f, 0f);
		clyde.transform.position = new Vector3(12.5f, 17f, 0f);
		pacman.GetComponent<PlayerController>().ResetDestination();
        StartCoroutine("ShowReadyScreen", initialDelay);
    }

    IEnumerator ShowReadyScreen(float seconds)
    {
        gameState = GameState.Init;
        ReadyCanvas.enabled = true;
        yield return new WaitForSeconds(seconds);
        ReadyCanvas.enabled = false;
        gameState = GameState.Game;
    }

     IEnumerator ShowGameOverScreen()
     {
         GameOverCanvas.enabled = true;
         yield return new WaitForSeconds(2);
     }

    public void H_ShowGameOverScreen()
    {
        StartCoroutine("ShowGameOverScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleScare()
	{
        if (!scared)
        {
            ScareGhosts();
        }
        else
        {
            CalmGhosts();
        }
	}

	public void ScareGhosts()
	{
        scared = true;
		blinky.GetComponent<GhostMoveWp>().Terrify();
		pinky.GetComponent<GhostMoveWp>().Terrify();
		inky.GetComponent<GhostMoveWp>().Terrify();
		clyde.GetComponent<GhostMoveWp>().Terrify();
		_timeToCalm = Time.time + scareLength;
	}

	public void CalmGhosts()
	{
		scared = false;
		blinky.GetComponent<GhostMoveWp>().Calm();
		pinky.GetComponent<GhostMoveWp>().Calm();
		inky.GetComponent<GhostMoveWp>().Calm();
		clyde.GetComponent<GhostMoveWp>().Calm();
    }

    void AssignGhosts()
    {
        clyde = GameObject.Find("clyde");
        pinky = GameObject.Find("pinky");
        inky = GameObject.Find("inky");
        blinky = GameObject.Find("blinky");
        pacman = GameObject.Find("pacman");

        if (clyde == null || pinky == null || inky == null || blinky == null) {
            Debug.Log("One of ghosts are NULL");
        }
        if (pacman == null) {
            Debug.Log("Pacman is NULL");
        }
    }

    public void LoseLife()
    {
        lives--;
        gameState = GameState.Dead;

        UIScript ui = GameObject.FindObjectOfType<UIScript>();
        Destroy(ui.lives[ui.lives.Count - 1]);
        ui.lives.RemoveAt(ui.lives.Count - 1);
    }

    public static void DestroySelf()
    {
        score = 0;
        lives = 3;
        Destroy(GameObject.Find("Game Manager"));
    }
}
