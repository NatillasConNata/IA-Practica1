using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts.SampleMind
{
    public class Amplitud : AbstractPathMind
    {
        //habra que crear la clase nodo y el arbol to guapo

        class Nodo
        {
            public Nodo nodo;
            int coste = 1;
        }
        public override void Repath()
        {

        }

        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            //en este apartado se implementa la IA

            //mirar si hay plan
            //ejecutar el a*
            //guardar cadena de mov
            //cada q se ejecuta next move, se hace el movimiento

            var val = Random.Range(0, 4);
            if (val == 0) return Locomotion.MoveDirection.Up;
            if (val == 1) return Locomotion.MoveDirection.Down;
            if (val == 2) return Locomotion.MoveDirection.Left;
            return Locomotion.MoveDirection.Right;
        }
    }
}
