using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

   public event EventHandler OnStateChanged;
   public event EventHandler OnGamePaused;
   public event EventHandler onGameUnPaused;

   private enum State
   {
      WaitingToStart,
      CountdownToStart,
      GamePlaying,
      GameOver,
   }

   private State state;
   
   [SerializeField]private float countdownToStartTimer = 3f;
   [SerializeField]private float gamePlayingTimerMax = 10f;
   private float gamePlayingTimer;
   private bool isGamePaused = false;
   
   private void Awake()
   {
      state = State.WaitingToStart;
      Instance = this;
   }

   private void Start()
   {
      GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
      GameInput.Instance.OnInteractActoin += GameInput_OnInteractActoin;
   }

   private void GameInput_OnInteractActoin(object sender, EventArgs e)
   {
      if (state == State.WaitingToStart)
      {
         state = State.CountdownToStart;
         OnStateChanged?.Invoke(this,EventArgs.Empty);
      }
   }

   private void GameInput_OnPauseAction(object sender, EventArgs e)
   {
      TogglePauseGame();
   }

   public void TogglePauseGame()
   {
      isGamePaused = !isGamePaused;
      if (isGamePaused)
      {
         Time.timeScale = 0f;
         OnGamePaused?.Invoke(this,EventArgs.Empty);
      }
      else
      {
         Time.timeScale = 1f;
         onGameUnPaused?.Invoke(this,EventArgs.Empty);
      }

   }

   private void Update()
   {
      switch (state)
      {
         case State.WaitingToStart:
         

            break;
         case State.CountdownToStart:
            countdownToStartTimer -= Time.deltaTime;
            if (countdownToStartTimer < 0f)
            {
               state = State.GamePlaying;
               gamePlayingTimer = gamePlayingTimerMax;
               OnStateChanged?.Invoke(this, EventArgs.Empty);

            }

            break;
         case State.GamePlaying:
            gamePlayingTimer-= Time.deltaTime;
            if (gamePlayingTimer < 0f)
            {
               state = State.GameOver;
               OnStateChanged?.Invoke(this, EventArgs.Empty);
            }

            break;
         case State.GameOver:
            break;
      }
   }

   public bool IsGamePlaying()
   {
      return state == State.GamePlaying;
   }

   public bool IsCountdownToStartActive()
   {
      return state == State.CountdownToStart;
   }

   public float GetCountdownToStartTimer()
   {
      return countdownToStartTimer;
   }

   public bool IsGameOver()
   {
      return state == State.GameOver;
   }

   public float GetPlayingTimerNomalized()
   {
      return 1-(gamePlayingTimer / gamePlayingTimerMax);
   }
}
