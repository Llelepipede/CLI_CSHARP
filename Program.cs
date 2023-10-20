namespace Program
{

    //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
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Program.Menu();
            }
            else
            {

                if (args[0] == "--c")
                {
                    Program.Correction();
                }
                else if (args[0] == "--t")
                {
                    Program.Trad();
                }
                else if (args[0] == "create")
                {
                    Program.Create();
                }
                else
                {
                    Console.WriteLine("unknown params");
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
                    input = Program.Trad();
                }
                else if (input == "create")
                {
                    input = Program.Create();
                }
                else
                {
                    input = Console.ReadLine();
                }
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
                    Console.WriteLine("heyooo");
                    string response = await CallOpenAI(input);
                    Console.WriteLine("ok");
                    if(response != "")
                    {
                        Console.WriteLine(response);

                    }else
                    {
                        Console.WriteLine("error");
                    }
                }
                else
                {
                    input = Console.ReadLine();
                }
            }
            return input;
        }

        static string Trad()
        {
            return "";
        }

        static string Create()
        {
            return "";
        }


        static async Task<string> CallOpenAI(string content)
        {
            // Your OpenAI API key
            string apiKey = "";

            // The endpoint URL for the GPT-3 API
            string apiUrl = "https://api.openai.com/v1/engines/davinci/completions";
            Console.WriteLine("koki");

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("Prout");
                // Set the authorization header with your API key
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                Console.WriteLine("Prout");
                // Create a request message
                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                Console.WriteLine("Prout");
                // Set the content of the request (for example, a prompt for the chat model)
                request.Content = new StringContent("{\"prompt\": \" "+  content + " \" corrige moi ce texte }");
                Console.WriteLine("Prout");
                // Send the request and get the response
                var response = await client.SendAsync(request);
                Console.WriteLine("Prout");


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    return responseContent;
                }
                else
                {
                    Console.WriteLine($"API Request failed with status code: {response.StatusCode}");
                    return "";
                }

            }
        }

    }
}