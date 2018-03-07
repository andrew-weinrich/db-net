using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Record;

namespace ReadRecords
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
                throw new Exception("Invalid number of parameters: should be fileName sortType (got '" + 
                                    String.Join("' '", args) + "')");

            var records = new Dictionary<string, Person>();

            var fileName = args[0];
            var sortType = args[1];

            using (var reader = new System.IO.StreamReader(fileName))
            {
                var line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    var person = Person.ParsePerson(line);
                    records[person.Key] = person;
                }
            }

            var orderedRecords = new List<Person>(records.Values);
            orderedRecords.Sort(Person.SortMethods[args[1]]);

            foreach (var person in orderedRecords)
            {
                Console.WriteLine("{0} {1} {2} {3} {4:MM/dd/yyyy}",
                                  person.LastName,
                                  person.FirstName,
                                  person.FavoriteColor,
                                  person.Gender,
                                  person.Birthdate);
            }
        }
    }
}
