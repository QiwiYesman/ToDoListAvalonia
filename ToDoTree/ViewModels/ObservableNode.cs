using System.Collections.ObjectModel;
using ReactiveUI;
using ToDoTree.Models;

namespace ToDoTree.ViewModels;

public class ObservableNode: ViewModelBase
{
    private string _header = "", _description = "";
    private ToDoStates _toDoState = ToDoStates.ToDo;
    private Priority _priority = Priority.Medium;
    public ObservableCollection<ObservableNode> Children { get; set; } = new();
    public ObservableNode? Parent { get; set; } = null;

    public string Header
    {
        get => _header;
        set => this.RaiseAndSetIfChanged(ref _header, value);

    }

    public string Description
    {
        get => _description; 
        set => this.RaiseAndSetIfChanged(ref _description, value);
        
    }
    public ToDoStates ToDoState
    {
        get => _toDoState;
        set => this.RaiseAndSetIfChanged(ref _toDoState, value);
    }

    public Priority Priority
    {
        get => _priority;
        set => this.RaiseAndSetIfChanged(ref _priority, value);
    }

    public void AddNode(ObservableNode node)
    {
        Children.Add(node);
        node.Parent = this;
    }

    public void RemoveNode(ObservableNode node)
    {
        Children.Remove(node);
        node.Parent = null;
    }

    public void MoveNode(ObservableNode nodeToMove, ObservableNode newParentNode)
    {
        Children.Remove(nodeToMove);
        newParentNode.AddNode(nodeToMove);
    }

    public static Node ToNode(ObservableNode observableNode, Node? parent)
    {
        var node = new Node()
        {
            Children = new(),
            Description = observableNode._description,
            Header = observableNode._header,
            Priority = observableNode._priority,
            ToDoState = observableNode._toDoState,
            Parent = parent
        };
        foreach (var childNode in observableNode.Children)
        {
            node.Children.Add(ToNode(childNode, node));
        }

        return node;
    }
    public static ObservableNode FromNode(Node node, ObservableNode? parent)
    {
        var observableNode = new ObservableNode()
        {
            Description = node.Description,
            Header = node.Header,
            Priority = node.Priority,
            ToDoState = node.ToDoState,
            Parent = parent,
            Children = new()
        };
        foreach (var childNode in node.Children)
        {
            observableNode.Children.Add(FromNode(childNode, observableNode));
        }

        return observableNode;
    }
}