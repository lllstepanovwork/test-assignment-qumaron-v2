using System.Collections.Generic;

namespace OleksiiStepanov.Utils
{
    public static class StackExtension 
    {
        public static Stack<T> ReverseStack<T>(this Stack<T> stack)
        {
            if (stack.Count > 0)
            {
                T temp = stack.Pop();
                stack.ReverseStack(); 
                stack.InsertAtBottom(temp);
            }
            
            return stack;
        }

        private static void InsertAtBottom<T>(this Stack<T> stack, T item)
        {
            if (stack.Count == 0)
            {
                stack.Push(item);
            }
            else
            {
                T temp = stack.Pop();
                stack.InsertAtBottom(item);
                stack.Push(temp);
            }
        }
    }
}



