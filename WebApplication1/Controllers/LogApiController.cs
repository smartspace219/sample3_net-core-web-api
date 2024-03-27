using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;
using Npgsql;
using System.Security.AccessControl;
using StackExchange.Redis;
namespace WebApplication1.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api")]
    public class LogApiController : ControllerBase
    {
        private const string CONNECTION_STRING = "Host=localhost;Port=5432; Username=postgres; Password=123; Database=log_database";
        private ConfigurationOptions REDIS_OPTION = new ConfigurationOptions
        {
            EndPoints = { "127.0.0.1:6379" },
            AbortOnConnectFail = false,
            Ssl = false,
            Password = ""
        };
        private readonly ILogger<LogApiController> _logger;
        private const int INSERT_SUCCESSFULLY = 100;
        private const int INSERT_ERROR = 101;
        private const int INVALID_DEVICE = 102;
        private const int SERIAL_ADD = 99;
        
        public LogApiController(ILogger<LogApiController> logger)
        {
            _logger = logger;
        }

        [HttpPost("v1/log/post")]
        public async Task<IActionResult> PostLogData([FromBody] object receive)
        {
            try
            {
                Console.WriteLine("body:" + receive);
                await using var connection = new NpgsqlConnection(CONNECTION_STRING);
                await connection.OpenAsync();
                
                //connect redis
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(REDIS_OPTION);
                IDatabase redisDB = redis.GetDatabase();

                String[] logLines = Regex.Split(receive + "", "\r\n|\r|\n");
                jsonData json = JsonSerializer.Deserialize<jsonData>(logLines[0]);
                
                string serial = json.serial;
                string serial_value = "___";
                int sequence_number = json.sequence;
                if (serial.Length > 4) serial = serial.Substring(0, 4);
                bool isExist= await redisDB.KeyExistsAsync(serial);
                if (isExist)
                {
                    
                    // Create a temporary file to store the log data
                    string tempFilePath = Path.GetTempFileName();

                    FileSecurity fileSecurity = FileSystemAclExtensions.GetAccessControl(new FileInfo(tempFilePath));

                    // Add read permission for everyone
                    fileSecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.Read, AccessControlType.Allow));

                    // Set the modified access control back to the file
                    FileSystemAclExtensions.SetAccessControl(new FileInfo(tempFilePath), fileSecurity);

                    // Write the log data to the temporary file in the format required by PostgreSQL
                    using (StreamWriter writer = new StreamWriter(tempFilePath))
                    {
                        foreach (string line in logLines.Skip(1)) // Skip the first line since it's already processed
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                string[] values = line.Split(new char[] { ' ', ';' });
                                if (serial_value.Equals("___")) serial_value = values[0];
                                string created = localtime(long.Parse(values[0]));
                                string process = values[1];
                                string ev = values[2];
                                string short_1 = values[3];
                                string short_2 = values[4];
                                string information = values[5] + values[6];
                                if (process.Length > 3) process = process.Substring(0, 3);
                                if (ev.Length > 3) ev = ev.Substring(0, 4);
                                if (short_1.Length > 10) short_1 = short_1.Substring(0, 10);
                                if (short_2.Length > 10) short_2 = short_2.Substring(0, 10);
                                writer.WriteLine($"{serial}\t{sequence_number}\t{created}\t{process}\t{ev}\t{short_1}\t{short_2}\t{information}");
                            }
                        }
                    }

                    // Execute COPY statement to bulk insert data from the temporary file
                    await using var cmd = new NpgsqlCommand($"COPY log_data(serial, sequence_number , created, process, event, short_1, short_2, information) FROM '{tempFilePath}' DELIMITER E'\\t' CSV", connection);
                    await cmd.ExecuteNonQueryAsync();
                    redisDB.StringSet(serial, serial_value);
                    // Delete the temporary file after COPY operation
                    System.IO.File.Delete(tempFilePath);
                }
                else {
                    return Ok(INVALID_DEVICE);
                }

            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return Ok(INSERT_ERROR);
            }
            return Ok(INSERT_SUCCESSFULLY);
        }
        [HttpPost("v1/log/get-time/{serial}")]
        public async Task<IActionResult> getTime([FromRoute] string serial) {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(REDIS_OPTION);
            IDatabase redisDB = redis.GetDatabase();
            bool isExist = await redisDB.KeyExistsAsync(serial);
            string value = "0";
            if (isExist)
                value = redisDB.StringGet(serial).ToString();
            return Ok(value);
        }
        [HttpPost("v1/log/add-serial/{serial}")]
        public IActionResult InsertSerial([FromRoute] string serial)
        {
            ConnectionMultiplexer redis =  ConnectionMultiplexer.Connect(REDIS_OPTION);
            IDatabase redisDB = redis.GetDatabase();
            redisDB.StringSet(serial, "___");
            return Ok(SERIAL_ADD);
        }
        public static string localtime(long unixTime)
        {
            // Convert Unix time to DateTime
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
            string time = dateTimeOffset.LocalDateTime.ToString("HH:mm:ss");

            return time;
        }
    }
    public class jsonData
    {
        public string serial { get; set; }
        public int sequence { get; set; }

    }
    public class receiveData
    {
        public string data { get; set; }
    }
}