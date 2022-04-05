using Posts;
using System.Text.Json;

namespace GetPosts
{
    class Programm
    {
        private const string file = "Posts.txt";
        private static readonly HttpClient client = new();

        static async Task Main()
        {
            CheckFile(file);

            for (int postId = 4; postId < 14; postId++)
            {
                Console.WriteLine($"Получение поста №{postId}");
                var post = await GetPosts(postId);
                await File.AppendAllTextAsync(file, post + Environment.NewLine);
            }
            Console.WriteLine($"Сохранено в файле {file}");
            Console.ReadLine();
        }

        private static void CheckFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private static async Task<Post?> GetPosts(int postId)
        {
            var count = 1;

            while (true)
            {
                try
                {
                    var responseMessage = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{postId}");
                    await using var responseBody = await responseMessage.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var value = JsonSerializer.DeserializeAsync<Post>(responseBody, options);
                    return value.IsCompleted
                        ? value.Result
                        : throw new Exception("Получены неверные данные!");
                }
                catch (Exception e)
                {
                    count++;
                    if (count <= 3)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine($"Сбой при получении данных!\nПопытка №{count}");
                        await Task.Delay(1000);
                    }
                    else
                    {
                        Console.WriteLine("Не удалось получить данные...");
                        return null;
                    }
                }
            }
        }
    }
}