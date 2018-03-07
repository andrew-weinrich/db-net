using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;

using Record;


namespace RecordAPI.Controllers
{
    [Route("api/records")]
    public class RecordsController : Controller
    {
        private static Dictionary<string, Person> records = new Dictionary<string, Person>();

        // GET api/values
        [HttpGet("{sortMethod}")]
        public string Get(string sortMethod)
        {
            if (!Person.SortMethods.ContainsKey(sortMethod))
                return "invalid sort method: " + sortMethod;

            var orderedRecords = new List<Person>(records.Values);
            orderedRecords.Sort(Person.SortMethods[sortMethod]);

            // for a program this simple, generate JSON text manually instead of using serializers
            var jsonList = new List<string>();
            foreach (var person in orderedRecords)
            {
                jsonList.Add("{" +
                             "\"firstName\":\"" + HttpUtility.JavaScriptStringEncode(person.FirstName) + "\"," +
                             "\"lastName\":\"" + HttpUtility.JavaScriptStringEncode(person.LastName) + "\"," +
                             "\"gender\":\"" + HttpUtility.JavaScriptStringEncode(person.Gender.ToString()) + "\"," +
                             "\"favoriteColor\":\"" + HttpUtility.JavaScriptStringEncode(person.FavoriteColor) + "\"," +
                             "\"birthDate\":\"" + HttpUtility.JavaScriptStringEncode(person.Birthdate.ToString("MM/dd/yyyy")) + "\"" +
                             "}");
            }

            return "[" + String.Join(",\n", jsonList) + "]";
        }


        // POST api/records
        [HttpPost]
        public void PostRecord()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var input = reader.ReadToEnd();
                var person = Person.ParsePerson(input);
                records.Add(person.Key, person);
            }
        }
    }
}
