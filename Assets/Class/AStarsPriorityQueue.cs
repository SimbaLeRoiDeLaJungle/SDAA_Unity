using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDAA{
    /// <summary>
    /// Priority Queue adapted for the AstarsPathFinding with a binary heap base.
    /// NB : /!\  This priority queue do not stock the Node, it stock pointer to the Node. You have to store all nodes in an other dataStructure 
    /// and passing the node by reference in the Priority.Enqueue methods.
    /// The goal of this class is to give you the most speedly i can, the node of smallest f cost in the Astars pathFinding 
    /// </summary>
    /// <remarks>
    /// Thanks to programmersought.com for the base of this implementation ==> https://www.programmersought.com/article/85405734627/
    /// </remarks>
    public class AStarsPriorityQueue
    {
        unsafe private Node*[] _elements;
        private int _count;
        private const int DEFAULT_CAPACITY = 6;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// The default capacity of _elements is 6, this value can be asign for winning little time at begining, see PriorityQueue.ShrinkStore() and PriorityQueue.ResizeItemStore().
        /// </remarks>
        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public AStarsPriorityQueue() : this(DEFAULT_CAPACITY) { }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        unsafe public AStarsPriorityQueue(int begin_capacity){
            if(begin_capacity<0){
                throw new System.IndexOutOfRangeException();
            }
            _elements = new Node*[begin_capacity];
        }
        
        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        /// <summary>
        /// Stock data in to the queue
        /// </summary>
        unsafe public bool Enqueue(ref Node node){
            // if the size is to small we resize it
            if(_count >= _elements.Length-1){
                ResizeItemStore(_elements.Length*2);
            }
            
            // Add the new node at the end of the array
            fixed(Node* nodePtr = &node){
                _elements[_count] = nodePtr;
            }
            
            _count++;

            // Maintain the structure of priority queue
            int position = BubbleUp(_count -1);
            return (position == 0);
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        /// <summary>
        /// Get data from the queue, if shrink the inerit-array will be resize if needed.
        /// </summary>
        unsafe public Node Dequeue(bool shrink=true){
            // you can't Dequeue if the array is empty
            if(Empty()){
                throw new System.InvalidOperationException();
            }

            // taking the first elements 
            Node* result = _elements[0];
            if(_count == 1){
                // the array is empty
                _count =0;
                _elements[0] = null;
            }
            else{
                _count--;
                // pass the last element in first
                _elements[0] = _elements[_count];
                _elements[_count] = null;

                // Maintain the structure of priority queue, the BubbleDown will take the first elements and relax it (in the binary tree) if it's needed.
                BubbleDown();
                
            }

            if(shrink){
                ShrinkStore();
            }

            return (*result);
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        /// <summary>
        /// Update a node in the queue. return true if update is done, and false if the Node is not found. /!\ can be slow if the array is big.
        /// </summary>
        unsafe public bool Update(ref Node node){
            for(int i = 0 ; i < _count ; i++){
                if((*_elements[i]) == node){
                    int parent = (i-1)>>1;
                    int leftChild = (i<<1) + 1;
                    int rightChild = leftChild + 1;
                    if(parent >=0){
                        if((_elements[i]->f-_elements[parent]->f) < 0){
                            BubbleUp(i);
                        }
                    }
                    if(leftChild<_count && (_elements[i]->f - _elements[leftChild]->f) < 0 ){
                        BubbleDown(i);
                    }
                    else if(rightChild<_count && (_elements[i]->f - _elements[rightChild]->f) < 0 ){
                        BubbleDown(i);
                    }
                    return true;
                }
            }
            return false;
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public int Count{
            get{
                return _count;
            }
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        public bool Empty(){
            return _count == 0;
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        unsafe public void Clear(int begin_capacity = DEFAULT_CAPACITY){
            _count = 0;

            _elements = new Node*[begin_capacity];
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        /// <summary>
        /// Rezize stuff
        /// </summary>
        private void ShrinkStore(){
            if(_elements.Length > DEFAULT_CAPACITY && _count < (_elements.Length>>1)){
                
                int newSize = Mathf.Max(DEFAULT_CAPACITY, (((_count / DEFAULT_CAPACITY) + 1) * DEFAULT_CAPACITY));

                ResizeItemStore(newSize);
            }
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        /// <summary>
        /// Rezize stuff
        /// </summary>
        unsafe private void ResizeItemStore(int newSize){
            if(_count < newSize || DEFAULT_CAPACITY <= newSize){
                return;
            }
            Node*[] temp = new Node*[newSize];
            System.Array.Copy(_elements,0,temp,0,_count);
            _elements = temp;
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        /// <summary>
        /// maintain the binaryHeap structure by down
        /// </summary>
        unsafe private void BubbleDown(int startIndex = 0){
            int parent = startIndex;

            int leftChild = (parent << 1) +1;
            while(leftChild < _count){
                int rightChild = leftChild +1;
                int bestChild = (rightChild < _count && (_elements[rightChild]->f - _elements[leftChild]->f) < 0)? rightChild : leftChild;
                if( _elements[bestChild]->f < _elements[parent]->f ){
                    //switch parent with bestchild
                    Node* temp = _elements[parent];
                    _elements[parent] = _elements[bestChild];
                    _elements[bestChild] = temp;
                    parent = bestChild;
                    leftChild = (parent<<1) +1;
                }
                else{
                    break;
                }
            }
        }

        /*--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--#--*/
        /// <summary>
        /// maintain the binaryHeap structure by up
        /// </summary>
        unsafe private int BubbleUp(int startIndex){
            int index = startIndex;
            while(index >0){
                int parent = (index-1)>>1;

                if( _elements[index]->f<_elements[parent]->f ){
                    //switch parent with current index
                    Node* temp = _elements[index];
                    _elements[index] = _elements[parent];
                    _elements[parent] = temp;
                }
                else{
                    break;
                }
                index = parent;
            }
            return index;
        }

        unsafe public void DebugGetContentInLog(string before = ""){
            string result = "------------------------"; 
            result += before + " ::: ";
            foreach(var n in _elements){
                if(n == null){
                    break;
                }
                result += "\n - " + "( " + n->line + " , " + n->col + " )" ;
            }
            result += "\n------------------------";
            Utilitary.Log(result);

        }

    }
}