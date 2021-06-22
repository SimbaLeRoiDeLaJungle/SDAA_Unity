using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDAA{
    public class Seeker: MonoBehaviour
    {
        public Path path;
        public int id;
        private bool isReady=false;

        void Start(){
            path = new Path();
        }

        void Update(){
            if(PathManager.IsReady() && !isReady){
                PathManager.AddSeeker(this);
                isReady = true;
            }

        }

        public void SetPath(Path path){
            this.path = path;
        }

        public void StartPath(GridPosition start, GridPosition dest){
            PathRequest req = new PathRequest(start,dest,id);
            PathManager.AddToQueue(req);
        }

        public bool IsReady(){
            return isReady;
        }

        void OnDrawGizmosSelected(){
            if(!IsReady() || path.way == null){
                return;
            }
            Gizmos.color = new Color(255,0,0,100);

            foreach(GridPosition gp in path.way){
                float ss = Pathfinder.Instance.SquareSize;
                Debug.Log("line : " + gp.line + " col : " + gp.col);
                Vector2 centerSquare = Pathfinder.MetricToWorldPoint(gp)+ new Vector2(ss,-ss)*0.5f; 
                Gizmos.DrawCube(centerSquare,new Vector3(ss,ss,1));
            }
            
        }

    }
}