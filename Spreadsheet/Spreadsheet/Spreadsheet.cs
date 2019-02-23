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
using System.Xml;
using System.Xml.Schema;

namespace SS
{
    //spreadsheet class that implements the abstract class AbstractSpreadsheet
    public class Spreadsheet : AbstractSpreadsheet
    {
        Dictionary<string, cell> cells;
        DependencyGraph dependencyGraph;
        Regex isValid;
        bool changedVariable;

        // ADDED FOR PS6
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        //if I succesfully change something set CHANGED to true, if saved, set back to false
        public override bool Changed
        {
            get
            {
                return changedVariable;
            }
            protected set
            {
                changedVariable = value;
            }
        }

        //constructor which initializes a new dictionary of cells and a new dependency
        //graph to keep track of the cells which contain formulas of other cells and their 
        //relationship to one another
        //its isValid regular expression accepts every string
        public Spreadsheet()
        {
            cells = new Dictionary<string, cell>();
            dependencyGraph = new DependencyGraph();
            //CREATE A NEW REGEX THAT ACCEPTS EVERY STRING
            isValid = new Regex("(/S?/s)*");
        }


        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        public Spreadsheet(Regex _isValid)
        {
            //string regX = _isValid.ToString();


            cells = new Dictionary<string, cell>();
            dependencyGraph = new DependencyGraph();
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
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add(null, "Spreadsheet.xsd");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += ValidationCallback;


            cells = new Dictionary<string, cell>();
            dependencyGraph = new DependencyGraph();
            string name;
            string contents;
            Regex oldIsValid;

            isValid = newIsValid;


            try
            {
                using (XmlReader reader = XmlReader.Create(source))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    oldIsValid = new Regex(reader["IsValid"]);
                                    break;
                                case "cell":
                                    name = reader["name"];
                                    contents = reader["contents"];
                                    SetContentsOfCell(name, contents);
                                    break;

                            }
                        }
                    }
                }
            }
            catch (IOException e)
            {
                throw e;
            }

            isValid = newIsValid;


        }

        // Display any validation errors.
        private static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            Console.WriteLine(" *** Validation Error: {0}", e.Message);
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
            if (name == null)
            {
                throw new InvalidNameException();
            }
            //Ensures a valid name
            else if (!validName(name))
            {
                throw new InvalidNameException();
            }
            //If the cell name passed in has not been initialized yet, returns an
            //empty string for its contents
            else if (!cells.ContainsKey(name))
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
            foreach (KeyValuePair<string, cell> cell in cells)
            {
                if (cell.Value.content != null && cell.Value.content != "")
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
            else if (!validName(name))
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
                tempCell.type = typeof(double);
                tempCell.value = number;

                cells.Remove(name);

                cells.Add(name, tempCell);

                foreach (string el in cellsToRecalculateEnumerator)
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
                tempCell.type = typeof(double);

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
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            else if (name == null)
            {
                throw new InvalidNameException();
            }
            else if (!validName(name))
            {
                throw new InvalidNameException();
            }
            cell tempCell = new cell();
            ISet<string> cellsToRecalculate;

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
                cellsToRecalculate = new HashSet<string>();


                tempCell.content = text;
                tempCell.value = text;
                tempCell.type = typeof(string);

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
                tempCell.type = typeof(string);

                cells.Add(name, tempCell);
                cellsToRecalculate = new HashSet<string>();
                cellsToRecalculate.Add(name);
                return cellsToRecalculate;
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
            if (name == null)
            {
                throw new InvalidNameException();
            }
            else if (!validName(name))
            {
                throw new InvalidNameException();
            }

            foreach (string el in formula.GetVariables())
            {
                if (!validName(el))
                {
                    throw new InvalidNameException();
                }
                if (el == name)
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
            tempCell.type = typeof(Formula);
            try
            {
                tempCell.value = formula.Evaluate(looker);

            }
            catch (FormulaEvaluationException e)
            {
                tempCell.value = new FormulaError();
            }
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
            if (cells.ContainsKey(name))
            {
                IEnumerable<string> cellsToRecalculateEnumerator = GetCellsToRecalculate(variables);

                foreach (string el in cellsToRecalculateEnumerator)
                {
                    if (el != name)
                        dependencyGraph.RemoveDependency(el, name);
                }

                foreach (string el in formula.GetVariables())
                {
                    if (el != name)
                        dependencyGraph.AddDependency(el, name);
                }

                cells[name] = tempCell;

                dependentCells.Add(name);

                foreach (string el in cellsToRecalculateEnumerator)
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
                foreach (string el in formula.GetVariables())
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
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            else if (!validName(name))
            {
                throw new InvalidNameException();
            }
            else if (!cells.ContainsKey(name))
            {
                return new HashSet<string>();
            }
            else if (cells[name].content == name)
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

        public bool validName(string name, Regex r)
        {
            Match match = r.Match(name);

            bool check = match.Success;

            return check;
        }


        // ADDED FOR PS6
        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the IsValid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            //Console.Write(dest.ToString());
            try
            {
                using (XmlWriter writer = XmlWriter.Create(dest))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("IsValid", isValid.ToString());
                    writer.WriteString(Environment.NewLine);
                    foreach (KeyValuePair<string, cell> el in cells)
                    {
                        writer.WriteString("/t");
                        writer.WriteStartElement("cell");
                        writer.WriteAttributeString("name", el.Key.ToString());
                        if (el.Value.content.GetType() == typeof(Formula))
                        {
                            writer.WriteAttributeString("contents", "=" + el.Value.content.ToString());
                        }
                        else
                        {
                            writer.WriteAttributeString("contents", el.Value.content.ToString());
                        }
                        writer.WriteEndElement();
                        writer.WriteString(Environment.NewLine);
                    }
                    writer.WriteEndElement();
                    //writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

            }
            catch (IOException e)
            {
                throw e;
            }

            Changed = false;

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
            cell TempCell;
            if (name == null || !validName(name))
            {
                throw new InvalidNameException();
            }
            if (cells.ContainsKey(name))
            {
                if (cells[name].value == null)
                {
                    TempCell = cells[name];
                    TempCell.value = "";
                    cells[name] = TempCell;
                }
                return cells[name].value;
            }
            else
            {
                return "";
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
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            else if (name == null || !validName(name))
            {
                throw new InvalidNameException();
            }

            cell tempCell;

            ISet<string> cellsToRecalculate;

            bool isDouble = Double.TryParse(content, out double result);

            if (isDouble)
            {
                cellsToRecalculate = SetCellContents(name, Double.Parse(content));
            }
            else if (content.Substring(0, 1) == "=")
            {
                int lengthOfString = content.Length;
                string substring = content.Substring(1);

                Formula f;

                try
                {
                    f = new Formula(substring, s => s.ToUpper(), s => isValid.IsMatch(s.ToUpper()));
                }
                //catch a specific exception
                catch (Exception e)
                {
                    throw e;
                }

                cellsToRecalculate = SetCellContents(name, f);
            }
            else
            {

                cellsToRecalculate = SetCellContents(name, content);
            }

            Changed = true;

            return cellsToRecalculate;
        }





        public double looker(string name)
        {
            bool tryParse;
            try
            {
                tryParse = Double.TryParse(cells[name].content.ToString(), out double result);
            }
            catch (Exception e)
            {
                tryParse = false;
            }
            if (tryParse == true)
            {
                return Double.Parse(cells[name].content.ToString());
            }
            else
            {
                throw new UndefinedVariableException("no content that is a double");
            }
        }


    }


}