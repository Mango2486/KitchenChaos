using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeAnimation : MonoBehaviour
{
   [SerializeField]private Player m_player;
   private Animator _animatior;
   private const string IS_WALKING = "IsWalking";
   private void Awake()
   {
      _animatior = GetComponent<Animator>();
   }

   private void Update()
   {
      _animatior.SetBool(IS_WALKING,m_player.IsWalking());
   }
}
