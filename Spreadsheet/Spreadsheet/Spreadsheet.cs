using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        Dictionary<string, cell> cells = new Dictionary<string, cell>();
        
        
        public Spreadsheet()
        {
            
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
            else if(!cells.ContainsKey(name))
            {
                throw new InvalidNameException();
            }

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
            else if(!cells.ContainsKey(name))
            {
                throw new InvalidNameException();
            }

            //Not sure how to do the dependency part


            throw new NotImplementedException();
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
            HashSet<string> nameNDependants = new HashSet<string>();
            if(text == null)
            {
                throw new ArgumentNullException();
            }
            else if(name == null || !(cells.ContainsKey(name)))
            {
                throw new InvalidNameException();
            }

            return nameNDependants;

            
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }
    }
}
