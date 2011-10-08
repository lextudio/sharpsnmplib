

 


using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Tuples
{
#region ZeroTuple

    /// <summary>
    /// Tuple utility class. This class does not represent a zero-tuple, but is used
	/// for creating new tuples with the New method or converting lists to tuples using
	/// the ToTuple method
    /// </summary>
	public static class Tuple
	{
        /// <summary>
        /// Creates a new tuple of length 1 containing the passed values.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1</typeparam>
        /// <param name="t1">Value of the new Element1</param>
        /// <returns>Tuple of length 1 containing the values passed in.</returns>
		public static Tuple<T1> New<T1>(T1 t1)
		{
			return new Tuple<T1>(t1);
		}
		
        /// <summary>
        /// Creates a new tuple of length 2 containing the passed values.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1</typeparam>
        /// <param name="t1">Value of the new Element1</param>
        /// <typeparam name="T2">Type of the new Element2</typeparam>
        /// <param name="t2">Value of the new Element2</param>
        /// <returns>Tuple of length 2 containing the values passed in.</returns>
		public static Tuple<T1, T2> New<T1, T2>(T1 t1, T2 t2)
		{
			return new Tuple<T1, T2>(t1, t2);
		}
		
        /// <summary>
        /// Creates a new tuple of length 3 containing the passed values.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1</typeparam>
        /// <param name="t1">Value of the new Element1</param>
        /// <typeparam name="T2">Type of the new Element2</typeparam>
        /// <param name="t2">Value of the new Element2</param>
        /// <typeparam name="T3">Type of the new Element3</typeparam>
        /// <param name="t3">Value of the new Element3</param>
        /// <returns>Tuple of length 3 containing the values passed in.</returns>
		public static Tuple<T1, T2, T3> New<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
		{
			return new Tuple<T1, T2, T3>(t1, t2, t3);
		}
		
        /// <summary>
        /// Creates a new tuple of length 4 containing the passed values.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1</typeparam>
        /// <param name="t1">Value of the new Element1</param>
        /// <typeparam name="T2">Type of the new Element2</typeparam>
        /// <param name="t2">Value of the new Element2</param>
        /// <typeparam name="T3">Type of the new Element3</typeparam>
        /// <param name="t3">Value of the new Element3</param>
        /// <typeparam name="T4">Type of the new Element4</typeparam>
        /// <param name="t4">Value of the new Element4</param>
        /// <returns>Tuple of length 4 containing the values passed in.</returns>
		public static Tuple<T1, T2, T3, T4> New<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
		{
			return new Tuple<T1, T2, T3, T4>(t1, t2, t3, t4);
		}
		
        /// <summary>
        /// Creates a new tuple of length 5 containing the passed values.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1</typeparam>
        /// <param name="t1">Value of the new Element1</param>
        /// <typeparam name="T2">Type of the new Element2</typeparam>
        /// <param name="t2">Value of the new Element2</param>
        /// <typeparam name="T3">Type of the new Element3</typeparam>
        /// <param name="t3">Value of the new Element3</param>
        /// <typeparam name="T4">Type of the new Element4</typeparam>
        /// <param name="t4">Value of the new Element4</param>
        /// <typeparam name="T5">Type of the new Element5</typeparam>
        /// <param name="t5">Value of the new Element5</param>
        /// <returns>Tuple of length 5 containing the values passed in.</returns>
		public static Tuple<T1, T2, T3, T4, T5> New<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
		{
			return new Tuple<T1, T2, T3, T4, T5>(t1, t2, t3, t4, t5);
		}
		
        /// <summary>
        /// Creates a new tuple of length 6 containing the passed values.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1</typeparam>
        /// <param name="t1">Value of the new Element1</param>
        /// <typeparam name="T2">Type of the new Element2</typeparam>
        /// <param name="t2">Value of the new Element2</param>
        /// <typeparam name="T3">Type of the new Element3</typeparam>
        /// <param name="t3">Value of the new Element3</param>
        /// <typeparam name="T4">Type of the new Element4</typeparam>
        /// <param name="t4">Value of the new Element4</param>
        /// <typeparam name="T5">Type of the new Element5</typeparam>
        /// <param name="t5">Value of the new Element5</param>
        /// <typeparam name="T6">Type of the new Element6</typeparam>
        /// <param name="t6">Value of the new Element6</param>
        /// <returns>Tuple of length 6 containing the values passed in.</returns>
		public static Tuple<T1, T2, T3, T4, T5, T6> New<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6>(t1, t2, t3, t4, t5, t6);
		}
		
        /// <summary>
        /// Creates a new tuple of length 7 containing the passed values.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1</typeparam>
        /// <param name="t1">Value of the new Element1</param>
        /// <typeparam name="T2">Type of the new Element2</typeparam>
        /// <param name="t2">Value of the new Element2</param>
        /// <typeparam name="T3">Type of the new Element3</typeparam>
        /// <param name="t3">Value of the new Element3</param>
        /// <typeparam name="T4">Type of the new Element4</typeparam>
        /// <param name="t4">Value of the new Element4</param>
        /// <typeparam name="T5">Type of the new Element5</typeparam>
        /// <param name="t5">Value of the new Element5</param>
        /// <typeparam name="T6">Type of the new Element6</typeparam>
        /// <param name="t6">Value of the new Element6</param>
        /// <typeparam name="T7">Type of the new Element7</typeparam>
        /// <param name="t7">Value of the new Element7</param>
        /// <returns>Tuple of length 7 containing the values passed in.</returns>
		public static Tuple<T1, T2, T3, T4, T5, T6, T7> New<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7>(t1, t2, t3, t4, t5, t6, t7);
		}
		
        /// <summary>
        /// Creates a new tuple of length 8 containing the passed values.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1</typeparam>
        /// <param name="t1">Value of the new Element1</param>
        /// <typeparam name="T2">Type of the new Element2</typeparam>
        /// <param name="t2">Value of the new Element2</param>
        /// <typeparam name="T3">Type of the new Element3</typeparam>
        /// <param name="t3">Value of the new Element3</param>
        /// <typeparam name="T4">Type of the new Element4</typeparam>
        /// <param name="t4">Value of the new Element4</param>
        /// <typeparam name="T5">Type of the new Element5</typeparam>
        /// <param name="t5">Value of the new Element5</param>
        /// <typeparam name="T6">Type of the new Element6</typeparam>
        /// <param name="t6">Value of the new Element6</param>
        /// <typeparam name="T7">Type of the new Element7</typeparam>
        /// <param name="t7">Value of the new Element7</param>
        /// <typeparam name="T8">Type of the new Element8</typeparam>
        /// <param name="t8">Value of the new Element8</param>
        /// <returns>Tuple of length 8 containing the values passed in.</returns>
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8> New<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(t1, t2, t3, t4, t5, t6, t7, t8);
		}
		
        /// <summary>
        /// Creates a new tuple of length 9 containing the passed values.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1</typeparam>
        /// <param name="t1">Value of the new Element1</param>
        /// <typeparam name="T2">Type of the new Element2</typeparam>
        /// <param name="t2">Value of the new Element2</param>
        /// <typeparam name="T3">Type of the new Element3</typeparam>
        /// <param name="t3">Value of the new Element3</param>
        /// <typeparam name="T4">Type of the new Element4</typeparam>
        /// <param name="t4">Value of the new Element4</param>
        /// <typeparam name="T5">Type of the new Element5</typeparam>
        /// <param name="t5">Value of the new Element5</param>
        /// <typeparam name="T6">Type of the new Element6</typeparam>
        /// <param name="t6">Value of the new Element6</param>
        /// <typeparam name="T7">Type of the new Element7</typeparam>
        /// <param name="t7">Value of the new Element7</param>
        /// <typeparam name="T8">Type of the new Element8</typeparam>
        /// <param name="t8">Value of the new Element8</param>
        /// <typeparam name="T9">Type of the new Element9</typeparam>
        /// <param name="t9">Value of the new Element9</param>
        /// <returns>Tuple of length 9 containing the values passed in.</returns>
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> New<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1, t2, t3, t4, t5, t6, t7, t8, t9);
		}
		
        /// <summary>
        /// Creates a new tuple of length 10 containing the passed values.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1</typeparam>
        /// <param name="t1">Value of the new Element1</param>
        /// <typeparam name="T2">Type of the new Element2</typeparam>
        /// <param name="t2">Value of the new Element2</param>
        /// <typeparam name="T3">Type of the new Element3</typeparam>
        /// <param name="t3">Value of the new Element3</param>
        /// <typeparam name="T4">Type of the new Element4</typeparam>
        /// <param name="t4">Value of the new Element4</param>
        /// <typeparam name="T5">Type of the new Element5</typeparam>
        /// <param name="t5">Value of the new Element5</param>
        /// <typeparam name="T6">Type of the new Element6</typeparam>
        /// <param name="t6">Value of the new Element6</param>
        /// <typeparam name="T7">Type of the new Element7</typeparam>
        /// <param name="t7">Value of the new Element7</param>
        /// <typeparam name="T8">Type of the new Element8</typeparam>
        /// <param name="t8">Value of the new Element8</param>
        /// <typeparam name="T9">Type of the new Element9</typeparam>
        /// <param name="t9">Value of the new Element9</param>
        /// <typeparam name="T10">Type of the new Element10</typeparam>
        /// <param name="t10">Value of the new Element10</param>
        /// <returns>Tuple of length 10 containing the values passed in.</returns>
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
		{
			return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
		}
		
#region ToTuple methods

        /// <summary>
        /// Creates a tuple of length 1 by taking values from the enumerable passed in.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1. Enumerable must have an object of the same type at position 1</typeparam>
        /// <param name="pList">The Enumerable form which to take the new tuple elements. Should have at least 1 elements.</param>
        /// <returns>Tuple of length 1 that contains values from the enumerable.</returns>
		public static Tuple<T1> ToTuple<T1>(IEnumerable pList)
		{
			Tuple<T1> t = new Tuple<T1>();
			IEnumerator e = pList.GetEnumerator();
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 1 elements.", "pList");
			t.Element1 = (T1)e.Current;
			return t;
		}
		
        /// <summary>
        /// Creates a tuple of length 2 by taking values from the enumerable passed in.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1. Enumerable must have an object of the same type at position 1</typeparam>
        /// <typeparam name="T2">Type of the new Element2. Enumerable must have an object of the same type at position 2</typeparam>
        /// <param name="pList">The Enumerable form which to take the new tuple elements. Should have at least 2 elements.</param>
        /// <returns>Tuple of length 2 that contains values from the enumerable.</returns>
		public static Tuple<T1, T2> ToTuple<T1, T2>(IEnumerable pList)
		{
			Tuple<T1, T2> t = new Tuple<T1, T2>();
			IEnumerator e = pList.GetEnumerator();
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 2 elements.", "pList");
			t.Element1 = (T1)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 2 elements.", "pList");
			t.Element2 = (T2)e.Current;
			return t;
		}
		
        /// <summary>
        /// Creates a tuple of length 3 by taking values from the enumerable passed in.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1. Enumerable must have an object of the same type at position 1</typeparam>
        /// <typeparam name="T2">Type of the new Element2. Enumerable must have an object of the same type at position 2</typeparam>
        /// <typeparam name="T3">Type of the new Element3. Enumerable must have an object of the same type at position 3</typeparam>
        /// <param name="pList">The Enumerable form which to take the new tuple elements. Should have at least 3 elements.</param>
        /// <returns>Tuple of length 3 that contains values from the enumerable.</returns>
		public static Tuple<T1, T2, T3> ToTuple<T1, T2, T3>(IEnumerable pList)
		{
			Tuple<T1, T2, T3> t = new Tuple<T1, T2, T3>();
			IEnumerator e = pList.GetEnumerator();
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 3 elements.", "pList");
			t.Element1 = (T1)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 3 elements.", "pList");
			t.Element2 = (T2)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 3 elements.", "pList");
			t.Element3 = (T3)e.Current;
			return t;
		}
		
        /// <summary>
        /// Creates a tuple of length 4 by taking values from the enumerable passed in.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1. Enumerable must have an object of the same type at position 1</typeparam>
        /// <typeparam name="T2">Type of the new Element2. Enumerable must have an object of the same type at position 2</typeparam>
        /// <typeparam name="T3">Type of the new Element3. Enumerable must have an object of the same type at position 3</typeparam>
        /// <typeparam name="T4">Type of the new Element4. Enumerable must have an object of the same type at position 4</typeparam>
        /// <param name="pList">The Enumerable form which to take the new tuple elements. Should have at least 4 elements.</param>
        /// <returns>Tuple of length 4 that contains values from the enumerable.</returns>
		public static Tuple<T1, T2, T3, T4> ToTuple<T1, T2, T3, T4>(IEnumerable pList)
		{
			Tuple<T1, T2, T3, T4> t = new Tuple<T1, T2, T3, T4>();
			IEnumerator e = pList.GetEnumerator();
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 4 elements.", "pList");
			t.Element1 = (T1)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 4 elements.", "pList");
			t.Element2 = (T2)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 4 elements.", "pList");
			t.Element3 = (T3)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 4 elements.", "pList");
			t.Element4 = (T4)e.Current;
			return t;
		}
		
        /// <summary>
        /// Creates a tuple of length 5 by taking values from the enumerable passed in.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1. Enumerable must have an object of the same type at position 1</typeparam>
        /// <typeparam name="T2">Type of the new Element2. Enumerable must have an object of the same type at position 2</typeparam>
        /// <typeparam name="T3">Type of the new Element3. Enumerable must have an object of the same type at position 3</typeparam>
        /// <typeparam name="T4">Type of the new Element4. Enumerable must have an object of the same type at position 4</typeparam>
        /// <typeparam name="T5">Type of the new Element5. Enumerable must have an object of the same type at position 5</typeparam>
        /// <param name="pList">The Enumerable form which to take the new tuple elements. Should have at least 5 elements.</param>
        /// <returns>Tuple of length 5 that contains values from the enumerable.</returns>
		public static Tuple<T1, T2, T3, T4, T5> ToTuple<T1, T2, T3, T4, T5>(IEnumerable pList)
		{
			Tuple<T1, T2, T3, T4, T5> t = new Tuple<T1, T2, T3, T4, T5>();
			IEnumerator e = pList.GetEnumerator();
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 5 elements.", "pList");
			t.Element1 = (T1)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 5 elements.", "pList");
			t.Element2 = (T2)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 5 elements.", "pList");
			t.Element3 = (T3)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 5 elements.", "pList");
			t.Element4 = (T4)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 5 elements.", "pList");
			t.Element5 = (T5)e.Current;
			return t;
		}
		
        /// <summary>
        /// Creates a tuple of length 6 by taking values from the enumerable passed in.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1. Enumerable must have an object of the same type at position 1</typeparam>
        /// <typeparam name="T2">Type of the new Element2. Enumerable must have an object of the same type at position 2</typeparam>
        /// <typeparam name="T3">Type of the new Element3. Enumerable must have an object of the same type at position 3</typeparam>
        /// <typeparam name="T4">Type of the new Element4. Enumerable must have an object of the same type at position 4</typeparam>
        /// <typeparam name="T5">Type of the new Element5. Enumerable must have an object of the same type at position 5</typeparam>
        /// <typeparam name="T6">Type of the new Element6. Enumerable must have an object of the same type at position 6</typeparam>
        /// <param name="pList">The Enumerable form which to take the new tuple elements. Should have at least 6 elements.</param>
        /// <returns>Tuple of length 6 that contains values from the enumerable.</returns>
		public static Tuple<T1, T2, T3, T4, T5, T6> ToTuple<T1, T2, T3, T4, T5, T6>(IEnumerable pList)
		{
			Tuple<T1, T2, T3, T4, T5, T6> t = new Tuple<T1, T2, T3, T4, T5, T6>();
			IEnumerator e = pList.GetEnumerator();
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 6 elements.", "pList");
			t.Element1 = (T1)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 6 elements.", "pList");
			t.Element2 = (T2)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 6 elements.", "pList");
			t.Element3 = (T3)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 6 elements.", "pList");
			t.Element4 = (T4)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 6 elements.", "pList");
			t.Element5 = (T5)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 6 elements.", "pList");
			t.Element6 = (T6)e.Current;
			return t;
		}
		
        /// <summary>
        /// Creates a tuple of length 7 by taking values from the enumerable passed in.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1. Enumerable must have an object of the same type at position 1</typeparam>
        /// <typeparam name="T2">Type of the new Element2. Enumerable must have an object of the same type at position 2</typeparam>
        /// <typeparam name="T3">Type of the new Element3. Enumerable must have an object of the same type at position 3</typeparam>
        /// <typeparam name="T4">Type of the new Element4. Enumerable must have an object of the same type at position 4</typeparam>
        /// <typeparam name="T5">Type of the new Element5. Enumerable must have an object of the same type at position 5</typeparam>
        /// <typeparam name="T6">Type of the new Element6. Enumerable must have an object of the same type at position 6</typeparam>
        /// <typeparam name="T7">Type of the new Element7. Enumerable must have an object of the same type at position 7</typeparam>
        /// <param name="pList">The Enumerable form which to take the new tuple elements. Should have at least 7 elements.</param>
        /// <returns>Tuple of length 7 that contains values from the enumerable.</returns>
		public static Tuple<T1, T2, T3, T4, T5, T6, T7> ToTuple<T1, T2, T3, T4, T5, T6, T7>(IEnumerable pList)
		{
			Tuple<T1, T2, T3, T4, T5, T6, T7> t = new Tuple<T1, T2, T3, T4, T5, T6, T7>();
			IEnumerator e = pList.GetEnumerator();
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 7 elements.", "pList");
			t.Element1 = (T1)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 7 elements.", "pList");
			t.Element2 = (T2)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 7 elements.", "pList");
			t.Element3 = (T3)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 7 elements.", "pList");
			t.Element4 = (T4)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 7 elements.", "pList");
			t.Element5 = (T5)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 7 elements.", "pList");
			t.Element6 = (T6)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 7 elements.", "pList");
			t.Element7 = (T7)e.Current;
			return t;
		}
		
        /// <summary>
        /// Creates a tuple of length 8 by taking values from the enumerable passed in.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1. Enumerable must have an object of the same type at position 1</typeparam>
        /// <typeparam name="T2">Type of the new Element2. Enumerable must have an object of the same type at position 2</typeparam>
        /// <typeparam name="T3">Type of the new Element3. Enumerable must have an object of the same type at position 3</typeparam>
        /// <typeparam name="T4">Type of the new Element4. Enumerable must have an object of the same type at position 4</typeparam>
        /// <typeparam name="T5">Type of the new Element5. Enumerable must have an object of the same type at position 5</typeparam>
        /// <typeparam name="T6">Type of the new Element6. Enumerable must have an object of the same type at position 6</typeparam>
        /// <typeparam name="T7">Type of the new Element7. Enumerable must have an object of the same type at position 7</typeparam>
        /// <typeparam name="T8">Type of the new Element8. Enumerable must have an object of the same type at position 8</typeparam>
        /// <param name="pList">The Enumerable form which to take the new tuple elements. Should have at least 8 elements.</param>
        /// <returns>Tuple of length 8 that contains values from the enumerable.</returns>
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8>(IEnumerable pList)
		{
			Tuple<T1, T2, T3, T4, T5, T6, T7, T8> t = new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>();
			IEnumerator e = pList.GetEnumerator();
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 8 elements.", "pList");
			t.Element1 = (T1)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 8 elements.", "pList");
			t.Element2 = (T2)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 8 elements.", "pList");
			t.Element3 = (T3)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 8 elements.", "pList");
			t.Element4 = (T4)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 8 elements.", "pList");
			t.Element5 = (T5)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 8 elements.", "pList");
			t.Element6 = (T6)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 8 elements.", "pList");
			t.Element7 = (T7)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 8 elements.", "pList");
			t.Element8 = (T8)e.Current;
			return t;
		}
		
        /// <summary>
        /// Creates a tuple of length 9 by taking values from the enumerable passed in.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1. Enumerable must have an object of the same type at position 1</typeparam>
        /// <typeparam name="T2">Type of the new Element2. Enumerable must have an object of the same type at position 2</typeparam>
        /// <typeparam name="T3">Type of the new Element3. Enumerable must have an object of the same type at position 3</typeparam>
        /// <typeparam name="T4">Type of the new Element4. Enumerable must have an object of the same type at position 4</typeparam>
        /// <typeparam name="T5">Type of the new Element5. Enumerable must have an object of the same type at position 5</typeparam>
        /// <typeparam name="T6">Type of the new Element6. Enumerable must have an object of the same type at position 6</typeparam>
        /// <typeparam name="T7">Type of the new Element7. Enumerable must have an object of the same type at position 7</typeparam>
        /// <typeparam name="T8">Type of the new Element8. Enumerable must have an object of the same type at position 8</typeparam>
        /// <typeparam name="T9">Type of the new Element9. Enumerable must have an object of the same type at position 9</typeparam>
        /// <param name="pList">The Enumerable form which to take the new tuple elements. Should have at least 9 elements.</param>
        /// <returns>Tuple of length 9 that contains values from the enumerable.</returns>
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IEnumerable pList)
		{
			Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> t = new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>();
			IEnumerator e = pList.GetEnumerator();
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 9 elements.", "pList");
			t.Element1 = (T1)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 9 elements.", "pList");
			t.Element2 = (T2)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 9 elements.", "pList");
			t.Element3 = (T3)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 9 elements.", "pList");
			t.Element4 = (T4)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 9 elements.", "pList");
			t.Element5 = (T5)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 9 elements.", "pList");
			t.Element6 = (T6)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 9 elements.", "pList");
			t.Element7 = (T7)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 9 elements.", "pList");
			t.Element8 = (T8)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 9 elements.", "pList");
			t.Element9 = (T9)e.Current;
			return t;
		}
		
        /// <summary>
        /// Creates a tuple of length 10 by taking values from the enumerable passed in.
        /// </summary>
        /// <typeparam name="T1">Type of the new Element1. Enumerable must have an object of the same type at position 1</typeparam>
        /// <typeparam name="T2">Type of the new Element2. Enumerable must have an object of the same type at position 2</typeparam>
        /// <typeparam name="T3">Type of the new Element3. Enumerable must have an object of the same type at position 3</typeparam>
        /// <typeparam name="T4">Type of the new Element4. Enumerable must have an object of the same type at position 4</typeparam>
        /// <typeparam name="T5">Type of the new Element5. Enumerable must have an object of the same type at position 5</typeparam>
        /// <typeparam name="T6">Type of the new Element6. Enumerable must have an object of the same type at position 6</typeparam>
        /// <typeparam name="T7">Type of the new Element7. Enumerable must have an object of the same type at position 7</typeparam>
        /// <typeparam name="T8">Type of the new Element8. Enumerable must have an object of the same type at position 8</typeparam>
        /// <typeparam name="T9">Type of the new Element9. Enumerable must have an object of the same type at position 9</typeparam>
        /// <typeparam name="T10">Type of the new Element10. Enumerable must have an object of the same type at position 10</typeparam>
        /// <param name="pList">The Enumerable form which to take the new tuple elements. Should have at least 10 elements.</param>
        /// <returns>Tuple of length 10 that contains values from the enumerable.</returns>
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IEnumerable pList)
		{
			Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> t = new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
			IEnumerator e = pList.GetEnumerator();
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 10 elements.", "pList");
			t.Element1 = (T1)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 10 elements.", "pList");
			t.Element2 = (T2)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 10 elements.", "pList");
			t.Element3 = (T3)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 10 elements.", "pList");
			t.Element4 = (T4)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 10 elements.", "pList");
			t.Element5 = (T5)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 10 elements.", "pList");
			t.Element6 = (T6)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 10 elements.", "pList");
			t.Element7 = (T7)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 10 elements.", "pList");
			t.Element8 = (T8)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 10 elements.", "pList");
			t.Element9 = (T9)e.Current;
			if(!e.MoveNext())
				throw new ArgumentException("Enumerable is too short. Needs to have at least 10 elements.", "pList");
			t.Element10 = (T10)e.Current;
			return t;
		}
		

#endregion
	}

#endregion

#region Tuples


/// <summary>
/// Represents a tuple of length 1
/// </summary>
/// <typeparam name="T1">Type of the tuple's Element1</typeparam>
[DataContract]
public class Tuple<T1> : ICollection, IEnumerable, IEnumerable<Object>,
	IEquatable<Tuple<T1>>, IComparable<Tuple<T1>>
{
    /// <summary>
    /// An empty tuple constructor. All elements will have their default values.
    /// </summary>
	public Tuple()
	{
	}
	
    /// <summary>
    /// Tuple constructor. The first 1 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	public Tuple(T1 t1)
	{
		Element1 = t1;
	}
	
    /// <summary>
    /// Creates a new tuple of length 2 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <param name="t2">Value of the new tuple's Element2</param>
    /// <returns>A new tuple of length 2 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2> Append<T2>(T2 t2)
	{
		return Tuple.New(
			Element1
			,t2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 3 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <param name="t2">Value of the new tuple's Element2</param>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
    /// <returns>A new tuple of length 3 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3> Append<T2, T3>(T2 t2, T3 t3)
	{
		return Tuple.New(
			Element1
			,t2
			,t3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 4 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <param name="t2">Value of the new tuple's Element2</param>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
    /// <returns>A new tuple of length 4 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4> Append<T2, T3, T4>(T2 t2, T3 t3, T4 t4)
	{
		return Tuple.New(
			Element1
			,t2
			,t3
			,t4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <param name="t2">Value of the new tuple's Element2</param>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5> Append<T2, T3, T4, T5>(T2 t2, T3 t3, T4 t4, T5 t5)
	{
		return Tuple.New(
			Element1
			,t2
			,t3
			,t4
			,t5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <param name="t2">Value of the new tuple's Element2</param>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6> Append<T2, T3, T4, T5, T6>(T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{
		return Tuple.New(
			Element1
			,t2
			,t3
			,t4
			,t5
			,t6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <param name="t2">Value of the new tuple's Element2</param>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T2, T3, T4, T5, T6, T7>(T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		return Tuple.New(
			Element1
			,t2
			,t3
			,t4
			,t5
			,t6
			,t7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <param name="t2">Value of the new tuple's Element2</param>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T2, T3, T4, T5, T6, T7, T8>(T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		return Tuple.New(
			Element1
			,t2
			,t3
			,t4
			,t5
			,t6
			,t7
			,t8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <param name="t2">Value of the new tuple's Element2</param>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T2, T3, T4, T5, T6, T7, T8, T9>(T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			Element1
			,t2
			,t3
			,t4
			,t5
			,t6
			,t7
			,t8
			,t9
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <param name="t2">Value of the new tuple's Element2</param>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="t10">Value of the new tuple's Element10</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T2, T3, T4, T5, T6, T7, T8, T9, T10>(T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			Element1
			,t2
			,t3
			,t4
			,t5
			,t6
			,t7
			,t8
			,t9
			,t10
		);
	}

    /// <summary>
    /// Creates a new tuple of length 2 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <param name="t2">Value of the new tuple's Element1</param>
    /// <returns>A new tuple of length 2 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T1> Prepend<T2>(T2 t2)
	{
		return Tuple.New(
			t2
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 3 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <param name="t2">Value of the new tuple's Element1</param>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <param name="t3">Value of the new tuple's Element2</param>
    /// <returns>A new tuple of length 3 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T1> Prepend<T2, T3>(T2 t2, T3 t3)
	{
		return Tuple.New(
			t2
			,t3
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 4 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <param name="t2">Value of the new tuple's Element1</param>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <param name="t3">Value of the new tuple's Element2</param>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <param name="t4">Value of the new tuple's Element3</param>
    /// <returns>A new tuple of length 4 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T1> Prepend<T2, T3, T4>(T2 t2, T3 t3, T4 t4)
	{
		return Tuple.New(
			t2
			,t3
			,t4
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <param name="t2">Value of the new tuple's Element1</param>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <param name="t3">Value of the new tuple's Element2</param>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <param name="t4">Value of the new tuple's Element3</param>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <param name="t5">Value of the new tuple's Element4</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T1> Prepend<T2, T3, T4, T5>(T2 t2, T3 t3, T4 t4, T5 t5)
	{
		return Tuple.New(
			t2
			,t3
			,t4
			,t5
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <param name="t2">Value of the new tuple's Element1</param>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <param name="t3">Value of the new tuple's Element2</param>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <param name="t4">Value of the new tuple's Element3</param>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <param name="t5">Value of the new tuple's Element4</param>
	/// <typeparam name="T6">Type of the new tuple's Element5</typeparam>
	/// <param name="t6">Value of the new tuple's Element5</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T6, T1> Prepend<T2, T3, T4, T5, T6>(T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{
		return Tuple.New(
			t2
			,t3
			,t4
			,t5
			,t6
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <param name="t2">Value of the new tuple's Element1</param>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <param name="t3">Value of the new tuple's Element2</param>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <param name="t4">Value of the new tuple's Element3</param>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <param name="t5">Value of the new tuple's Element4</param>
	/// <typeparam name="T6">Type of the new tuple's Element5</typeparam>
	/// <param name="t6">Value of the new tuple's Element5</param>
	/// <typeparam name="T7">Type of the new tuple's Element6</typeparam>
	/// <param name="t7">Value of the new tuple's Element6</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T6, T7, T1> Prepend<T2, T3, T4, T5, T6, T7>(T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		return Tuple.New(
			t2
			,t3
			,t4
			,t5
			,t6
			,t7
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <param name="t2">Value of the new tuple's Element1</param>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <param name="t3">Value of the new tuple's Element2</param>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <param name="t4">Value of the new tuple's Element3</param>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <param name="t5">Value of the new tuple's Element4</param>
	/// <typeparam name="T6">Type of the new tuple's Element5</typeparam>
	/// <param name="t6">Value of the new tuple's Element5</param>
	/// <typeparam name="T7">Type of the new tuple's Element6</typeparam>
	/// <param name="t7">Value of the new tuple's Element6</param>
	/// <typeparam name="T8">Type of the new tuple's Element7</typeparam>
	/// <param name="t8">Value of the new tuple's Element7</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T6, T7, T8, T1> Prepend<T2, T3, T4, T5, T6, T7, T8>(T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		return Tuple.New(
			t2
			,t3
			,t4
			,t5
			,t6
			,t7
			,t8
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <param name="t2">Value of the new tuple's Element1</param>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <param name="t3">Value of the new tuple's Element2</param>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <param name="t4">Value of the new tuple's Element3</param>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <param name="t5">Value of the new tuple's Element4</param>
	/// <typeparam name="T6">Type of the new tuple's Element5</typeparam>
	/// <param name="t6">Value of the new tuple's Element5</param>
	/// <typeparam name="T7">Type of the new tuple's Element6</typeparam>
	/// <param name="t7">Value of the new tuple's Element6</param>
	/// <typeparam name="T8">Type of the new tuple's Element7</typeparam>
	/// <param name="t8">Value of the new tuple's Element7</param>
	/// <typeparam name="T9">Type of the new tuple's Element8</typeparam>
	/// <param name="t9">Value of the new tuple's Element8</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T6, T7, T8, T9, T1> Prepend<T2, T3, T4, T5, T6, T7, T8, T9>(T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			t2
			,t3
			,t4
			,t5
			,t6
			,t7
			,t8
			,t9
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <param name="t2">Value of the new tuple's Element1</param>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <param name="t3">Value of the new tuple's Element2</param>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <param name="t4">Value of the new tuple's Element3</param>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <param name="t5">Value of the new tuple's Element4</param>
	/// <typeparam name="T6">Type of the new tuple's Element5</typeparam>
	/// <param name="t6">Value of the new tuple's Element5</param>
	/// <typeparam name="T7">Type of the new tuple's Element6</typeparam>
	/// <param name="t7">Value of the new tuple's Element6</param>
	/// <typeparam name="T8">Type of the new tuple's Element7</typeparam>
	/// <param name="t8">Value of the new tuple's Element7</param>
	/// <typeparam name="T9">Type of the new tuple's Element8</typeparam>
	/// <param name="t9">Value of the new tuple's Element8</param>
	/// <typeparam name="T10">Type of the new tuple's Element9</typeparam>
	/// <param name="t10">Value of the new tuple's Element9</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T6, T7, T8, T9, T10, T1> Prepend<T2, T3, T4, T5, T6, T7, T8, T9, T10>(T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			t2
			,t3
			,t4
			,t5
			,t6
			,t7
			,t8
			,t9
			,t10
			,Element1
		);
	}

	
    /// <summary>
    /// Creates a new tuple of length 2 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 2 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2> Append<T2>(Tuple<T2> pOther)
	{
		return Tuple.New(
			Element1
			,pOther.Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 3 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 3 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3> Append<T2, T3>(Tuple<T2, T3> pOther)
	{
		return Tuple.New(
			Element1
			,pOther.Element1
			,pOther.Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 4 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 4 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4> Append<T2, T3, T4>(Tuple<T2, T3, T4> pOther)
	{
		return Tuple.New(
			Element1
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 5 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5> Append<T2, T3, T4, T5>(Tuple<T2, T3, T4, T5> pOther)
	{
		return Tuple.New(
			Element1
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="pOther">Tuple of length 5 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 6 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6> Append<T2, T3, T4, T5, T6>(Tuple<T2, T3, T4, T5, T6> pOther)
	{
		return Tuple.New(
			Element1
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="pOther">Tuple of length 6 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 7 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T2, T3, T4, T5, T6, T7>(Tuple<T2, T3, T4, T5, T6, T7> pOther)
	{
		return Tuple.New(
			Element1
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="pOther">Tuple of length 7 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 8 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T2, T3, T4, T5, T6, T7, T8>(Tuple<T2, T3, T4, T5, T6, T7, T8> pOther)
	{
		return Tuple.New(
			Element1
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="pOther">Tuple of length 8 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 9 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T2, T3, T4, T5, T6, T7, T8, T9>(Tuple<T2, T3, T4, T5, T6, T7, T8, T9> pOther)
	{
		return Tuple.New(
			Element1
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
			,pOther.Element8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="pOther">Tuple of length 9 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 10 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T2, T3, T4, T5, T6, T7, T8, T9, T10>(Tuple<T2, T3, T4, T5, T6, T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			Element1
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
			,pOther.Element8
			,pOther.Element9
		);
	}

    /// <summary>
    /// Creates a new tuple of length 2 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 2 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T1> Prepend<T2>(Tuple<T2> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 3 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 3 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T1> Prepend<T2, T3>(Tuple<T2, T3> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 4 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 4 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T1> Prepend<T2, T3, T4>(Tuple<T2, T3, T4> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T1> Prepend<T2, T3, T4, T5>(Tuple<T2, T3, T4, T5> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element5</typeparam>
	/// <param name="pOther">Tuple of length 5 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T6, T1> Prepend<T2, T3, T4, T5, T6>(Tuple<T2, T3, T4, T5, T6> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element6</typeparam>
	/// <param name="pOther">Tuple of length 6 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T6, T7, T1> Prepend<T2, T3, T4, T5, T6, T7>(Tuple<T2, T3, T4, T5, T6, T7> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element7</typeparam>
	/// <param name="pOther">Tuple of length 7 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T6, T7, T8, T1> Prepend<T2, T3, T4, T5, T6, T7, T8>(Tuple<T2, T3, T4, T5, T6, T7, T8> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element8</typeparam>
	/// <param name="pOther">Tuple of length 8 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T6, T7, T8, T9, T1> Prepend<T2, T3, T4, T5, T6, T7, T8, T9>(Tuple<T2, T3, T4, T5, T6, T7, T8, T9> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
			,pOther.Element8
			,Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T2">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T3">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element9</typeparam>
	/// <param name="pOther">Tuple of length 9 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T2, T3, T4, T5, T6, T7, T8, T9, T10, T1> Prepend<T2, T3, T4, T5, T6, T7, T8, T9, T10>(Tuple<T2, T3, T4, T5, T6, T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
			,pOther.Element8
			,pOther.Element9
			,Element1
		);
	}


#region Object overrides

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>Hash code of the object.</returns>
	public override int GetHashCode()
	{
		int hash = 0;
		hash ^= Element1.GetHashCode();
		return hash;
	}
	
    /// <summary>
    /// Returns a value indicating weather this instance is equal to another instance.
    /// </summary>
    /// <param name="pObj">The object we wish to compare with this instance.</param>
    /// <returns>A value indicating if this object is equal to the one passed in.</returns>
	public override bool Equals(Object pObj)
	{
		if(pObj == null)
			return false;
		if(!(pObj is Tuple<T1>))
			return false;

		return Equals((Tuple<T1>)pObj);
	}
	
    /// <summary>
    /// Converts the tuple to a string. This will be a comma separated list
	/// of the string values of the elements enclosed in brackets.
    /// </summary>
    /// <returns>A string representation of the tuple.</returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(Element1);
		sb.Append(")");
		return sb.ToString();
	}
	
	
    /// <summary>
	/// Returns a string representation of the tuple using the specified format.
    /// </summary>
	/// <param name="pFormat">The format to use for the string representation.</param>
    /// <returns>A string representation of the tuple.</returns>
	public string ToString(String pFormat)
	{
		return String.Format(pFormat
			,Element1
		);
	}
	
#endregion

#region IEquatable<> implementation

    /// <summary>
    /// A value indicating if this tuple is equal to a tuple
	/// of the same length and type. This will be so if all members are
	/// equal.
    /// </summary>
    /// <returns>A value indicating weather this tuple is equal to another tuple of the same length and type.</returns>
	public bool Equals(Tuple<T1> pObj)
	{
		if(pObj == null)
			return false;

		bool result = true;
		result = result && EqualityComparer<T1>.Default.Equals(Element1, pObj.Element1);
		return result;
	}
	
#endregion

#region ICollection implementation

    /// <summary>
    /// Copies the elements of this tuple to an Array.
	/// The array should have at least 1 elements available
	/// after the index parameter.
    /// </summary>
	/// <param name="pArray">The array to copy the values to.</param>
	/// <param name="pIndex">The offset in the array at which to start inserting the values.</param>
	void ICollection.CopyTo(Array pArray, int pIndex)
	{
		if (pArray == null)
			throw new ArgumentNullException("pArray");
		if (pIndex < 0)
			throw new ArgumentOutOfRangeException("pIndex");
		if (pArray.Length - pIndex <= 0 || (pArray.Length - pIndex) < 1)
			throw new ArgumentException("pIndex");

		pArray.SetValue(Element1, pIndex + 0);
	}
	
    /// <summary>
    /// Gets the length of the tuple, that is it returns 1.
    /// </summary>
	int ICollection.Count
	{
		get { return 1; }
	}
	
	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	Object ICollection.SyncRoot 
	{
		get { return this; }
	}
	
#endregion

#region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

#endregion

#region IEnumerable<object> implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	public IEnumerator<Object> GetEnumerator()
    {
		yield return Element1;
    }

#endregion

#region IComparable<> implementation

    /// <summary>
    /// Returns a value indicating the order of this tuple compared
	/// to another tuple of the same length and type. The order is defined
	/// as the order of the first element of the tuples.
    /// </summary>
	/// <param name="pOther">The tuple we are comparing this one to.</param>
    /// <returns>value indicating the order of this tuple compared to another tuple of the same length and type.</returns>
	public int CompareTo(Tuple<T1> pOther)
	{
		return Comparer<T1>.Default.Compare(Element1, pOther.Element1);
	}

#endregion

    /// <summary>
    /// Get or sets the value of the element at
	/// the specified index in the tuple.
    /// </summary>
    /// <param name="pIndex">The index of the element in the tuple.</param>
	public Object this[int pIndex]
	{
		get
		{
			switch(pIndex)
			{
				case 0 : return Element1;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
			
		set
		{
			switch(pIndex)
			{
				case 0 :
					if(value is T1)
						Element1 = (T1)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
	}
	
	/// <summary>
    /// Compares two tuples and returns a value indicating if they are equal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are equal.</returns>
	public static bool operator==(Tuple<T1> pA, Tuple<T1> pB)
	{
		if(System.Object.ReferenceEquals(pA, pB))
			return true;

		if((object)pA == null || (object)pB == null)
			return false;

		return pA.Equals(pB);
	}

	/// <summary>
    /// Compares two tuples and returns a value indicating if they are unequal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are unequal.</returns>
	public static bool operator!=(Tuple<T1> pA, Tuple<T1> pB)
	{
		return !pA.Equals(pB);
	}
	
    /// <summary>
    /// Gets the element of the tuple at position 1.
    /// </summary>
	[DataMember]
	public T1 Element1 { get; set; }
	
 
    /// <summary>
    /// Gets or sets the first element of
	/// the tuple. Same as using Element1. Only added for
	/// syntax reasons.
    /// </summary>
	public T1 First
	{ 
		get { return Element1; }
		set { Element1 = value; }
	}
	
 
    /// <summary>
    /// Gets or sets the head of the tuple, that is the first element.
	/// Same as using the properties Element1 or First. Only Added
	/// for syntax reasons.
    /// </summary>
	public T1 Head
	{
		get { return Element1; }
		set { Element1 = value; }
	}
	
}


/// <summary>
/// Represents a tuple of length 2
/// </summary>
/// <typeparam name="T1">Type of the tuple's Element1</typeparam>
/// <typeparam name="T2">Type of the tuple's Element2</typeparam>
[DataContract]
public class Tuple<T1, T2> : ICollection, IEnumerable, IEnumerable<Object>,
	IEquatable<Tuple<T1, T2>>, IComparable<Tuple<T1, T2>>
{
    /// <summary>
    /// An empty tuple constructor. All elements will have their default values.
    /// </summary>
	public Tuple()
	{
	}
	
    /// <summary>
    /// Tuple constructor. The first 1 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	public Tuple(T1 t1)
	{
		Element1 = t1;
	}
	
    /// <summary>
    /// Tuple constructor. The first 2 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	public Tuple(T1 t1, T2 t2)
	{
		Element1 = t1;
		Element2 = t2;
	}
	
    /// <summary>
    /// Creates a new tuple of length 3 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
    /// <returns>A new tuple of length 3 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3> Append<T3>(T3 t3)
	{
		return Tuple.New(
			Element1
			,Element2
			,t3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 4 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
    /// <returns>A new tuple of length 4 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4> Append<T3, T4>(T3 t3, T4 t4)
	{
		return Tuple.New(
			Element1
			,Element2
			,t3
			,t4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5> Append<T3, T4, T5>(T3 t3, T4 t4, T5 t5)
	{
		return Tuple.New(
			Element1
			,Element2
			,t3
			,t4
			,t5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6> Append<T3, T4, T5, T6>(T3 t3, T4 t4, T5 t5, T6 t6)
	{
		return Tuple.New(
			Element1
			,Element2
			,t3
			,t4
			,t5
			,t6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T3, T4, T5, T6, T7>(T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		return Tuple.New(
			Element1
			,Element2
			,t3
			,t4
			,t5
			,t6
			,t7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T3, T4, T5, T6, T7, T8>(T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		return Tuple.New(
			Element1
			,Element2
			,t3
			,t4
			,t5
			,t6
			,t7
			,t8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T3, T4, T5, T6, T7, T8, T9>(T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			Element1
			,Element2
			,t3
			,t4
			,t5
			,t6
			,t7
			,t8
			,t9
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="t3">Value of the new tuple's Element3</param>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="t10">Value of the new tuple's Element10</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T3, T4, T5, T6, T7, T8, T9, T10>(T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			Element1
			,Element2
			,t3
			,t4
			,t5
			,t6
			,t7
			,t8
			,t9
			,t10
		);
	}

    /// <summary>
    /// Creates a new tuple of length 3 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <param name="t3">Value of the new tuple's Element1</param>
    /// <returns>A new tuple of length 3 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T1, T2> Prepend<T3>(T3 t3)
	{
		return Tuple.New(
			t3
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 4 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <param name="t3">Value of the new tuple's Element1</param>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <param name="t4">Value of the new tuple's Element2</param>
    /// <returns>A new tuple of length 4 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T1, T2> Prepend<T3, T4>(T3 t3, T4 t4)
	{
		return Tuple.New(
			t3
			,t4
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <param name="t3">Value of the new tuple's Element1</param>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <param name="t4">Value of the new tuple's Element2</param>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <param name="t5">Value of the new tuple's Element3</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T1, T2> Prepend<T3, T4, T5>(T3 t3, T4 t4, T5 t5)
	{
		return Tuple.New(
			t3
			,t4
			,t5
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <param name="t3">Value of the new tuple's Element1</param>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <param name="t4">Value of the new tuple's Element2</param>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <param name="t5">Value of the new tuple's Element3</param>
	/// <typeparam name="T6">Type of the new tuple's Element4</typeparam>
	/// <param name="t6">Value of the new tuple's Element4</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T6, T1, T2> Prepend<T3, T4, T5, T6>(T3 t3, T4 t4, T5 t5, T6 t6)
	{
		return Tuple.New(
			t3
			,t4
			,t5
			,t6
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <param name="t3">Value of the new tuple's Element1</param>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <param name="t4">Value of the new tuple's Element2</param>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <param name="t5">Value of the new tuple's Element3</param>
	/// <typeparam name="T6">Type of the new tuple's Element4</typeparam>
	/// <param name="t6">Value of the new tuple's Element4</param>
	/// <typeparam name="T7">Type of the new tuple's Element5</typeparam>
	/// <param name="t7">Value of the new tuple's Element5</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T6, T7, T1, T2> Prepend<T3, T4, T5, T6, T7>(T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		return Tuple.New(
			t3
			,t4
			,t5
			,t6
			,t7
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <param name="t3">Value of the new tuple's Element1</param>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <param name="t4">Value of the new tuple's Element2</param>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <param name="t5">Value of the new tuple's Element3</param>
	/// <typeparam name="T6">Type of the new tuple's Element4</typeparam>
	/// <param name="t6">Value of the new tuple's Element4</param>
	/// <typeparam name="T7">Type of the new tuple's Element5</typeparam>
	/// <param name="t7">Value of the new tuple's Element5</param>
	/// <typeparam name="T8">Type of the new tuple's Element6</typeparam>
	/// <param name="t8">Value of the new tuple's Element6</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T6, T7, T8, T1, T2> Prepend<T3, T4, T5, T6, T7, T8>(T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		return Tuple.New(
			t3
			,t4
			,t5
			,t6
			,t7
			,t8
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <param name="t3">Value of the new tuple's Element1</param>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <param name="t4">Value of the new tuple's Element2</param>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <param name="t5">Value of the new tuple's Element3</param>
	/// <typeparam name="T6">Type of the new tuple's Element4</typeparam>
	/// <param name="t6">Value of the new tuple's Element4</param>
	/// <typeparam name="T7">Type of the new tuple's Element5</typeparam>
	/// <param name="t7">Value of the new tuple's Element5</param>
	/// <typeparam name="T8">Type of the new tuple's Element6</typeparam>
	/// <param name="t8">Value of the new tuple's Element6</param>
	/// <typeparam name="T9">Type of the new tuple's Element7</typeparam>
	/// <param name="t9">Value of the new tuple's Element7</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T6, T7, T8, T9, T1, T2> Prepend<T3, T4, T5, T6, T7, T8, T9>(T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			t3
			,t4
			,t5
			,t6
			,t7
			,t8
			,t9
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <param name="t3">Value of the new tuple's Element1</param>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <param name="t4">Value of the new tuple's Element2</param>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <param name="t5">Value of the new tuple's Element3</param>
	/// <typeparam name="T6">Type of the new tuple's Element4</typeparam>
	/// <param name="t6">Value of the new tuple's Element4</param>
	/// <typeparam name="T7">Type of the new tuple's Element5</typeparam>
	/// <param name="t7">Value of the new tuple's Element5</param>
	/// <typeparam name="T8">Type of the new tuple's Element6</typeparam>
	/// <param name="t8">Value of the new tuple's Element6</param>
	/// <typeparam name="T9">Type of the new tuple's Element7</typeparam>
	/// <param name="t9">Value of the new tuple's Element7</param>
	/// <typeparam name="T10">Type of the new tuple's Element8</typeparam>
	/// <param name="t10">Value of the new tuple's Element8</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T6, T7, T8, T9, T10, T1, T2> Prepend<T3, T4, T5, T6, T7, T8, T9, T10>(T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			t3
			,t4
			,t5
			,t6
			,t7
			,t8
			,t9
			,t10
			,Element1
			,Element2
		);
	}

	
    /// <summary>
    /// Creates a new tuple of length 3 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 3 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3> Append<T3>(Tuple<T3> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,pOther.Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 4 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 4 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4> Append<T3, T4>(Tuple<T3, T4> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,pOther.Element1
			,pOther.Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 5 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5> Append<T3, T4, T5>(Tuple<T3, T4, T5> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 6 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6> Append<T3, T4, T5, T6>(Tuple<T3, T4, T5, T6> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="pOther">Tuple of length 5 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 7 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T3, T4, T5, T6, T7>(Tuple<T3, T4, T5, T6, T7> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="pOther">Tuple of length 6 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 8 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T3, T4, T5, T6, T7, T8>(Tuple<T3, T4, T5, T6, T7, T8> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="pOther">Tuple of length 7 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 9 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T3, T4, T5, T6, T7, T8, T9>(Tuple<T3, T4, T5, T6, T7, T8, T9> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="pOther">Tuple of length 8 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 10 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T3, T4, T5, T6, T7, T8, T9, T10>(Tuple<T3, T4, T5, T6, T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
			,pOther.Element8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 3 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 3 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T1, T2> Prepend<T3>(Tuple<T3> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 4 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 4 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T1, T2> Prepend<T3, T4>(Tuple<T3, T4> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T1, T2> Prepend<T3, T4, T5>(Tuple<T3, T4, T5> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element4</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T6, T1, T2> Prepend<T3, T4, T5, T6>(Tuple<T3, T4, T5, T6> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element5</typeparam>
	/// <param name="pOther">Tuple of length 5 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T6, T7, T1, T2> Prepend<T3, T4, T5, T6, T7>(Tuple<T3, T4, T5, T6, T7> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element6</typeparam>
	/// <param name="pOther">Tuple of length 6 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T6, T7, T8, T1, T2> Prepend<T3, T4, T5, T6, T7, T8>(Tuple<T3, T4, T5, T6, T7, T8> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element7</typeparam>
	/// <param name="pOther">Tuple of length 7 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T6, T7, T8, T9, T1, T2> Prepend<T3, T4, T5, T6, T7, T8, T9>(Tuple<T3, T4, T5, T6, T7, T8, T9> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
			,Element1
			,Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T3">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T4">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element8</typeparam>
	/// <param name="pOther">Tuple of length 8 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T3, T4, T5, T6, T7, T8, T9, T10, T1, T2> Prepend<T3, T4, T5, T6, T7, T8, T9, T10>(Tuple<T3, T4, T5, T6, T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
			,pOther.Element8
			,Element1
			,Element2
		);
	}


#region Object overrides

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>Hash code of the object.</returns>
	public override int GetHashCode()
	{
		int hash = 0;
		hash ^= Element1.GetHashCode();
		hash ^= Element2.GetHashCode();
		return hash;
	}
	
    /// <summary>
    /// Returns a value indicating weather this instance is equal to another instance.
    /// </summary>
    /// <param name="pObj">The object we wish to compare with this instance.</param>
    /// <returns>A value indicating if this object is equal to the one passed in.</returns>
	public override bool Equals(Object pObj)
	{
		if(pObj == null)
			return false;
		if(!(pObj is Tuple<T1, T2>))
			return false;

		return Equals((Tuple<T1, T2>)pObj);
	}
	
    /// <summary>
    /// Converts the tuple to a string. This will be a comma separated list
	/// of the string values of the elements enclosed in brackets.
    /// </summary>
    /// <returns>A string representation of the tuple.</returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(Element1);
		sb.Append(", ");
		sb.Append(Element2);
		sb.Append(")");
		return sb.ToString();
	}
	
	
    /// <summary>
	/// Returns a string representation of the tuple using the specified format.
    /// </summary>
	/// <param name="pFormat">The format to use for the string representation.</param>
    /// <returns>A string representation of the tuple.</returns>
	public string ToString(String pFormat)
	{
		return String.Format(pFormat
			,Element1
			,Element2
		);
	}
	
#endregion

#region IEquatable<> implementation

    /// <summary>
    /// A value indicating if this tuple is equal to a tuple
	/// of the same length and type. This will be so if all members are
	/// equal.
    /// </summary>
    /// <returns>A value indicating weather this tuple is equal to another tuple of the same length and type.</returns>
	public bool Equals(Tuple<T1, T2> pObj)
	{
		if(pObj == null)
			return false;

		bool result = true;
		result = result && EqualityComparer<T1>.Default.Equals(Element1, pObj.Element1);
		result = result && EqualityComparer<T2>.Default.Equals(Element2, pObj.Element2);
		return result;
	}
	
#endregion

#region ICollection implementation

    /// <summary>
    /// Copies the elements of this tuple to an Array.
	/// The array should have at least 2 elements available
	/// after the index parameter.
    /// </summary>
	/// <param name="pArray">The array to copy the values to.</param>
	/// <param name="pIndex">The offset in the array at which to start inserting the values.</param>
	void ICollection.CopyTo(Array pArray, int pIndex)
	{
		if (pArray == null)
			throw new ArgumentNullException("pArray");
		if (pIndex < 0)
			throw new ArgumentOutOfRangeException("pIndex");
		if (pArray.Length - pIndex <= 0 || (pArray.Length - pIndex) < 2)
			throw new ArgumentException("pIndex");

		pArray.SetValue(Element1, pIndex + 0);
		pArray.SetValue(Element2, pIndex + 1);
	}
	
    /// <summary>
    /// Gets the length of the tuple, that is it returns 2.
    /// </summary>
	int ICollection.Count
	{
		get { return 2; }
	}
	
	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	Object ICollection.SyncRoot 
	{
		get { return this; }
	}
	
#endregion

#region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

#endregion

#region IEnumerable<object> implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	public IEnumerator<Object> GetEnumerator()
    {
		yield return Element1;
		yield return Element2;
    }

#endregion

#region IComparable<> implementation

    /// <summary>
    /// Returns a value indicating the order of this tuple compared
	/// to another tuple of the same length and type. The order is defined
	/// as the order of the first element of the tuples.
    /// </summary>
	/// <param name="pOther">The tuple we are comparing this one to.</param>
    /// <returns>value indicating the order of this tuple compared to another tuple of the same length and type.</returns>
	public int CompareTo(Tuple<T1, T2> pOther)
	{
		return Comparer<T1>.Default.Compare(Element1, pOther.Element1);
	}

#endregion

    /// <summary>
    /// Get or sets the value of the element at
	/// the specified index in the tuple.
    /// </summary>
    /// <param name="pIndex">The index of the element in the tuple.</param>
	public Object this[int pIndex]
	{
		get
		{
			switch(pIndex)
			{
				case 0 : return Element1;
				case 1 : return Element2;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
			
		set
		{
			switch(pIndex)
			{
				case 0 :
					if(value is T1)
						Element1 = (T1)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 1 :
					if(value is T2)
						Element2 = (T2)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
	}
	
	/// <summary>
    /// Compares two tuples and returns a value indicating if they are equal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are equal.</returns>
	public static bool operator==(Tuple<T1, T2> pA, Tuple<T1, T2> pB)
	{
		if(System.Object.ReferenceEquals(pA, pB))
			return true;

		if((object)pA == null || (object)pB == null)
			return false;

		return pA.Equals(pB);
	}

	/// <summary>
    /// Compares two tuples and returns a value indicating if they are unequal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are unequal.</returns>
	public static bool operator!=(Tuple<T1, T2> pA, Tuple<T1, T2> pB)
	{
		return !pA.Equals(pB);
	}
	
    /// <summary>
    /// Gets the element of the tuple at position 1.
    /// </summary>
	[DataMember]
	public T1 Element1 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 2.
    /// </summary>
	[DataMember]
	public T2 Element2 { get; set; }
	
 
    /// <summary>
    /// Gets or sets the first element of
	/// the tuple. Same as using Element1. Only added for
	/// syntax reasons.
    /// </summary>
	public T1 First
	{ 
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets or sets the second element of
	/// the tuple. Same as using Element2. Only added for
	/// syntax reasons.
    /// </summary>
	public T2 Second
	{ 
		get { return Element2; }
		set { Element2 = value; }
	}
	
 
    /// <summary>
    /// Gets or sets the head of the tuple, that is the first element.
	/// Same as using the properties Element1 or First. Only Added
	/// for syntax reasons.
    /// </summary>
	public T1 Head
	{
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets the tail of the tuple, that is, all elements
	/// except the first one. This property actually returns a completely
	/// new tuple so be careful about that as changing the tail
	/// not change the original tuple.
    /// </summary>
	public Tuple <T2> Tail
	{
		get
		{
			return Tuple.New(
				Element2
			);
		}
	}
}


/// <summary>
/// Represents a tuple of length 3
/// </summary>
/// <typeparam name="T1">Type of the tuple's Element1</typeparam>
/// <typeparam name="T2">Type of the tuple's Element2</typeparam>
/// <typeparam name="T3">Type of the tuple's Element3</typeparam>
[DataContract]
public class Tuple<T1, T2, T3> : ICollection, IEnumerable, IEnumerable<Object>,
	IEquatable<Tuple<T1, T2, T3>>, IComparable<Tuple<T1, T2, T3>>
{
    /// <summary>
    /// An empty tuple constructor. All elements will have their default values.
    /// </summary>
	public Tuple()
	{
	}
	
    /// <summary>
    /// Tuple constructor. The first 1 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	public Tuple(T1 t1)
	{
		Element1 = t1;
	}
	
    /// <summary>
    /// Tuple constructor. The first 2 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	public Tuple(T1 t1, T2 t2)
	{
		Element1 = t1;
		Element2 = t2;
	}
	
    /// <summary>
    /// Tuple constructor. The first 3 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	public Tuple(T1 t1, T2 t2, T3 t3)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
	}
	
    /// <summary>
    /// Creates a new tuple of length 4 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
    /// <returns>A new tuple of length 4 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4> Append<T4>(T4 t4)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,t4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5> Append<T4, T5>(T4 t4, T5 t5)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,t4
			,t5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6> Append<T4, T5, T6>(T4 t4, T5 t5, T6 t6)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,t4
			,t5
			,t6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T4, T5, T6, T7>(T4 t4, T5 t5, T6 t6, T7 t7)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,t4
			,t5
			,t6
			,t7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T4, T5, T6, T7, T8>(T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,t4
			,t5
			,t6
			,t7
			,t8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T4, T5, T6, T7, T8, T9>(T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,t4
			,t5
			,t6
			,t7
			,t8
			,t9
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="t4">Value of the new tuple's Element4</param>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="t10">Value of the new tuple's Element10</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T4, T5, T6, T7, T8, T9, T10>(T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,t4
			,t5
			,t6
			,t7
			,t8
			,t9
			,t10
		);
	}

    /// <summary>
    /// Creates a new tuple of length 4 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <param name="t4">Value of the new tuple's Element1</param>
    /// <returns>A new tuple of length 4 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T1, T2, T3> Prepend<T4>(T4 t4)
	{
		return Tuple.New(
			t4
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <param name="t4">Value of the new tuple's Element1</param>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <param name="t5">Value of the new tuple's Element2</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T1, T2, T3> Prepend<T4, T5>(T4 t4, T5 t5)
	{
		return Tuple.New(
			t4
			,t5
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <param name="t4">Value of the new tuple's Element1</param>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <param name="t5">Value of the new tuple's Element2</param>
	/// <typeparam name="T6">Type of the new tuple's Element3</typeparam>
	/// <param name="t6">Value of the new tuple's Element3</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T6, T1, T2, T3> Prepend<T4, T5, T6>(T4 t4, T5 t5, T6 t6)
	{
		return Tuple.New(
			t4
			,t5
			,t6
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <param name="t4">Value of the new tuple's Element1</param>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <param name="t5">Value of the new tuple's Element2</param>
	/// <typeparam name="T6">Type of the new tuple's Element3</typeparam>
	/// <param name="t6">Value of the new tuple's Element3</param>
	/// <typeparam name="T7">Type of the new tuple's Element4</typeparam>
	/// <param name="t7">Value of the new tuple's Element4</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T6, T7, T1, T2, T3> Prepend<T4, T5, T6, T7>(T4 t4, T5 t5, T6 t6, T7 t7)
	{
		return Tuple.New(
			t4
			,t5
			,t6
			,t7
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <param name="t4">Value of the new tuple's Element1</param>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <param name="t5">Value of the new tuple's Element2</param>
	/// <typeparam name="T6">Type of the new tuple's Element3</typeparam>
	/// <param name="t6">Value of the new tuple's Element3</param>
	/// <typeparam name="T7">Type of the new tuple's Element4</typeparam>
	/// <param name="t7">Value of the new tuple's Element4</param>
	/// <typeparam name="T8">Type of the new tuple's Element5</typeparam>
	/// <param name="t8">Value of the new tuple's Element5</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T6, T7, T8, T1, T2, T3> Prepend<T4, T5, T6, T7, T8>(T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		return Tuple.New(
			t4
			,t5
			,t6
			,t7
			,t8
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <param name="t4">Value of the new tuple's Element1</param>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <param name="t5">Value of the new tuple's Element2</param>
	/// <typeparam name="T6">Type of the new tuple's Element3</typeparam>
	/// <param name="t6">Value of the new tuple's Element3</param>
	/// <typeparam name="T7">Type of the new tuple's Element4</typeparam>
	/// <param name="t7">Value of the new tuple's Element4</param>
	/// <typeparam name="T8">Type of the new tuple's Element5</typeparam>
	/// <param name="t8">Value of the new tuple's Element5</param>
	/// <typeparam name="T9">Type of the new tuple's Element6</typeparam>
	/// <param name="t9">Value of the new tuple's Element6</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T6, T7, T8, T9, T1, T2, T3> Prepend<T4, T5, T6, T7, T8, T9>(T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			t4
			,t5
			,t6
			,t7
			,t8
			,t9
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <param name="t4">Value of the new tuple's Element1</param>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <param name="t5">Value of the new tuple's Element2</param>
	/// <typeparam name="T6">Type of the new tuple's Element3</typeparam>
	/// <param name="t6">Value of the new tuple's Element3</param>
	/// <typeparam name="T7">Type of the new tuple's Element4</typeparam>
	/// <param name="t7">Value of the new tuple's Element4</param>
	/// <typeparam name="T8">Type of the new tuple's Element5</typeparam>
	/// <param name="t8">Value of the new tuple's Element5</param>
	/// <typeparam name="T9">Type of the new tuple's Element6</typeparam>
	/// <param name="t9">Value of the new tuple's Element6</param>
	/// <typeparam name="T10">Type of the new tuple's Element7</typeparam>
	/// <param name="t10">Value of the new tuple's Element7</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T6, T7, T8, T9, T10, T1, T2, T3> Prepend<T4, T5, T6, T7, T8, T9, T10>(T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			t4
			,t5
			,t6
			,t7
			,t8
			,t9
			,t10
			,Element1
			,Element2
			,Element3
		);
	}

	
    /// <summary>
    /// Creates a new tuple of length 4 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 4 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4> Append<T4>(Tuple<T4> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,pOther.Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 5 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5> Append<T4, T5>(Tuple<T4, T5> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,pOther.Element1
			,pOther.Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 6 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6> Append<T4, T5, T6>(Tuple<T4, T5, T6> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 7 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T4, T5, T6, T7>(Tuple<T4, T5, T6, T7> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="pOther">Tuple of length 5 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 8 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T4, T5, T6, T7, T8>(Tuple<T4, T5, T6, T7, T8> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="pOther">Tuple of length 6 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 9 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T4, T5, T6, T7, T8, T9>(Tuple<T4, T5, T6, T7, T8, T9> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="pOther">Tuple of length 7 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 10 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T4, T5, T6, T7, T8, T9, T10>(Tuple<T4, T5, T6, T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 4 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 4 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T1, T2, T3> Prepend<T4>(Tuple<T4> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T1, T2, T3> Prepend<T4, T5>(Tuple<T4, T5> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element3</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T6, T1, T2, T3> Prepend<T4, T5, T6>(Tuple<T4, T5, T6> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element4</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T6, T7, T1, T2, T3> Prepend<T4, T5, T6, T7>(Tuple<T4, T5, T6, T7> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element5</typeparam>
	/// <param name="pOther">Tuple of length 5 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T6, T7, T8, T1, T2, T3> Prepend<T4, T5, T6, T7, T8>(Tuple<T4, T5, T6, T7, T8> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element6</typeparam>
	/// <param name="pOther">Tuple of length 6 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T6, T7, T8, T9, T1, T2, T3> Prepend<T4, T5, T6, T7, T8, T9>(Tuple<T4, T5, T6, T7, T8, T9> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,Element1
			,Element2
			,Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T4">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T5">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element7</typeparam>
	/// <param name="pOther">Tuple of length 7 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T4, T5, T6, T7, T8, T9, T10, T1, T2, T3> Prepend<T4, T5, T6, T7, T8, T9, T10>(Tuple<T4, T5, T6, T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,pOther.Element7
			,Element1
			,Element2
			,Element3
		);
	}


#region Object overrides

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>Hash code of the object.</returns>
	public override int GetHashCode()
	{
		int hash = 0;
		hash ^= Element1.GetHashCode();
		hash ^= Element2.GetHashCode();
		hash ^= Element3.GetHashCode();
		return hash;
	}
	
    /// <summary>
    /// Returns a value indicating weather this instance is equal to another instance.
    /// </summary>
    /// <param name="pObj">The object we wish to compare with this instance.</param>
    /// <returns>A value indicating if this object is equal to the one passed in.</returns>
	public override bool Equals(Object pObj)
	{
		if(pObj == null)
			return false;
		if(!(pObj is Tuple<T1, T2, T3>))
			return false;

		return Equals((Tuple<T1, T2, T3>)pObj);
	}
	
    /// <summary>
    /// Converts the tuple to a string. This will be a comma separated list
	/// of the string values of the elements enclosed in brackets.
    /// </summary>
    /// <returns>A string representation of the tuple.</returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(Element1);
		sb.Append(", ");
		sb.Append(Element2);
		sb.Append(", ");
		sb.Append(Element3);
		sb.Append(")");
		return sb.ToString();
	}
	
	
    /// <summary>
	/// Returns a string representation of the tuple using the specified format.
    /// </summary>
	/// <param name="pFormat">The format to use for the string representation.</param>
    /// <returns>A string representation of the tuple.</returns>
	public string ToString(String pFormat)
	{
		return String.Format(pFormat
			,Element1
			,Element2
			,Element3
		);
	}
	
#endregion

#region IEquatable<> implementation

    /// <summary>
    /// A value indicating if this tuple is equal to a tuple
	/// of the same length and type. This will be so if all members are
	/// equal.
    /// </summary>
    /// <returns>A value indicating weather this tuple is equal to another tuple of the same length and type.</returns>
	public bool Equals(Tuple<T1, T2, T3> pObj)
	{
		if(pObj == null)
			return false;

		bool result = true;
		result = result && EqualityComparer<T1>.Default.Equals(Element1, pObj.Element1);
		result = result && EqualityComparer<T2>.Default.Equals(Element2, pObj.Element2);
		result = result && EqualityComparer<T3>.Default.Equals(Element3, pObj.Element3);
		return result;
	}
	
#endregion

#region ICollection implementation

    /// <summary>
    /// Copies the elements of this tuple to an Array.
	/// The array should have at least 3 elements available
	/// after the index parameter.
    /// </summary>
	/// <param name="pArray">The array to copy the values to.</param>
	/// <param name="pIndex">The offset in the array at which to start inserting the values.</param>
	void ICollection.CopyTo(Array pArray, int pIndex)
	{
		if (pArray == null)
			throw new ArgumentNullException("pArray");
		if (pIndex < 0)
			throw new ArgumentOutOfRangeException("pIndex");
		if (pArray.Length - pIndex <= 0 || (pArray.Length - pIndex) < 3)
			throw new ArgumentException("pIndex");

		pArray.SetValue(Element1, pIndex + 0);
		pArray.SetValue(Element2, pIndex + 1);
		pArray.SetValue(Element3, pIndex + 2);
	}
	
    /// <summary>
    /// Gets the length of the tuple, that is it returns 3.
    /// </summary>
	int ICollection.Count
	{
		get { return 3; }
	}
	
	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	Object ICollection.SyncRoot 
	{
		get { return this; }
	}
	
#endregion

#region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

#endregion

#region IEnumerable<object> implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	public IEnumerator<Object> GetEnumerator()
    {
		yield return Element1;
		yield return Element2;
		yield return Element3;
    }

#endregion

#region IComparable<> implementation

    /// <summary>
    /// Returns a value indicating the order of this tuple compared
	/// to another tuple of the same length and type. The order is defined
	/// as the order of the first element of the tuples.
    /// </summary>
	/// <param name="pOther">The tuple we are comparing this one to.</param>
    /// <returns>value indicating the order of this tuple compared to another tuple of the same length and type.</returns>
	public int CompareTo(Tuple<T1, T2, T3> pOther)
	{
		return Comparer<T1>.Default.Compare(Element1, pOther.Element1);
	}

#endregion

    /// <summary>
    /// Get or sets the value of the element at
	/// the specified index in the tuple.
    /// </summary>
    /// <param name="pIndex">The index of the element in the tuple.</param>
	public Object this[int pIndex]
	{
		get
		{
			switch(pIndex)
			{
				case 0 : return Element1;
				case 1 : return Element2;
				case 2 : return Element3;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
			
		set
		{
			switch(pIndex)
			{
				case 0 :
					if(value is T1)
						Element1 = (T1)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 1 :
					if(value is T2)
						Element2 = (T2)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 2 :
					if(value is T3)
						Element3 = (T3)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
	}
	
	/// <summary>
    /// Compares two tuples and returns a value indicating if they are equal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are equal.</returns>
	public static bool operator==(Tuple<T1, T2, T3> pA, Tuple<T1, T2, T3> pB)
	{
		if(System.Object.ReferenceEquals(pA, pB))
			return true;

		if((object)pA == null || (object)pB == null)
			return false;

		return pA.Equals(pB);
	}

	/// <summary>
    /// Compares two tuples and returns a value indicating if they are unequal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are unequal.</returns>
	public static bool operator!=(Tuple<T1, T2, T3> pA, Tuple<T1, T2, T3> pB)
	{
		return !pA.Equals(pB);
	}
	
    /// <summary>
    /// Gets the element of the tuple at position 1.
    /// </summary>
	[DataMember]
	public T1 Element1 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 2.
    /// </summary>
	[DataMember]
	public T2 Element2 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 3.
    /// </summary>
	[DataMember]
	public T3 Element3 { get; set; }
	
 
    /// <summary>
    /// Gets or sets the first element of
	/// the tuple. Same as using Element1. Only added for
	/// syntax reasons.
    /// </summary>
	public T1 First
	{ 
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets or sets the second element of
	/// the tuple. Same as using Element2. Only added for
	/// syntax reasons.
    /// </summary>
	public T2 Second
	{ 
		get { return Element2; }
		set { Element2 = value; }
	}
	
    /// <summary>
    /// Gets or sets the third element of
	/// the tuple. Same as using Element3. Only added for
	/// syntax reasons.
    /// </summary>
	public T3 Third
	{ 
		get { return Element3; }
		set { Element3 = value; }
	}
	
 
    /// <summary>
    /// Gets or sets the head of the tuple, that is the first element.
	/// Same as using the properties Element1 or First. Only Added
	/// for syntax reasons.
    /// </summary>
	public T1 Head
	{
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets the tail of the tuple, that is, all elements
	/// except the first one. This property actually returns a completely
	/// new tuple so be careful about that as changing the tail
	/// not change the original tuple.
    /// </summary>
	public Tuple <T2, T3> Tail
	{
		get
		{
			return Tuple.New(
				Element2
				,Element3
			);
		}
	}
}


/// <summary>
/// Represents a tuple of length 4
/// </summary>
/// <typeparam name="T1">Type of the tuple's Element1</typeparam>
/// <typeparam name="T2">Type of the tuple's Element2</typeparam>
/// <typeparam name="T3">Type of the tuple's Element3</typeparam>
/// <typeparam name="T4">Type of the tuple's Element4</typeparam>
[DataContract]
public class Tuple<T1, T2, T3, T4> : ICollection, IEnumerable, IEnumerable<Object>,
	IEquatable<Tuple<T1, T2, T3, T4>>, IComparable<Tuple<T1, T2, T3, T4>>
{
    /// <summary>
    /// An empty tuple constructor. All elements will have their default values.
    /// </summary>
	public Tuple()
	{
	}
	
    /// <summary>
    /// Tuple constructor. The first 1 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	public Tuple(T1 t1)
	{
		Element1 = t1;
	}
	
    /// <summary>
    /// Tuple constructor. The first 2 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	public Tuple(T1 t1, T2 t2)
	{
		Element1 = t1;
		Element2 = t2;
	}
	
    /// <summary>
    /// Tuple constructor. The first 3 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	public Tuple(T1 t1, T2 t2, T3 t3)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
	}
	
    /// <summary>
    /// Tuple constructor. The first 4 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
	}
	
    /// <summary>
    /// Creates a new tuple of length 5 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5> Append<T5>(T5 t5)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,t5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6> Append<T5, T6>(T5 t5, T6 t6)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,t5
			,t6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T5, T6, T7>(T5 t5, T6 t6, T7 t7)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,t5
			,t6
			,t7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T5, T6, T7, T8>(T5 t5, T6 t6, T7 t7, T8 t8)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,t5
			,t6
			,t7
			,t8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T5, T6, T7, T8, T9>(T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,t5
			,t6
			,t7
			,t8
			,t9
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="t5">Value of the new tuple's Element5</param>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="t10">Value of the new tuple's Element10</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T5, T6, T7, T8, T9, T10>(T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,t5
			,t6
			,t7
			,t8
			,t9
			,t10
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <param name="t5">Value of the new tuple's Element1</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T1, T2, T3, T4> Prepend<T5>(T5 t5)
	{
		return Tuple.New(
			t5
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <param name="t5">Value of the new tuple's Element1</param>
	/// <typeparam name="T6">Type of the new tuple's Element2</typeparam>
	/// <param name="t6">Value of the new tuple's Element2</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T6, T1, T2, T3, T4> Prepend<T5, T6>(T5 t5, T6 t6)
	{
		return Tuple.New(
			t5
			,t6
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <param name="t5">Value of the new tuple's Element1</param>
	/// <typeparam name="T6">Type of the new tuple's Element2</typeparam>
	/// <param name="t6">Value of the new tuple's Element2</param>
	/// <typeparam name="T7">Type of the new tuple's Element3</typeparam>
	/// <param name="t7">Value of the new tuple's Element3</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T6, T7, T1, T2, T3, T4> Prepend<T5, T6, T7>(T5 t5, T6 t6, T7 t7)
	{
		return Tuple.New(
			t5
			,t6
			,t7
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <param name="t5">Value of the new tuple's Element1</param>
	/// <typeparam name="T6">Type of the new tuple's Element2</typeparam>
	/// <param name="t6">Value of the new tuple's Element2</param>
	/// <typeparam name="T7">Type of the new tuple's Element3</typeparam>
	/// <param name="t7">Value of the new tuple's Element3</param>
	/// <typeparam name="T8">Type of the new tuple's Element4</typeparam>
	/// <param name="t8">Value of the new tuple's Element4</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T6, T7, T8, T1, T2, T3, T4> Prepend<T5, T6, T7, T8>(T5 t5, T6 t6, T7 t7, T8 t8)
	{
		return Tuple.New(
			t5
			,t6
			,t7
			,t8
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <param name="t5">Value of the new tuple's Element1</param>
	/// <typeparam name="T6">Type of the new tuple's Element2</typeparam>
	/// <param name="t6">Value of the new tuple's Element2</param>
	/// <typeparam name="T7">Type of the new tuple's Element3</typeparam>
	/// <param name="t7">Value of the new tuple's Element3</param>
	/// <typeparam name="T8">Type of the new tuple's Element4</typeparam>
	/// <param name="t8">Value of the new tuple's Element4</param>
	/// <typeparam name="T9">Type of the new tuple's Element5</typeparam>
	/// <param name="t9">Value of the new tuple's Element5</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T6, T7, T8, T9, T1, T2, T3, T4> Prepend<T5, T6, T7, T8, T9>(T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			t5
			,t6
			,t7
			,t8
			,t9
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <param name="t5">Value of the new tuple's Element1</param>
	/// <typeparam name="T6">Type of the new tuple's Element2</typeparam>
	/// <param name="t6">Value of the new tuple's Element2</param>
	/// <typeparam name="T7">Type of the new tuple's Element3</typeparam>
	/// <param name="t7">Value of the new tuple's Element3</param>
	/// <typeparam name="T8">Type of the new tuple's Element4</typeparam>
	/// <param name="t8">Value of the new tuple's Element4</param>
	/// <typeparam name="T9">Type of the new tuple's Element5</typeparam>
	/// <param name="t9">Value of the new tuple's Element5</param>
	/// <typeparam name="T10">Type of the new tuple's Element6</typeparam>
	/// <param name="t10">Value of the new tuple's Element6</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T6, T7, T8, T9, T10, T1, T2, T3, T4> Prepend<T5, T6, T7, T8, T9, T10>(T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			t5
			,t6
			,t7
			,t8
			,t9
			,t10
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

	
    /// <summary>
    /// Creates a new tuple of length 5 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 5 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5> Append<T5>(Tuple<T5> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,pOther.Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 6 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6> Append<T5, T6>(Tuple<T5, T6> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,pOther.Element1
			,pOther.Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 7 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T5, T6, T7>(Tuple<T5, T6, T7> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 8 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T5, T6, T7, T8>(Tuple<T5, T6, T7, T8> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="pOther">Tuple of length 5 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 9 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T5, T6, T7, T8, T9>(Tuple<T5, T6, T7, T8, T9> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="pOther">Tuple of length 6 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 10 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T5, T6, T7, T8, T9, T10>(Tuple<T5, T6, T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 5 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 5 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T1, T2, T3, T4> Prepend<T5>(Tuple<T5> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element2</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T6, T1, T2, T3, T4> Prepend<T5, T6>(Tuple<T5, T6> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element3</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T6, T7, T1, T2, T3, T4> Prepend<T5, T6, T7>(Tuple<T5, T6, T7> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element4</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T6, T7, T8, T1, T2, T3, T4> Prepend<T5, T6, T7, T8>(Tuple<T5, T6, T7, T8> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element5</typeparam>
	/// <param name="pOther">Tuple of length 5 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T6, T7, T8, T9, T1, T2, T3, T4> Prepend<T5, T6, T7, T8, T9>(Tuple<T5, T6, T7, T8, T9> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T5">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T6">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element5</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element6</typeparam>
	/// <param name="pOther">Tuple of length 6 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T5, T6, T7, T8, T9, T10, T1, T2, T3, T4> Prepend<T5, T6, T7, T8, T9, T10>(Tuple<T5, T6, T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,pOther.Element6
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}


#region Object overrides

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>Hash code of the object.</returns>
	public override int GetHashCode()
	{
		int hash = 0;
		hash ^= Element1.GetHashCode();
		hash ^= Element2.GetHashCode();
		hash ^= Element3.GetHashCode();
		hash ^= Element4.GetHashCode();
		return hash;
	}
	
    /// <summary>
    /// Returns a value indicating weather this instance is equal to another instance.
    /// </summary>
    /// <param name="pObj">The object we wish to compare with this instance.</param>
    /// <returns>A value indicating if this object is equal to the one passed in.</returns>
	public override bool Equals(Object pObj)
	{
		if(pObj == null)
			return false;
		if(!(pObj is Tuple<T1, T2, T3, T4>))
			return false;

		return Equals((Tuple<T1, T2, T3, T4>)pObj);
	}
	
    /// <summary>
    /// Converts the tuple to a string. This will be a comma separated list
	/// of the string values of the elements enclosed in brackets.
    /// </summary>
    /// <returns>A string representation of the tuple.</returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(Element1);
		sb.Append(", ");
		sb.Append(Element2);
		sb.Append(", ");
		sb.Append(Element3);
		sb.Append(", ");
		sb.Append(Element4);
		sb.Append(")");
		return sb.ToString();
	}
	
	
    /// <summary>
	/// Returns a string representation of the tuple using the specified format.
    /// </summary>
	/// <param name="pFormat">The format to use for the string representation.</param>
    /// <returns>A string representation of the tuple.</returns>
	public string ToString(String pFormat)
	{
		return String.Format(pFormat
			,Element1
			,Element2
			,Element3
			,Element4
		);
	}
	
#endregion

#region IEquatable<> implementation

    /// <summary>
    /// A value indicating if this tuple is equal to a tuple
	/// of the same length and type. This will be so if all members are
	/// equal.
    /// </summary>
    /// <returns>A value indicating weather this tuple is equal to another tuple of the same length and type.</returns>
	public bool Equals(Tuple<T1, T2, T3, T4> pObj)
	{
		if(pObj == null)
			return false;

		bool result = true;
		result = result && EqualityComparer<T1>.Default.Equals(Element1, pObj.Element1);
		result = result && EqualityComparer<T2>.Default.Equals(Element2, pObj.Element2);
		result = result && EqualityComparer<T3>.Default.Equals(Element3, pObj.Element3);
		result = result && EqualityComparer<T4>.Default.Equals(Element4, pObj.Element4);
		return result;
	}
	
#endregion

#region ICollection implementation

    /// <summary>
    /// Copies the elements of this tuple to an Array.
	/// The array should have at least 4 elements available
	/// after the index parameter.
    /// </summary>
	/// <param name="pArray">The array to copy the values to.</param>
	/// <param name="pIndex">The offset in the array at which to start inserting the values.</param>
	void ICollection.CopyTo(Array pArray, int pIndex)
	{
		if (pArray == null)
			throw new ArgumentNullException("pArray");
		if (pIndex < 0)
			throw new ArgumentOutOfRangeException("pIndex");
		if (pArray.Length - pIndex <= 0 || (pArray.Length - pIndex) < 4)
			throw new ArgumentException("pIndex");

		pArray.SetValue(Element1, pIndex + 0);
		pArray.SetValue(Element2, pIndex + 1);
		pArray.SetValue(Element3, pIndex + 2);
		pArray.SetValue(Element4, pIndex + 3);
	}
	
    /// <summary>
    /// Gets the length of the tuple, that is it returns 4.
    /// </summary>
	int ICollection.Count
	{
		get { return 4; }
	}
	
	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	Object ICollection.SyncRoot 
	{
		get { return this; }
	}
	
#endregion

#region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

#endregion

#region IEnumerable<object> implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	public IEnumerator<Object> GetEnumerator()
    {
		yield return Element1;
		yield return Element2;
		yield return Element3;
		yield return Element4;
    }

#endregion

#region IComparable<> implementation

    /// <summary>
    /// Returns a value indicating the order of this tuple compared
	/// to another tuple of the same length and type. The order is defined
	/// as the order of the first element of the tuples.
    /// </summary>
	/// <param name="pOther">The tuple we are comparing this one to.</param>
    /// <returns>value indicating the order of this tuple compared to another tuple of the same length and type.</returns>
	public int CompareTo(Tuple<T1, T2, T3, T4> pOther)
	{
		return Comparer<T1>.Default.Compare(Element1, pOther.Element1);
	}

#endregion

    /// <summary>
    /// Get or sets the value of the element at
	/// the specified index in the tuple.
    /// </summary>
    /// <param name="pIndex">The index of the element in the tuple.</param>
	public Object this[int pIndex]
	{
		get
		{
			switch(pIndex)
			{
				case 0 : return Element1;
				case 1 : return Element2;
				case 2 : return Element3;
				case 3 : return Element4;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
			
		set
		{
			switch(pIndex)
			{
				case 0 :
					if(value is T1)
						Element1 = (T1)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 1 :
					if(value is T2)
						Element2 = (T2)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 2 :
					if(value is T3)
						Element3 = (T3)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 3 :
					if(value is T4)
						Element4 = (T4)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
	}
	
	/// <summary>
    /// Compares two tuples and returns a value indicating if they are equal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are equal.</returns>
	public static bool operator==(Tuple<T1, T2, T3, T4> pA, Tuple<T1, T2, T3, T4> pB)
	{
		if(System.Object.ReferenceEquals(pA, pB))
			return true;

		if((object)pA == null || (object)pB == null)
			return false;

		return pA.Equals(pB);
	}

	/// <summary>
    /// Compares two tuples and returns a value indicating if they are unequal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are unequal.</returns>
	public static bool operator!=(Tuple<T1, T2, T3, T4> pA, Tuple<T1, T2, T3, T4> pB)
	{
		return !pA.Equals(pB);
	}
	
    /// <summary>
    /// Gets the element of the tuple at position 1.
    /// </summary>
	[DataMember]
	public T1 Element1 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 2.
    /// </summary>
	[DataMember]
	public T2 Element2 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 3.
    /// </summary>
	[DataMember]
	public T3 Element3 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 4.
    /// </summary>
	[DataMember]
	public T4 Element4 { get; set; }
	
 
    /// <summary>
    /// Gets or sets the first element of
	/// the tuple. Same as using Element1. Only added for
	/// syntax reasons.
    /// </summary>
	public T1 First
	{ 
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets or sets the second element of
	/// the tuple. Same as using Element2. Only added for
	/// syntax reasons.
    /// </summary>
	public T2 Second
	{ 
		get { return Element2; }
		set { Element2 = value; }
	}
	
    /// <summary>
    /// Gets or sets the third element of
	/// the tuple. Same as using Element3. Only added for
	/// syntax reasons.
    /// </summary>
	public T3 Third
	{ 
		get { return Element3; }
		set { Element3 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fourth element of
	/// the tuple. Same as using Element4. Only added for
	/// syntax reasons.
    /// </summary>
	public T4 Fourth
	{ 
		get { return Element4; }
		set { Element4 = value; }
	}
	
 
    /// <summary>
    /// Gets or sets the head of the tuple, that is the first element.
	/// Same as using the properties Element1 or First. Only Added
	/// for syntax reasons.
    /// </summary>
	public T1 Head
	{
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets the tail of the tuple, that is, all elements
	/// except the first one. This property actually returns a completely
	/// new tuple so be careful about that as changing the tail
	/// not change the original tuple.
    /// </summary>
	public Tuple <T2, T3, T4> Tail
	{
		get
		{
			return Tuple.New(
				Element2
				,Element3
				,Element4
			);
		}
	}
}


/// <summary>
/// Represents a tuple of length 5
/// </summary>
/// <typeparam name="T1">Type of the tuple's Element1</typeparam>
/// <typeparam name="T2">Type of the tuple's Element2</typeparam>
/// <typeparam name="T3">Type of the tuple's Element3</typeparam>
/// <typeparam name="T4">Type of the tuple's Element4</typeparam>
/// <typeparam name="T5">Type of the tuple's Element5</typeparam>
[DataContract]
public class Tuple<T1, T2, T3, T4, T5> : ICollection, IEnumerable, IEnumerable<Object>,
	IEquatable<Tuple<T1, T2, T3, T4, T5>>, IComparable<Tuple<T1, T2, T3, T4, T5>>
{
    /// <summary>
    /// An empty tuple constructor. All elements will have their default values.
    /// </summary>
	public Tuple()
	{
	}
	
    /// <summary>
    /// Tuple constructor. The first 1 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	public Tuple(T1 t1)
	{
		Element1 = t1;
	}
	
    /// <summary>
    /// Tuple constructor. The first 2 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	public Tuple(T1 t1, T2 t2)
	{
		Element1 = t1;
		Element2 = t2;
	}
	
    /// <summary>
    /// Tuple constructor. The first 3 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	public Tuple(T1 t1, T2 t2, T3 t3)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
	}
	
    /// <summary>
    /// Tuple constructor. The first 4 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
	}
	
    /// <summary>
    /// Tuple constructor. The first 5 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
	}
	
    /// <summary>
    /// Creates a new tuple of length 6 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6> Append<T6>(T6 t6)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,t6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T6, T7>(T6 t6, T7 t7)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,t6
			,t7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T6, T7, T8>(T6 t6, T7 t7, T8 t8)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,t6
			,t7
			,t8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T6, T7, T8, T9>(T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,t6
			,t7
			,t8
			,t9
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="t6">Value of the new tuple's Element6</param>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="t10">Value of the new tuple's Element10</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T6, T7, T8, T9, T10>(T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,t6
			,t7
			,t8
			,t9
			,t10
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element1</typeparam>
	/// <param name="t6">Value of the new tuple's Element1</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the beginning.</returns>
	public Tuple<T6, T1, T2, T3, T4, T5> Prepend<T6>(T6 t6)
	{
		return Tuple.New(
			t6
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element1</typeparam>
	/// <param name="t6">Value of the new tuple's Element1</param>
	/// <typeparam name="T7">Type of the new tuple's Element2</typeparam>
	/// <param name="t7">Value of the new tuple's Element2</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T6, T7, T1, T2, T3, T4, T5> Prepend<T6, T7>(T6 t6, T7 t7)
	{
		return Tuple.New(
			t6
			,t7
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element1</typeparam>
	/// <param name="t6">Value of the new tuple's Element1</param>
	/// <typeparam name="T7">Type of the new tuple's Element2</typeparam>
	/// <param name="t7">Value of the new tuple's Element2</param>
	/// <typeparam name="T8">Type of the new tuple's Element3</typeparam>
	/// <param name="t8">Value of the new tuple's Element3</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T6, T7, T8, T1, T2, T3, T4, T5> Prepend<T6, T7, T8>(T6 t6, T7 t7, T8 t8)
	{
		return Tuple.New(
			t6
			,t7
			,t8
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element1</typeparam>
	/// <param name="t6">Value of the new tuple's Element1</param>
	/// <typeparam name="T7">Type of the new tuple's Element2</typeparam>
	/// <param name="t7">Value of the new tuple's Element2</param>
	/// <typeparam name="T8">Type of the new tuple's Element3</typeparam>
	/// <param name="t8">Value of the new tuple's Element3</param>
	/// <typeparam name="T9">Type of the new tuple's Element4</typeparam>
	/// <param name="t9">Value of the new tuple's Element4</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T6, T7, T8, T9, T1, T2, T3, T4, T5> Prepend<T6, T7, T8, T9>(T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			t6
			,t7
			,t8
			,t9
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element1</typeparam>
	/// <param name="t6">Value of the new tuple's Element1</param>
	/// <typeparam name="T7">Type of the new tuple's Element2</typeparam>
	/// <param name="t7">Value of the new tuple's Element2</param>
	/// <typeparam name="T8">Type of the new tuple's Element3</typeparam>
	/// <param name="t8">Value of the new tuple's Element3</param>
	/// <typeparam name="T9">Type of the new tuple's Element4</typeparam>
	/// <param name="t9">Value of the new tuple's Element4</param>
	/// <typeparam name="T10">Type of the new tuple's Element5</typeparam>
	/// <param name="t10">Value of the new tuple's Element5</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T6, T7, T8, T9, T10, T1, T2, T3, T4, T5> Prepend<T6, T7, T8, T9, T10>(T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			t6
			,t7
			,t8
			,t9
			,t10
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}

	
    /// <summary>
    /// Creates a new tuple of length 6 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 6 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6> Append<T6>(Tuple<T6> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,pOther.Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 7 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T6, T7>(Tuple<T6, T7> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,pOther.Element1
			,pOther.Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 8 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T6, T7, T8>(Tuple<T6, T7, T8> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 9 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T6, T7, T8, T9>(Tuple<T6, T7, T8, T9> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element6</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="pOther">Tuple of length 5 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 10 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T6, T7, T8, T9, T10>(Tuple<T6, T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 6 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element1</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 6 with the passed in elements added at the beginning.</returns>
	public Tuple<T6, T1, T2, T3, T4, T5> Prepend<T6>(Tuple<T6> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element2</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T6, T7, T1, T2, T3, T4, T5> Prepend<T6, T7>(Tuple<T6, T7> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element3</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T6, T7, T8, T1, T2, T3, T4, T5> Prepend<T6, T7, T8>(Tuple<T6, T7, T8> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element4</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T6, T7, T8, T9, T1, T2, T3, T4, T5> Prepend<T6, T7, T8, T9>(Tuple<T6, T7, T8, T9> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T6">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T7">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element4</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element5</typeparam>
	/// <param name="pOther">Tuple of length 5 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T6, T7, T8, T9, T10, T1, T2, T3, T4, T5> Prepend<T6, T7, T8, T9, T10>(Tuple<T6, T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,pOther.Element5
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}


#region Object overrides

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>Hash code of the object.</returns>
	public override int GetHashCode()
	{
		int hash = 0;
		hash ^= Element1.GetHashCode();
		hash ^= Element2.GetHashCode();
		hash ^= Element3.GetHashCode();
		hash ^= Element4.GetHashCode();
		hash ^= Element5.GetHashCode();
		return hash;
	}
	
    /// <summary>
    /// Returns a value indicating weather this instance is equal to another instance.
    /// </summary>
    /// <param name="pObj">The object we wish to compare with this instance.</param>
    /// <returns>A value indicating if this object is equal to the one passed in.</returns>
	public override bool Equals(Object pObj)
	{
		if(pObj == null)
			return false;
		if(!(pObj is Tuple<T1, T2, T3, T4, T5>))
			return false;

		return Equals((Tuple<T1, T2, T3, T4, T5>)pObj);
	}
	
    /// <summary>
    /// Converts the tuple to a string. This will be a comma separated list
	/// of the string values of the elements enclosed in brackets.
    /// </summary>
    /// <returns>A string representation of the tuple.</returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(Element1);
		sb.Append(", ");
		sb.Append(Element2);
		sb.Append(", ");
		sb.Append(Element3);
		sb.Append(", ");
		sb.Append(Element4);
		sb.Append(", ");
		sb.Append(Element5);
		sb.Append(")");
		return sb.ToString();
	}
	
	
    /// <summary>
	/// Returns a string representation of the tuple using the specified format.
    /// </summary>
	/// <param name="pFormat">The format to use for the string representation.</param>
    /// <returns>A string representation of the tuple.</returns>
	public string ToString(String pFormat)
	{
		return String.Format(pFormat
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
		);
	}
	
#endregion

#region IEquatable<> implementation

    /// <summary>
    /// A value indicating if this tuple is equal to a tuple
	/// of the same length and type. This will be so if all members are
	/// equal.
    /// </summary>
    /// <returns>A value indicating weather this tuple is equal to another tuple of the same length and type.</returns>
	public bool Equals(Tuple<T1, T2, T3, T4, T5> pObj)
	{
		if(pObj == null)
			return false;

		bool result = true;
		result = result && EqualityComparer<T1>.Default.Equals(Element1, pObj.Element1);
		result = result && EqualityComparer<T2>.Default.Equals(Element2, pObj.Element2);
		result = result && EqualityComparer<T3>.Default.Equals(Element3, pObj.Element3);
		result = result && EqualityComparer<T4>.Default.Equals(Element4, pObj.Element4);
		result = result && EqualityComparer<T5>.Default.Equals(Element5, pObj.Element5);
		return result;
	}
	
#endregion

#region ICollection implementation

    /// <summary>
    /// Copies the elements of this tuple to an Array.
	/// The array should have at least 5 elements available
	/// after the index parameter.
    /// </summary>
	/// <param name="pArray">The array to copy the values to.</param>
	/// <param name="pIndex">The offset in the array at which to start inserting the values.</param>
	void ICollection.CopyTo(Array pArray, int pIndex)
	{
		if (pArray == null)
			throw new ArgumentNullException("pArray");
		if (pIndex < 0)
			throw new ArgumentOutOfRangeException("pIndex");
		if (pArray.Length - pIndex <= 0 || (pArray.Length - pIndex) < 5)
			throw new ArgumentException("pIndex");

		pArray.SetValue(Element1, pIndex + 0);
		pArray.SetValue(Element2, pIndex + 1);
		pArray.SetValue(Element3, pIndex + 2);
		pArray.SetValue(Element4, pIndex + 3);
		pArray.SetValue(Element5, pIndex + 4);
	}
	
    /// <summary>
    /// Gets the length of the tuple, that is it returns 5.
    /// </summary>
	int ICollection.Count
	{
		get { return 5; }
	}
	
	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	Object ICollection.SyncRoot 
	{
		get { return this; }
	}
	
#endregion

#region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

#endregion

#region IEnumerable<object> implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	public IEnumerator<Object> GetEnumerator()
    {
		yield return Element1;
		yield return Element2;
		yield return Element3;
		yield return Element4;
		yield return Element5;
    }

#endregion

#region IComparable<> implementation

    /// <summary>
    /// Returns a value indicating the order of this tuple compared
	/// to another tuple of the same length and type. The order is defined
	/// as the order of the first element of the tuples.
    /// </summary>
	/// <param name="pOther">The tuple we are comparing this one to.</param>
    /// <returns>value indicating the order of this tuple compared to another tuple of the same length and type.</returns>
	public int CompareTo(Tuple<T1, T2, T3, T4, T5> pOther)
	{
		return Comparer<T1>.Default.Compare(Element1, pOther.Element1);
	}

#endregion

    /// <summary>
    /// Get or sets the value of the element at
	/// the specified index in the tuple.
    /// </summary>
    /// <param name="pIndex">The index of the element in the tuple.</param>
	public Object this[int pIndex]
	{
		get
		{
			switch(pIndex)
			{
				case 0 : return Element1;
				case 1 : return Element2;
				case 2 : return Element3;
				case 3 : return Element4;
				case 4 : return Element5;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
			
		set
		{
			switch(pIndex)
			{
				case 0 :
					if(value is T1)
						Element1 = (T1)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 1 :
					if(value is T2)
						Element2 = (T2)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 2 :
					if(value is T3)
						Element3 = (T3)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 3 :
					if(value is T4)
						Element4 = (T4)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 4 :
					if(value is T5)
						Element5 = (T5)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
	}
	
	/// <summary>
    /// Compares two tuples and returns a value indicating if they are equal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are equal.</returns>
	public static bool operator==(Tuple<T1, T2, T3, T4, T5> pA, Tuple<T1, T2, T3, T4, T5> pB)
	{
		if(System.Object.ReferenceEquals(pA, pB))
			return true;

		if((object)pA == null || (object)pB == null)
			return false;

		return pA.Equals(pB);
	}

	/// <summary>
    /// Compares two tuples and returns a value indicating if they are unequal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are unequal.</returns>
	public static bool operator!=(Tuple<T1, T2, T3, T4, T5> pA, Tuple<T1, T2, T3, T4, T5> pB)
	{
		return !pA.Equals(pB);
	}
	
    /// <summary>
    /// Gets the element of the tuple at position 1.
    /// </summary>
	[DataMember]
	public T1 Element1 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 2.
    /// </summary>
	[DataMember]
	public T2 Element2 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 3.
    /// </summary>
	[DataMember]
	public T3 Element3 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 4.
    /// </summary>
	[DataMember]
	public T4 Element4 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 5.
    /// </summary>
	[DataMember]
	public T5 Element5 { get; set; }
	
 
    /// <summary>
    /// Gets or sets the first element of
	/// the tuple. Same as using Element1. Only added for
	/// syntax reasons.
    /// </summary>
	public T1 First
	{ 
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets or sets the second element of
	/// the tuple. Same as using Element2. Only added for
	/// syntax reasons.
    /// </summary>
	public T2 Second
	{ 
		get { return Element2; }
		set { Element2 = value; }
	}
	
    /// <summary>
    /// Gets or sets the third element of
	/// the tuple. Same as using Element3. Only added for
	/// syntax reasons.
    /// </summary>
	public T3 Third
	{ 
		get { return Element3; }
		set { Element3 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fourth element of
	/// the tuple. Same as using Element4. Only added for
	/// syntax reasons.
    /// </summary>
	public T4 Fourth
	{ 
		get { return Element4; }
		set { Element4 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fifth element of
	/// the tuple. Same as using Element5. Only added for
	/// syntax reasons.
    /// </summary>
	public T5 Fifth
	{ 
		get { return Element5; }
		set { Element5 = value; }
	}
	
 
    /// <summary>
    /// Gets or sets the head of the tuple, that is the first element.
	/// Same as using the properties Element1 or First. Only Added
	/// for syntax reasons.
    /// </summary>
	public T1 Head
	{
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets the tail of the tuple, that is, all elements
	/// except the first one. This property actually returns a completely
	/// new tuple so be careful about that as changing the tail
	/// not change the original tuple.
    /// </summary>
	public Tuple <T2, T3, T4, T5> Tail
	{
		get
		{
			return Tuple.New(
				Element2
				,Element3
				,Element4
				,Element5
			);
		}
	}
}


/// <summary>
/// Represents a tuple of length 6
/// </summary>
/// <typeparam name="T1">Type of the tuple's Element1</typeparam>
/// <typeparam name="T2">Type of the tuple's Element2</typeparam>
/// <typeparam name="T3">Type of the tuple's Element3</typeparam>
/// <typeparam name="T4">Type of the tuple's Element4</typeparam>
/// <typeparam name="T5">Type of the tuple's Element5</typeparam>
/// <typeparam name="T6">Type of the tuple's Element6</typeparam>
[DataContract]
public class Tuple<T1, T2, T3, T4, T5, T6> : ICollection, IEnumerable, IEnumerable<Object>,
	IEquatable<Tuple<T1, T2, T3, T4, T5, T6>>, IComparable<Tuple<T1, T2, T3, T4, T5, T6>>
{
    /// <summary>
    /// An empty tuple constructor. All elements will have their default values.
    /// </summary>
	public Tuple()
	{
	}
	
    /// <summary>
    /// Tuple constructor. The first 1 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	public Tuple(T1 t1)
	{
		Element1 = t1;
	}
	
    /// <summary>
    /// Tuple constructor. The first 2 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	public Tuple(T1 t1, T2 t2)
	{
		Element1 = t1;
		Element2 = t2;
	}
	
    /// <summary>
    /// Tuple constructor. The first 3 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	public Tuple(T1 t1, T2 t2, T3 t3)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
	}
	
    /// <summary>
    /// Tuple constructor. The first 4 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
	}
	
    /// <summary>
    /// Tuple constructor. The first 5 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
	}
	
    /// <summary>
    /// Tuple constructor. The first 6 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
	}
	
    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T7>(T7 t7)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,t7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T7, T8>(T7 t7, T8 t8)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,t7
			,t8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T7, T8, T9>(T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,t7
			,t8
			,t9
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="t7">Value of the new tuple's Element7</param>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="t10">Value of the new tuple's Element10</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T7, T8, T9, T10>(T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,t7
			,t8
			,t9
			,t10
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element1</typeparam>
	/// <param name="t7">Value of the new tuple's Element1</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T7, T1, T2, T3, T4, T5, T6> Prepend<T7>(T7 t7)
	{
		return Tuple.New(
			t7
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element1</typeparam>
	/// <param name="t7">Value of the new tuple's Element1</param>
	/// <typeparam name="T8">Type of the new tuple's Element2</typeparam>
	/// <param name="t8">Value of the new tuple's Element2</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T7, T8, T1, T2, T3, T4, T5, T6> Prepend<T7, T8>(T7 t7, T8 t8)
	{
		return Tuple.New(
			t7
			,t8
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element1</typeparam>
	/// <param name="t7">Value of the new tuple's Element1</param>
	/// <typeparam name="T8">Type of the new tuple's Element2</typeparam>
	/// <param name="t8">Value of the new tuple's Element2</param>
	/// <typeparam name="T9">Type of the new tuple's Element3</typeparam>
	/// <param name="t9">Value of the new tuple's Element3</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T7, T8, T9, T1, T2, T3, T4, T5, T6> Prepend<T7, T8, T9>(T7 t7, T8 t8, T9 t9)
	{
		return Tuple.New(
			t7
			,t8
			,t9
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element1</typeparam>
	/// <param name="t7">Value of the new tuple's Element1</param>
	/// <typeparam name="T8">Type of the new tuple's Element2</typeparam>
	/// <param name="t8">Value of the new tuple's Element2</param>
	/// <typeparam name="T9">Type of the new tuple's Element3</typeparam>
	/// <param name="t9">Value of the new tuple's Element3</param>
	/// <typeparam name="T10">Type of the new tuple's Element4</typeparam>
	/// <param name="t10">Value of the new tuple's Element4</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T7, T8, T9, T10, T1, T2, T3, T4, T5, T6> Prepend<T7, T8, T9, T10>(T7 t7, T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			t7
			,t8
			,t9
			,t10
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
		);
	}

	
    /// <summary>
    /// Creates a new tuple of length 7 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 7 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T7>(Tuple<T7> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,pOther.Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 8 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T7, T8>(Tuple<T7, T8> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,pOther.Element1
			,pOther.Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 9 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T7, T8, T9>(Tuple<T7, T8, T9> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element7</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 10 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T7, T8, T9, T10>(Tuple<T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
		);
	}

    /// <summary>
    /// Creates a new tuple of length 7 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element1</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 7 with the passed in elements added at the beginning.</returns>
	public Tuple<T7, T1, T2, T3, T4, T5, T6> Prepend<T7>(Tuple<T7> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element2</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T7, T8, T1, T2, T3, T4, T5, T6> Prepend<T7, T8>(Tuple<T7, T8> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element3</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T7, T8, T9, T1, T2, T3, T4, T5, T6> Prepend<T7, T8, T9>(Tuple<T7, T8, T9> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T7">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T8">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element3</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element4</typeparam>
	/// <param name="pOther">Tuple of length 4 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T7, T8, T9, T10, T1, T2, T3, T4, T5, T6> Prepend<T7, T8, T9, T10>(Tuple<T7, T8, T9, T10> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,pOther.Element4
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
		);
	}


#region Object overrides

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>Hash code of the object.</returns>
	public override int GetHashCode()
	{
		int hash = 0;
		hash ^= Element1.GetHashCode();
		hash ^= Element2.GetHashCode();
		hash ^= Element3.GetHashCode();
		hash ^= Element4.GetHashCode();
		hash ^= Element5.GetHashCode();
		hash ^= Element6.GetHashCode();
		return hash;
	}
	
    /// <summary>
    /// Returns a value indicating weather this instance is equal to another instance.
    /// </summary>
    /// <param name="pObj">The object we wish to compare with this instance.</param>
    /// <returns>A value indicating if this object is equal to the one passed in.</returns>
	public override bool Equals(Object pObj)
	{
		if(pObj == null)
			return false;
		if(!(pObj is Tuple<T1, T2, T3, T4, T5, T6>))
			return false;

		return Equals((Tuple<T1, T2, T3, T4, T5, T6>)pObj);
	}
	
    /// <summary>
    /// Converts the tuple to a string. This will be a comma separated list
	/// of the string values of the elements enclosed in brackets.
    /// </summary>
    /// <returns>A string representation of the tuple.</returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(Element1);
		sb.Append(", ");
		sb.Append(Element2);
		sb.Append(", ");
		sb.Append(Element3);
		sb.Append(", ");
		sb.Append(Element4);
		sb.Append(", ");
		sb.Append(Element5);
		sb.Append(", ");
		sb.Append(Element6);
		sb.Append(")");
		return sb.ToString();
	}
	
	
    /// <summary>
	/// Returns a string representation of the tuple using the specified format.
    /// </summary>
	/// <param name="pFormat">The format to use for the string representation.</param>
    /// <returns>A string representation of the tuple.</returns>
	public string ToString(String pFormat)
	{
		return String.Format(pFormat
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
		);
	}
	
#endregion

#region IEquatable<> implementation

    /// <summary>
    /// A value indicating if this tuple is equal to a tuple
	/// of the same length and type. This will be so if all members are
	/// equal.
    /// </summary>
    /// <returns>A value indicating weather this tuple is equal to another tuple of the same length and type.</returns>
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6> pObj)
	{
		if(pObj == null)
			return false;

		bool result = true;
		result = result && EqualityComparer<T1>.Default.Equals(Element1, pObj.Element1);
		result = result && EqualityComparer<T2>.Default.Equals(Element2, pObj.Element2);
		result = result && EqualityComparer<T3>.Default.Equals(Element3, pObj.Element3);
		result = result && EqualityComparer<T4>.Default.Equals(Element4, pObj.Element4);
		result = result && EqualityComparer<T5>.Default.Equals(Element5, pObj.Element5);
		result = result && EqualityComparer<T6>.Default.Equals(Element6, pObj.Element6);
		return result;
	}
	
#endregion

#region ICollection implementation

    /// <summary>
    /// Copies the elements of this tuple to an Array.
	/// The array should have at least 6 elements available
	/// after the index parameter.
    /// </summary>
	/// <param name="pArray">The array to copy the values to.</param>
	/// <param name="pIndex">The offset in the array at which to start inserting the values.</param>
	void ICollection.CopyTo(Array pArray, int pIndex)
	{
		if (pArray == null)
			throw new ArgumentNullException("pArray");
		if (pIndex < 0)
			throw new ArgumentOutOfRangeException("pIndex");
		if (pArray.Length - pIndex <= 0 || (pArray.Length - pIndex) < 6)
			throw new ArgumentException("pIndex");

		pArray.SetValue(Element1, pIndex + 0);
		pArray.SetValue(Element2, pIndex + 1);
		pArray.SetValue(Element3, pIndex + 2);
		pArray.SetValue(Element4, pIndex + 3);
		pArray.SetValue(Element5, pIndex + 4);
		pArray.SetValue(Element6, pIndex + 5);
	}
	
    /// <summary>
    /// Gets the length of the tuple, that is it returns 6.
    /// </summary>
	int ICollection.Count
	{
		get { return 6; }
	}
	
	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	Object ICollection.SyncRoot 
	{
		get { return this; }
	}
	
#endregion

#region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

#endregion

#region IEnumerable<object> implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	public IEnumerator<Object> GetEnumerator()
    {
		yield return Element1;
		yield return Element2;
		yield return Element3;
		yield return Element4;
		yield return Element5;
		yield return Element6;
    }

#endregion

#region IComparable<> implementation

    /// <summary>
    /// Returns a value indicating the order of this tuple compared
	/// to another tuple of the same length and type. The order is defined
	/// as the order of the first element of the tuples.
    /// </summary>
	/// <param name="pOther">The tuple we are comparing this one to.</param>
    /// <returns>value indicating the order of this tuple compared to another tuple of the same length and type.</returns>
	public int CompareTo(Tuple<T1, T2, T3, T4, T5, T6> pOther)
	{
		return Comparer<T1>.Default.Compare(Element1, pOther.Element1);
	}

#endregion

    /// <summary>
    /// Get or sets the value of the element at
	/// the specified index in the tuple.
    /// </summary>
    /// <param name="pIndex">The index of the element in the tuple.</param>
	public Object this[int pIndex]
	{
		get
		{
			switch(pIndex)
			{
				case 0 : return Element1;
				case 1 : return Element2;
				case 2 : return Element3;
				case 3 : return Element4;
				case 4 : return Element5;
				case 5 : return Element6;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
			
		set
		{
			switch(pIndex)
			{
				case 0 :
					if(value is T1)
						Element1 = (T1)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 1 :
					if(value is T2)
						Element2 = (T2)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 2 :
					if(value is T3)
						Element3 = (T3)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 3 :
					if(value is T4)
						Element4 = (T4)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 4 :
					if(value is T5)
						Element5 = (T5)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 5 :
					if(value is T6)
						Element6 = (T6)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
	}
	
	/// <summary>
    /// Compares two tuples and returns a value indicating if they are equal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are equal.</returns>
	public static bool operator==(Tuple<T1, T2, T3, T4, T5, T6> pA, Tuple<T1, T2, T3, T4, T5, T6> pB)
	{
		if(System.Object.ReferenceEquals(pA, pB))
			return true;

		if((object)pA == null || (object)pB == null)
			return false;

		return pA.Equals(pB);
	}

	/// <summary>
    /// Compares two tuples and returns a value indicating if they are unequal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are unequal.</returns>
	public static bool operator!=(Tuple<T1, T2, T3, T4, T5, T6> pA, Tuple<T1, T2, T3, T4, T5, T6> pB)
	{
		return !pA.Equals(pB);
	}
	
    /// <summary>
    /// Gets the element of the tuple at position 1.
    /// </summary>
	[DataMember]
	public T1 Element1 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 2.
    /// </summary>
	[DataMember]
	public T2 Element2 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 3.
    /// </summary>
	[DataMember]
	public T3 Element3 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 4.
    /// </summary>
	[DataMember]
	public T4 Element4 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 5.
    /// </summary>
	[DataMember]
	public T5 Element5 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 6.
    /// </summary>
	[DataMember]
	public T6 Element6 { get; set; }
	
 
    /// <summary>
    /// Gets or sets the first element of
	/// the tuple. Same as using Element1. Only added for
	/// syntax reasons.
    /// </summary>
	public T1 First
	{ 
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets or sets the second element of
	/// the tuple. Same as using Element2. Only added for
	/// syntax reasons.
    /// </summary>
	public T2 Second
	{ 
		get { return Element2; }
		set { Element2 = value; }
	}
	
    /// <summary>
    /// Gets or sets the third element of
	/// the tuple. Same as using Element3. Only added for
	/// syntax reasons.
    /// </summary>
	public T3 Third
	{ 
		get { return Element3; }
		set { Element3 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fourth element of
	/// the tuple. Same as using Element4. Only added for
	/// syntax reasons.
    /// </summary>
	public T4 Fourth
	{ 
		get { return Element4; }
		set { Element4 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fifth element of
	/// the tuple. Same as using Element5. Only added for
	/// syntax reasons.
    /// </summary>
	public T5 Fifth
	{ 
		get { return Element5; }
		set { Element5 = value; }
	}
	
    /// <summary>
    /// Gets or sets the sixth element of
	/// the tuple. Same as using Element6. Only added for
	/// syntax reasons.
    /// </summary>
	public T6 Sixth
	{ 
		get { return Element6; }
		set { Element6 = value; }
	}
	
 
    /// <summary>
    /// Gets or sets the head of the tuple, that is the first element.
	/// Same as using the properties Element1 or First. Only Added
	/// for syntax reasons.
    /// </summary>
	public T1 Head
	{
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets the tail of the tuple, that is, all elements
	/// except the first one. This property actually returns a completely
	/// new tuple so be careful about that as changing the tail
	/// not change the original tuple.
    /// </summary>
	public Tuple <T2, T3, T4, T5, T6> Tail
	{
		get
		{
			return Tuple.New(
				Element2
				,Element3
				,Element4
				,Element5
				,Element6
			);
		}
	}
}


/// <summary>
/// Represents a tuple of length 7
/// </summary>
/// <typeparam name="T1">Type of the tuple's Element1</typeparam>
/// <typeparam name="T2">Type of the tuple's Element2</typeparam>
/// <typeparam name="T3">Type of the tuple's Element3</typeparam>
/// <typeparam name="T4">Type of the tuple's Element4</typeparam>
/// <typeparam name="T5">Type of the tuple's Element5</typeparam>
/// <typeparam name="T6">Type of the tuple's Element6</typeparam>
/// <typeparam name="T7">Type of the tuple's Element7</typeparam>
[DataContract]
public class Tuple<T1, T2, T3, T4, T5, T6, T7> : ICollection, IEnumerable, IEnumerable<Object>,
	IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7>>, IComparable<Tuple<T1, T2, T3, T4, T5, T6, T7>>
{
    /// <summary>
    /// An empty tuple constructor. All elements will have their default values.
    /// </summary>
	public Tuple()
	{
	}
	
    /// <summary>
    /// Tuple constructor. The first 1 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	public Tuple(T1 t1)
	{
		Element1 = t1;
	}
	
    /// <summary>
    /// Tuple constructor. The first 2 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	public Tuple(T1 t1, T2 t2)
	{
		Element1 = t1;
		Element2 = t2;
	}
	
    /// <summary>
    /// Tuple constructor. The first 3 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	public Tuple(T1 t1, T2 t2, T3 t3)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
	}
	
    /// <summary>
    /// Tuple constructor. The first 4 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
	}
	
    /// <summary>
    /// Tuple constructor. The first 5 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
	}
	
    /// <summary>
    /// Tuple constructor. The first 6 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
	}
	
    /// <summary>
    /// Tuple constructor. The first 7 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	/// <param name="t7">Value of the tuple's Element7</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
		Element7 = t7;
	}
	
    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T8>(T8 t8)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,t8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T8, T9>(T8 t8, T9 t9)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,t8
			,t9
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="t8">Value of the new tuple's Element8</param>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="t10">Value of the new tuple's Element10</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T8, T9, T10>(T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,t8
			,t9
			,t10
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element1</typeparam>
	/// <param name="t8">Value of the new tuple's Element1</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T8, T1, T2, T3, T4, T5, T6, T7> Prepend<T8>(T8 t8)
	{
		return Tuple.New(
			t8
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element1</typeparam>
	/// <param name="t8">Value of the new tuple's Element1</param>
	/// <typeparam name="T9">Type of the new tuple's Element2</typeparam>
	/// <param name="t9">Value of the new tuple's Element2</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T8, T9, T1, T2, T3, T4, T5, T6, T7> Prepend<T8, T9>(T8 t8, T9 t9)
	{
		return Tuple.New(
			t8
			,t9
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element1</typeparam>
	/// <param name="t8">Value of the new tuple's Element1</param>
	/// <typeparam name="T9">Type of the new tuple's Element2</typeparam>
	/// <param name="t9">Value of the new tuple's Element2</param>
	/// <typeparam name="T10">Type of the new tuple's Element3</typeparam>
	/// <param name="t10">Value of the new tuple's Element3</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T8, T9, T10, T1, T2, T3, T4, T5, T6, T7> Prepend<T8, T9, T10>(T8 t8, T9 t9, T10 t10)
	{
		return Tuple.New(
			t8
			,t9
			,t10
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
		);
	}

	
    /// <summary>
    /// Creates a new tuple of length 8 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 8 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Append<T8>(Tuple<T8> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,pOther.Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 9 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T8, T9>(Tuple<T8, T9> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,pOther.Element1
			,pOther.Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element8</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 10 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T8, T9, T10>(Tuple<T8, T9, T10> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,pOther.Element1
			,pOther.Element2
			,pOther.Element3
		);
	}

    /// <summary>
    /// Creates a new tuple of length 8 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element1</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 8 with the passed in elements added at the beginning.</returns>
	public Tuple<T8, T1, T2, T3, T4, T5, T6, T7> Prepend<T8>(Tuple<T8> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element2</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T8, T9, T1, T2, T3, T4, T5, T6, T7> Prepend<T8, T9>(Tuple<T8, T9> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T8">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T9">Type of the new tuple's Element2</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element3</typeparam>
	/// <param name="pOther">Tuple of length 3 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T8, T9, T10, T1, T2, T3, T4, T5, T6, T7> Prepend<T8, T9, T10>(Tuple<T8, T9, T10> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,pOther.Element3
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
		);
	}


#region Object overrides

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>Hash code of the object.</returns>
	public override int GetHashCode()
	{
		int hash = 0;
		hash ^= Element1.GetHashCode();
		hash ^= Element2.GetHashCode();
		hash ^= Element3.GetHashCode();
		hash ^= Element4.GetHashCode();
		hash ^= Element5.GetHashCode();
		hash ^= Element6.GetHashCode();
		hash ^= Element7.GetHashCode();
		return hash;
	}
	
    /// <summary>
    /// Returns a value indicating weather this instance is equal to another instance.
    /// </summary>
    /// <param name="pObj">The object we wish to compare with this instance.</param>
    /// <returns>A value indicating if this object is equal to the one passed in.</returns>
	public override bool Equals(Object pObj)
	{
		if(pObj == null)
			return false;
		if(!(pObj is Tuple<T1, T2, T3, T4, T5, T6, T7>))
			return false;

		return Equals((Tuple<T1, T2, T3, T4, T5, T6, T7>)pObj);
	}
	
    /// <summary>
    /// Converts the tuple to a string. This will be a comma separated list
	/// of the string values of the elements enclosed in brackets.
    /// </summary>
    /// <returns>A string representation of the tuple.</returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(Element1);
		sb.Append(", ");
		sb.Append(Element2);
		sb.Append(", ");
		sb.Append(Element3);
		sb.Append(", ");
		sb.Append(Element4);
		sb.Append(", ");
		sb.Append(Element5);
		sb.Append(", ");
		sb.Append(Element6);
		sb.Append(", ");
		sb.Append(Element7);
		sb.Append(")");
		return sb.ToString();
	}
	
	
    /// <summary>
	/// Returns a string representation of the tuple using the specified format.
    /// </summary>
	/// <param name="pFormat">The format to use for the string representation.</param>
    /// <returns>A string representation of the tuple.</returns>
	public string ToString(String pFormat)
	{
		return String.Format(pFormat
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
		);
	}
	
#endregion

#region IEquatable<> implementation

    /// <summary>
    /// A value indicating if this tuple is equal to a tuple
	/// of the same length and type. This will be so if all members are
	/// equal.
    /// </summary>
    /// <returns>A value indicating weather this tuple is equal to another tuple of the same length and type.</returns>
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7> pObj)
	{
		if(pObj == null)
			return false;

		bool result = true;
		result = result && EqualityComparer<T1>.Default.Equals(Element1, pObj.Element1);
		result = result && EqualityComparer<T2>.Default.Equals(Element2, pObj.Element2);
		result = result && EqualityComparer<T3>.Default.Equals(Element3, pObj.Element3);
		result = result && EqualityComparer<T4>.Default.Equals(Element4, pObj.Element4);
		result = result && EqualityComparer<T5>.Default.Equals(Element5, pObj.Element5);
		result = result && EqualityComparer<T6>.Default.Equals(Element6, pObj.Element6);
		result = result && EqualityComparer<T7>.Default.Equals(Element7, pObj.Element7);
		return result;
	}
	
#endregion

#region ICollection implementation

    /// <summary>
    /// Copies the elements of this tuple to an Array.
	/// The array should have at least 7 elements available
	/// after the index parameter.
    /// </summary>
	/// <param name="pArray">The array to copy the values to.</param>
	/// <param name="pIndex">The offset in the array at which to start inserting the values.</param>
	void ICollection.CopyTo(Array pArray, int pIndex)
	{
		if (pArray == null)
			throw new ArgumentNullException("pArray");
		if (pIndex < 0)
			throw new ArgumentOutOfRangeException("pIndex");
		if (pArray.Length - pIndex <= 0 || (pArray.Length - pIndex) < 7)
			throw new ArgumentException("pIndex");

		pArray.SetValue(Element1, pIndex + 0);
		pArray.SetValue(Element2, pIndex + 1);
		pArray.SetValue(Element3, pIndex + 2);
		pArray.SetValue(Element4, pIndex + 3);
		pArray.SetValue(Element5, pIndex + 4);
		pArray.SetValue(Element6, pIndex + 5);
		pArray.SetValue(Element7, pIndex + 6);
	}
	
    /// <summary>
    /// Gets the length of the tuple, that is it returns 7.
    /// </summary>
	int ICollection.Count
	{
		get { return 7; }
	}
	
	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	Object ICollection.SyncRoot 
	{
		get { return this; }
	}
	
#endregion

#region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

#endregion

#region IEnumerable<object> implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	public IEnumerator<Object> GetEnumerator()
    {
		yield return Element1;
		yield return Element2;
		yield return Element3;
		yield return Element4;
		yield return Element5;
		yield return Element6;
		yield return Element7;
    }

#endregion

#region IComparable<> implementation

    /// <summary>
    /// Returns a value indicating the order of this tuple compared
	/// to another tuple of the same length and type. The order is defined
	/// as the order of the first element of the tuples.
    /// </summary>
	/// <param name="pOther">The tuple we are comparing this one to.</param>
    /// <returns>value indicating the order of this tuple compared to another tuple of the same length and type.</returns>
	public int CompareTo(Tuple<T1, T2, T3, T4, T5, T6, T7> pOther)
	{
		return Comparer<T1>.Default.Compare(Element1, pOther.Element1);
	}

#endregion

    /// <summary>
    /// Get or sets the value of the element at
	/// the specified index in the tuple.
    /// </summary>
    /// <param name="pIndex">The index of the element in the tuple.</param>
	public Object this[int pIndex]
	{
		get
		{
			switch(pIndex)
			{
				case 0 : return Element1;
				case 1 : return Element2;
				case 2 : return Element3;
				case 3 : return Element4;
				case 4 : return Element5;
				case 5 : return Element6;
				case 6 : return Element7;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
			
		set
		{
			switch(pIndex)
			{
				case 0 :
					if(value is T1)
						Element1 = (T1)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 1 :
					if(value is T2)
						Element2 = (T2)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 2 :
					if(value is T3)
						Element3 = (T3)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 3 :
					if(value is T4)
						Element4 = (T4)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 4 :
					if(value is T5)
						Element5 = (T5)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 5 :
					if(value is T6)
						Element6 = (T6)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 6 :
					if(value is T7)
						Element7 = (T7)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
	}
	
	/// <summary>
    /// Compares two tuples and returns a value indicating if they are equal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are equal.</returns>
	public static bool operator==(Tuple<T1, T2, T3, T4, T5, T6, T7> pA, Tuple<T1, T2, T3, T4, T5, T6, T7> pB)
	{
		if(System.Object.ReferenceEquals(pA, pB))
			return true;

		if((object)pA == null || (object)pB == null)
			return false;

		return pA.Equals(pB);
	}

	/// <summary>
    /// Compares two tuples and returns a value indicating if they are unequal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are unequal.</returns>
	public static bool operator!=(Tuple<T1, T2, T3, T4, T5, T6, T7> pA, Tuple<T1, T2, T3, T4, T5, T6, T7> pB)
	{
		return !pA.Equals(pB);
	}
	
    /// <summary>
    /// Gets the element of the tuple at position 1.
    /// </summary>
	[DataMember]
	public T1 Element1 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 2.
    /// </summary>
	[DataMember]
	public T2 Element2 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 3.
    /// </summary>
	[DataMember]
	public T3 Element3 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 4.
    /// </summary>
	[DataMember]
	public T4 Element4 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 5.
    /// </summary>
	[DataMember]
	public T5 Element5 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 6.
    /// </summary>
	[DataMember]
	public T6 Element6 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 7.
    /// </summary>
	[DataMember]
	public T7 Element7 { get; set; }
	
 
    /// <summary>
    /// Gets or sets the first element of
	/// the tuple. Same as using Element1. Only added for
	/// syntax reasons.
    /// </summary>
	public T1 First
	{ 
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets or sets the second element of
	/// the tuple. Same as using Element2. Only added for
	/// syntax reasons.
    /// </summary>
	public T2 Second
	{ 
		get { return Element2; }
		set { Element2 = value; }
	}
	
    /// <summary>
    /// Gets or sets the third element of
	/// the tuple. Same as using Element3. Only added for
	/// syntax reasons.
    /// </summary>
	public T3 Third
	{ 
		get { return Element3; }
		set { Element3 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fourth element of
	/// the tuple. Same as using Element4. Only added for
	/// syntax reasons.
    /// </summary>
	public T4 Fourth
	{ 
		get { return Element4; }
		set { Element4 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fifth element of
	/// the tuple. Same as using Element5. Only added for
	/// syntax reasons.
    /// </summary>
	public T5 Fifth
	{ 
		get { return Element5; }
		set { Element5 = value; }
	}
	
    /// <summary>
    /// Gets or sets the sixth element of
	/// the tuple. Same as using Element6. Only added for
	/// syntax reasons.
    /// </summary>
	public T6 Sixth
	{ 
		get { return Element6; }
		set { Element6 = value; }
	}
	
    /// <summary>
    /// Gets or sets the seventh element of
	/// the tuple. Same as using Element7. Only added for
	/// syntax reasons.
    /// </summary>
	public T7 Seventh
	{ 
		get { return Element7; }
		set { Element7 = value; }
	}
	
 
    /// <summary>
    /// Gets or sets the head of the tuple, that is the first element.
	/// Same as using the properties Element1 or First. Only Added
	/// for syntax reasons.
    /// </summary>
	public T1 Head
	{
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets the tail of the tuple, that is, all elements
	/// except the first one. This property actually returns a completely
	/// new tuple so be careful about that as changing the tail
	/// not change the original tuple.
    /// </summary>
	public Tuple <T2, T3, T4, T5, T6, T7> Tail
	{
		get
		{
			return Tuple.New(
				Element2
				,Element3
				,Element4
				,Element5
				,Element6
				,Element7
			);
		}
	}
}


/// <summary>
/// Represents a tuple of length 8
/// </summary>
/// <typeparam name="T1">Type of the tuple's Element1</typeparam>
/// <typeparam name="T2">Type of the tuple's Element2</typeparam>
/// <typeparam name="T3">Type of the tuple's Element3</typeparam>
/// <typeparam name="T4">Type of the tuple's Element4</typeparam>
/// <typeparam name="T5">Type of the tuple's Element5</typeparam>
/// <typeparam name="T6">Type of the tuple's Element6</typeparam>
/// <typeparam name="T7">Type of the tuple's Element7</typeparam>
/// <typeparam name="T8">Type of the tuple's Element8</typeparam>
[DataContract]
public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8> : ICollection, IEnumerable, IEnumerable<Object>,
	IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>>, IComparable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>>
{
    /// <summary>
    /// An empty tuple constructor. All elements will have their default values.
    /// </summary>
	public Tuple()
	{
	}
	
    /// <summary>
    /// Tuple constructor. The first 1 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	public Tuple(T1 t1)
	{
		Element1 = t1;
	}
	
    /// <summary>
    /// Tuple constructor. The first 2 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	public Tuple(T1 t1, T2 t2)
	{
		Element1 = t1;
		Element2 = t2;
	}
	
    /// <summary>
    /// Tuple constructor. The first 3 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	public Tuple(T1 t1, T2 t2, T3 t3)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
	}
	
    /// <summary>
    /// Tuple constructor. The first 4 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
	}
	
    /// <summary>
    /// Tuple constructor. The first 5 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
	}
	
    /// <summary>
    /// Tuple constructor. The first 6 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
	}
	
    /// <summary>
    /// Tuple constructor. The first 7 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	/// <param name="t7">Value of the tuple's Element7</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
		Element7 = t7;
	}
	
    /// <summary>
    /// Tuple constructor. The first 8 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	/// <param name="t7">Value of the tuple's Element7</param>
	/// <param name="t8">Value of the tuple's Element8</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
		Element7 = t7;
		Element8 = t8;
	}
	
    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T9>(T9 t9)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
			,t9
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="t9">Value of the new tuple's Element9</param>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="t10">Value of the new tuple's Element10</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T9, T10>(T9 t9, T10 t10)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
			,t9
			,t10
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T9">Type of the new tuple's Element1</typeparam>
	/// <param name="t9">Value of the new tuple's Element1</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T9, T1, T2, T3, T4, T5, T6, T7, T8> Prepend<T9>(T9 t9)
	{
		return Tuple.New(
			t9
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T9">Type of the new tuple's Element1</typeparam>
	/// <param name="t9">Value of the new tuple's Element1</param>
	/// <typeparam name="T10">Type of the new tuple's Element2</typeparam>
	/// <param name="t10">Value of the new tuple's Element2</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T9, T10, T1, T2, T3, T4, T5, T6, T7, T8> Prepend<T9, T10>(T9 t9, T10 t10)
	{
		return Tuple.New(
			t9
			,t10
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
		);
	}

	
    /// <summary>
    /// Creates a new tuple of length 9 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 9 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Append<T9>(Tuple<T9> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
			,pOther.Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T9">Type of the new tuple's Element9</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 10 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T9, T10>(Tuple<T9, T10> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
			,pOther.Element1
			,pOther.Element2
		);
	}

    /// <summary>
    /// Creates a new tuple of length 9 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T9">Type of the new tuple's Element1</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 9 with the passed in elements added at the beginning.</returns>
	public Tuple<T9, T1, T2, T3, T4, T5, T6, T7, T8> Prepend<T9>(Tuple<T9> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T9">Type of the new tuple's Element1</typeparam>
	/// <typeparam name="T10">Type of the new tuple's Element2</typeparam>
	/// <param name="pOther">Tuple of length 2 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T9, T10, T1, T2, T3, T4, T5, T6, T7, T8> Prepend<T9, T10>(Tuple<T9, T10> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,pOther.Element2
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
		);
	}


#region Object overrides

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>Hash code of the object.</returns>
	public override int GetHashCode()
	{
		int hash = 0;
		hash ^= Element1.GetHashCode();
		hash ^= Element2.GetHashCode();
		hash ^= Element3.GetHashCode();
		hash ^= Element4.GetHashCode();
		hash ^= Element5.GetHashCode();
		hash ^= Element6.GetHashCode();
		hash ^= Element7.GetHashCode();
		hash ^= Element8.GetHashCode();
		return hash;
	}
	
    /// <summary>
    /// Returns a value indicating weather this instance is equal to another instance.
    /// </summary>
    /// <param name="pObj">The object we wish to compare with this instance.</param>
    /// <returns>A value indicating if this object is equal to the one passed in.</returns>
	public override bool Equals(Object pObj)
	{
		if(pObj == null)
			return false;
		if(!(pObj is Tuple<T1, T2, T3, T4, T5, T6, T7, T8>))
			return false;

		return Equals((Tuple<T1, T2, T3, T4, T5, T6, T7, T8>)pObj);
	}
	
    /// <summary>
    /// Converts the tuple to a string. This will be a comma separated list
	/// of the string values of the elements enclosed in brackets.
    /// </summary>
    /// <returns>A string representation of the tuple.</returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(Element1);
		sb.Append(", ");
		sb.Append(Element2);
		sb.Append(", ");
		sb.Append(Element3);
		sb.Append(", ");
		sb.Append(Element4);
		sb.Append(", ");
		sb.Append(Element5);
		sb.Append(", ");
		sb.Append(Element6);
		sb.Append(", ");
		sb.Append(Element7);
		sb.Append(", ");
		sb.Append(Element8);
		sb.Append(")");
		return sb.ToString();
	}
	
	
    /// <summary>
	/// Returns a string representation of the tuple using the specified format.
    /// </summary>
	/// <param name="pFormat">The format to use for the string representation.</param>
    /// <returns>A string representation of the tuple.</returns>
	public string ToString(String pFormat)
	{
		return String.Format(pFormat
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
		);
	}
	
#endregion

#region IEquatable<> implementation

    /// <summary>
    /// A value indicating if this tuple is equal to a tuple
	/// of the same length and type. This will be so if all members are
	/// equal.
    /// </summary>
    /// <returns>A value indicating weather this tuple is equal to another tuple of the same length and type.</returns>
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> pObj)
	{
		if(pObj == null)
			return false;

		bool result = true;
		result = result && EqualityComparer<T1>.Default.Equals(Element1, pObj.Element1);
		result = result && EqualityComparer<T2>.Default.Equals(Element2, pObj.Element2);
		result = result && EqualityComparer<T3>.Default.Equals(Element3, pObj.Element3);
		result = result && EqualityComparer<T4>.Default.Equals(Element4, pObj.Element4);
		result = result && EqualityComparer<T5>.Default.Equals(Element5, pObj.Element5);
		result = result && EqualityComparer<T6>.Default.Equals(Element6, pObj.Element6);
		result = result && EqualityComparer<T7>.Default.Equals(Element7, pObj.Element7);
		result = result && EqualityComparer<T8>.Default.Equals(Element8, pObj.Element8);
		return result;
	}
	
#endregion

#region ICollection implementation

    /// <summary>
    /// Copies the elements of this tuple to an Array.
	/// The array should have at least 8 elements available
	/// after the index parameter.
    /// </summary>
	/// <param name="pArray">The array to copy the values to.</param>
	/// <param name="pIndex">The offset in the array at which to start inserting the values.</param>
	void ICollection.CopyTo(Array pArray, int pIndex)
	{
		if (pArray == null)
			throw new ArgumentNullException("pArray");
		if (pIndex < 0)
			throw new ArgumentOutOfRangeException("pIndex");
		if (pArray.Length - pIndex <= 0 || (pArray.Length - pIndex) < 8)
			throw new ArgumentException("pIndex");

		pArray.SetValue(Element1, pIndex + 0);
		pArray.SetValue(Element2, pIndex + 1);
		pArray.SetValue(Element3, pIndex + 2);
		pArray.SetValue(Element4, pIndex + 3);
		pArray.SetValue(Element5, pIndex + 4);
		pArray.SetValue(Element6, pIndex + 5);
		pArray.SetValue(Element7, pIndex + 6);
		pArray.SetValue(Element8, pIndex + 7);
	}
	
    /// <summary>
    /// Gets the length of the tuple, that is it returns 8.
    /// </summary>
	int ICollection.Count
	{
		get { return 8; }
	}
	
	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	Object ICollection.SyncRoot 
	{
		get { return this; }
	}
	
#endregion

#region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

#endregion

#region IEnumerable<object> implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	public IEnumerator<Object> GetEnumerator()
    {
		yield return Element1;
		yield return Element2;
		yield return Element3;
		yield return Element4;
		yield return Element5;
		yield return Element6;
		yield return Element7;
		yield return Element8;
    }

#endregion

#region IComparable<> implementation

    /// <summary>
    /// Returns a value indicating the order of this tuple compared
	/// to another tuple of the same length and type. The order is defined
	/// as the order of the first element of the tuples.
    /// </summary>
	/// <param name="pOther">The tuple we are comparing this one to.</param>
    /// <returns>value indicating the order of this tuple compared to another tuple of the same length and type.</returns>
	public int CompareTo(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> pOther)
	{
		return Comparer<T1>.Default.Compare(Element1, pOther.Element1);
	}

#endregion

    /// <summary>
    /// Get or sets the value of the element at
	/// the specified index in the tuple.
    /// </summary>
    /// <param name="pIndex">The index of the element in the tuple.</param>
	public Object this[int pIndex]
	{
		get
		{
			switch(pIndex)
			{
				case 0 : return Element1;
				case 1 : return Element2;
				case 2 : return Element3;
				case 3 : return Element4;
				case 4 : return Element5;
				case 5 : return Element6;
				case 6 : return Element7;
				case 7 : return Element8;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
			
		set
		{
			switch(pIndex)
			{
				case 0 :
					if(value is T1)
						Element1 = (T1)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 1 :
					if(value is T2)
						Element2 = (T2)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 2 :
					if(value is T3)
						Element3 = (T3)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 3 :
					if(value is T4)
						Element4 = (T4)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 4 :
					if(value is T5)
						Element5 = (T5)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 5 :
					if(value is T6)
						Element6 = (T6)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 6 :
					if(value is T7)
						Element7 = (T7)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 7 :
					if(value is T8)
						Element8 = (T8)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
	}
	
	/// <summary>
    /// Compares two tuples and returns a value indicating if they are equal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are equal.</returns>
	public static bool operator==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> pA, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> pB)
	{
		if(System.Object.ReferenceEquals(pA, pB))
			return true;

		if((object)pA == null || (object)pB == null)
			return false;

		return pA.Equals(pB);
	}

	/// <summary>
    /// Compares two tuples and returns a value indicating if they are unequal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are unequal.</returns>
	public static bool operator!=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> pA, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> pB)
	{
		return !pA.Equals(pB);
	}
	
    /// <summary>
    /// Gets the element of the tuple at position 1.
    /// </summary>
	[DataMember]
	public T1 Element1 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 2.
    /// </summary>
	[DataMember]
	public T2 Element2 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 3.
    /// </summary>
	[DataMember]
	public T3 Element3 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 4.
    /// </summary>
	[DataMember]
	public T4 Element4 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 5.
    /// </summary>
	[DataMember]
	public T5 Element5 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 6.
    /// </summary>
	[DataMember]
	public T6 Element6 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 7.
    /// </summary>
	[DataMember]
	public T7 Element7 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 8.
    /// </summary>
	[DataMember]
	public T8 Element8 { get; set; }
	
 
    /// <summary>
    /// Gets or sets the first element of
	/// the tuple. Same as using Element1. Only added for
	/// syntax reasons.
    /// </summary>
	public T1 First
	{ 
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets or sets the second element of
	/// the tuple. Same as using Element2. Only added for
	/// syntax reasons.
    /// </summary>
	public T2 Second
	{ 
		get { return Element2; }
		set { Element2 = value; }
	}
	
    /// <summary>
    /// Gets or sets the third element of
	/// the tuple. Same as using Element3. Only added for
	/// syntax reasons.
    /// </summary>
	public T3 Third
	{ 
		get { return Element3; }
		set { Element3 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fourth element of
	/// the tuple. Same as using Element4. Only added for
	/// syntax reasons.
    /// </summary>
	public T4 Fourth
	{ 
		get { return Element4; }
		set { Element4 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fifth element of
	/// the tuple. Same as using Element5. Only added for
	/// syntax reasons.
    /// </summary>
	public T5 Fifth
	{ 
		get { return Element5; }
		set { Element5 = value; }
	}
	
    /// <summary>
    /// Gets or sets the sixth element of
	/// the tuple. Same as using Element6. Only added for
	/// syntax reasons.
    /// </summary>
	public T6 Sixth
	{ 
		get { return Element6; }
		set { Element6 = value; }
	}
	
    /// <summary>
    /// Gets or sets the seventh element of
	/// the tuple. Same as using Element7. Only added for
	/// syntax reasons.
    /// </summary>
	public T7 Seventh
	{ 
		get { return Element7; }
		set { Element7 = value; }
	}
	
    /// <summary>
    /// Gets or sets the eight element of
	/// the tuple. Same as using Element8. Only added for
	/// syntax reasons.
    /// </summary>
	public T8 Eight
	{ 
		get { return Element8; }
		set { Element8 = value; }
	}
	
 
    /// <summary>
    /// Gets or sets the head of the tuple, that is the first element.
	/// Same as using the properties Element1 or First. Only Added
	/// for syntax reasons.
    /// </summary>
	public T1 Head
	{
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets the tail of the tuple, that is, all elements
	/// except the first one. This property actually returns a completely
	/// new tuple so be careful about that as changing the tail
	/// not change the original tuple.
    /// </summary>
	public Tuple <T2, T3, T4, T5, T6, T7, T8> Tail
	{
		get
		{
			return Tuple.New(
				Element2
				,Element3
				,Element4
				,Element5
				,Element6
				,Element7
				,Element8
			);
		}
	}
}


/// <summary>
/// Represents a tuple of length 9
/// </summary>
/// <typeparam name="T1">Type of the tuple's Element1</typeparam>
/// <typeparam name="T2">Type of the tuple's Element2</typeparam>
/// <typeparam name="T3">Type of the tuple's Element3</typeparam>
/// <typeparam name="T4">Type of the tuple's Element4</typeparam>
/// <typeparam name="T5">Type of the tuple's Element5</typeparam>
/// <typeparam name="T6">Type of the tuple's Element6</typeparam>
/// <typeparam name="T7">Type of the tuple's Element7</typeparam>
/// <typeparam name="T8">Type of the tuple's Element8</typeparam>
/// <typeparam name="T9">Type of the tuple's Element9</typeparam>
[DataContract]
public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> : ICollection, IEnumerable, IEnumerable<Object>,
	IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>, IComparable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
{
    /// <summary>
    /// An empty tuple constructor. All elements will have their default values.
    /// </summary>
	public Tuple()
	{
	}
	
    /// <summary>
    /// Tuple constructor. The first 1 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	public Tuple(T1 t1)
	{
		Element1 = t1;
	}
	
    /// <summary>
    /// Tuple constructor. The first 2 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	public Tuple(T1 t1, T2 t2)
	{
		Element1 = t1;
		Element2 = t2;
	}
	
    /// <summary>
    /// Tuple constructor. The first 3 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	public Tuple(T1 t1, T2 t2, T3 t3)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
	}
	
    /// <summary>
    /// Tuple constructor. The first 4 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
	}
	
    /// <summary>
    /// Tuple constructor. The first 5 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
	}
	
    /// <summary>
    /// Tuple constructor. The first 6 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
	}
	
    /// <summary>
    /// Tuple constructor. The first 7 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	/// <param name="t7">Value of the tuple's Element7</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
		Element7 = t7;
	}
	
    /// <summary>
    /// Tuple constructor. The first 8 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	/// <param name="t7">Value of the tuple's Element7</param>
	/// <param name="t8">Value of the tuple's Element8</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
		Element7 = t7;
		Element8 = t8;
	}
	
    /// <summary>
    /// Tuple constructor. The first 9 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	/// <param name="t7">Value of the tuple's Element7</param>
	/// <param name="t8">Value of the tuple's Element8</param>
	/// <param name="t9">Value of the tuple's Element9</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
		Element7 = t7;
		Element8 = t8;
		Element9 = t9;
	}
	
    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in appended to the end.
    /// </summary>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="t10">Value of the new tuple's Element10</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T10>(T10 t10)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
			,Element9
			,t10
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T10">Type of the new tuple's Element1</typeparam>
	/// <param name="t10">Value of the new tuple's Element1</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T10, T1, T2, T3, T4, T5, T6, T7, T8, T9> Prepend<T10>(T10 t10)
	{
		return Tuple.New(
			t10
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
			,Element9
		);
	}

	
    /// <summary>
    /// Creates a new tuple of length 10 with the values of the tuple passed in appended to the end.
    /// </summary>
	/// <typeparam name="T10">Type of the new tuple's Element10</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be appended.</param>
	/// <returns>A new tuple of length 10 with the elements of the tuple passed in added at the end.</returns>
	public Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Append<T10>(Tuple<T10> pOther)
	{
		return Tuple.New(
			Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
			,Element9
			,pOther.Element1
		);
	}

    /// <summary>
    /// Creates a new tuple of length 10 with the values passed from the tuple in prepended to the beginning.
    /// </summary>
	/// <typeparam name="T10">Type of the new tuple's Element1</typeparam>
	/// <param name="pOther">Tuple of length 1 containing the elements to be prepended.</param>
    /// <returns>A new tuple of length 10 with the passed in elements added at the beginning.</returns>
	public Tuple<T10, T1, T2, T3, T4, T5, T6, T7, T8, T9> Prepend<T10>(Tuple<T10> pOther)
	{
		return Tuple.New(
			pOther.Element1
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
			,Element9
		);
	}


#region Object overrides

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>Hash code of the object.</returns>
	public override int GetHashCode()
	{
		int hash = 0;
		hash ^= Element1.GetHashCode();
		hash ^= Element2.GetHashCode();
		hash ^= Element3.GetHashCode();
		hash ^= Element4.GetHashCode();
		hash ^= Element5.GetHashCode();
		hash ^= Element6.GetHashCode();
		hash ^= Element7.GetHashCode();
		hash ^= Element8.GetHashCode();
		hash ^= Element9.GetHashCode();
		return hash;
	}
	
    /// <summary>
    /// Returns a value indicating weather this instance is equal to another instance.
    /// </summary>
    /// <param name="pObj">The object we wish to compare with this instance.</param>
    /// <returns>A value indicating if this object is equal to the one passed in.</returns>
	public override bool Equals(Object pObj)
	{
		if(pObj == null)
			return false;
		if(!(pObj is Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>))
			return false;

		return Equals((Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>)pObj);
	}
	
    /// <summary>
    /// Converts the tuple to a string. This will be a comma separated list
	/// of the string values of the elements enclosed in brackets.
    /// </summary>
    /// <returns>A string representation of the tuple.</returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(Element1);
		sb.Append(", ");
		sb.Append(Element2);
		sb.Append(", ");
		sb.Append(Element3);
		sb.Append(", ");
		sb.Append(Element4);
		sb.Append(", ");
		sb.Append(Element5);
		sb.Append(", ");
		sb.Append(Element6);
		sb.Append(", ");
		sb.Append(Element7);
		sb.Append(", ");
		sb.Append(Element8);
		sb.Append(", ");
		sb.Append(Element9);
		sb.Append(")");
		return sb.ToString();
	}
	
	
    /// <summary>
	/// Returns a string representation of the tuple using the specified format.
    /// </summary>
	/// <param name="pFormat">The format to use for the string representation.</param>
    /// <returns>A string representation of the tuple.</returns>
	public string ToString(String pFormat)
	{
		return String.Format(pFormat
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
			,Element9
		);
	}
	
#endregion

#region IEquatable<> implementation

    /// <summary>
    /// A value indicating if this tuple is equal to a tuple
	/// of the same length and type. This will be so if all members are
	/// equal.
    /// </summary>
    /// <returns>A value indicating weather this tuple is equal to another tuple of the same length and type.</returns>
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> pObj)
	{
		if(pObj == null)
			return false;

		bool result = true;
		result = result && EqualityComparer<T1>.Default.Equals(Element1, pObj.Element1);
		result = result && EqualityComparer<T2>.Default.Equals(Element2, pObj.Element2);
		result = result && EqualityComparer<T3>.Default.Equals(Element3, pObj.Element3);
		result = result && EqualityComparer<T4>.Default.Equals(Element4, pObj.Element4);
		result = result && EqualityComparer<T5>.Default.Equals(Element5, pObj.Element5);
		result = result && EqualityComparer<T6>.Default.Equals(Element6, pObj.Element6);
		result = result && EqualityComparer<T7>.Default.Equals(Element7, pObj.Element7);
		result = result && EqualityComparer<T8>.Default.Equals(Element8, pObj.Element8);
		result = result && EqualityComparer<T9>.Default.Equals(Element9, pObj.Element9);
		return result;
	}
	
#endregion

#region ICollection implementation

    /// <summary>
    /// Copies the elements of this tuple to an Array.
	/// The array should have at least 9 elements available
	/// after the index parameter.
    /// </summary>
	/// <param name="pArray">The array to copy the values to.</param>
	/// <param name="pIndex">The offset in the array at which to start inserting the values.</param>
	void ICollection.CopyTo(Array pArray, int pIndex)
	{
		if (pArray == null)
			throw new ArgumentNullException("pArray");
		if (pIndex < 0)
			throw new ArgumentOutOfRangeException("pIndex");
		if (pArray.Length - pIndex <= 0 || (pArray.Length - pIndex) < 9)
			throw new ArgumentException("pIndex");

		pArray.SetValue(Element1, pIndex + 0);
		pArray.SetValue(Element2, pIndex + 1);
		pArray.SetValue(Element3, pIndex + 2);
		pArray.SetValue(Element4, pIndex + 3);
		pArray.SetValue(Element5, pIndex + 4);
		pArray.SetValue(Element6, pIndex + 5);
		pArray.SetValue(Element7, pIndex + 6);
		pArray.SetValue(Element8, pIndex + 7);
		pArray.SetValue(Element9, pIndex + 8);
	}
	
    /// <summary>
    /// Gets the length of the tuple, that is it returns 9.
    /// </summary>
	int ICollection.Count
	{
		get { return 9; }
	}
	
	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	Object ICollection.SyncRoot 
	{
		get { return this; }
	}
	
#endregion

#region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

#endregion

#region IEnumerable<object> implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	public IEnumerator<Object> GetEnumerator()
    {
		yield return Element1;
		yield return Element2;
		yield return Element3;
		yield return Element4;
		yield return Element5;
		yield return Element6;
		yield return Element7;
		yield return Element8;
		yield return Element9;
    }

#endregion

#region IComparable<> implementation

    /// <summary>
    /// Returns a value indicating the order of this tuple compared
	/// to another tuple of the same length and type. The order is defined
	/// as the order of the first element of the tuples.
    /// </summary>
	/// <param name="pOther">The tuple we are comparing this one to.</param>
    /// <returns>value indicating the order of this tuple compared to another tuple of the same length and type.</returns>
	public int CompareTo(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> pOther)
	{
		return Comparer<T1>.Default.Compare(Element1, pOther.Element1);
	}

#endregion

    /// <summary>
    /// Get or sets the value of the element at
	/// the specified index in the tuple.
    /// </summary>
    /// <param name="pIndex">The index of the element in the tuple.</param>
	public Object this[int pIndex]
	{
		get
		{
			switch(pIndex)
			{
				case 0 : return Element1;
				case 1 : return Element2;
				case 2 : return Element3;
				case 3 : return Element4;
				case 4 : return Element5;
				case 5 : return Element6;
				case 6 : return Element7;
				case 7 : return Element8;
				case 8 : return Element9;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
			
		set
		{
			switch(pIndex)
			{
				case 0 :
					if(value is T1)
						Element1 = (T1)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 1 :
					if(value is T2)
						Element2 = (T2)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 2 :
					if(value is T3)
						Element3 = (T3)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 3 :
					if(value is T4)
						Element4 = (T4)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 4 :
					if(value is T5)
						Element5 = (T5)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 5 :
					if(value is T6)
						Element6 = (T6)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 6 :
					if(value is T7)
						Element7 = (T7)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 7 :
					if(value is T8)
						Element8 = (T8)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 8 :
					if(value is T9)
						Element9 = (T9)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
	}
	
	/// <summary>
    /// Compares two tuples and returns a value indicating if they are equal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are equal.</returns>
	public static bool operator==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> pA, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> pB)
	{
		if(System.Object.ReferenceEquals(pA, pB))
			return true;

		if((object)pA == null || (object)pB == null)
			return false;

		return pA.Equals(pB);
	}

	/// <summary>
    /// Compares two tuples and returns a value indicating if they are unequal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are unequal.</returns>
	public static bool operator!=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> pA, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> pB)
	{
		return !pA.Equals(pB);
	}
	
    /// <summary>
    /// Gets the element of the tuple at position 1.
    /// </summary>
	[DataMember]
	public T1 Element1 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 2.
    /// </summary>
	[DataMember]
	public T2 Element2 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 3.
    /// </summary>
	[DataMember]
	public T3 Element3 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 4.
    /// </summary>
	[DataMember]
	public T4 Element4 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 5.
    /// </summary>
	[DataMember]
	public T5 Element5 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 6.
    /// </summary>
	[DataMember]
	public T6 Element6 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 7.
    /// </summary>
	[DataMember]
	public T7 Element7 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 8.
    /// </summary>
	[DataMember]
	public T8 Element8 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 9.
    /// </summary>
	[DataMember]
	public T9 Element9 { get; set; }
	
 
    /// <summary>
    /// Gets or sets the first element of
	/// the tuple. Same as using Element1. Only added for
	/// syntax reasons.
    /// </summary>
	public T1 First
	{ 
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets or sets the second element of
	/// the tuple. Same as using Element2. Only added for
	/// syntax reasons.
    /// </summary>
	public T2 Second
	{ 
		get { return Element2; }
		set { Element2 = value; }
	}
	
    /// <summary>
    /// Gets or sets the third element of
	/// the tuple. Same as using Element3. Only added for
	/// syntax reasons.
    /// </summary>
	public T3 Third
	{ 
		get { return Element3; }
		set { Element3 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fourth element of
	/// the tuple. Same as using Element4. Only added for
	/// syntax reasons.
    /// </summary>
	public T4 Fourth
	{ 
		get { return Element4; }
		set { Element4 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fifth element of
	/// the tuple. Same as using Element5. Only added for
	/// syntax reasons.
    /// </summary>
	public T5 Fifth
	{ 
		get { return Element5; }
		set { Element5 = value; }
	}
	
    /// <summary>
    /// Gets or sets the sixth element of
	/// the tuple. Same as using Element6. Only added for
	/// syntax reasons.
    /// </summary>
	public T6 Sixth
	{ 
		get { return Element6; }
		set { Element6 = value; }
	}
	
    /// <summary>
    /// Gets or sets the seventh element of
	/// the tuple. Same as using Element7. Only added for
	/// syntax reasons.
    /// </summary>
	public T7 Seventh
	{ 
		get { return Element7; }
		set { Element7 = value; }
	}
	
    /// <summary>
    /// Gets or sets the eight element of
	/// the tuple. Same as using Element8. Only added for
	/// syntax reasons.
    /// </summary>
	public T8 Eight
	{ 
		get { return Element8; }
		set { Element8 = value; }
	}
	
    /// <summary>
    /// Gets or sets the ninth element of
	/// the tuple. Same as using Element9. Only added for
	/// syntax reasons.
    /// </summary>
	public T9 Ninth
	{ 
		get { return Element9; }
		set { Element9 = value; }
	}
	
 
    /// <summary>
    /// Gets or sets the head of the tuple, that is the first element.
	/// Same as using the properties Element1 or First. Only Added
	/// for syntax reasons.
    /// </summary>
	public T1 Head
	{
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets the tail of the tuple, that is, all elements
	/// except the first one. This property actually returns a completely
	/// new tuple so be careful about that as changing the tail
	/// not change the original tuple.
    /// </summary>
	public Tuple <T2, T3, T4, T5, T6, T7, T8, T9> Tail
	{
		get
		{
			return Tuple.New(
				Element2
				,Element3
				,Element4
				,Element5
				,Element6
				,Element7
				,Element8
				,Element9
			);
		}
	}
}


/// <summary>
/// Represents a tuple of length 10
/// </summary>
/// <typeparam name="T1">Type of the tuple's Element1</typeparam>
/// <typeparam name="T2">Type of the tuple's Element2</typeparam>
/// <typeparam name="T3">Type of the tuple's Element3</typeparam>
/// <typeparam name="T4">Type of the tuple's Element4</typeparam>
/// <typeparam name="T5">Type of the tuple's Element5</typeparam>
/// <typeparam name="T6">Type of the tuple's Element6</typeparam>
/// <typeparam name="T7">Type of the tuple's Element7</typeparam>
/// <typeparam name="T8">Type of the tuple's Element8</typeparam>
/// <typeparam name="T9">Type of the tuple's Element9</typeparam>
/// <typeparam name="T10">Type of the tuple's Element10</typeparam>
[DataContract]
public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ICollection, IEnumerable, IEnumerable<Object>,
	IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>, IComparable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
{
    /// <summary>
    /// An empty tuple constructor. All elements will have their default values.
    /// </summary>
	public Tuple()
	{
	}
	
    /// <summary>
    /// Tuple constructor. The first 1 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	public Tuple(T1 t1)
	{
		Element1 = t1;
	}
	
    /// <summary>
    /// Tuple constructor. The first 2 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	public Tuple(T1 t1, T2 t2)
	{
		Element1 = t1;
		Element2 = t2;
	}
	
    /// <summary>
    /// Tuple constructor. The first 3 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	public Tuple(T1 t1, T2 t2, T3 t3)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
	}
	
    /// <summary>
    /// Tuple constructor. The first 4 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
	}
	
    /// <summary>
    /// Tuple constructor. The first 5 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
	}
	
    /// <summary>
    /// Tuple constructor. The first 6 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
	}
	
    /// <summary>
    /// Tuple constructor. The first 7 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	/// <param name="t7">Value of the tuple's Element7</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
		Element7 = t7;
	}
	
    /// <summary>
    /// Tuple constructor. The first 8 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	/// <param name="t7">Value of the tuple's Element7</param>
	/// <param name="t8">Value of the tuple's Element8</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
		Element7 = t7;
		Element8 = t8;
	}
	
    /// <summary>
    /// Tuple constructor. The first 9 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	/// <param name="t7">Value of the tuple's Element7</param>
	/// <param name="t8">Value of the tuple's Element8</param>
	/// <param name="t9">Value of the tuple's Element9</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
		Element7 = t7;
		Element8 = t8;
		Element9 = t9;
	}
	
    /// <summary>
    /// Tuple constructor. The first 10 elements will be assigned the values passed in.
    /// </summary>
	/// <param name="t1">Value of the tuple's Element1</param>
	/// <param name="t2">Value of the tuple's Element2</param>
	/// <param name="t3">Value of the tuple's Element3</param>
	/// <param name="t4">Value of the tuple's Element4</param>
	/// <param name="t5">Value of the tuple's Element5</param>
	/// <param name="t6">Value of the tuple's Element6</param>
	/// <param name="t7">Value of the tuple's Element7</param>
	/// <param name="t8">Value of the tuple's Element8</param>
	/// <param name="t9">Value of the tuple's Element9</param>
	/// <param name="t10">Value of the tuple's Element10</param>
	public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
	{
		Element1 = t1;
		Element2 = t2;
		Element3 = t3;
		Element4 = t4;
		Element5 = t5;
		Element6 = t6;
		Element7 = t7;
		Element8 = t8;
		Element9 = t9;
		Element10 = t10;
	}
	

#region Object overrides

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>Hash code of the object.</returns>
	public override int GetHashCode()
	{
		int hash = 0;
		hash ^= Element1.GetHashCode();
		hash ^= Element2.GetHashCode();
		hash ^= Element3.GetHashCode();
		hash ^= Element4.GetHashCode();
		hash ^= Element5.GetHashCode();
		hash ^= Element6.GetHashCode();
		hash ^= Element7.GetHashCode();
		hash ^= Element8.GetHashCode();
		hash ^= Element9.GetHashCode();
		hash ^= Element10.GetHashCode();
		return hash;
	}
	
    /// <summary>
    /// Returns a value indicating weather this instance is equal to another instance.
    /// </summary>
    /// <param name="pObj">The object we wish to compare with this instance.</param>
    /// <returns>A value indicating if this object is equal to the one passed in.</returns>
	public override bool Equals(Object pObj)
	{
		if(pObj == null)
			return false;
		if(!(pObj is Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>))
			return false;

		return Equals((Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)pObj);
	}
	
    /// <summary>
    /// Converts the tuple to a string. This will be a comma separated list
	/// of the string values of the elements enclosed in brackets.
    /// </summary>
    /// <returns>A string representation of the tuple.</returns>
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(Element1);
		sb.Append(", ");
		sb.Append(Element2);
		sb.Append(", ");
		sb.Append(Element3);
		sb.Append(", ");
		sb.Append(Element4);
		sb.Append(", ");
		sb.Append(Element5);
		sb.Append(", ");
		sb.Append(Element6);
		sb.Append(", ");
		sb.Append(Element7);
		sb.Append(", ");
		sb.Append(Element8);
		sb.Append(", ");
		sb.Append(Element9);
		sb.Append(", ");
		sb.Append(Element10);
		sb.Append(")");
		return sb.ToString();
	}
	
	
    /// <summary>
	/// Returns a string representation of the tuple using the specified format.
    /// </summary>
	/// <param name="pFormat">The format to use for the string representation.</param>
    /// <returns>A string representation of the tuple.</returns>
	public string ToString(String pFormat)
	{
		return String.Format(pFormat
			,Element1
			,Element2
			,Element3
			,Element4
			,Element5
			,Element6
			,Element7
			,Element8
			,Element9
			,Element10
		);
	}
	
#endregion

#region IEquatable<> implementation

    /// <summary>
    /// A value indicating if this tuple is equal to a tuple
	/// of the same length and type. This will be so if all members are
	/// equal.
    /// </summary>
    /// <returns>A value indicating weather this tuple is equal to another tuple of the same length and type.</returns>
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> pObj)
	{
		if(pObj == null)
			return false;

		bool result = true;
		result = result && EqualityComparer<T1>.Default.Equals(Element1, pObj.Element1);
		result = result && EqualityComparer<T2>.Default.Equals(Element2, pObj.Element2);
		result = result && EqualityComparer<T3>.Default.Equals(Element3, pObj.Element3);
		result = result && EqualityComparer<T4>.Default.Equals(Element4, pObj.Element4);
		result = result && EqualityComparer<T5>.Default.Equals(Element5, pObj.Element5);
		result = result && EqualityComparer<T6>.Default.Equals(Element6, pObj.Element6);
		result = result && EqualityComparer<T7>.Default.Equals(Element7, pObj.Element7);
		result = result && EqualityComparer<T8>.Default.Equals(Element8, pObj.Element8);
		result = result && EqualityComparer<T9>.Default.Equals(Element9, pObj.Element9);
		result = result && EqualityComparer<T10>.Default.Equals(Element10, pObj.Element10);
		return result;
	}
	
#endregion

#region ICollection implementation

    /// <summary>
    /// Copies the elements of this tuple to an Array.
	/// The array should have at least 10 elements available
	/// after the index parameter.
    /// </summary>
	/// <param name="pArray">The array to copy the values to.</param>
	/// <param name="pIndex">The offset in the array at which to start inserting the values.</param>
	void ICollection.CopyTo(Array pArray, int pIndex)
	{
		if (pArray == null)
			throw new ArgumentNullException("pArray");
		if (pIndex < 0)
			throw new ArgumentOutOfRangeException("pIndex");
		if (pArray.Length - pIndex <= 0 || (pArray.Length - pIndex) < 10)
			throw new ArgumentException("pIndex");

		pArray.SetValue(Element1, pIndex + 0);
		pArray.SetValue(Element2, pIndex + 1);
		pArray.SetValue(Element3, pIndex + 2);
		pArray.SetValue(Element4, pIndex + 3);
		pArray.SetValue(Element5, pIndex + 4);
		pArray.SetValue(Element6, pIndex + 5);
		pArray.SetValue(Element7, pIndex + 6);
		pArray.SetValue(Element8, pIndex + 7);
		pArray.SetValue(Element9, pIndex + 8);
		pArray.SetValue(Element10, pIndex + 9);
	}
	
    /// <summary>
    /// Gets the length of the tuple, that is it returns 10.
    /// </summary>
	int ICollection.Count
	{
		get { return 10; }
	}
	
	bool ICollection.IsSynchronized
	{
		get { return false; }
	}

	Object ICollection.SyncRoot 
	{
		get { return this; }
	}
	
#endregion

#region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

#endregion

#region IEnumerable<object> implementation

    /// <summary>
    /// Returns an enumerator to this tuple.
    /// </summary>
    /// <returns>An enumerator of the elements of the tuple.</returns>
	public IEnumerator<Object> GetEnumerator()
    {
		yield return Element1;
		yield return Element2;
		yield return Element3;
		yield return Element4;
		yield return Element5;
		yield return Element6;
		yield return Element7;
		yield return Element8;
		yield return Element9;
		yield return Element10;
    }

#endregion

#region IComparable<> implementation

    /// <summary>
    /// Returns a value indicating the order of this tuple compared
	/// to another tuple of the same length and type. The order is defined
	/// as the order of the first element of the tuples.
    /// </summary>
	/// <param name="pOther">The tuple we are comparing this one to.</param>
    /// <returns>value indicating the order of this tuple compared to another tuple of the same length and type.</returns>
	public int CompareTo(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> pOther)
	{
		return Comparer<T1>.Default.Compare(Element1, pOther.Element1);
	}

#endregion

    /// <summary>
    /// Get or sets the value of the element at
	/// the specified index in the tuple.
    /// </summary>
    /// <param name="pIndex">The index of the element in the tuple.</param>
	public Object this[int pIndex]
	{
		get
		{
			switch(pIndex)
			{
				case 0 : return Element1;
				case 1 : return Element2;
				case 2 : return Element3;
				case 3 : return Element4;
				case 4 : return Element5;
				case 5 : return Element6;
				case 6 : return Element7;
				case 7 : return Element8;
				case 8 : return Element9;
				case 9 : return Element10;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
			
		set
		{
			switch(pIndex)
			{
				case 0 :
					if(value is T1)
						Element1 = (T1)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 1 :
					if(value is T2)
						Element2 = (T2)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 2 :
					if(value is T3)
						Element3 = (T3)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 3 :
					if(value is T4)
						Element4 = (T4)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 4 :
					if(value is T5)
						Element5 = (T5)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 5 :
					if(value is T6)
						Element6 = (T6)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 6 :
					if(value is T7)
						Element7 = (T7)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 7 :
					if(value is T8)
						Element8 = (T8)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 8 :
					if(value is T9)
						Element9 = (T9)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
				case 9 :
					if(value is T10)
						Element10 = (T10)value;
					else
						throw new Exception("Trying to set a field with an object of the wrong type");
					return;
			}
			throw new ArgumentOutOfRangeException("pIndex");
		}
	}
	
	/// <summary>
    /// Compares two tuples and returns a value indicating if they are equal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are equal.</returns>
	public static bool operator==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> pA, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> pB)
	{
		if(System.Object.ReferenceEquals(pA, pB))
			return true;

		if((object)pA == null || (object)pB == null)
			return false;

		return pA.Equals(pB);
	}

	/// <summary>
    /// Compares two tuples and returns a value indicating if they are unequal.
    /// </summary>
    /// <param name="pA">A tuple to compare.</param>
    /// <param name="pB">A tuple to compare.</param>
    /// <returns>A value indicating if the two tuples are unequal.</returns>
	public static bool operator!=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> pA, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> pB)
	{
		return !pA.Equals(pB);
	}
	
    /// <summary>
    /// Gets the element of the tuple at position 1.
    /// </summary>
	[DataMember]
	public T1 Element1 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 2.
    /// </summary>
	[DataMember]
	public T2 Element2 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 3.
    /// </summary>
	[DataMember]
	public T3 Element3 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 4.
    /// </summary>
	[DataMember]
	public T4 Element4 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 5.
    /// </summary>
	[DataMember]
	public T5 Element5 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 6.
    /// </summary>
	[DataMember]
	public T6 Element6 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 7.
    /// </summary>
	[DataMember]
	public T7 Element7 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 8.
    /// </summary>
	[DataMember]
	public T8 Element8 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 9.
    /// </summary>
	[DataMember]
	public T9 Element9 { get; set; }
	
    /// <summary>
    /// Gets the element of the tuple at position 10.
    /// </summary>
	[DataMember]
	public T10 Element10 { get; set; }
	
 
    /// <summary>
    /// Gets or sets the first element of
	/// the tuple. Same as using Element1. Only added for
	/// syntax reasons.
    /// </summary>
	public T1 First
	{ 
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets or sets the second element of
	/// the tuple. Same as using Element2. Only added for
	/// syntax reasons.
    /// </summary>
	public T2 Second
	{ 
		get { return Element2; }
		set { Element2 = value; }
	}
	
    /// <summary>
    /// Gets or sets the third element of
	/// the tuple. Same as using Element3. Only added for
	/// syntax reasons.
    /// </summary>
	public T3 Third
	{ 
		get { return Element3; }
		set { Element3 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fourth element of
	/// the tuple. Same as using Element4. Only added for
	/// syntax reasons.
    /// </summary>
	public T4 Fourth
	{ 
		get { return Element4; }
		set { Element4 = value; }
	}
	
    /// <summary>
    /// Gets or sets the fifth element of
	/// the tuple. Same as using Element5. Only added for
	/// syntax reasons.
    /// </summary>
	public T5 Fifth
	{ 
		get { return Element5; }
		set { Element5 = value; }
	}
	
    /// <summary>
    /// Gets or sets the sixth element of
	/// the tuple. Same as using Element6. Only added for
	/// syntax reasons.
    /// </summary>
	public T6 Sixth
	{ 
		get { return Element6; }
		set { Element6 = value; }
	}
	
    /// <summary>
    /// Gets or sets the seventh element of
	/// the tuple. Same as using Element7. Only added for
	/// syntax reasons.
    /// </summary>
	public T7 Seventh
	{ 
		get { return Element7; }
		set { Element7 = value; }
	}
	
    /// <summary>
    /// Gets or sets the eight element of
	/// the tuple. Same as using Element8. Only added for
	/// syntax reasons.
    /// </summary>
	public T8 Eight
	{ 
		get { return Element8; }
		set { Element8 = value; }
	}
	
    /// <summary>
    /// Gets or sets the ninth element of
	/// the tuple. Same as using Element9. Only added for
	/// syntax reasons.
    /// </summary>
	public T9 Ninth
	{ 
		get { return Element9; }
		set { Element9 = value; }
	}
	
    /// <summary>
    /// Gets or sets the tenth element of
	/// the tuple. Same as using Element10. Only added for
	/// syntax reasons.
    /// </summary>
	public T10 Tenth
	{ 
		get { return Element10; }
		set { Element10 = value; }
	}
	
 
    /// <summary>
    /// Gets or sets the head of the tuple, that is the first element.
	/// Same as using the properties Element1 or First. Only Added
	/// for syntax reasons.
    /// </summary>
	public T1 Head
	{
		get { return Element1; }
		set { Element1 = value; }
	}
	
    /// <summary>
    /// Gets the tail of the tuple, that is, all elements
	/// except the first one. This property actually returns a completely
	/// new tuple so be careful about that as changing the tail
	/// not change the original tuple.
    /// </summary>
	public Tuple <T2, T3, T4, T5, T6, T7, T8, T9, T10> Tail
	{
		get
		{
			return Tuple.New(
				Element2
				,Element3
				,Element4
				,Element5
				,Element6
				,Element7
				,Element8
				,Element9
				,Element10
			);
		}
	}
}


#endregion
}