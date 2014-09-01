using System;
using NUnit.Framework;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace WhatIsFaster
{
	/// <summary>
	/// Testing the performance of .First() vs array[0]
	/// </summary>
	[TestFixture]
	public class FirstVersusArray
	{
		private const int amount_of_iterations = 1000000;
		private Stopwatch stopwatch { get; set; }
		private int[] collection;

		[SetUp]
		public void StartMeasuring()
		{
			// Arrange
			var _collection = new List<int> ();
			for (int i = 1; i < amount_of_iterations; i++) {
				_collection.Add (i);
			}

			collection = _collection.ToArray ();

			stopwatch = new Stopwatch ();
			stopwatch.Start ();
		}

		public long StopMeasuring(string functionName)
		{
			stopwatch.Stop ();

			if (!String.IsNullOrEmpty (functionName))
				Console.WriteLine (String.Format ("{0} took {1} ms to complete", functionName, stopwatch.ElapsedMilliseconds));

			return stopwatch.ElapsedMilliseconds;
		}

		[Test]
		public void Fetch_First_Value_Using_First()
		{
			// act
			var target = collection.First ();

			// assert
			StopMeasuring ("UsingFirst");
			target.Should ().Be (1);
		}

		[Test]
		public void Fetch_First_Value_Using_ArrayIndex()
		{
			// act 
			var target = collection [0];

			// assert
			StopMeasuring ("UsingArrayIndex");
			target.Should ().Be (1);

		}
	}
}

