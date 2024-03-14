using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogDataController : ControllerBase
    {
        private readonly string _connectionString = "Host=localhost;Port=5432;Database=your_database;Username=your_username;Password=your_password";
        [HttpPost]
        //public async Task<IActionResult> PostLogData()
        //{
        //    try
        //    {
        //        //using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
        //        //{
        //        //    string body = stream.ReadToEnd();
        //        //    Console.WriteLine(body);
        //        //    // body = "param=somevalue&param2=someothervalue"
        //        //}
                
        //        //var requestData = JsonConvert.DeserializeObject<LogDataRequest>(requestBody);

        //        //await InsertLogData(requestData);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        //private async Task InsertLogData(LogDataRequest requestData)
        //{
        //    using var connection = new NpgsqlConnection(_connectionString);
        //    await connection.OpenAsync();

        //    foreach (var logEntry in requestData.LogEntries)
        //    {
        //        var parsedData = ParseLogEntry(logEntry);
        //        await InsertLogEntryIntoDatabase(connection, parsedData);
        //    }
        //}

        //private LogEntry ParseLogEntry(string logEntry)
        //{
        //    // Assuming the format is fixed and consistent
        //    var parts = logEntry.Split(';');
        //    var timestamp = DateTime.Parse(parts[0]);
        //    var component = parts[1].Trim();
        //    var eventCode = parts[2].Trim();
        //    var num1 = int.Parse(parts[3].Trim());
        //    var num2 = int.Parse(parts[4].Trim());
        //    var text = parts[5].Trim();

        //    return new LogEntry
        //    {
        //        Timestamp = timestamp,
        //        Component = component,
        //        EventCode = eventCode,
        //        Num1 = num1,
        //        Num2 = num2,
        //        Text = text
        //    };
        //}

        //private async Task InsertLogEntryIntoDatabase(NpgsqlConnection connection, LogEntry logEntry)
        //{
        //    using var cmd = new NpgsqlCommand(
        //        "INSERT INTO log_data (created, component, event, num_1, num_2, text) VALUES (@created, @component, @event, @num1, @num2, @text)",
        //        connection);

        //    cmd.Parameters.AddWithValue("created", logEntry.Timestamp);
        //    cmd.Parameters.AddWithValue("component", logEntry.Component);
        //    cmd.Parameters.AddWithValue("event", logEntry.EventCode);
        //    cmd.Parameters.AddWithValue("num1", logEntry.Num1);
        //    cmd.Parameters.AddWithValue("num2", logEntry.Num2);
        //    cmd.Parameters.AddWithValue("text", logEntry.Text);

        //    await cmd.ExecuteNonQueryAsync();
        //}
        // GET: api/<LogDataController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LogDataController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LogDataController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LogDataController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LogDataController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
