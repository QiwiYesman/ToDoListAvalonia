using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using ReactiveUI;
using ToDoTree.Models;

namespace ToDoTree.ViewModels;

public class MainWindowViewModel : ViewModelBase
{

    public static ToDoStates[] ToDoStates => Enum.GetValues<ToDoStates>();
    public static Priority[] Priorities => Enum.GetValues<Priority>();

    private ToDoStates? _selectedState;
    private Priority? _selectedPriority;
    public ObservableCollection<ObservableNode> SelectedNodes { get; set; } = new();
    private ObservableNode? _selectedNode;
    public ObservableCollection<ObservableNode> MainNodeList { get; set; } = new() {new()};

    public ObservableNode? SelectedNode => SelectedNodes.Count == 1 ? SelectedNodes[0] : null; 

    public ObservableNode MainNode
    {
        get=>MainNodeList[0];
        set => MainNodeList[0] = value;
    }
    
    
    public ToDoStates? SelectedState
    {
        get => _selectedState;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedState, value);
            if (SelectedNodes.Count==0 || _selectedState is null) return;
            foreach (var node in SelectedNodes)
            {
                node.ToDoState = (ToDoStates)_selectedState;
            }
        }
    }
    public Priority? SelectedPriority
    {
        get => _selectedPriority;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedPriority, value);
            if (SelectedNodes.Count==0 || _selectedPriority is null) return;
            foreach (var node in SelectedNodes)
            {
                node.Priority = (Priority)_selectedPriority;
            }
        }
    }

    public void Read()
    {
        var app = App.Current;
        if (app is null) return;
        MainNode.Children.Clear();   
        try
        {
            var node = Serializer.DeserializeFromFileJson<Node>(app.DefaultToDoSavePath)
                    ?? new();
            MainNode = ObservableNode.FromNode(node, null);
        }
        catch
        {
            LoadDefault();
            return;
        }
        if(MainNode.Children.Count == 0) LoadDefault();
    }

    public void LoadDefault()
    {
        MainNode = new() { Header = "Завдання", Description = "Завдання до виконання" };
    }

    public void Save()
    {
        var app = App.Current;
        if (app is null) return;
        Serializer.SerializeToFileJson(ObservableNode.ToNode(MainNode, null), app.DefaultToDoSavePath);
        Read();
    }

    public void Remove()
    {
        var t = SelectedNodes.ToArray();
        foreach (var node in t)
        {
            node.Parent?.RemoveNode(node);
        }
    }

    public void Add()
    {
        foreach (var node in SelectedNodes)
        {
            node.AddNode(new());
        }
    }

    
    

    public MainWindowViewModel()
    {
        Read();
        SelectedNodes.CollectionChanged += (_, _) => this.RaisePropertyChanged(nameof(SelectedNode));
    }
    
}