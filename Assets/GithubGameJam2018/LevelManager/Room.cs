using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    public SpawnPoint[] spawning;

    private enum RoomState 
    {
        Idle,
        PlayerInside
    }

    private RoomState state;

    /// <summary>
    /// When the player enters the room
    /// </summary>
    public void Enter()
    {
        if(state == RoomState.PlayerInside) 
        {
            //Cannot re-enter room that already has player inside
            return;
        }

        state = RoomState.PlayerInside;

        //Spawn all the enenmies in the room
        foreach (var sp in spawning)
        {
            sp.Spawn();
        }
    }

    public void Exit()
    {
        state = RoomState.Idle;
    }
}
