using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Program
{

    public class ChatCompletion
    {
        public string id { get; set; }
        public string @object { get; set; } 
        public long created { get; set; }
        public string model { get; set; }
        public Choice[] choices { get; set; }
        public Usage usage { get; set; }
    }

    public class Choice
    {
        public int index { get; set; }
        public Message message { get; set; }
        public string finish_reason { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }





    class Program
    {

        static string apiKey = "Votre clé";

        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Program.Menu().Wait();
            }
            else
            {

                if (args[0] == "--c")
                {
                    Program.Correction().Wait();
                    Console.WriteLine("voulez vous continuer ?");
                    
                }
                else if (args[0] == "--t")
                {
                    Program.Trad().Wait();
                }
                else if (args[0] == "create")
                {
                    Program.Create().Wait();
                }
                else
                {
                    Console.WriteLine("unknown params");
                }
                string input = Console.ReadLine();
                if (input == "y" || input == "yes")
                {
                    Menu();
                }
            }
            

        }

        static async Task Menu()
        {
            Console.WriteLine("Veuillez entrer l'option de commande (exemple --t):");
            string input = Console.ReadLine();
            while (input != "n" && input != "no" && input != "non")
            {
                if (input == "--c")
                {
                    input = await Program.Correction();
                }
                else if (input == "--t")
                {
                    input = await Program.Trad();
                }
                else if (input == "create")
                {
                    input = await Program.Create();
                }
                else
                {
                    Console.WriteLine("commande inconnue");
                    input = Console.ReadLine();
                }
            }
            Console.WriteLine("voulez vous continuer ?");
            input = Console.ReadLine();
            if (input == "y" || input == "yes")
            {
                Menu();
            }
            Console.WriteLine("bye <3");

        }




        static async Task<string> Correction()
        {
            Console.WriteLine("\tEcrire le texte a ecrire :");
            string input = Console.ReadLine();
            while (input != "n" && input != "no" && input != "non")
            {
                if (input != "")
                {
                    string response = await correctAPI(input);
                    Console.WriteLine("--------------");
                    if(response != "")
                    {
                        Console.WriteLine("->"+response);
                        input = "n";

                    }
                    else
                    {
                        Console.WriteLine("error");
                        input = "";
                    }
                }
                else
                {
                    input = Console.ReadLine();
                }
            }
            return input;
        }

        static async Task<string> Trad()
        {
            Console.WriteLine("\tEcrire le texte a ecrire :");
            string input = Console.ReadLine();
            while (input != "n" && input != "no" && input != "non")
            {
                if (input != "" && input != "n" && input != "no" && input != "non")
                {
                    string response = await tradAPI(input);
                    Console.WriteLine("--------------");
                    if (response != "")
                    {
                        Console.WriteLine("->" + response);
                        input = "n";

                    }
                    else
                    {
                        Console.WriteLine("error");
                        input = "";
                    }
                }
                else
                {
                    input = Console.ReadLine();
                }
            }
            return input;
        }

        static async Task<string> Create()
        {
            Console.WriteLine("dans quelle dossier voulez vous crée votre app react ?");
            string directory = Console.ReadLine();

            if (!Directory.Exists(directory))
            {
                ProcessStartInfo directoryInfo = new ProcessStartInfo
                {
                    FileName = "mkdir",
                    Arguments = directory,
                    WorkingDirectory = "/",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (Process process = new Process { StartInfo = directoryInfo })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                }
            }
            if (!Directory.Exists(directory))
            {
                Console.WriteLine("error");
                return "";
            }
            Console.WriteLine("quelle sera le nom de ton appli ?");
            string name = Console.ReadLine();

            string command = "npx"; // Replace with the actual executable
            string arguments = "create-react-app " + name; // Replace with the actual command-line arguments

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                WorkingDirectory = directory,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }

            return "n";
        }


        static async Task<string> correctAPI(string content)
        {
            // Your OpenAI API key
            string apiEndpoint = "https://api.openai.com/v1/chat/completions";

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");


            var message = new
            {
                role = "user",
                content = "corrige moi ce texte:" + content
            };

            var messages = new[]{message};

            var requestData = new
            {
                messages,
                model = "gpt-3.5-turbo",
                max_tokens = 50
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            var temp = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(apiEndpoint, temp);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                ChatCompletion ret = JsonConvert.DeserializeObject<ChatCompletion>(responseContent);
                return ret.choices[0].message.content;
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
                return "";
            }
        }

        static async Task<string> tradAPI(string content)
        {
            // Your OpenAI API key
            string apiEndpoint = "https://api.openai.com/v1/chat/completions";

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");


            var message = new
            {
                role = "user",
                content = "Traduit moi ce texte en anglais:" + content
            };

            var messages = new[]{message};

            var requestData = new
            {
                messages,
                model = "gpt-3.5-turbo",
                max_tokens = 50
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            var temp = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(apiEndpoint, temp);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                ChatCompletion ret = JsonConvert.DeserializeObject<ChatCompletion>(responseContent);
                return ret.choices[0].message.content;
            }
            else
            {
                Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
                return "";
            }
        }

    }
}