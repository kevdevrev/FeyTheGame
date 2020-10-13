using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    //this interface is for any objects that take damage
    //this includes enemies, Fey, and objects in the world.
    //This is a contract to implement everything below.
    int Health { get; set; }
    void Damage(int dmgTaken);
}
