using Retros.Program.Workstation.Image;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Threading;

namespace Retros.Program.Workstation.Changes {
    public class ChangeHistory {
        private Node _current;
        private Node _rootNode;
        private HashSet<Node> _allNodes = new();

        public readonly Node RootChange;
        public Node CurrentNode => _current;


        public delegate void AllChildrenRemovedHandler(Node parent);
        public delegate void ChangedHandler(Node node);
        public delegate void MoveBackHandler(uint steps); // the amount of steps to take
        public delegate void MoveForwardHandler(uint[] steps); // the index is the step and the value is the index to take at the step
        public delegate void NodeRemovedHandler(uint index);
        public event MoveBackHandler? MoveBack;
        public event MoveForwardHandler? MoveForward;
        public event NodeRemovedHandler? ChangeRemoved; // One Change has been removed
        public event AllChildrenRemovedHandler? AllChildrenRemoved; // All changes from a parent have been removed
        public event ChangedHandler? ChangeAdded; // A change has been added
        public event ChangedHandler? PositionChanged; // the position of _current has changed

        public event Action? ImageChanged;


        public ChangeHistory() {
            RootChange = new Node(null, ImageState.FromEmpty());

            _allNodes.Add(RootChange);
            _current = RootChange;
            ChangeAdded?.Invoke(_current);
            _rootNode = _current!;

            MoveBack += (_) => ImageChanged?.Invoke();
            MoveForward += (_) => ImageChanged?.Invoke();
            PositionChanged += (_) => ImageChanged?.Invoke();

            //DebugLibrary.Console.Log("---NEW ARR");
            /*
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(300);
            dispatcherTimer.Tick += delegate {
                try {
                    FilterManager filterManager = WindowManager.MainWindow.SelectedWorkstation.ImageElement.GetFilterManager;
                    DebugLibrary.Console.Log(filterManager.ContainsFilter(_current.Value.Filters[0]));
                    DebugLibrary.Console.Log(filterManager.GetFilter(_current.Value.Filters[0]).GetHashCode());
                    DebugLibrary.Console.Log((_current.Value.Filters[0]).GetHashCode());
                }
                catch {
                    
                }
            };
            dispatcherTimer.Start();
            */
        }


        public void Reset() {
            _current = _rootNode;
            _rootNode.CutChildren();
            _allNodes.Clear();
            _allNodes.Add(_rootNode);
        }


        public void Add(ImageState imageState) {
            Node newNode = new(_current, imageState);
            _current.Add(newNode);
            _allNodes.Add(newNode);
            ChangeAdded?.Invoke(newNode);

            //DebugLibrary.Console.Log("Add:" + imageState.Filters[0].ToString());
        }
        public void AddAndStep(ImageState imageState) {
            uint index = 
                _current.Children is not null
                ? (uint)_current.Children.Count
                : 0;

            Add(imageState);

            Node next = _current.Next(index)!;
            _current = next;

            MoveForward?.Invoke(new uint[] { index });
        }

        public void Jump(Node node) {
            if (_allNodes.Contains(node)) {
                _current = node;
                PositionChanged?.Invoke(node);
            }
        }

        public void Forward(uint index) {
            uint uindex = index >= 0 ? index : throw new IndexOutOfRangeException();
            Node? next = _current?.Next(uindex);
            _current = next is not null ? next : _current!;

            MoveForward?.Invoke(new uint[] { index });
        }

        public void Redo() {
            _current = _current.HasChildren
                ? _current.Children![0]
                : _current;

            MoveForward?.Invoke(new uint[] { 0 });
        }

        public void Undo() {
            var prev = _current.Previous;
            _current = prev is not null
                ? prev
                : _current;

            MoveBack?.Invoke(1);
        }

        public void Cut() {
            if (_current is not null && _current.HasChildren) {
                foreach (var child in _current.Children!) {
                    _allNodes.Remove(child);
                }
                _current.CutChildren();
                AllChildrenRemoved?.Invoke(_current);
            }
        }

        public void Cut(uint index) {
            if (_current is not null && _current.HasChildren) {
                var cutNode = _current.CutNode(index);
                if (cutNode is null) return;
                ChangeRemoved?.Invoke(index);
                _allNodes.Remove(cutNode);
            }
        }

        public void Clear() {
            _current = _rootNode;
            _rootNode.CutChildren();
            _allNodes.Clear();
            _allNodes.Add(_rootNode);

            AllChildrenRemoved?.Invoke(_rootNode);
            PositionChanged?.Invoke(_rootNode);
        }


        public class Node {
            private string _name;
            private List<Node>? _nodes;
            private Node? _prev;

            public ImageState Value { get; }
            public List<Node>? Children => _nodes;
            public string Name => _name;


            public Node(Node? prevNode, ImageState imageState) {
                _prev = prevNode;
                Value = imageState;

                if (prevNode is null) {
                    _name = ImageState.IdentifyDifference(null, imageState);
                }
                else {
                    _name = ImageState.IdentifyDifference(prevNode.Value, imageState);
                }
            }


            public bool HasChildren => _nodes != null;
            public Node? Next(uint index) => index < _nodes?.Count ? _nodes[(int)index] : null;
            public Node? Previous => _prev is not null ? _prev : null;

            public void Add(Node node) {
                _nodes ??= new List<Node>();
                _nodes.Add(node);
            }

            public void CutChildren() {
                _nodes = null;
            }

            public void CutFromParent() {
                _prev = null;
            }

            public Node? CutNode(uint index) {
                if (index < _nodes?.Count) {
                    Node node = Next(index)!;
                    _nodes.RemoveAt((int)index);
                    node!.CutFromParent();
                    return node;
                }
                return null;
            }

        }
    }
}
