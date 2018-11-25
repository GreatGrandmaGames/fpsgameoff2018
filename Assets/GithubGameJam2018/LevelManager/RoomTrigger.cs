using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour 
{
    public enum Trigger 
    {
        Enter,
        Exit
    }

    public Trigger trigger;

    public Room room;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            return;
        }

        if(room != null)
        {
            switch(trigger) 
            {
                case Trigger.Enter:
                    room.Enter();
                    break;
                case Trigger.Exit:
                    room.Exit();
                    break;
            }
        }
    }
}
