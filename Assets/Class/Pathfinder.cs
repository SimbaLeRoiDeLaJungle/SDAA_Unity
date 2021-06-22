using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDAA{
    public class Pathfinder : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float squareSize;
        public int Width {get{return width;}}
        public int Height {get{return height;}}
        public float SquareSize {get{return squareSize;}}
        private static Pathfinder instance;
        private bool[,] walkables;
        
        void Start(){

            walkables = new bool[height+1,width+1];
            for(int line = 0 ; line <= height ; line++){
                for(int col = 0; col <= width ; col++){
                    walkables[line,col] = true;
                }
            }
        }

        void Awake(){
            if(instance == null){
                instance = this;
            }
            else{
                // throw error
            }
        }
        public Path Pathfinding(GridPosition start,GridPosition dest){
            /*----------------- initializer ----------------------*/
            Node[,] nodes = new Node[height+1,width+1];
            bool[,] inOpen = new bool[height+1,width+1];
            bool[,] inClose = new bool[height+1,width+1];
            for(int line = 0; line <= height ; line++){
                for(int col = 0; col <= width ; col++){
                    nodes[line,col] = new Node(line,col);
                    inOpen[line,col] = false;
                    inClose[line,col] = false;
                }
            }
            AStarsPriorityQueue openList = new AStarsPriorityQueue(width*height);
            nodes[start.line,start.col].g = 0;
            nodes[start.line,start.col].h = CalculH(nodes[start.line,start.col],dest);
            nodes[start.line,start.col].f = nodes[start.line,start.col].h;
            if(dest.col == start.col && start.line == dest.line){
                return new Path();
            }

            /* ------------------ loop ------------------------*/    
            int h = nodes[start.line,start.col].h;
            GridPosition currentPos = start;
            while(h!= 0){

                List<GridPosition> nearNodesPos = GetNearNodes(currentPos);
                // calculation of g,h & f cost  
                for(int i = 0 ; i< nearNodesPos.Count ; i++){
                    int line = nearNodesPos[i].line;
                    int col = nearNodesPos[i].col;
                    if(!inOpen[line,col] && !inClose[line,col] && walkables[line,col]){
                        // some calcul and Enqueue in the open set
                        nodes[line,col].parentLine = nodes[currentPos.line,currentPos.col].line;
                        nodes[line,col].parentCol = nodes[currentPos.line,currentPos.col].col;
                        int parentG = nodes[nodes[line,col].parentLine,nodes[line,col].parentCol].g;
                        nodes[line,col].g = CalculG(nodes[line,col],parentG);
                        nodes[line,col].h = CalculH(nodes[line,col], dest);
                        nodes[line,col].f = nodes[line,col].g + nodes[line,col].h;
                        inOpen[line,col] = true;
                        openList.Enqueue(ref nodes[line,col]);
                    }
                    else if(inOpen[line,col])
                    {
                        // some calcul and Update the open set
                        int parentLine = nodes[currentPos.line,currentPos.col].parentLine;
                        int parentCol = nodes[currentPos.line,currentPos.col].parentCol;
                        if((parentCol != -1) && (parentLine != -1)){
                            Node parent = nodes[parentLine,parentCol];
                            int oldParentLine = nodes[parentLine,parentCol].parentLine;
                            int oldParentCol = nodes[parentLine,parentCol].parentCol;
                            if((oldParentCol != -1) && (parentLine != -1) ){
                                Node oldParent = nodes[oldParentLine,parentCol];
                                int CGWOP = CalculG(nodes[line,col],oldParent.g);
                                if(CGWOP < nodes[line,col].g){
                                    nodes[line,col].g = CGWOP;
                                    nodes[line,col].f = nodes[line,col].g + nodes[line,col].h;
                                    Utilis.Log("pass");
                                    openList.Update(ref nodes[line,col]);
                                }
                            }
                        }
                    }
                }

                if(openList.Empty()){
                    return new Path();
                }

                inOpen[currentPos.line,currentPos.col] = false;
                inClose[currentPos.line,currentPos.col] = true;
                Node n = openList.Dequeue(false);
                currentPos.line = n.line;
                currentPos.col = n.col;
                h = n.h;
            }

            /* ------------------- end ----------------------*/
            Stack<GridPosition> way = new Stack<GridPosition>();
            way.Push(currentPos);
            while(nodes[currentPos.line,currentPos.col].parentLine != -1){
                int line = nodes[currentPos.line,currentPos.col].parentLine;
                int col = nodes[currentPos.line,currentPos.col].parentCol;

                currentPos.line = line;
                currentPos.col = col;

                way.Push(currentPos);
            }

            return new Path(way);

        }

        public static int CalculH(Node node , GridPosition dest){
            int X = Mathf.Abs(node.col-dest.col);
            int Y = Mathf.Abs(node.line-dest.line);
            return X+Y;
        }

        public static int CalculG(Node node,int parentG){
            int tempG;
            if(node.parentLine == -1){
                return 0;
            }

            tempG = parentG + 1;
            return tempG;
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



        public static Vector2 MetricToWorldPoint(GridPosition gridPosition){
            return Utilis.MetricToWorldPoint(instance,gridPosition);
        }


        public static GridPosition WorldToMetricPoint(Vector2 worldPosition){
            return Utilis.WorldToMetricPoint(instance, worldPosition);
        }

        void OnDrawGizmos(){
            if(squareSize==0){
                return;
            }
            Gizmos.color = Color.blue;
            for(int line = 0; line <= height; line++){
                Vector2 begin = Utilis.MetricToWorldPoint(this, new GridPosition(line,0) );
                Vector2 end = Utilis.MetricToWorldPoint(this, new GridPosition(line,width) );
                Gizmos.DrawLine(begin,end);
                for(int col = 0; col <= width; col++){
                    if(walkables == null){
                        break;
                    }
                    if(!walkables[line,col]){
                        float ss = Pathfinder.Instance.SquareSize;
                        Vector2 center = Pathfinder.MetricToWorldPoint(new GridPosition(line,col))+ new Vector2(ss,-ss)*0.5f;
                        Gizmos.DrawCube(center, new Vector3(squareSize,squareSize,0));
                    }
                }
            }
            for(int col = 0; col <= width; col++){
                Vector2 begin = Utilis.MetricToWorldPoint(this, new GridPosition(0,col) );
                Vector2 end = Utilis.MetricToWorldPoint(this, new GridPosition(height,col) );
                Gizmos.DrawLine(begin,end);
            }


            
        }

        public static Pathfinder Instance{get{return instance;}}

        public static void SetWalkable(GridPosition gp, bool walkable ){
            instance.walkables[gp.line,gp.col] = walkable;
        }
    }
}