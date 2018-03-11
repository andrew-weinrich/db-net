using System;
using Xunit;

using Record;

namespace Record_Tests
{
    public class RecordTests
    {
        // test person data
        private static readonly string FirstName = "Bill";
        private static readonly string LastName = "Smith";
        private static readonly string FavoriteColor= "Green";
        private static readonly Gender Gender = Gender.M;
        private static readonly DateTime BirthDate = new DateTime(1960, 12, 13);


        [Fact(DisplayName = "TestConstructor")]
        public void TestConstructor()
        {
            var person = new Person(LastName, FirstName, Gender, FavoriteColor, BirthDate);

            Assert.Equal(FirstName, person.FirstName);
            Assert.Equal(LastName, person.LastName);
            Assert.Equal(Gender, person.Gender);
            Assert.Equal(FavoriteColor, person.FavoriteColor);
            Assert.Equal(BirthDate, person.Birthdate);
        }


        private static readonly string[] Delimiters = new string[] { " ", ", ", " | " };

        [Fact(DisplayName = "TestParser")]
        public void TestParser()
        {
            foreach (var delimiter in Delimiters)
            {
                var personString = String.Join(delimiter,
                                               LastName,
                                               FirstName,
                                               Gender.ToString(),
                                               FavoriteColor,
                                               BirthDate.ToString("12/13/1960"));

                var person = Person.ParsePerson(personString);

                Assert.Equal(FirstName, person.FirstName);
                Assert.Equal(LastName, person.LastName);
                Assert.Equal(Gender, person.Gender);
                Assert.Equal(FavoriteColor, person.FavoriteColor);
                Assert.Equal(BirthDate, person.Birthdate);
            }
        }

        [Fact(DisplayName = "TestAlternateDateFormat")]
        public void TestAlternateDateFormat()
        {
            var personString = String.Join(Delimiters[0],
                                           LastName,
                                           FirstName,
                                           Gender.ToString(),
                                           FavoriteColor,
                                           BirthDate.ToString("1960-12-13"));

            var person = Person.ParsePerson(personString);

            Assert.Equal(BirthDate, person.Birthdate);
        }


        [Fact(DisplayName = "TestDateParseFailure")]
        public void TestDateParseFailure()
        {
            var personString = String.Join(Delimiters[0],
                                           LastName,
                                           FirstName,
                                           Gender.ToString(),
                                           FavoriteColor,
                                           "foo");

            var ex = Assert.Throws<FormatException>(() => Person.ParsePerson(personString));
        }

        [Fact(DisplayName = "TestGenderParseFailure")]
        public void TestGenderParseFailure()
        {
            var personString = String.Join(Delimiters[0],
                                           LastName,
                                           FirstName,
                                           "X",
                                           FavoriteColor,
                                           BirthDate.ToString("MM/dd/yyyy"));

            var ex = Assert.Throws<ArgumentException>(() => Person.ParsePerson(personString));
        }

        [Fact(DisplayName = "TestDelimiterParseFailure")]
        public void TestDelimiterParseFailure()
        {
            var personString = String.Join(":",
                                           LastName,
                                           FirstName,
                                           Gender.ToString(),
                                           FavoriteColor,
                                           BirthDate.ToString("MM/dd/yyyy"));

            var ex = Assert.Throws<Exception>(() => Person.ParsePerson(personString));
        }

        [Fact(DisplayName = "TestFieldCountParseFailure")]
        public void TestFieldCountParseFailure()
        {
            var personString = String.Join(Delimiters[0],
                                           LastName,
                                           FirstName,
                                           Gender.ToString(),
                                           FavoriteColor,
                                           BirthDate.ToString("MM/dd/yyyy"),
                                           "foo");

            var ex = Assert.Throws<Exception>(() => Person.ParsePerson(personString));
        }
}
}
