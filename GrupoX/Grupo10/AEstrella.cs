using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.DataStructures;
using Assets.Scripts;

namespace Assets.Scripts.Grupo10
{
    public class Nodo
    {
        //int columna; //ya están en cell
        //int fila;
        public float g;
        public float h;
        public float f;
        public Nodo Parent;
        public CellInfo cell;
        public Locomotion.MoveDirection ProducedBy;

        public Nodo(float ga,float ha, float fa, Nodo Parenta, CellInfo cella, Locomotion.MoveDirection locomia) 
        {
            this.g = ga;
            this.h = ha;
            this.f = fa;
            this.Parent = Parenta;
            this.cell = cella;
            this.ProducedBy = locomia; 
        }
    }
    public class AEstrella : AbstractPathMind
    {
        // declarar Stack de Locomotion.MoveDirection de los movimientos hasta llegar al objetivo
        private Stack<Locomotion.MoveDirection> currentPlan = new Stack<Locomotion.MoveDirection>();  //pila donde se almacenan los mov.     

        public override void Repath()
        {
            // limpiar Stack 
            currentPlan.Clear();
        }

        public override Locomotion.MoveDirection GetNextMove(BoardInfo board, CellInfo currentPos, CellInfo[] goals)
        {
            // calcular camino, devuelve resultado de A*
            var searchResult = Search(board, currentPos, goals);

            // recorre searchResult and copia el camino a currentPlan
            while (searchResult.Parent != null)
            {
                currentPlan.Push(searchResult.ProducedBy); 
                searchResult = searchResult.Parent;
            }

            // returns next move (pop Stack)
            if (currentPlan.Count != 0) //si la Stack no está vacía, hacer siguiente movimiento
                return currentPlan.Pop();   

            return Locomotion.MoveDirection.None;

        }

        private Nodo Search(BoardInfo board, CellInfo start, CellInfo[] goals) //ANTES PONIA Node
        {
            // crea una lista vacía de nodos
            var open = new List<Nodo>();  //está vacia

            // node inicial
            float g = 0;
            float h = Mathf.Abs(goals[0].ColumnId - start.ColumnId) + Mathf.Abs( goals[0].RowId - start.RowId);
            float f = h + g;
            //Nodo parent;////// ¿hay que hacer algo? El primer nodo padre es vacío
            Nodo Parent = null;
            CellInfo cell = start;
            Locomotion.MoveDirection locomia = Locomotion.MoveDirection.None;
            var n = new Nodo(g,h,f,Parent,cell,locomia);

            // añade nodo inicial a la lista
            open.Add(n);

            // mientras la lista no esté vacia
            int counterStrike = 0;                                                                                                             //////////////////////quitar luego
            while (open.Count != 0 && counterStrike < 200) //la cuenta de la lista sea distinto de vacio
            {
                // mira el primer nodo de la lista
                var nAux = open[0];
                open.RemoveAt(0);//elimina el primer elemento y mueve el resto de elemento una posición

                // si el primer nodo es goal, returns current node
                if (nAux.h == 0)
                {
                    return nAux;
                }
                // expande vecinos (calcula coste de cada uno, etc) y los añade en la lista
                var neighbours = nAux.cell.WalkableNeighbours(board); //array de vecinos
                var id = 0;
                while (id < neighbours.Length) { //recorrer array de vecinos
                    if(neighbours[id] != null) { //cuando no sean nulos, se añaden
                        //falta bucle para el array y q filtre ciclos  //->no añadir ningún nodo existente en el árbol al conjunto de sucesores
                        float g2 = 0; ///
                        float h2 = Mathf.Abs(goals[0].ColumnId - start.ColumnId) + Mathf.Abs(goals[0].RowId - start.RowId);
                        float f2 = h + g;  ///
                        //Nodo parent;////// ¿hay que hacer algo? El primer nodo padre es vacío
                        Nodo Parent2 = null;
                        CellInfo cell2 = start;
                        Locomotion.MoveDirection locomia2 = Locomotion.MoveDirection.None;  ////CALCULAR CUAL ES EL MOVIMIENTO DEL PAPA AL HIJO

                        var n2 = new Nodo(g2, h2, f2, Parent2, cell2, locomia2);

                        id++;
                        open.Add(n2);
                    }
                }
                
                // ordena lista
                open.Sort();
                counterStrike++;                                                                                                        //////////////////////quitar luego
            }
            return null;
        }
    }
}
