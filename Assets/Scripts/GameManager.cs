using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 

public class GameManager : MonoBehaviour
{   
    public GameObject ballPrefab;
    public GameObject playerPrefab;
    public Text scoreText;
    public Text ballsText;
    public Text levelText;
    public Text highestscoreText;
    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;
    public GameObject panelFinish;
    public AudioSource audio1;
    public GameObject FW1;
    public GameObject FW2;
    public GameObject FW3;
    public GameObject FW4;
    public GameObject FW5;
    public GameObject FW6;


    public GameObject[] levels;

    public static GameManager Instance
    { get; private set; }
    public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER, FINISH}
    // Start is called before the first frame update
    State _state;
    GameObject _currentball;
    GameObject _currentlevel;
    bool _isSwitchingState;

    private  float _score;
    public float Score
    {
        get { return _score; }
        set { _score  = value;
            scoreText.text="SCORE:"+_score; 
            }
            
    }
    
    private int _level;
    public int Level
    {
        get { return _level; }
        set { _level  = value; 
            levelText.text="LEVEL:"+_level;
            }
    }

    private int _balls;
    public int Balls
    {
        get { return _balls; }
        set { _balls  = value; 
            ballsText.text="BALL:"+_balls;
            }
    }

    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }
    void Start()
    {   
        Instance = this;
        SwitchState(State.MENU);
        audio1=GetComponent<AudioSource>();
    }

    public void SwitchState(State newState, float delay=0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }
    
    IEnumerator SwitchDelay(State newState, float delay)
    {   
        _isSwitchingState=true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state=newState;
        BeginState(newState);
        _isSwitchingState=false;
    }
    void BeginState(State newState)
    {
        switch(newState)
        {
            case State.MENU:
                Cursor.visible=true;
                highestscoreText.text="HighestScore:"+PlayerPrefs.GetInt("highestscore");
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                Cursor.visible=false;
                panelPlay.SetActive(true);
                Score=0;
                Level=0;
                Balls=3;
                if(_currentlevel != null)
                {
                    Destroy(_currentlevel);
                }
                Instantiate(playerPrefab);
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(_currentball);
                Destroy(_currentlevel);
                Level++;
                panelLevelCompleted.SetActive(true);
                SwitchState(State.LOADLEVEL, 2f);
                break;
            case State.LOADLEVEL:
                if(Level>=levels.Length)
                {
                    SwitchState(State.FINISH);
                }
                else
                {
                    _currentlevel=Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                if(Score > PlayerPrefs.GetInt("highestscore"))
                {
                    PlayerPrefs.SetFloat("highestscore",Score);
                }
                panelGameOver.SetActive(true);
                break;
            case State.FINISH:
                Score=6264;
                audio1.Play();
                panelFinish.SetActive(true);
                FW1.SetActive(true);
                FW2.SetActive(true);
                FW3.SetActive(true);
                FW4.SetActive(true);
                FW5.SetActive(true);
                FW6.SetActive(true);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch(_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if(_currentball==null)
                {
                    if (Balls>0)
                    {
                        _currentball=Instantiate(ballPrefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }
                if(_currentlevel !=null && _currentlevel.transform.childCount==0 && !_isSwitchingState)
                {
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if(Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;
        }
    }

    void EndState()
    {
        switch(_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;
        }
    }
}
