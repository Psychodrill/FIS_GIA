using System;
using System.Collections.Generic;

namespace FogSoft.Helpers.Trees
{
    /// <summary>
    ///     Содержимое дерева с возможностью быстрого поиска.
    /// </summary>
    /// <typeparam name="T"> Тип для узла дерева. </typeparam>
    /// <typeparam name="TId"> Тип идентификатора для узла дерева. </typeparam>
    public class TreeContainer<T, TId>
        where T : class, ITreeNode<T, TId>
        where TId : struct, IComparable<TId>
    {
        internal readonly Dictionary<TId, T> Map;

        public TreeContainer(List<T> items = null) : this(new Dictionary<TId, T>(), items)
        {
        }

        public TreeContainer(Dictionary<TId, T> map, List<T> items = null)
        {
            if (map == null) throw new ArgumentNullException("map");
            Map = map;
            Items = items;
        }

        /// <summary>
        ///     Для дерева - список корневых узлов (у каждого узла могут быть <see cref="ITreeNode{T,TId}.Children" /> ).
        ///     Для плоского списка - просто список.
        /// </summary>
        public List<T> Items { get; private set; }

        /// <summary>
        ///     Заблокирован ли маппинг для добавления новых элементов.
        /// </summary>
        public bool MapLocked { get; private set; }

        public void AddToMap(T node)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (MapLocked) throw new InvalidOperationException("Map is locked.");
            Map.Add(node.Id, node);
        }

        public bool TryGetFromMap(TId id, out T value)
        {
            return Map.TryGetValue(id, out value);
        }

        public T GetFromMap(TId id)
        {
            return Map[id];
        }

        public void LockMap()
        {
            MapLocked = true;
        }
    }
}