using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTree : MonoBehaviour
{
    public int life;
    public bool canAttack;
    public bool canRaid;

    void InitializeTree()
    {
        ActionNode dead = new ActionNode(() => print("Dead"));
        ActionNode attack = new ActionNode(() => print("Patrol"));
        ActionNode raid = new ActionNode(() => print("Raid"));

        QuestionNode qCloseToBuilding = new QuestionNode(QuestionCloseToBuilding,attack, raid);
        QuestionNode qHasLife = new QuestionNode(() => life > 0, qCloseToBuilding, dead);
    }

    public bool QuestionCloseToBuilding()
    {
        //Detectar cuando el enemigo se encuentra cerca de una estructura para pegarle o seguir caminando
        return false;
    }
    
    
}
