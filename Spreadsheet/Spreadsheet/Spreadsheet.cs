using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using static SS.cell;
using System.Text.RegularExpressions;
using System.IO;

namespace SS
{
    //spreadsheet class that implements the abstract class AbstractSpreadsheet
    public class Spreadsheet : AbstractSpreadsheet
    {
        Dictionary<string, cell> cells;
        DependencyGraph dependencyGraph;

        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        //constructor which initializes a new dictionary of cells and a new dependency
        //graph to keep track of the cells which contain formulas of other cells and their 
        //relationship to one another
        public Spreadsheet()
        {
            cells = new Dictionary<string, cell>();
            dependencyGraph = new DependencyGraph();
        }

        //NEW CONSTRUCTORS
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        public Spreadsheet(string d)
        {

        }


        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        public Spreadsheet(Regex isValid)
        {

        }


        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        ///
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  
        ///
        /// If there's a problem reading source, throws an IOException.
        ///
        /// Else if the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  
        ///
        /// Else if the IsValid string contained in source is not a valid C# regular expression, throws
        /// a SpreadsheetReadException.  (If the exception is not thrown, this regex is referred to
        /// below as oldIsValid.)
        ///
        /// Else if there is a duplicate cell name in the source, throws a SpreadsheetReadException.
        /// (Two cell names are duplicates if they are identical after being converted to upper case.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a 
        /// SpreadsheetReadException.  (Use oldIsValid in place of IsValid in the definition of 
        /// cell name validity.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetVersionException.  (Use newIsValid in place of IsValid in the definition of
        /// cell name validity.)
        ///
        /// Else if there's a formula that causes a circular dependency, throws a SpreadsheetReadException. 
        ///
        /// Else, create a Spreadsheet that is a duplicate of the one encoded in source except that
        /// the new Spreadsheet's IsValid regular expression should be newIsValid.
        public Spreadsheet(TextReader source, Regex newIsValid)
        {

        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {  
            //Ensures name is not null
            if(name == null)
            {
                throw new InvalidNameException();
            }
            //Ensures a valid name
            else if(!validName(name))
            {
                throw new InvalidNameException();
            }
            //If the cell name passed in has not been initialized yet, returns an
            //empty string for its contents
            else if(!cells.ContainsKey(name))
            {
                return "";
            }


            return cells[name].content;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            //initialize a hashSet used to return the names of all nonEmptyCells
            HashSet<string> returner = new HashSet<string>();

            //returns the name of all cells contained in the cells dictionary
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
        protected override ISet<string> SetCellContents(string name, double number)
        {
            //Ensures name is not null and is valid
            if (name == null)
            {
                throw new InvalidNameException();
            }
            else if(!validName(name))
            {
                throw new InvalidNameException();
            }

            //Initialize a temporary cell used to hold the content of the new cell or replacements cell
            cell tempCell = new cell();

            //if the name of the cell passed in already exists in the dictionary, its contents will need to be replaced
            //and this may affect the values of other cells

            //create an enumerator which traverses through all the cells whos value depends on
            //the value of the cell being changed
            //also create a hashSet to contain the names of these cells

            //set the content of the temporary cell to the content
            //that is to be used for replacing the named cell

            //remove the named cell

            //re-add the named cell using the data from temp cell

            //add all the names of cells that will need to be recalculated to hashSet cellsToRecalculate

            //return the cellsToRecalculate
            if (cells.ContainsKey(name))
            { 
                IEnumerable<string> cellsToRecalculateEnumerator = GetCellsToRecalculate(name);
                ISet<string> cellsToRecalculate = new HashSet<string>();
                
                tempCell.content = number;

                cells.Remove(name);
                
                cells.Add(name, tempCell);
               
                foreach(string el in cellsToRecalculateEnumerator)
                {
                    cellsToRecalculate.Add(el);
                }
                
                return cellsToRecalculate;
            }
            //if the name of the cell does not exist in the dictionary
            //add content to temp cell
            //add to dictionary the name of the cell and the data contained in temp cell
            //return an empty hashSet as no other cells will have depended on this cell yet
            else
            {
                tempCell.content = number;

                cells.Add(name, tempCell);

                return new HashSet<string>();
            }

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
        protected override ISet<string> SetCellContents(string name, string text)
        {
            //Ensures that the text, & name is not null as well that the name is valid
            //Creates a new temporary cell to hold data of the new cell
            if(text == null)
            {
                throw new ArgumentNullException();
            }
            else if(name == null)
            {
                throw new InvalidNameException();
            }
            else if(!validName(name))
            {
                throw new InvalidNameException();
            }
            cell tempCell = new cell();

            //if the cell already exists in the dictionary

            //Create an enumerator which can iterate through the names of all the cells whose value
            //depends on the named cell. Also create an HashSet to hold all the names of these dependent cells

            //set the content of the temp cell to the text to be used for the content of the new cell

            //remove the named cell from the dictionary

            //re-add the cell to the dictionary with the data contained in tempCell

            //add all the cells that will have to be recalculated into the HashSet and return it
            if (cells.ContainsKey(name))
            {
                IEnumerable<string> cellsToRecalculateEnumerator = GetCellsToRecalculate(name);
                ISet<string> cellsToRecalculate = new HashSet<string>();


                tempCell.content = text;

                cells.Remove(name);

                cells.Add(name, tempCell);


                foreach (string el in cellsToRecalculateEnumerator)
                {
                    cellsToRecalculate.Add(el);
                }

                return cellsToRecalculate;
            }
            //if the cell does not exist in the dictionary

            //set the content of the temporary cell to the text

            //add the cell to the dictionary using the data contained in tempCell

            //return an empty hashSet since no other cells depend on it yet
            else
            {
                tempCell.content = text;

                cells.Add(name, tempCell);

                return new HashSet<string>();
            }
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            //Ensures that the name is not null or invalid as well as any name contained
            //in the formula passed in
            if(name == null)
            {
                throw new InvalidNameException();
            }
            else if(!validName(name))
            {
                throw new InvalidNameException();
            }

            foreach(string el in formula.GetVariables())
            {
                if(!validName(el))
                {
                    throw new InvalidNameException();
                }
                if(el == name)
                {
                    throw new InvalidNameException();
                }
            }

            //creates a new temporary cell to hold the new formula passed in

            //sets the content of the tempCell to formula

            //creates an ISet of strings which contain the names of all the cells within the formula

            //creates a new HashSet of strings which will contain the names of all the
            //cells which will need to be recalculated after changing the contents of 
            //the current cell

            //calls GetCellsToRecalculate to check if a circularException will occur after changing the 
            //contents of the current cell to the new formula
            cell tempCell = new cell();
            tempCell.content = formula;
            ISet<string> variables = formula.GetVariables();
            ISet<string> dependentCells = new HashSet<string>();
            GetCellsToRecalculate(variables);

            //if the dictionary contains the current cell

            //Create an IEnumerable cellsToRecalculateEnumerator which will contain all the cells that will need to be recalculated
            //after changing the contents of the current cell

            //Iterate through the cellsToRecalculateEnumerator and remove any dependency which will be affected by
            //changing the contents of the cell

            //iterate through the variables contained in the new formula and add the name of the cell to their dependencies

            //replace the current cell with tempCell, while keeping the name of the cell

            //add the name of the cell to the HashSet of dependentCells

            //iterate through the cellsToRecalculateEnumerator and add all the cell names to dependentCells

            //return dependent cells
            if(cells.ContainsKey(name))
            { 
                IEnumerable<string> cellsToRecalculateEnumerator = GetCellsToRecalculate(variables);
                
                foreach(string el in cellsToRecalculateEnumerator)
                {
                    if(el != name)
                    dependencyGraph.RemoveDependency(el, name);
                }

                foreach(string el in formula.GetVariables())
                {
                    if(el != name)
                    dependencyGraph.AddDependency(el, name);
                }

                cells[name] = tempCell;

                dependentCells.Add(name);

                foreach(string el in cellsToRecalculateEnumerator)
                {
                    dependentCells.Add(el);
                }

                return dependentCells;
            }
            //if the dictionary does not contain the name of the current cell
            
            //Iterate through the variables contained in the formula passed in and
            //add the name of the current cell to the dependencis of these variables

            //add the name of the cell and its contents to the dictionary

            //call getCellsToRecalculate using the new variables in the formula to ensure a 
            //circularException will not occur

            //add the name of the cell to dependentCells and return dependentCells
            else
            {
                foreach(string el in formula.GetVariables())
                {
                    dependencyGraph.AddDependency(el, name);
                }

                cells.Add(name, tempCell);

                GetCellsToRecalculate(variables);

                dependentCells.Add(name);

                return dependentCells;
            }

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
            //ensures name is not null or invalid
            
            //if the dictionary does not contain the named cell, return an empty hashSet
            
            //if the named cells content is the name of the cell, throw a circularException

            //return the dependents of the named cell
            if(name == null)
            {
                throw new ArgumentNullException();
            }
            else if(!validName(name))
            {
                throw new InvalidNameException();
            }
            else if(!cells.ContainsKey(name))
            {
                return new HashSet<string>();
            }
            else if(cells[name].content == name)
            {
                throw new CircularException();
            }

            return dependencyGraph.GetDependents(name);
        }


        //Ensures the name of the cell passed in is in the correct format

        //uses a regex which contains the pattern of a cell name
        //this pattern is one or more letters which are upper or lowercase followed by one number 1-9 followed by
        //0 or more numbers 1-9

        //creates a match to determine whether the name of the cell matches the regex pattern

        //returns true of there is a match and false otherwise
        public bool validName(String name)
        {
            string namePattern = "^([a-z]{1}[a-z]*)?([a-z]{1}[A-Z]*)?([A-Z]{1}[a-z]*)?([A-Z]{1}[A-Z]*)([1-9]{1}[0-9]*)\\z";

            Regex nameRegex = new Regex(namePattern);

            Match match = nameRegex.Match(name);

            bool check = match.Success;

            return check;
            
        }

        public override void Save(TextWriter dest)
        {
            throw new NotImplementedException();
        }

        // ADDED FOR PS6
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if(name == null || !validName(name))
            {
                throw new InvalidNameException();
            }
            if(cells.ContainsKey(name))
            {
                return cells[name].value;
            }
            else
            {
                return 0;
            }
            
        }

        // ADDED FOR PS6
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>  
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if(content == null)
            {
                throw new ArgumentNullException();
            }
            else if(name == null || !validName(name))
            {
                throw new InvalidNameException();
            }

            bool isDouble = Double.TryParse(content, out double result);

            if(isDouble)
            {
                cell tempCell = new cell(content, Double.Parse(content));
            }
            else if(content.Substring(0,1) == "=")
            {

            }
            


            throw new NotImplementedException();
        }
    }
}
