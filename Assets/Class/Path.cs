using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDAA{
    /// <summary>
    /// Represent the result of a pathfinding
    /// </summary>
    public struct Path 
    {  
        private List<GridPosition> _way;
        public List<GridPosition> way{get{return _way;}}
        
        public Path(List<GridPosition> p_way){
            this._way = p_way;
        }

        public bool Empty(){
            if(_way == null){
                return true;
            }
            return (_way.Count == 0);
        }

        public void DebugGetContent(string s = ""){
            foreach(var gp in _way){
                s += "line : " + gp.line + ", col : " + gp.col;
            }
            Utilitary.Log(s);
        }

        
    }
}
