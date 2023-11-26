using System.Collections.Generic;

namespace ToDoTree.Models;

public class Node
{
    public string Header { get; set; }
    public string Description { get; set; }
    public List<Node> Children { get; set; }
    public ToDoStates ToDoState { get; set; }
    public Priority Priority { get; set; }
    public Node? Parent { get; set; }
}