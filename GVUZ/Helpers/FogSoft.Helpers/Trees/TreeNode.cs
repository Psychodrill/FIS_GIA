using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FogSoft.Helpers.Trees
{
    /// <summary>
    ///     Стандартная реализация <see cref="ITreeNode{T,TId}" />
    /// </summary>
    [DebuggerDisplay("{Id} ({ParentId})")]
    public class TreeNode<T, TId> : ITreeNode<T, TId>, IEquatable<TreeNode<T, TId>> where TId : struct, IComparable<TId>
    {
        private List<T> _children;

        public bool Equals(TreeNode<T, TId> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id.Equals(Id);
        }

        public TId Id { get; set; }

        public TId? ParentId { get; set; }

        public List<T> Children
        {
            get { return _children ?? (_children = new List<T>()); }
        }

        public bool HasChildren
        {
            get { return _children != null && _children.Count > 0; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TreeNode<T, TId>)) return false;
            return Equals((TreeNode<T, TId>) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}