using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
namespace SDAA{
    /// <summary>
    /// The implementation of the threading systeme (ThreadPool)
    /// </summary>
    public class PathManager : MonoBehaviour
    {

        private static PathManager instance;
        private static List<Seeker> seekers;
        private static List<int> cmp;
        private static int seekerId=0;
        
        void Start(){

        }

        void Awake(){
            if(instance == null){
                instance = this;
                seekers = new List<Seeker>();
                cmp = new List<int>();
            }
            else{
                // throw error
            }
        }

        public static void AddToQueue(PathRequest request){
            ThreadPool.QueueUserWorkItem(new WaitCallback(MakePath),request);
        }

        private static void MakePath(object request){
            PathRequest req = (PathRequest)request;
            int index = IdToIndex(req.id);
            if(cmp[index]>30){
                Path path = Pathfinder.Pathfinding(req.start,req.dest);
                if(index != -1){
                    seekers[index].SetPath(path);
                }
                cmp[req.id] = 0;
            }
        }

        private static int IdToIndex(int id){
            for(int i = 0; i<seekers.Count ; i++){
                if(seekers[i].id == id){
                    return i;
                }
            }
            return -1;
        }

        public static void AddSeeker(Seeker seeker){
            seeker.id = seekerId;
            seekers.Add(seeker);
            cmp.Add(0);
            seekerId++;
        }
        public static bool IsReady(){
            return instance != null;
        }

        void Update(){
            for(int i = 0; i <cmp.Count; i++){
                cmp[i]++;
            }

        }
    }
}