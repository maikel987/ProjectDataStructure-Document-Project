using ProjectDataStructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataStructure
{
    
    internal class BST<T> where T : IComparable<T>
    {
        Node root;
        IComparer<T> alternativeComparer;
        internal int count { get; private set; }

        public BST(IComparer<T> alternativeComparer = null)
        {
            this.alternativeComparer = alternativeComparer;
        }

        private int ItemsCompare(T item1, T item2)
        {
            if (alternativeComparer == null)
                return item1.CompareTo(item2);
            return alternativeComparer.Compare(item1, item2);
        }

        public void Add(T newData)
        {
            count++;
            Node newNode = new Node(newData);
            if (root == null)
            {
                root = newNode;
                return;
            }
            Node tmp = root;
            Node parent = null;

            while (tmp != null)
            {
                parent = tmp;

                if (ItemsCompare(newData, tmp.data) < 0)
                    tmp = tmp.left;
                else
                    tmp = tmp.right;
            }
            if (ItemsCompare(newData, parent.data) < 0)
                parent.left = newNode;
            else
                parent.right = newNode;
        }

        private void Replace(Node parent, Node ToBeReplaced, bool isLeft)
        {

            if (ToBeReplaced.left == null && ToBeReplaced.right == null)
            {
                if (parent == null) { root = null; }
                else if (isLeft) parent.left = null;
                else parent.right = null;
            }
            else if (ToBeReplaced.left == null)
            {
                if (parent == null) { root = ToBeReplaced.right; }
                else if (isLeft) parent.left = ToBeReplaced.right;
                else parent.right = ToBeReplaced.right;
            }
            else if (ToBeReplaced.right == null)
            {
                if (parent == null) { root = ToBeReplaced.left; }
                else if (isLeft) parent.left = ToBeReplaced.left;
                else parent.right = ToBeReplaced.left;
            }
            else
            {
                Node tmpParent;
                Node tmp = SearchLeftNode(ToBeReplaced.right, out tmpParent);
                ToBeReplaced.data = tmp.data;

                if (tmpParent == null)
                {
                    ToBeReplaced.right = tmp.right;
                }
                else
                {
                    tmpParent.left = tmp.right;
                }
            }
        }


        public bool Remove(T item, out T removedItem)
        {
            bool isLeft = false;
            Node tmp = root;
            Node parent = null;
            removedItem = default(T);
            while (tmp != null)
            {


                if (ItemsCompare(item, tmp.data) == 0)
                {

                    if (tmp == root)
                    {

                    }

                    removedItem = tmp.data;
                    Replace(parent, tmp, isLeft);
                    count--;
                    return true;
                }
                parent = tmp;
                if (ItemsCompare(item, tmp.data) < 0)
                {
                    tmp = tmp.left;
                    isLeft = true;
                }
                else
                {
                    tmp = tmp.right;
                    isLeft = false;
                }
            }
            return false;
        }

        private Node SearchLeftNode(Node Departure, out Node parent)
        {
            parent = null;
            Node tmp = Departure;

            while (tmp != null)
            {
                if (tmp.left == null) return tmp;
                else
                {
                    parent = tmp;
                    tmp = tmp.left;
                }
            }
            return tmp;
        }




        public bool Search(T item, out T foundItem)
        {
            T tmp;
            Matching match = InnerSearch(item, out foundItem, out tmp);
            if (match == Matching.Match) return true;
            return false;
        }

        private Matching InnerSearch(T item, out T foundItem, out T closestItem)
        {

            Node tmp = root;
            closestItem = default(T);
            foundItem = default(T);
            while (tmp != null)
            {
                if (ItemsCompare(item, tmp.data) == 0)
                {
                    foundItem = tmp.data;
                    return Matching.Match;
                }
                if (ItemsCompare(item, tmp.data) < 0)
                {
                    closestItem = tmp.data;
                    tmp = tmp.left;
                }
                else
                    tmp = tmp.right;
            }
            if (closestItem != null) return Matching.Closest;
            return Matching.NoMatch;

        }

        public void InOrder(Action<T> todo)
        {

            InOrder(root, todo);
        }

        private void InOrder(Node tmp, Action<T> todo)
        {
            if (tmp == null) return;
            InOrder(tmp.left, todo);
            //Console.WriteLine(tmp.data);
            todo(tmp.data);
            InOrder(tmp.right, todo);
        }

        //enum (Match, Closest ,No Match)
        public Matching SearchClosest(T item, out T closestItem)
        {
            T tmp;
            return InnerSearch(item, out tmp, out closestItem);
        }

        public int GetLevels()
        {
            return GetLevels(root);
        }

        private int GetLevels(Node tmp)
        {
            if (tmp == null) return 0;
            return Math.Max(GetLevels(tmp.left), GetLevels(tmp.right) + 1);
        }

        class Node
        {
            public T data;
            public Node left;
            public Node right;

            public Node(T data)
            {
                this.data = data;
            }
        }
    }
}
