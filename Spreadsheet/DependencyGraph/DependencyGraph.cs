// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.

using System;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> dependees;
        private Dictionary<string, HashSet<string>> dependants;
        private int num = 0;

        public struct copy
        {
            public HashSet<string> dentss { get; set; }
        }

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependees = new Dictionary<string, HashSet<string>>();
            dependants = new Dictionary<string, HashSet<string>>();
        }

        public DependencyGraph(DependencyGraph d1)
        {
            if(d1 == null)
            {
                throw new ArgumentNullException("DependencyGraph passed in was null");
            }



            string tempString;
            HashSet<string> tempSet;
            dependants = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
            



            foreach(KeyValuePair<string, HashSet<string>> pair in d1.dependants)
            {
                if(pair.Key == null)
                {
                    throw new ArgumentNullException("cant copy d1 if a key in d1.dependents is null");
                }
                tempString = "" + pair.Key;
                tempSet = new HashSet<string>();
                foreach(string el in pair.Value)
                {
                    if(el == null)
                    {
                        throw new ArgumentNullException("cant copy d1 if an element in the values of d1.dependents is null");
                    }
                    tempSet.Add("" + el);
                }

                dependants.Add(tempString, tempSet);
            }

            foreach(KeyValuePair<string, HashSet<string>> pair in d1.dependees)
            {
                if (pair.Key == null)
                {
                    throw new ArgumentNullException("cant copy d1 if a key in d1.dependees is null");
                }
                tempString = "" + pair.Key;
                tempSet = new HashSet<string>();
                foreach (string el in pair.Value)
                {
                    if(el == null)
                    {
                        throw new ArgumentNullException("cant copy d1 if an element in the values of d1.dependees is null");
                    }
                    tempSet.Add("" + el);
                }

                dependees.Add(tempString, tempSet);
            }



            num += d1.Size;
            
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                return num;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {

            if (s != null)
            {
                if (dependees.ContainsKey(s))
                {
                    if (dependees[s].Count > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    Console.WriteLine(s + " not found in dependents");
                    return false;
                }
            }
            else
                throw new ArgumentNullException("Can not search Dependants using a null key");
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s != null)
            {
                if (dependants.ContainsKey(s))
                {
                    if (dependants[s].Count > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    Console.WriteLine(s + " not found in dependees");
                    return false;
                }
            }
            else
                throw new ArgumentNullException("Can not search Dependees using a null key");
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s != null)
            {
                if (dependees.ContainsKey(s))
                {
                    return dependees[s];
                }
                else
                {
                    Console.WriteLine("Cant return an IEnumerable of key: " + s + " because " + s + " is not a key in Dependants");
                    return new HashSet<string>();
                }
            }
            else
            {
                throw new ArgumentNullException("Can not return an IEnumerable of Dependents of a null key value");
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s != null)
            {
                if (dependants.ContainsKey(s))
                {
                    return dependants[s];
                }
                else
                {
                    Console.WriteLine("Can't return an IEnumerable of key: " + s + "because " + s + " is not a key in Dependees");
                    return new HashSet<string>();
                }
            }
            else
            {
                throw new ArgumentNullException("Can not return an IEnumerable of Dependees of a null key value");
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if(s == null)
            {
                throw new ArgumentNullException("Cant AddDependency using a null s value");
            }
            else if(t == null)
            {
                throw new ArgumentNullException("Cant AddDependency using a null t value");
            }


            if (dependees.ContainsKey(s))
            {
                if (!dependees[s].Contains(t))
                {
                    dependees[s].Add(t);
                    num++;
                }

            }
            else
            {
                dependees.Add(s, new HashSet<string>());
                dependees[s].Add(t);
                num++;
            }


            if (dependants.ContainsKey(t))
            {
                if (!dependants[t].Contains(s))
                {
                    dependants[t].Add(s);
                }

            }
            else
            {
                dependants.Add(t, new HashSet<string>());
                dependants[t].Add(s);
            }

        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if(s == null)
            {
                throw new ArgumentNullException("cant remove dependency of a null s value");
            }
            else if(t == null)
            {
                throw new ArgumentNullException("cant remove dependency of a null t value");
            }


            if (s != null && t != null)
            {
                if (dependees.ContainsKey(s))
                {
                    if (dependees[s].Contains(t))
                    {
                        dependees[s].Remove(t);
                        num--;
                        if (dependees[s].Count == 0)
                        {
                            dependees.Remove(s);
                        }
                    }
                }
                if (dependants.ContainsKey(t))
                {
                    if (dependants[t].Contains(s))
                    {
                        dependants[t].Remove(s);
                        if (dependants[t].Count == 0)
                        {
                            dependants.Remove(t);
                        }
                    }
                }
            }
           
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if(s == null)
            {
                throw new ArgumentNullException("cant replace dependents using a null s value");
            }
            else if(newDependents == null)
            {
                throw new ArgumentNullException("cant replace dependents using a null newDependents value");
            }

            if (s != null)
            {
                HashSet<string> temp1 = new HashSet<string>(GetDependents(s));

                foreach(string el in temp1)
                {
                    RemoveDependency(s, el);
                }


                foreach(string el in newDependents)
                {
                    if(el == null)
                    {
                        throw new ArgumentNullException("cant replace dependents if there is a null value inside newDependents");
                    }
                    AddDependency(s, el);
                }

            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if(t == null)
            {
                throw new ArgumentNullException("cant replace dependees using a null t value");
            }
            else if(newDependees == null)
            {
                throw new ArgumentNullException("cant replace dependees using a null newDependees value");
            }

            if (t != null)
            {


                HashSet<string> temp1 = new HashSet<string>(GetDependees(t));

                foreach(string el in temp1)
                {
                    RemoveDependency(el, t);
                }

                foreach(string el in newDependees)
                {
                    if(el == null)
                    {
                        throw new ArgumentNullException("cant replace dependees if there is a null value inside newDependees");
                    }
                    AddDependency(el, t);
                }

            }
        }
    }
}
