using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDAA;
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(MovableBody))]
public class Character : MonoBehaviour
{
    private Seeker seeker;
    private MovableBody body;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 size;
    public Vector2 LeftCorner   {   
                                    get{
                                        return (Vector2)this.transform.position + new Vector2(-size.x, size.y)*0.5f;
                                    }
                                }
    int indexPath = 0;
    private Path path;

    void Start()
    {
        seeker = GetComponent<Seeker>();      
        body = GetComponent<MovableBody>();
        seeker.SetOPS(DefaultOnPathSet);
    }

    void DefaultOnPathSet(Path receivePath){
        path = receivePath;
        if(path.Empty()){
            return;
        }
        GridPosition gp = path.way[0];
        if(body.gridTransform.position != (GridPositionFloat)gp ){
            if(path.way.Count > 1){
                GridPosition gpNext = path.way[1];
                GridPosition delta = gpNext - gp;
                if(delta.col != 0){
                    bool rightcol = delta.col>0 && body.gridTransform.position.col > gp.col;
                    bool leftcol = delta.col < 0 && body.gridTransform.gridPosition.col < gp.col;
                    if(rightcol || leftcol){
                        indexPath = 1;
                        gp = gpNext;
                    }
                    else{
                        indexPath = 0;
                    }
                }
                if(delta.line != 0){
                    bool rightline = delta.line > 0 && body.gridTransform.position.line > gp.line;
                    bool leftline = delta.line < 0  && body.gridTransform.gridPosition.line < gp.line;
                    if(rightline || leftline){
                        indexPath = 1;
                        gp = gpNext;
                    }
                    else{
                        indexPath = 0;
                    }
                }
            }            
        }
        body.MoveTo(gp,speed);
    }

    void FixedUpdate()
    {
        if(path.Empty()){
            return;
        }
       
       if(body.target == null){
           indexPath ++;
           if(indexPath >= path.way.Count){
               path.way.Clear();
               indexPath = 0;
               return;
           }
           body.MoveTo(path.way[indexPath],speed);
       }

    }


    void Update(){
        if(Input.GetMouseButton(1)){
            Vector2 mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GridPosition dest = Pathfinder.metric.WorldToMetricPoint(mouseP);
            GridPosition start = Pathfinder.metric.WorldToMetricPoint(transform.position);
            seeker.StartPath(start,dest);
        }
        
    }


    void OnDrawGizmosSelected(){
        if(path.way == null){
            return;
        }
        Gizmos.color = new Color(255,0,0,0.5f);
        float ss = Pathfinder.metric.SquareSize;
        foreach(GridPosition gp in path.way){
            Vector2 centerSquare = Pathfinder.metric.MetricToWorldPoint(gp)+ new Vector2(ss,-ss)*0.5f; 
            Gizmos.DrawCube(new Vector3(centerSquare.x,centerSquare.y,100),new Vector3(ss,ss,1));
        }
    }
    
}
