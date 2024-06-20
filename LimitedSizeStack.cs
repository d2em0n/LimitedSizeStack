using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    public int Count { get; private set; }

    public readonly LinkedList<T> LinkedList;
    public int UndoLimit { get; set; }

    public LimitedSizeStack(int undoLimit)
    {
        Count = 0;
        UndoLimit = undoLimit;
        LinkedList = new LinkedList<T>();
    }

    public void Push(T item)
    {
        if (UndoLimit == 0) return;
        if (Count < UndoLimit)
        {
            LinkedList.AddLast(item);
            Count++;
        }
        else
        {
            LinkedList.RemoveFirst();
            LinkedList.AddLast(item);
        }
    }

    public T Pop()
    {
        if (Count > 0)
            Count--;
        var result = LinkedList.Last ?? throw new InvalidOperationException();
        
        LinkedList.RemoveLast();
        return result.Value;
    }
}