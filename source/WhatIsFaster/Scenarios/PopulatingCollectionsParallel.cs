using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using System.Diagnostics;

namespace WhatIsFaster
{
	[TestFixture]
	public class WhenPopulatingCollectionsParallel
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

			return stopwatch.ElapsedMilliseconds;
		}

		[Test]
        public void Duration_Of_Populate_Collection_Parallel_As_Array()
		{
			// arrange
			var collection = new int[amount_of_iterations];

			// act
		    Parallel.For(0, amount_of_iterations, i =>
		        {
                    collection[i] = i;
		        });

			// assert
			StopMeasuring ("Populate_Collection_As_Array");
			collection.Count().Should ().Be (amount_of_iterations, "Unexpected amount of iterations");
		}

		[Test]
		public void Duration_Of_Populate_Collection_Parallel_As_ConcurrentBag()
		{
			// arrange
			var collection = new ConcurrentBag<int> ();

			// act
		    Parallel.For(0, amount_of_iterations, collection.Add);

			// assert
			StopMeasuring ("PopulateCollection_As_ConcurrentBag");
			collection.Count.Should ().Be (amount_of_iterations, "Unexpected amount of iterations");
		}

		[Test]
        public void Duration_Of_Populate_Collection_Parallel_As_List()
		{
			// arrange
			var collection = new List<int> ();

            // act
            Parallel.For(0, amount_of_iterations, i =>
            {
                // note: due to non-thread safety of IList<T>, we need to lock it first
                // in order to prevent an ArgumentException in parallel scenarios
                lock (collection)
                {
                    collection.Add(i);
                }
            });

			// assert
			StopMeasuring ("PopulateCollection_As_List");
			collection.Count.Should ().Be (amount_of_iterations, "Unexpected amount of iterations");
		}
			
	}
}

