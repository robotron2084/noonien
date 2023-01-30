using System;
using System.Collections;
using System.Collections.Generic;
using Code.Data;
using com.enemyhideout.noonien;

using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
  private float _paddleSpeed = 5.0f;
  private World _world;
  private Unit _ballUnit;
  private Unit _player1Unit;
  private Unit _player2Unit;
  private float _ballSpeed = 10.0f;

  public static NodeManager NodeManager { get; set; }
  
  void Awake()
  {
    var notifyManager = GetComponent<NotifyManager>();
    Root = new Node(notifyManager, "Root");

    NodeManager = new NodeManager(Root);
    
    var players = Root.AddNewChild("Players");
    var player1 = MakePlayer(new Vector2(-5, 0), _paddleSpeed, "Player One", players);
    var player2 = MakePlayer(new Vector2(5, 0), _paddleSpeed, "Player Two", players);
    var ball = MakeBall(_ballSpeed, Root);
    var world = Root.AddNewChild("World");
    _world = world.AddElement<World>();
    _world.Bounds = new Rect(new Vector2(-6,-3), new Vector2(6*2,3*2));

    _ballUnit = ball.GetElement<Unit>();
    _player1Unit = player1.GetElement<Unit>();
    _player2Unit = player2.GetElement<Unit>();

#if UNITY_EDITOR
    com.enemyhideout.noonien.editor.NodeGraphEditor.Root = Root;
#endif

    
  }

  public static Node Root { get; set; }

  private static Node MakePlayer(Vector2 position, float speed, string name, Node parent)
  {
    var player = parent.AddNewChild(name);
    player.AddElement<Player>();
    var unit = player.AddElement<Unit>();
    unit.Bounds = new Rect(position, new Vector2(0.125f, 1f));
    unit.Velocity = new Vector2(0, Random.Range(0.5f * speed, speed));
    return player;
  }

  private static Node MakeBall(float ballSpeed, Node root)
  {
    var ball = root.AddNewChild("Ball");
    var ballPosition = ball.AddElement<Unit>();
    ballPosition.Bounds = new Rect(new Vector2(0, 0), new Vector2(0.125f, 0.125f));
    ballPosition.Velocity = new Vector2(Random.Range(0,ballSpeed), Random.Range(0,ballSpeed));
    return ball;
  }

  void Start()
  {

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
        yield return LoseLife();
        yield break;
      }

      yield return null;
    }
  }

  private IEnumerator LoseLife()
  {
    var losingUnit = _ballUnit.Position.x > 0 ? _player2Unit : _player1Unit;
    var loser = losingUnit.Parent.GetElement<Player>();
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

  private void GameOver(Node winner)
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
