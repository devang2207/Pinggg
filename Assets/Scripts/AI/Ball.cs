using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour 
{
    [Header("Tags for Collision Check")]
    [SerializeField] const string _playerTag = "Player";
    [SerializeField] const string _playerGoalTag = "PlayerGoal";
    [SerializeField] const string _enemyGoalTag = "EnemyGoal";
    [SerializeField] const string _wallTag = "Wall";

    [Header("Speed Variables")]
    [SerializeField, Tooltip("initial Speed on reset")] float _initialSpeed = 10.0f;
    [SerializeField, Tooltip("maximum ball speed")] float _ballMaxSpeed = 20.0f;
    [Tooltip("speed of ball")] float _speed;

    [Tooltip("Event actions called in other scripts")]
    public static event Action OnPlayerScore;
    public static event Action OnEnemyScore;

    [Tooltip("private Serialized variables")]
    [SerializeField]ParticleSystem _particleSys;
    [SerializeField] int _maxWallHit = 4;

    [Tooltip("private variables")]
    Rigidbody2D _rb2D;
    float _timePassed;
    int _continuosWallHit;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(ResetAndLaunch());
    }
    void IncreaseSpeed()
    {
        if (_speed <= _ballMaxSpeed)
        {
            _speed += 1;
            _rb2D.velocity = _rb2D.velocity.normalized * _speed;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayParticleEffect();
        //check for collision tags and call functions accordingly
        switch (collision.gameObject.tag)
        {
            case _playerTag:
                //increase ball speed
                IncreaseSpeed();
                //reset continous wall hit
                _continuosWallHit = 0;
                break;
            case _playerGoalTag:
                //increment score by 1..
                OnPlayerScore?.Invoke();
                StartCoroutine(ResetAndLaunch());
                break;
            case _enemyGoalTag:
                //game over try again screen...
                OnEnemyScore?.Invoke();
                break;
            case _wallTag:
                CheckForRebounds();
                print(collision.gameObject.tag);
                break;
            default:
                //print(collision.gameObject.name);
                break;
        }
    }
    void PlayParticleEffect()
    {
        //_particleSys = this.GetComponentInChildren<ParticleSystem>();
        if (_particleSys != null)
        {
            _particleSys.Play();
        }
        else
        {
            Debug.LogWarning("Particle System not found!");
        }
    }
    void CheckForRebounds()
    {
        //check if the ball is colliding with wall continuosly and reset if it is!
        if(_continuosWallHit >= _maxWallHit)
        {
            ResetBall();
            _continuosWallHit = 0;
        }
        else
        {
            _continuosWallHit++;
        }
    }
    public void ResetBall() { StartCoroutine(ResetAndLaunch()); }//function to call Inumerator from game manager script
    IEnumerator ResetAndLaunch()    //pause then give a delay and respawn
    {
        transform.position = Vector2.zero;
        _rb2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(1);
        _speed = _initialSpeed;
        float dirX = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        float dirY = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        _rb2D.velocity = new Vector2(dirX * _speed, dirY * _speed);
    }
}
