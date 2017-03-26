using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace tree_class
{
    /// <summary>
    /// Класс, предназначенный для хранения данных в виде дерева
    /// с произвольным числом ветвлений в каждом узле.
    /// </summary>
    /// <typeparam name="T">Тип данных.</typeparam>
    public class Tree<T> : ICloneable, IEnumerable<Tree<T>>, IEnumerable<T>
    {
        #region Nested classes
        public class TreeItemActionEventArgs : EventArgs
        {
            public readonly Tree<T> TreeItem;

            public TreeItemActionEventArgs(Tree<T> treeItem)
            {
                TreeItem = treeItem;
            }
        }

        public class TreeItemsActionEventArgs : EventArgs
        {
            public readonly IEnumerable<Tree<T>> TreeItems;

            public TreeItemsActionEventArgs(IEnumerable<Tree<T>> treeItems)
            {
                TreeItems = treeItems;
            }
        }

        public class TreeItemBeforeActionEventArgs : TreeItemActionEventArgs
        {
            public bool Cancel;

            public TreeItemBeforeActionEventArgs(Tree<T> treeItem)
                : base(treeItem)
            {
            }
        }

        public class TreeItemsBeforeActionEventArgs : TreeItemsActionEventArgs
        {
            public bool Cancel;

            public TreeItemsBeforeActionEventArgs(List<Tree<T>> treeItem)
                : base(treeItem)
            {
            }
        }
        #endregion


        public Tree<T> Parent { get; private set; }
        public T Value;

        public List<Tree<T>> localChildren;
        //private readonly List<Tree<T>> localChildren;
        protected int version;

        public Tree()
            : this(default(T))
        {
        }

        /// <summary>
        /// Создать корневой узел (Root).
        /// </summary>
        /// <param name="value">Значение узла.</param>
        public Tree(T value)
            : this(null, null, value)
        {
        }

        /// <summary>
        /// Создать корневой узел (Root) и установить список дочерних узлов <c>children</c>.
        /// </summary>
        /// <param name="children">Список дочерних узлов.</param>
        /// <param name="value">Значение узла.</param>
        public Tree(List<Tree<T>> children, T value)
            : this(null, children, value)
        {
        }

        /// <summary>
        /// Создать узел, являющийся потомком узла <c>parent</c>, и установить список дочерних узлов <c>children</c>.
        /// </summary>
        /// <param name="parent">Родительский узел.</param>
        /// <param name="children">Список дочерних узлов.</param>
        /// <param name="value">Значение узла.</param>
        public Tree(Tree<T> parent, List<Tree<T>> children, T value)
        {
            localChildren = children ?? new List<Tree<T>>();
            Value = value;

            for (int i = 0; i < localChildren.Count; i++)
                localChildren[i].Parent = this;

            if (parent != null)
                parent.AddChild(this);
            else
                Parent = null;
        }


        public event EventHandler<TreeItemActionEventArgs> ItemAdded;
        public event EventHandler<TreeItemsActionEventArgs> ItemsAdded;
        public event EventHandler<TreeItemActionEventArgs> ItemRemoving;
        public event EventHandler<TreeItemsActionEventArgs> ItemsRemoving;


        public void Add(Tree<T> child)
        {
            AddChild(child);
        }

        protected virtual void AddChild(Tree<T> child)
        {
            child.Parent = this;
            localChildren.Add(child);
            version++;
            if (ItemAdded != null)
                ItemAdded(this, new TreeItemActionEventArgs(child));
        }

        public void AddRange(IEnumerable<Tree<T>> children)
        {
            AddChildren(children);
        }

        protected virtual void AddChildren(IEnumerable<Tree<T>> children)
        {
            foreach (Tree<T> child in children)
                child.Parent = this;
            localChildren.AddRange(children);
            version++;
            if (ItemsAdded != null)
                ItemsAdded(this, new TreeItemsActionEventArgs(children));
        }

        public bool Remove(Tree<T> child)
        {
            return RemoveChild(child);
        }

        public virtual bool RemoveChild(Tree<T> child)
        {
            TreeItemBeforeActionEventArgs e = new TreeItemBeforeActionEventArgs(child);
            if (ItemRemoving != null)
            {
                ItemRemoving(this, e);
                if (e.Cancel)
                {
                    version++;
                    return false;
                }
            }
            return localChildren.Remove(child);
        }

        public void RemoveRange(int index, int count)
        {
            RemoveChildren(index, count);
        }

        public virtual void RemoveChildren(int index, int count)
        {
            TreeItemsBeforeActionEventArgs e = new TreeItemsBeforeActionEventArgs(localChildren.GetRange(index, count));
            if (ItemsRemoving != null)
            {
                ItemsRemoving(this, e);
                if (e.Cancel)
                {
                    version++;
                    return;
                }
            }
            localChildren.RemoveRange(index, count);
        }


        public Tree<T> this[int index]
        {
            get
            {
                return localChildren[index];
            }
        }


        public ReadOnlyCollection<Tree<T>> Children
        {
            get
            {
                return localChildren.AsReadOnly();
            }
        }

        public Tree<T> Root
        {
            get
            {
                return IsRoot ? this : Parent.Root;
            }
        }

        public bool IsRoot
        {
            get
            {
                return Parent == null;
            }
        }

        public bool HasChildren
        {
            get
            {
                return localChildren.Count != 0;
            }
        }

        public int Level
        {
            get
            {
                return IsRoot ? 0 : Parent.Level + 1;
            }
        }

        public int Length
        {
            get
            {
                int i = 0;
                ForEach(a => i++);
                return i;
            }
        }

        public List<Tree<T>> Parents
        {
            get
            {
                var list = new List<Tree<T>>(Level);
                var current = this;
                while (current.Parent != null)
                {
                    list.Add(Parent);
                    current = current.Parent;
                }
                return list;
            }
        }

        public bool IsChildOf(Tree<T> node)
        {
            var current = this;
            while (current.Parent != null)
            {
                if (current.Parent == node)
                    return true;
                current = current.Parent;
            }
            return false;
        }

        public bool IsParentOf(Tree<T> node)
        {
            return HasChildren && node.IsChildOf(this);
        }

        public void ForEach(Action<Tree<T>> action)
        {
            action(this);
            for (int i = 0; i < localChildren.Count; i++)
                localChildren[i].ForEach(action);
        }

        public void ForEach<S>(Action<Tree<T>> action) where S : T
        {
            if (Value is S)
                action(this);
            for (int i = 0; i < localChildren.Count; i++)
                if (localChildren[i].Value is S)
                    localChildren[i].ForEach(action);
        }

        public Tree<T> Find(Predicate<Tree<T>> match)
        {
            if (match(this))
                return this;
            for (int i = 0; i < localChildren.Count; i++)
            {
                Tree<T> node = localChildren[i].Find(match);
                if (node != null)
                    return node;
            }
            return null;
        }

        public Tree<T> Find(T value)
        {
            return Find(typeof(IEquatable<T>).IsAssignableFrom(typeof(T))
                            ? (Predicate<Tree<T>>)(node => (((IEquatable<T>)node.Value).Equals(value)))
                            : node => (ReferenceEquals(node.Value, value)));
        }


        public List<Tree<T>> FindAll(Predicate<Tree<T>> match)
        {
            List<Tree<T>> items = new List<Tree<T>>();
            if (match(this))
                items.Add(this);
            for (int i = 0; i < localChildren.Count; i++)
                items.AddRange(localChildren[i].FindAll(match));
            return items;
        }

        public List<T> FindAll(Predicate<T> match)
        {
            List<T> items = new List<T>();
            if (match(Value))
                items.Add(Value);
            for (int i = 0; i < localChildren.Count; i++)
                items.AddRange(localChildren[i].FindAll(match));
            return items;
        }

        public List<S> FindAll<S>() where S : T
        {
            List<S> items = new List<S>();
            if (Value is S)
                items.Add((S)Value);
            for (int i = 0; i < localChildren.Count; i++)
                items.AddRange(localChildren[i].FindAll<S>());
            return items;
        }


        #region ICloneable Members

        public object Clone()
        {
            return CloneTree();
        }

        #endregion

        public Tree<T> CloneTree()
        {
            return new Tree<T>(Parent, new List<Tree<T>>(localChildren), Value);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return Value;
            for (int i = 0; i < localChildren.Count; i++)
                ((IEnumerable<T>)localChildren[i]).GetEnumerator();
        }

        public IEnumerator<Tree<T>> GetEnumerator()
        {
            yield return this;
            for (int i = 0; i < localChildren.Count; i++)
                localChildren[i].GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
