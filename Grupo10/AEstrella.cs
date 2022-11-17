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
           // var estadosRepetidos = new List<Nodo>();  //está vacia


            // node inicial
            float g = 0;
            float h = Mathf.Abs(goals[0].ColumnId - start.ColumnId) + Mathf.Abs( goals[0].RowId - start.RowId);
            float f = h + g;
            //Nodo parent;////// ¿hay que hacer algo? El primer nodo padre es vacío
            Nodo Parent = null;
            CellInfo cell = start;
            Locomotion.MoveDirection locomia = Locomotion.MoveDirection.None;
            Nodo n = new Nodo(g,h,f,Parent,cell,locomia);

            // añade nodo inicial a la lista
            open.Add(n);
            //estadosRepetidos.Add(n);

            // mientras la lista no esté vacia
            while (open.Count > 0 ) //la cuenta de la lista sea distinto de vacio    while (open.Count != 0 )
            {
                // mira el primer nodo de la lista
                Nodo nAux = open[0];
                open.RemoveAt(0);//elimina el primer elemento y mueve el resto de elemento una posición

                // si el primer nodo es goal, returns current node
                if (nAux.h == 0)
                {
                    return nAux;
                }
                // expande vecinos (calcula coste de cada uno, etc) y los añade en la lista
                var neighbours = nAux.cell.WalkableNeighbours(board); //array de vecinos


                // Debug.Log("tamaño array vecinos = " + neighbours.Length);
                // Debug.Log("lista abierta1: " + open.Count);

                bool vecinoRepetido;
                for (int i = 0; i < neighbours.Length; i++)
                { //recorrre array vecinos
                    vecinoRepetido = false; 
                    if (neighbours[i] != null)
                    { //cuando no sean nulos, se añaden
                        for (int j = 0; j < open.Count; j++)
                        {
                            if (neighbours[i].RowId == open[j].cell.RowId && neighbours[i].ColumnId == open[j].cell.ColumnId)//comparas si en algun momento ha estado en la misma posicion
                            {
                                vecinoRepetido = true;
                            }
                        }
                        if (vecinoRepetido != true)
                        {
                            ////////////falta bucle para el array y q filtre ciclos  //->no añadir ningún nodo existente en el árbol al conjunto de sucesores
                            float g2 = nAux.g + 1; ///
                            float h2 = Mathf.Abs(goals[0].ColumnId - nAux.cell.ColumnId) + Mathf.Abs(goals[0].RowId - nAux.cell.RowId);
                            float f2 = h + g; 
                            Nodo Parent2 = nAux;
                            CellInfo cell2 = neighbours[i];

                            //MOVIMIENTO Q HARÁ
                            Locomotion.MoveDirection locomia2 = Locomotion.MoveDirection.None;
                            if (nAux != null && cell2.ColumnId > nAux.cell.ColumnId)
                            {
                                locomia2 = Locomotion.MoveDirection.Right;  /// MoveDirection.None ////CALCULAR CUAL ES EL MOVIMIENTO DEL PAPA AL HIJO
                            }
                            else if (nAux != null && cell2.RowId > nAux.cell.RowId)
                            {
                                locomia2 = Locomotion.MoveDirection.Up;  /// MoveDirection.None ////CALCULAR CUAL ES EL MOVIMIENTO DEL PAPA AL HIJO
                            }
                            else if (nAux != null && cell2.RowId < nAux.cell.RowId)
                            {
                                locomia2 = Locomotion.MoveDirection.Down;  /// MoveDirection.None ////CALCULAR CUAL ES EL MOVIMIENTO DEL PAPA AL HIJO
                            }
                            else if (nAux != null && cell2.ColumnId < nAux.cell.ColumnId)
                            {
                                locomia2 = Locomotion.MoveDirection.Left;  /// MoveDirection.None ////CALCULAR CUAL ES EL MOVIMIENTO DEL PAPA AL HIJO
                            }

                            Debug.Log("Eustaqui: ");
                            Nodo n2 = new Nodo(g2, h2, f2, Parent2, cell2, locomia2);

                            open.Add(n2);
                            
                            // Debug.Log("contador : " + contador); //////////////////////////////////
                            Debug.Log("lista abierta: " + open.Count);

                        }
                    }
                    // ordena lista
                    //ordInsertar(n, abierta, f *)
                    //public void Sort(int index, int count, IComparer<T> comparer);
                    //index:Índice inicial de base cero del intervalo que se va a ordenar.
                    //count:Longitud del intervalo que se va a ordenar.
                    //comparer:Implementación de System.Collections.Generic.IComparer`1 que se va a utilizar
                    //     al comparar elementos, o null para utilizar el comparador predeterminado System.Collections.Generic.Comparer`1.Default.                    

                    //open.Sort(0,open.Count,null);
                   //ORDENAR POR METODO BURBUJA
                   
                    for(int k=0; k < open.Count; k++)
                    {
                        for(int j=1; j < open.Count; j++)
                        {
                            if (open[j].f < open[k].f)
                            {
                                Nodo nodoextra = open[j];
                                open[j] = open[k];
                                open[k] = nodoextra;
                            }
                        }
                    }
                
               
                }


            }


            return null;
        }

    }
}
