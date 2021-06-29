using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SDAA{
    public class Seeker: MonoBehaviour
    {
        public Path _path;
        public int id;
        private bool isReady=false;
        public delegate void OnPathSet(Path path);
        private OnPathSet ops;

        void Update(){
            if(PathManager.IsReady() && !isReady){
                PathManager.AddSeeker(this);
                isReady = true;
            }

        }
        public void SetOPS(OnPathSet _ops){
            ops = new OnPathSet(_ops);
        }

        public void StartPath(GridPosition start, GridPosition dest){
            PathRequest req = new PathRequest(start,dest,id);
            PathManager.AddToQueue(req);
        }

        public bool IsReady(){
            return isReady;
        }

        public void SetPath(Path receivePath){
            if(ops != null){
                ops(receivePath);
            }
        }

    }
}