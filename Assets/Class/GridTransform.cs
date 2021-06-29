using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDAA{
    public class GridTransform : MonoBehaviour
    {
        // >è_é< - >è_é< - >è_é< - >è_é< - >è_é< - >è_é< - >è_é<
        private GridPositionFloat _position;
        private float _size;

        // -- # -- # -- # -- # -- # -- # -- # -- # -- # -- #
        public float size{get{return _size;}set{_size = value;}}
        
        // -- # -- # -- # -- # -- # -- # -- # -- # -- # -- #
        public Vector2 leftCorner{get{return (Vector2)transform.position + new Vector2(-_size, _size)*0.5f;}} 
        
        // -- # -- # -- # -- # -- # -- # -- # -- # -- # -- #
        public GridPositionFloat position{  
                                            get{return _position;}
                                            set{_position = value;}
                                        }
        
        // -- # -- # -- # -- # -- # -- # -- # -- # -- # -- #
        public GridPosition gridPosition{get{
                                                int line = (int)(_position.line);
                                                int col = (int)(_position.col);
                                                return new GridPosition(line,col);
                                            }
                                        }
        
        // -- # -- # -- # -- # -- # -- # -- # -- # -- # -- #
        public GridPositionFloat internPos{get{
                                                float line = _position.line - gridPosition.line;
                                                float col = _position.col - gridPosition.col;
                                                return new GridPositionFloat(line,col);
                                                }
                                            }
        // <~o~> <~o~> <~o~> <~o~> <~o~> <~o~> <~o~> <~o~> <~o~> 
        void Awake()
        {
            _position = Utilitary.WorldToMetricPoint(leftCorner);
            _size = Pathfinder.metric.SquareSize;   
        }

        // <~o~> <~o~> <~o~> <~o~> <~o~> <~o~> <~o~> <~o~> <~o~>     
        void Update()
        {
            transform.position = Utilitary.MetricToWorldPoint(_position)+ new Vector2(_size,-_size)*0.5f;
        }

    }
}