using System;
using System.Collections;
using System.Collections.Generic;
using Code.Data;
using Codice.CM.WorkspaceServer.Tree.GameUI.Checkin.Updater;
using com.enemyhideout.soong;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

  private INotifyManager _notifyManager;

  private DataEntity _root;

  [SerializeField]
  private EntitySource _ballSource;
  [SerializeField]
  private EntitySource _paddle1Source;
  [SerializeField]
  private EntitySource _paddle2Source;
  [SerializeField]
  private EntitySource _livesUIP1;
  [SerializeField]
  private EntitySource _livesUIP2;
  [SerializeField]
  private EntitySource _tableSource;
  [SerializeField]
  private EntitySource _gameOverUI;

  [SerializeField] private float _paddleSpeed = 5.0f;

  private World _world;
  private Unit _ballUnit;
  private Unit _player1Unit;
  private Unit _player2Unit;
  
  
  /**
   *  DataEntity <-> GameObject
   *  DataElement <-> MonoBehaviour/Component for data
   *  In the same way that a GameObject has MonoBehaviours, a DataEntity has DataElements.
   *  Root - Root Game Object
   *   |
   *   | - Players - A collection of player objects, (two)
   *      - Player One
   *      - Player Two
   *      +- PlayerElement - The number of lives.
   *      +- Unit - Location/Bounds/Velocity
   *   | - Table - The field within which we're playing.
   *   | - Ball - The ball :) 
   * 
   */

  private float _ballSpeed = 10.0f;

  void Start()
  {

    _notifyManager = GetComponent<NotifyManager>();
    _root = new DataEntity(_notifyManager, "Root");
    
    var players = new DataEntity(_notifyManager, "Players", _root);
    var player1 = MakePlayer(new Vector2(-5, 0), _paddleSpeed, "Player One", _notifyManager, players);
    var player2 = MakePlayer(new Vector2(5, 0), _paddleSpeed, "Player Two", _notifyManager, players);
    var ball = MakeBall(_ballSpeed, _notifyManager, _root);
    var world = new DataEntity(_notifyManager, "Table", _root);

    _ballUnit = ball.GetElement<Unit>();
    _player1Unit = player1.GetElement<Unit>();
    _player2Unit = player2.GetElement<Unit>();

    _world = new World(world);
    _world.Bounds = new Rect(new Vector2(-6,-3), new Vector2(6*2,3*2));

    _ballSource.Entity = ball;
    _paddle1Source.Entity = player1;
    _livesUIP1.Entity = player1;
    _paddle2Source.Entity = player2;
    _livesUIP2.Entity = player2;
    _tableSource.Entity = world;
    _gameOverUI.Entity = world;
    StartCoroutine(PlayGame());
    
  }

  private void ResetBoard()
  {
    _player1Unit.Position = new Vector2(-5, 0);
    _player2Unit.Position = new Vector2(5, 0);
    
    // move the ball back
    _ballUnit.Position = Vector2.zero;
    _ballUnit.Velocity = new Vector2(Random.Range(0,_ballSpeed), Random.Range(0,_ballSpeed));

    // the ball only displays its trail renderer when its alive.
    _ballUnit.Alive = true;
  }
  
  
  
  private static DataEntity MakePlayer(Vector2 position, float speed, string name, INotifyManager notifyManager, DataEntity parent)
  {
    var player = new DataEntity(notifyManager, name, parent);
    new PlayerElement(player);
    var unit = new Unit(new Rect(position, new Vector2(0.125f, 1f)), player);
    unit.Velocity = new Vector2(0, Random.Range(0.5f * speed, speed));
    return player;
  }

  private static DataEntity MakeBall(float ballSpeed, INotifyManager notifyManager, DataEntity root)
  {
    var ball = new DataEntity(notifyManager, "Ball", root);
    var _ballPosition = new Unit(new Rect(new Vector2(0, 0), new Vector2(0.125f, 0.125f)), ball);
    _ballPosition.Velocity = new Vector2(Random.Range(0,ballSpeed), Random.Range(0,ballSpeed));
    return ball;
  }


  IEnumerator PlayGame()
  {
    while (true)
    {
      UpdateBall(_ballUnit, Time.deltaTime,  _world.Bounds);
      
      // update each players position to follow the ball.
      UpdatePlayerPosition(_player1Unit, Time.deltaTime, _ballUnit.Position);
      UpdatePlayerPosition(_player2Unit, Time.deltaTime, _ballUnit.Position);
      
      HandleBallPlayerInteraction(_ballUnit, _player1Unit.Bounds);
      HandleBallPlayerInteraction(_ballUnit, _player2Unit.Bounds);
      
      
      //calculate the bounds to use for checking if we've won.

      Rect legalBounds = _world.Bounds;
      legalBounds.width -= 1.5f;
      legalBounds.center = Vector2.zero;
      if (!legalBounds.Overlaps(_ballUnit.Bounds))
      {
        Debug.Log($"LOST {legalBounds}, ball:{_ballUnit.Bounds}");
        yield return LoseLife();
        yield break;
      }

      yield return null;
    }
    
  }

  private IEnumerator LoseLife()
  {
    
    var losingUnit = _ballUnit.Position.x > 0 ? _player2Unit : _player1Unit;
    var loser = losingUnit.Parent.GetElement<PlayerElement>();
    loser.Lives--;
    _ballUnit.Alive = false;
    yield return new WaitForSeconds(1.0f);
    
    if (loser.Lives == 0)
    {
      var winner = losingUnit == _player1Unit ? _player2Unit.Parent : _player1Unit.Parent;
      GameOver(winner);
    }
    else
    {
      ResetBoard();
      StartCoroutine(PlayGame());
    }
  }

  private void GameOver(DataEntity winner)
  {
    _world.Winner = winner;
    _world.GameOver = true;
  }
  
  private static void HandleBallPlayerInteraction(Unit ballUnit, Rect playerBounds)
  {
    Rect ballBounds = ballUnit.Bounds;
    if (playerBounds.Overlaps(ballBounds))
    {
      // move the ball to be outside the bounds.
      float fudge = 0.0001f;
      float newX = ballBounds.center.x < 0 ? playerBounds.xMax + fudge : playerBounds.xMin - ballUnit.Bounds.width - fudge;
      ballBounds.position = new Vector2(newX, ballBounds.position.y);
      ballUnit.Bounds = ballBounds;
      var velocity = ballUnit.Velocity;
      ballUnit.Velocity = new Vector2(-velocity.x, velocity.y);
    }
  }
  private static void UpdatePlayerPosition(Unit player, float deltaTime, Vector2 ballPosition)
  {
    var pos = player.Position;
    float distance = Math.Abs(pos.y - ballPosition.y);
    distance = Math.Min(player.Velocity.y * deltaTime, distance);
    pos.y += pos.y > ballPosition.y ? -distance : distance;
    player.Position = pos;
  }

  private static void UpdateBall(Unit ballUnit, float deltaTime, Rect bounds)
  {
    // update the position of the ball.
    var ballPos = ballUnit.Position;
    var velocity = ballUnit.Velocity;
    if (bounds.Contains(ballPos + velocity * deltaTime))
    {
      // move the ball.
      ballPos += ballUnit.Velocity * deltaTime;
      ballUnit.Position = ballPos;
    }
    else
    {
      //change ball direction.
      var outsideBall = ballPos + ballUnit.Velocity;
      if (outsideBall.x > bounds.xMin || outsideBall.x < bounds.xMax)
      {
        //change direction:
        ballUnit.Velocity = new Vector2(-velocity.x, velocity.y);
      }
      if (outsideBall.y < bounds.yMin || outsideBall.y > bounds.yMax)
      {
        //change direction:
        ballUnit.Velocity = new Vector2(velocity.x, -velocity.y);
      }
    }
  }
}
