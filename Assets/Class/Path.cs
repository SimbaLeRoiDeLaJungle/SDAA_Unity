using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDAA{
    public struct Path 
    {  
        public Stack<GridPosition> way;

        public Path(Stack<GridPosition> way){
            this.way = way;
        }
    }
}
