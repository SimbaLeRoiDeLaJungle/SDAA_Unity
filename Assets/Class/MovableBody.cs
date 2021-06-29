using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SDAA{
    public class MovableBody : MonoBehaviour
    {
        private GridTransform _gridTransform;
        public GridTransform gridTransform{get{return _gridTransform;}}
        private float velocity;
        private GridPosition? _target;
        public GridPosition? target{get{return _target;}}
        void Start()
        {
            _gridTransform = GetComponent<GridTransform>();
            if(_gridTransform == null){
                gameObject.AddComponent(typeof(GridTransform));
            }
            _gridTransform = GetComponent<GridTransform>();
            gridTransform.position = gridTransform.gridPosition;
            
        }

        void Awake(){
            
        }

        void FixedUpdate()
        {
            if(target == null){
                return;
            }
            GridPosition t = target.Value;
            float distance = Utilitary.DistanceInMetric(gridTransform.position, _target.Value);
            float ndv = velocity*Time.fixedDeltaTime;
            bool arrived = distance <= ndv;
            if(arrived){
                int dl = 0;
                int dc = 0;
                if((gridTransform.internPos.line>= 1-ndv)){
                    dl = 1;
                }
                if(gridTransform.internPos.col >= 1-ndv){
                    dc = 1;
                }
                gridTransform.position = gridTransform.gridPosition + new GridPosition(dl , dc);
                Debug.Log(gridTransform.position.ToString());
                _target = null;
                velocity = 0;
                return;
            }
            Direction direction = Direction.NONE;
            
            Utilitary.SetDirectionByDest(gridTransform.position, t, ref direction);
            Debug.Log((int)direction);
            GridPositionFloat dv = Utilitary.DirectionToGridPositionFloat(direction)*ndv;
            gridTransform.position = gridTransform.position + dv;
            
        }

        public void SetVelocity(float velocity){
            this.velocity = velocity;
        }

        public void MoveTo(GridPosition p_target, float p_velocity){
            _target = p_target;
            velocity = p_velocity;
        }


    }
}
