using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace SDAA {
    [ExecuteInEditMode]
    public class Metric : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float squareSize;
        public int Width {get{return width;}}
        public int Height {get{return height;}}
        public float SquareSize {get{return squareSize;}}
        private bool[,] walkables;

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        void Start(){
            
            walkables = new bool[height+1,width+1];
            for(int line = 0 ; line <= height ; line++){
                for(int col = 0; col <= width ; col++){
                    walkables[line,col] = true;
                }
            }
            
        }
        void Awake(){
            Pathfinder.AddMetric(this);
            Utilitary.SetMetric(this);
        }
        public List<GridPosition> GetNearNodes(GridPosition pos){
            List<GridPosition> ns = new List<GridPosition>();
            if(pos.line-1>=0){
                ns.Add(new GridPosition(pos.line - 1, pos.col));
            }
            if(pos.line+1 < height){
                ns.Add(new GridPosition(pos.line + 1, pos.col));
            }
            if(pos.col -1 >= 0){
                ns.Add(new GridPosition(pos.line, pos.col - 1));
            }
            if(pos.col + 1 < width){
                ns.Add(new GridPosition(pos.line, pos.col + 1));
            }
                        
            return ns;
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public Vector2 MetricToWorldPoint(GridPosition gridPosition){
            return Utilitary.MetricToWorldPoint(gridPosition,this);
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public Vector2 MetricToWorldPoint(GridPositionFloat gridPositionFloat){
            return Utilitary.MetricToWorldPoint(gridPositionFloat);
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public GridPosition WorldToMetricPoint(Vector2 worldPosition){
            return Utilitary.WorldToMetricPoint(worldPosition);
        }
        
        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public GridPosition WorldToMetricPoint(Vector2 worldPosition, ref GridPositionFloat internPos){
            return Utilitary.WorldToMetricPoint(worldPosition, ref internPos);
        }
        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/        
        public void SetWalkable(GridPosition gp, bool walkable ){
            walkables[gp.line,gp.col] = walkable;
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        void OnDrawGizmos(){
            
            if(squareSize==0){
                return;
            }
            Gizmos.color = new Color(0,255,246,0.3f);
            GUIStyle style = new GUIStyle();
            style.fontSize = 10;
            style.normal.textColor = Gizmos.color;
            for(int line = 0; line <= height; line++){
                Vector2 begin = MetricToWorldPoint(new GridPosition(line,0) );
                Vector2 end = MetricToWorldPoint(new GridPosition(line,width) );
                Gizmos.DrawLine(begin,end);
                for(int col = 0; col <= width; col++){
                    if(walkables == null){
                        break;
                    }
                    if(!walkables[line,col]){
                        float ss = Pathfinder.metric.SquareSize;
                        Vector2 center = MetricToWorldPoint(new GridPosition(line,col))+ new Vector2(ss,-ss)*0.5f;
                        Gizmos.DrawCube(center, new Vector3(squareSize,squareSize,0));
                    }
                    if(line < height && col < width){
                        Vector3 screenPos = (Vector3)MetricToWorldPoint(new GridPosition(line,col));
                        Handles.Label(screenPos, line + "\n  " + col,style);
                    }
                }
            }
            for(int col = 0; col <= width; col++){
                Vector2 begin = MetricToWorldPoint(new GridPosition(0,col) );
                Vector2 end = MetricToWorldPoint(new GridPosition(height,col) );
                Gizmos.DrawLine(begin,end);
            }
        }
        void OnGUI()
        {

        }

        public bool Walkables(int line, int col){
            return walkables[line,col];
        }
    }
}