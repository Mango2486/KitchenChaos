using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

   public event EventHandler OnStateChanged;

   private enum State
   {
      WaitingToStart,
      CountdownToStart,
      GamePlaying,
      GameOver,
   }

   private State state;

   [SerializeField]private float watingToStartTimer = 1f;
   [SerializeField]private float countdownToStartTimer = 3f;
   [SerializeField]private float gamePlayingTimerMax = 10f;
   private float gamePlayingTimer;

   private void Awake()
   {
      state = State.WaitingToStart;
      Instance = this;
   }

   private void Update()
   {
      switch (state)
      {
         case State.WaitingToStart:
            watingToStartTimer -= Time.deltaTime;
            if (watingToStartTimer < 0f)
            {
               state = State.CountdownToStart;
               OnStateChanged?.Invoke(this, EventArgs.Empty);
            }

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

      Debug.Log(state);
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
