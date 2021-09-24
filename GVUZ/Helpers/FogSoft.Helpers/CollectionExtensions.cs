using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using FogSoft.Helpers.Trees;

namespace FogSoft.Helpers
{
	/// <summary>
	/// 	Расширяет поведение коллекций и списков.
	/// </summary>
	[DebuggerStepThrough]
	public static class CollectionExtensions
	{
		/// <summary>
		/// 	Возвращает true, если коллекция пуста или null.
		/// </summary>
		public static bool IsNullOrEmpty(this ICollection collection)
		{
			return collection == null || collection.Count == 0;
		}

		public static int[] ToIntArray(this IEnumerable<string> strings)
		{
			if (strings == null) return new int[0];
			var result = new List<int>();

			foreach (string s in strings)
			{
				int i;
				if (int.TryParse(s, NumberStyles.Integer, null, out i))
					result.Add(i);
			}

			return result.ToArray();
		}

		/// <summary>
		/// 	Разворачивает дерево в плоский список.
		/// </summary>
		/// <typeparam name="T"> Тип для узла дерева. </typeparam>
		/// <typeparam name="TId"> Тип идентификатора для узла дерева. </typeparam>
		/// <param name="tree"> <see cref="TreeContainer{T,TId}" /> для обработки. </param>
		/// <param name="skipMissedParent"> Нужно ли пропустить элемент, если <see cref="ITreeNode{T,TId}.ParentId" /> не найден в списке или бросить <see
		///  	cref="ArgumentException" /> . </param>
		/// <remarks>
		/// 	Иногда требуется просто пробежаться по всему списку, без учёта иерархии. Этот метод, фактически, получает на вход дерево за счёт <see
		///  	cref="ITreeNode{T,TId}.Children" /> , и возвращает те же узлы, но одним списком, которое можно проитерировать без необходимости заходить в <see
		///  	cref="ITreeNode{T,TId}.Children" /> .
		/// </remarks>
		public static List<T> ToPlaneList<T, TId>(this TreeContainer<T, TId> tree, bool skipMissedParent = false)
			where T : class, ITreeNode<T, TId>
			where TId : struct, IComparable<TId>
		{
			if (tree == null) throw new ArgumentNullException("tree");
			var result = new List<T>();
			tree.ApplyAction(skipMissedParent, arg => true, result.Add);
			return result;
		}

		/// <summary>
		/// 	Применяет одно действие, если выполняется другое. Метод не оптимизирован по скорости.
		/// </summary>
		/// <typeparam name="T"> Тип для узла дерева. </typeparam>
		/// <typeparam name="TId"> Тип идентификатора для узла дерева. </typeparam>
		/// <param name="tree"> <see cref="TreeContainer{T,TId}" /> для обработки. </param>
		/// <param name="match"> Проверка - нужно ли применять действие. </param>
		/// <param name="action"> Действие, которое нужно применить. </param>
		/// <param name="skipHierarchyMatch"> Применять ли действие, для некоторых узлов и пути "наверх" в иерархии. </param>
		/// <param name="skipMissedParent"> Нужно ли пропустить элемент, если <see cref="ITreeNode{T,TId}.ParentId" /> не найден в списке или бросить 
		/// <see cref="ArgumentException" />  </param>
		public static void ApplyActionIfMatch<T, TId>(this TreeContainer<T, TId> tree, Func<T, bool> match,
		                                              Action<T, T> action,
		                                              Func<T, bool> skipHierarchyMatch = null, bool skipMissedParent = true)
			where T : class, ITreeNode<T, TId>
			where TId : struct, IComparable<TId>
		{
			if (skipHierarchyMatch == null)
			{
				tree.ApplyAction(skipMissedParent, match,
				                 t => action(t, t.ParentId.HasValue ? tree.GetFromMap(t.ParentId.Value) : null));
				tree.LockMap();
				return;
			}

			// если нужно пропустить что-то из обработки - сначала строим "исключения"
			var skippedNodes = new HashSet<T>();
			tree.ApplyAction(
				skipMissedParent, skipHierarchyMatch,
					t =>
					{
				 		skippedNodes.Add(t);
				 		T node = t;
				 		while (node.ParentId.HasValue)
				 		{
				 			node = tree.GetFromMap(node.ParentId.Value);
				 			if (!skippedNodes.Contains(node))
				 				skippedNodes.Add(node);
				 		}
				 	});
			tree.LockMap();

			tree.ApplyAction(
				false, match,
				 t =>
				 	{
				 		if (!skippedNodes.Contains(t))
				 			action(t, t.ParentId.HasValue ? tree.GetFromMap(t.ParentId.Value) : null);
				 	});
		}

		private static void ApplyAction<T, TId>(this TreeContainer<T, TId> tree, bool skipMissedParent,
		                                        Func<T, bool> match, Action<T> action)
			where T : class, ITreeNode<T, TId>
			where TId : struct, IComparable<TId>
		{
			foreach (var model in tree.Items.ToArray())
			{
				if (!tree.MapLocked)
					tree.AddToMap(model);
				
				if (!skipMissedParent)
				{
					T parent;
					if (model.ParentId.HasValue && !tree.TryGetFromMap(model.ParentId.Value, out parent))
						throw new ArgumentException("Node #{0} contains reference to parent #{1} not presented in tree."
						                            	.FormatWith(model.Id, model.ParentId));
				}

				if (match(model))
					action(model);

				if (model.HasChildren)
				{
					var container = new TreeContainer<T, TId>(tree.Map, model.Children);
					if (tree.MapLocked) container.LockMap();
					ApplyAction(container, skipMissedParent, match, action);
				}
			}
		}

		/// <summary>
		/// 	Преобразует плоский список <see cref="TreeNode{T,TId}" /> в дерево (или "лес").
		/// </summary>
		/// <typeparam name="T"> Тип для узла дерева. </typeparam>
		/// <typeparam name="TId"> Тип идентификатора для узла дерева. </typeparam>
		/// <param name="planeList"> Плоский список узлов. </param>
		/// <param name="skipMissedParent"> Нужно ли пропустить элемент, если <see cref="ITreeNode{T,TId}.ParentId" /> не найден в списке или бросить <see
		///  	cref="ArgumentException" /> . </param>
		/// <returns> Возвращает <see cref="TreeContainer{T,TId}" /> , а в нём список корневых узлов (к дочерним можно обратиться через <see
		///  	cref="ITreeNode{T,TId}.Children" /> ). </returns>
		/// <remarks>
		/// 	Этот метод, фактически, получает на вход список всех узлов, которые ещё не разложены в <see
		///  	cref="ITreeNode{T,TId}.Children" /> , и заполняет эти коллекции на основе <see cref="ITreeNode{T,TId}.ParentId" /> .
		/// </remarks>
		public static TreeContainer<T, TId> ToTree<T, TId>(this List<T> planeList, bool skipMissedParent = false)
			where T : class, ITreeNode<T, TId>
			where TId : struct, IComparable<TId>
		{
			if (planeList == null) throw new ArgumentNullException("planeList");

			var result = new TreeContainer<T, TId>(planeList.ToDictionary(x => x.Id), new List<T>());
			result.LockMap();

			foreach (T node in planeList)
			{
				if (node.ParentId.HasValue)
				{
					T parent;
					if (!result.TryGetFromMap(node.ParentId.Value, out parent))
					{
						if (skipMissedParent) continue;
						throw new ArgumentException("Node #{0} contains reference to parent #{1} not presented in list."
						                            	.FormatWith(node.Id, node.ParentId));
					}

					parent.Children.Add(node);
				}
				else
				{
					result.Items.Add(node);
				}
			}
			
			return result;
		}
	}
}
