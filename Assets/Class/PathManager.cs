using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
namespace SDAA{
    public class PathManager : MonoBehaviour
    {

        private static PathManager instance;
        private static List<Seeker> seekers;
        private static int seekerId=0;
        
        void Start(){

        }

        void Awake(){
            if(instance == null){
                instance = this;
                seekers = new List<Seeker>();
            }
            else{
                // throw error
            }
        }

        public static void AddToQueue(PathRequest request){
            ThreadPool.QueueUserWorkItem(state => MakePath(request));
        }

        private static void MakePath(object request){
            PathRequest req = (PathRequest)request;
            Path path = Pathfinder.Instance.Pathfinding(req.start,req.dest);
            int index = IdToIndex(req.id);
            if(index != -1){
                seekers[index].SetPath(path);
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
            seekerId++;
        }
        public static bool IsReady(){
            return instance != null;
        }
    }
}