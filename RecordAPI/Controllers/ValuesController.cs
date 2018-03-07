using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Record;


namespace RecordAPI.Controllers
{
    [Route("api/records")]
    public class RecordsController : Controller
    {
        private Dictionary<string, Person> records = new Dictionary<string, Person>();

        // GET api/values
        [HttpGet("{sortMethod}")]
        public string Get(string sortMethod)
        {
            if (!Person.SortMethods.ContainsKey(sortMethod))
                return "invalid sort method: " + sortMethod;

            var orderedRecords = new List<Person>(records.Values);
            orderedRecords.Sort(Person.SortMethods[sortMethod]);

            string output = "";
            foreach (var person in orderedRecords)
            {
                output += String.Format("{0} {1} {2} {3} {4:MM/dd/yyyy}",
                                        person.LastName,
                                        person.FirstName,
                                        person.FavoriteColor,
                                        person.Gender,
                                        person.Birthdate);
            }

            return output;
        }





        // POST api/records
        [HttpPost]
        public void PostRecord([FromBody]string value)
        {
            throw new Exception("Input: " + value);
            //var person = Person.ParsePerson(value);
            //records[person.Key] = person;
        }
    }
}
