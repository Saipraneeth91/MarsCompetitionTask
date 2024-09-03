using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.IO;
using Mars_Competition.Models;

namespace Mars_Competition.Helpers
{
        public static class JSONHelper
        {
            public static T LoadData<T>(string filePath)
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"The file '{filePath}' was not found.");

                string jsonContent = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(jsonContent);
            }
        }
    }

