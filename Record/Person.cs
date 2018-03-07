using System;
using System.Collections.Generic;

namespace Record
{
    public enum Gender
    {
        F = 0,
        M = 1
    }

    public class Person
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Gender Gender { get; private set; }

        public string FavoriteColor { get; private set; }

        public DateTime Birthdate { get; private set; }

        public string Key
        {
            get
            {
                return LastName + "-" + FirstName;
            }
        }

        public override string ToString()
        {
            return String.Format("lastName:'{0}'; firstName:'{1}'; gender:'{2}'; favoriteColor:'{3}'; birthDate:'{4}'",
                                 LastName, FirstName, Gender, FavoriteColor, Birthdate);
        }

        public Person(string lastName, string firstName, Gender gender, string favoriteColor, DateTime birthDate)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.FavoriteColor = favoriteColor;
            this.Birthdate = birthDate;
        }


        /// <summary>
        /// Methods for sorting collections of Persons
        /// </summary>
        public static readonly Dictionary<string, Comparer<Person>> SortMethods = new Dictionary<string, Comparer<Person>>
        {
            { "birthdate", Comparer<Person>.Create((p1, p2) => p1.Birthdate.CompareTo(p2.Birthdate)) },
            { "name", Comparer<Person>.Create((p1, p2) => p2.LastName.CompareTo(p1.LastName)) },
            { "gender", Comparer<Person>.Create((p1, p2) => {
                    var genderComparison = p1.Birthdate.CompareTo(p2.Birthdate);
                    if (genderComparison != 0)
                        return genderComparison;
                    else
                        return p1.LastName.CompareTo(p2.LastName);
                })
            }
        };

        /// <summary>
        /// Parses a string to a Person object, with a specified field delimiter. Requires fields in the format
        /// "lastName firstName gender favoriteColor birthdate"
        /// </summary>
        /// <returns>The person.</returns>
        /// <param name="line">Input line</param>
        /// <param name="delimiter">Delimiter of fields</param>
        public static Person ParsePerson(string line, string delimiter)
        {
            var components = line.Split(new string[]{ delimiter }, System.StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 5)
                throw new Exception("String to parse did not have five components with delimiter '" + delimiter + "': " + line);
            var parsedDate = DateTime.Parse(components[4]);
            var gender = Enum.Parse(typeof(Gender), components[2]);

            return new Person(components[0], components[1], gender, components[3], parsedDate);
        }
    }
}
