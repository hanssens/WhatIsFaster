using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using System.Diagnostics;

namespace WhatIsFaster
{
	[TestFixture]
	public class WhenPopulatingCollections
	{
		private const int amount_of_iterations = 1000000;
		private Stopwatch stopwatch { get; set; }

		[SetUp]
		public void StartMeasuring()
		{
			stopwatch = new Stopwatch ();
			stopwatch.Start ();
		}

		public long StopMeasuring(string functionName)
		{
			stopwatch.Stop ();

			if (!String.IsNullOrEmpty (functionName))
				Console.WriteLine (String.Format ("{0} took {1} ms to complete", functionName, stopwatch.ElapsedMilliseconds));
				//Assert.Pass (String.Format ("{0} took {1} ms to complete", functionName, stopwatch.ElapsedMilliseconds));

			return stopwatch.ElapsedMilliseconds;
		}

		[Test]
		public void Duration_Of_Populate_Collection_As_Array()
		{
			// arrange
			var collection = new int[amount_of_iterations];

			// act
			for (int i = 0; i < amount_of_iterations; i++) {
				collection[i] = i;
			}

			// assert
			StopMeasuring ("Populate_Collection_As_Array");
			collection.Count().Should ().Be (amount_of_iterations, "Unexpected amount of iterations");
		}

		[Test]
		public void Duration_Of_Populate_Collection_As_ConcurrentBag()
		{
			// arrange
			var collection = new ConcurrentBag<int> ();

			// act
			for (int i = 0; i < amount_of_iterations; i++) {
				collection.Add (i);
			}

			// assert
			StopMeasuring ("PopulateCollection_As_ConcurrentBag");
			collection.Count.Should ().Be (amount_of_iterations, "Unexpected amount of iterations");
		}

		[Test]
		public void Duration_Of_Populate_Collection_As_List()
		{
			// arrange
			var collection = new List<int> ();

			// act
			for (int i = 0; i < amount_of_iterations; i++) {
				collection.Add (i);
			}

			// assert
			StopMeasuring ("PopulateCollection_As_List");
			collection.Count.Should ().Be (amount_of_iterations, "Unexpected amount of iterations");
		}
			
	}
}

