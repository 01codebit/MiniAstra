using System;
using Newtonsoft.Json;


namespace model
{
    public class Driver
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("birth_date")]
        public DateTime BirthDate { get; set; }
    

        public override string ToString()
        {
            return "Driver { Id: " + this.Id 
                    + ", Name: " + this.Name
                    + ", Age: " + this.Age
                    + ", BirthDate: " + this.BirthDate
                    + " }";
        }
    }
}