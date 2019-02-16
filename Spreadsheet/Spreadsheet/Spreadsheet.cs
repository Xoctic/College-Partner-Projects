using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using static SS.cell;
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        Dictionary<string, cell> cells;
        DependencyGraph dependencies;

        
        
        public Spreadsheet()
        {
            cells = new Dictionary<string, cell>();
            dependencies = new DependencyGraph();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if(name == null)
            {
                throw new InvalidNameException();
            }
            //Change to validate the name
            else if(!validName(name))
            {
                throw new InvalidNameException();
            }
            //
            else if(!cells.ContainsKey(name))
            {
                return "";
            }

            //return cells[name].content;

            Type t = cells[name].content.GetType();

            if((t == typeof(Formula)) || (t == typeof(string)) || (t == typeof(double)))
            {
                return cells[name].content;
            }
            else
            {
                return new Exception("oops"); 
            } 

        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> returner = new HashSet<string>();

            foreach(KeyValuePair<string, cell> cell in cells)
            {
                if(cell.Value.content != null && cell.Value.content != "")
                {
                    returner.Add(cell.Key);
                }
            }

            return returner;
            
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            if(name == null)
            {
                throw new InvalidNameException();
            }
            //Use validator
            else if(!validName(name))
            {
                throw new InvalidNameException();
            }
            //

            cell tempCell = new cell();

            tempCell.content = number;

            cells.Remove(name);

            cells.Add(name, tempCell);

            IEnumerable<string> tempDents;

            HashSet<string> dents = new HashSet<string>();

            tempDents = dependencies.GetDependents(name);

            dents.Add(name);

            foreach (string el in tempDents)
            {
                dents.Add(el);
            }

            return dents;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
           
            if(text == null)
            {
                throw new ArgumentNullException();
            }
            else if(name == null)
            {
                throw new InvalidNameException();
            }
            //Use validator here
            else if(!validName(name))
            {
                throw new InvalidNameException();
            }
            //

            cell tempCell = new cell();
            tempCell.content = text;

            cells.Remove(name);

            cells.Add(name, tempCell);

            IEnumerable<string> tempDents;

            HashSet<string> dents = new HashSet<string>();

            tempDents = dependencies.GetDependents(name);

            dents.Add(name);

            foreach (string el in tempDents)
            {
                dents.Add(el);
            }

            return dents;

        }


        /// <summary>
        /// Requires that all of the variables in formula are valid cell names.
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if(name == null)
            {
                throw new InvalidNameException();
            }
            else if(!validName(name))
            {
                throw new InvalidNameException();
            }
            else if(!validFormula(name, formula))
            {
                throw new CircularException();
            }

            cell tempCell = new cell();

            tempCell.content = formula;

            cells[name] = tempCell;

            IEnumerable<string> tempDents;

            HashSet<string> dents = new HashSet<string>();

            dents.Add(name);

            tempDents = dependencies.GetDependents(name);

            foreach(string el in tempDents)
            {
                dents.Add(el);
            }

            return dents;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if(name == null)
            {
                throw new ArgumentNullException();
            }
            else if(!validName(name))
            {
                throw new InvalidNameException();
            }




            throw new NotImplementedException();
        }


        private bool validFormula(string name, Formula _formula)
        {
            String formula = _formula.ToString();
            Regex nameRegex = new Regex(name);
            Match nameMatch = nameRegex.Match(formula);

            if(nameMatch.Success)
            {
                return false;
            }

            return true;
        }



        private bool validName(String name)
        {
            //string namePattern = "([a-z]?[A-Z])*[0-9]*";
            string namePattern = "[a-zA-Z]*[0-9]*";

            Regex nameRegex = new Regex(namePattern);

            return nameRegex.IsMatch(name);
        }
    }
}
