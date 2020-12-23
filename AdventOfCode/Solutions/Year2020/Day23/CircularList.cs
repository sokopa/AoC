using System;

namespace AdventOfCode.Solutions.Year2020
{
    internal class CircularList

    {
        private int data;
        public int Data => data;

        private CircularList next;
        public CircularList Next => next;


        public CircularList()
        {
            data = 0;

            next = this;
        }


        public CircularList(int value)

        {
            data = value;

            next = this;
        }


        public CircularList InsertNext(int value)

        {
            var node = new CircularList(value);

            if (next == this) // only one node in the circular list

            {
                // Easy to handle, after the two lines of executions,

                // there will be two nodes in the circular list

                node.next = this;

                next = node;
            }

            else

            {
                // Insert in the middle

                (node.next, next) = (next, node);
            }

            return node;
        }

        public CircularList InsertNext(CircularList node)

        {
            if (next == this) // only one node in the circular list

            {
                // Easy to handle, after the two lines of executions,

                // there will be two nodes in the circular list

                node.next = this;

                next = node;
            }

            else

            {
                // Insert in the middle

                (node.next, next) = (next, node);
            }

            return node;
        }


        public CircularList DeleteNext()

        {
            if (next == this)

            {
                Console.WriteLine(
                    "\nThe node can not be deleted as there is only one node in the circular list");

                return null;
            }


            var node = next;

            next = next.next;

            return node;
        }

        public void Traverse(CircularList node, Action<CircularList> action)

        {
            if (node == null)
                node = this;

            var startnode = node;
            do
            {
                action(node);
                node = node.next;
            } while (node != startnode);
        }
    }
}