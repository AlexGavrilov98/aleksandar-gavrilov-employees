using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SirmaProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>();

            Console.WriteLine("Please enter input file destination:");
            string destinationFile = Console.ReadLine();
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(destinationFile);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message + "\nPlease try again:");
                destinationFile = Console.ReadLine();
                sr = new StreamReader(destinationFile);
            }
            
            string line = sr.ReadLine();
            
            while(line != null)
            {
                string[] input = line.Split(", ");

                string empId = input[0];
                string projectId = input[1];
                DateTime start = DateTime.Parse(input[2]);
                DateTime end;
                if (input[3].ToLower() == "null")
                    end = DateTime.Now;
                else
                    end = DateTime.Parse(input[3]);

                Employee currentEmployee = new Employee(empId);
                currentEmployee.AddProject(projectId, new DateTime[] { start, end });
                employees.Add(currentEmployee);

                line = sr.ReadLine();
            }

            Employee empl1;
            Employee empl2;
            Dictionary<KeyValuePair<int, string>, HashSet<Employee>> pairs 
                = new Dictionary<KeyValuePair<int, string>, HashSet<Employee>>();

            //fills in all the pairs
            for(int i = 0; i < employees.Count - 1; i++)
            {
                for(int j = i+1; j < employees.Count; j++)
                {
                    empl1 = employees[i];
                    empl2 = employees[j];
                    foreach(var item in empl1.Projects)
                    {
                        foreach(var item2 in empl2.Projects)
                        {
                            if(item.Key == item2.Key)
                            {
                                DateTime startA = item.Value[0];
                                DateTime endA = item.Value[1];
                                DateTime startB = item2.Value[0];
                                DateTime endB = item2.Value[1];
                                if(IsOverlap(startA, endA, startB, endB))
                                {
                                    int overlap = FindOverlap(startA, endA, startB, endB);
                                    KeyValuePair<int, string> project = new KeyValuePair<int, string>(overlap, item.Key);
                                    HashSet<Employee> currentEmployees = new HashSet<Employee>
                                    {
                                        empl1,
                                        empl2
                                    };
                                    try
                                    {
                                        pairs.Add(project, currentEmployees);
                                    }
                                    catch (ArgumentException)
                                    {
                                        continue;
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }

            Dictionary<int, string> pairsKeys = new Dictionary<int, string>();
            //fills in all the pairsKeys
            foreach(var item in pairs)
            {
                try
                {
                    pairsKeys.Add(item.Key.Key, item.Key.Value);
                }
                catch (ArgumentException)
                {
                    continue;
                }
                
            }
            
            int maxValue = pairsKeys.Keys.Max();
            KeyValuePair<int, string> projectAndValue = new KeyValuePair<int, string>(maxValue, pairsKeys[maxValue]);

            Console.Write("\nEmployees: ");
            foreach (var item in pairs[projectAndValue])
            {
                Console.Write(item.Id + " ");
            }
            Console.WriteLine("\nProject ID: " + projectAndValue.Value
                                + " Overlap days: " + projectAndValue.Key);
        }

        public static int FindOverlap(DateTime startA, DateTime endA,
                                        DateTime startB, DateTime endB)
        {
            int overlapInDays = 0;

            if ((startA >= startB) && (endA <= endB))
                overlapInDays = (endA - startA).Days;

            else if ((startA <= startB) && (endA >= endB))
                overlapInDays = (endB - startB).Days;

            else if ((startA >= startB) && (endA >= endB))
                overlapInDays = (endB - startA).Days;

            else if ((startA <= startB) && (endA <= endB))
                overlapInDays = (endA - endB).Days;
            
            return overlapInDays;
        }
        public static bool IsOverlap(DateTime startA, DateTime endA,
                                        DateTime startB, DateTime endB)
        {
            return (startA <= endB) && (startB <= endA);
        }
    }
}
