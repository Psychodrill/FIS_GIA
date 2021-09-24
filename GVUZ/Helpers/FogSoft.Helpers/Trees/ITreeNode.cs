using System;
using System.Collections.Generic;

namespace FogSoft.Helpers.Trees
{
    /// <summary>
    ///     Интерфейс, описывающий узел "дерева" (в "лесу").
    /// </summary>
    /// <typeparam name="T">Тип для узла дерева.</typeparam>
    /// <typeparam name="TId">Тип идентификатора для узла дерева.</typeparam>
    /// <remarks>
    ///     При реализации наследников убедитесь, что свойства <see cref="Id" /> и <see cref="ParentId" />
    ///     будут правильно присвоены (лучше в конструкторе).
    ///     Вариант инициализации <see cref="Children" /> есть в <see cref="TreeNode{T,TId}" />.
    /// </remarks>
    public interface ITreeNode<T, TId> where TId : struct, IComparable<TId>
    {
        /// <summary>
        ///     Идентификатор узла.
        /// </summary>
        TId Id { get; set; }

        /// <summary>
        ///     Идентификатор родительского узла (или null).
        /// </summary>
        /// <remarks>
        ///     Если в будущем понадобится, можно сделать T Parent { get; set; } и дописать
        ///     <see
        ///         cref="CollectionExtensions.ToTree{T,TId}" />
        ///     .
        /// </remarks>
        TId? ParentId { get; set; }

        /// <summary>
        ///     Дочерние узлы. Класс, реализующий этот интерфейс, *обязан* обеспечить создание этой коллекции до или во время обращения к свойству.
        /// </summary>
        List<T> Children { get; }

        /// <summary>
        ///     Есть ли дочерние узлы.
        ///     Позволяет (при наличии правильной реализации) узнать наличие дочерних узлов без лишнего создания коллекции.
        /// </summary>
        bool HasChildren { get; }
    }
}