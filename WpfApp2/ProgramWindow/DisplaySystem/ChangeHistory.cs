using System;
using System.Collections.Generic;
using System.Reflection;

namespace Retros.ProgramWindow.DisplaySystem {
    public class ChangeHistory {
        private Node _current;
        private Node _rootNode;
        private HashSet<Node> _allNodes = new();

        public readonly Node RootChange;


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

        public ChangeHistory() {
            RootChange = new Node(null, new RootChange());
            _current = RootChange;
            ChangeAdded?.Invoke(_current);
            _rootNode = _current!;
        }


        public void Add(IChange change) {
            Node newNode = new Node(_current, change);
            _current.Add(newNode);
            _allNodes.Add(newNode);
            ChangeAdded?.Invoke(newNode);
            DebugLibrary.Console.Log($"History node added :{change.GetType().Name}");
        }
        public void AddAndStep(IChange change) {
            uint index = _current.Children is not null
                ? (uint)_current.Children.Count
                : 0;
            Add(change);
            Forward(index);
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
            var prev = _current?.Previous;
            _current = prev is not null 
                ? prev 
                : _current!;

            MoveBack?.Invoke(1);
        }

        public void Cut() {
            if (_current is not null && _current.HasChildren) {
                foreach (var child in _current.Children!) {
                    _allNodes.Remove(child);
                }
                _current.CutNodes();
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
            _rootNode.CutNodes();
            _allNodes.Clear();
            _allNodes.Add(_rootNode);

            AllChildrenRemoved?.Invoke(_rootNode);
            PositionChanged?.Invoke(_rootNode);
        }


        public class Node {
            private readonly IChange _value;
            private List<Node>? _nodes;
            private Node? _prev;


            public Node(Node? prevNode, IChange change) {
                _prev = prevNode;
                _value = change;
            }

            public void Add(Node node) {
                _nodes ??= new List<Node>();
                _nodes.Add(node);
            }

            public void CutNodes() {
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

            public bool HasChildren => _nodes != null;
            public List<Node>? Children => _nodes;
            public Node? Next(uint index) => index < _nodes?.Count ? _nodes[(int)index] : null;
            public Node? Previous => _prev is not null ? _prev : null;
            public IChange Value => _value;
        }
    }
}
