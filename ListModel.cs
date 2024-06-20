using Avalonia.Controls;
using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

interface ICommand
{
	void Execute();
	void Cancel();
}

class AddItemCommand<TItem> : ICommand
{
	public readonly  LimitedSizeStack.ListModel<TItem> List;
	public readonly TItem Item;

	public AddItemCommand(ListModel<TItem> list, TItem item)
    {
        List = list;
		Item = item;
    }

    public void Cancel()
    {
        List.Items.Remove(Item);
    }

    public void Execute()
    {
        List.Items.Add(Item);
    }
}

class RemoveItemCommand<TItem> : ICommand
{
    public readonly LimitedSizeStack.ListModel<TItem> List;
    public readonly TItem Item;
	public readonly int IndexNum;

    public RemoveItemCommand(ListModel<TItem> list, TItem item, int indexNum)
    {
        List = list;
        Item = item;
		IndexNum = indexNum;
    }

    public void Cancel()
    {
        List.Items.Insert(IndexNum, Item);
    }

    public void Execute()
    {
        List.Items.RemoveAt(IndexNum);
    }
}

public class ListModel<TItem>
{
	private LimitedSizeStack<ICommand> history;

	public List<TItem> Items { get; }
	public int UndoLimit;
        
	public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
		history = new LimitedSizeStack<ICommand> (undoLimit);
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
		history = new LimitedSizeStack<ICommand>(undoLimit);
	}

	public void AddItem(TItem item)
	{
		var command = new AddItemCommand<TItem>(this, item);
        history.Push(command);
		command.Execute();		
	}

	public void RemoveItem(int index)
	{
		var item = Items[index];
		var command = new RemoveItemCommand<TItem>(this, item, index);
		history.Push(command);
		command.Execute();
	}

	public bool CanUndo()
	{
		return history.Count != 0;
	}

	public void Undo()
	{
		if (CanUndo())
			history.Pop().Cancel();
	}
}