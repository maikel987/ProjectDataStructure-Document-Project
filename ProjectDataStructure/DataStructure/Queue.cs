using ProjectDataStructure.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataStructure
{
    internal class Queue<Mytype> : IEnumerable<Mytype>
    {
        Node start; //null by default
        Node end;   //null by default



        public int count { get; internal set; }

        public bool DeQueue(out Mytype saveData)  //Remove Last
        {
            saveData = default(Mytype);

            if (end == null)
                return false;
            saveData = end.data;
            end = end.previous;
            if (end == null) start = null;
            else end.next = null;
            count--;
            return true;
        }

        public bool ElementAt(int i, out Mytype value) //0 based
        {
            int count = 0;
            Node tmp = start;
            while (tmp != null)
            {
                if (count == i)
                {
                    value = tmp.data;
                    return true;
                }
                count++;
                tmp = tmp.next;
            }
            value = default(Mytype);
            return false;
        }

        public void Enqueue(Mytype newItem, out Node node)  //AddFirst
        {

            Node tmp = new Node(newItem);
            tmp.next = start; 
            if (start == null)
                end = tmp;
            start = tmp;
            tmp.previous = null; // ou start?
            if (tmp.next != null) tmp.next.previous = tmp;
            count++;
            node = tmp;
        }

        public bool Delate(Node node, out Node removedNode)
        {
            removedNode = default(Node);  
            if (start == null|| ( node.next == null && node.previous == null ))
                return false;

            if (node.next == null)
            {
                end = end.previous;
                if (end == null) start = null;
                else end.next = null;
                count--;
                return true;
            }

            if (node.previous == null)
            {
                start = start.next;
                if (start == null) end = null;
                else start.previous = null;
                count--;
                return true;
            }

            Node parent = node.previous;
            Node child = node.next;
            parent.next = child;
            child.previous = parent;
            count--;
            return true;

        }

        public string PrintAll()
        {
            StringBuilder strg = new StringBuilder();
            Node tmp = start;
            while (tmp != null)
            {
                strg.AppendLine(tmp.data.ToString());
                tmp = tmp.next;
            }
            return strg.ToString();
        }



        private bool RemoveFirst(out Mytype saveData)
        {
            saveData = default(Mytype);

            if (start == null)
                return false;
            saveData = start.data;
            start = start.next;
            if (start == null) end = null;
            else start.previous = null;
            count--;
            return true;
        }

        private void AddToEnd(Mytype newItem)
        {
            Node fictif;
            if (start == null)
            {
                Enqueue(newItem, out fictif);
                return;//ends the function without returning value
            }
            //explicit else
            Node tmp = new Node(newItem);
            tmp.previous = end;
            end.next = tmp;
            end = tmp;

            count++;
        }

        public bool LastElement(out Mytype value) //0 based
        {
            Node tmp = end;
            if (tmp != null)
            {
                value = tmp.data;
                return true;
            }
            else
            {
                value = default(Mytype);
                return false;
            }
        }



        public IEnumerator<Mytype> GetEnumerator()
        {
            //return new MyLinkedListEnumerator(start);
            Node tmp = start;
            while (tmp != null)
            {
                yield return tmp.data;
                tmp = tmp.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


         internal class Node
        {
            public Mytype data;
            public Node next; //null by default
            public Node previous;

            public Node(Mytype data)
            {
                this.data = data;
            }
        }
    }
}
