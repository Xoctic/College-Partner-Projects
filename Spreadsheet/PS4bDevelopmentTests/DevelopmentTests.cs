using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections.Generic;
using System;

namespace PS4DevelopmentTests
{
    [TestClass]
    public class DevelopmentTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null1()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", null);
        }

        [TestMethod]
        public void Copy1()
        {
            var d1 = new DependencyGraph();
            var d2 = new DependencyGraph(d1);
            Assert.AreEqual(0, d1.Size);
            Assert.AreEqual(0, d2.Size);
        }

        [TestMethod]
        public void Copy5()
        {
            var d1 = new DependencyGraph();
            d1.AddDependency("a", "b");
            d1.AddDependency("d", "e");
            var d2 = new DependencyGraph(d1);
            d1.AddDependency("a", "c");
            d2.AddDependency("d", "f");
            Assert.AreEqual(2, new List<string>(d1.GetDependents("a")).Count);
            Assert.AreEqual(1, new List<string>(d1.GetDependents("d")).Count);
            Assert.AreEqual(2, new List<string>(d2.GetDependents("d")).Count);
            Assert.AreEqual(1, new List<string>(d2.GetDependents("a")).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullCopy()
        {
            DependencyGraph d1 = new DependencyGraph(null);
        }

        [TestMethod]
        public void noDependents()
        {
            DependencyGraph d1 = new DependencyGraph();
            Assert.IsFalse(d1.HasDependents("d"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullSearchHasDependents()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.HasDependents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullSearchHasDependees()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.HasDependees(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullGetDependents()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.GetDependents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullGetDependees()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.GetDependees(null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullAddDependency()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.AddDependency(null, "5");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullRemoveDependency1()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.RemoveDependency(null, "5");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullRemoveDependency2()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.RemoveDependency("5", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullReplaceDents1()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.ReplaceDependents(null, new HashSet<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullReplaceDents2()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.ReplaceDependents("5", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullReplaceDents3()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.AddDependency("a", "b");
            d1.AddDependency("a", "c");
            d1.AddDependency("a", "d");

            HashSet<string> set = new HashSet<string>();
            set.Add("yo");
            set.Add("mama");
            set.Add(null);

            d1.ReplaceDependents("a", set);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullReplaceDees1()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.ReplaceDependees(null, new HashSet<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullReplaceDees2()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.ReplaceDependees("4", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void nullReplaceDees3()
        {
            DependencyGraph d1 = new DependencyGraph();
            d1.AddDependency("b", "a");
            d1.AddDependency("c", "a");
            d1.AddDependency("d", "a");

            HashSet<string> set = new HashSet<string>();
            set.Add("yo");
            set.Add("granny");
            set.Add(null);

            d1.ReplaceDependees("a", set);

        }

         

    }
}
