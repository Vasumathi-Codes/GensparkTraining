using System;

class Post
{
    public string Caption { get; set; }
    public int Likes { get; set; }
}

class Task_01_with_post_class
{
    static int ReadPositiveInt(string prompt)
    {
        int result;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out result) && result > 0)
                return result;
            Console.WriteLine("Invalid input. Please enter a positive integer.");
        }
    }

    static int ReadNonNegativeInt(string prompt)
    {
        int result;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out result) && result >= 0)
                return result;
            Console.WriteLine("Invalid input. Please enter a non-negative integer.");
        }
    }

    static string ReadNonEmptyString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                return input;
            Console.WriteLine("Input cannot be empty.");
        }
    }

    public void Run()
    {
        int userCount = ReadPositiveInt("Enter number of users: ");
        Post[][] postsByUser = new Post[userCount][];

        for (int i = 0; i < userCount; i++)
        {
            int postCount = ReadNonNegativeInt($"\nUser {i + 1}: How many posts? ");
            postsByUser[i] = new Post[postCount];

            for (int j = 0; j < postCount; j++)
            {
                string caption = ReadNonEmptyString($"Enter caption for post {j + 1}: ");
                int likes = ReadNonNegativeInt("Enter likes: ");
                postsByUser[i][j] = new Post
                {
                    Caption = caption,
                    Likes = likes
                };
            }
        }

        Console.WriteLine("\n--- Displaying Instagram Posts ---");

        for (int i = 0; i < postsByUser.Length; i++)
        {
            Console.WriteLine($"User {i + 1}:");
            for (int j = 0; j < postsByUser[i].Length; j++)
            {
                Console.WriteLine($"Post {j + 1} - Caption: {postsByUser[i][j].Caption} | Likes: {postsByUser[i][j].Likes}");
            }
            Console.WriteLine();
        }
    }

    ////Done with object type to store string and int together
    //public void Run()
    //{
    //    int userCount = ReadNonNegativeInt("Enter number of users: ");
    //    while (userCount == 0)
    //    {
    //        Console.WriteLine("There must be at least one user.");
    //        userCount = ReadNonNegativeInt("Enter number of users: ");
    //    }

    //    object[][] postsByUser = new object[userCount][];

    //    for (int i = 0; i < userCount; i++)
    //    {
    //        int postCount = ReadNonNegativeInt($"\nUser {i + 1}: How many posts? ");
    //        postsByUser[i] = new object[postCount * 2]; // twice size for caption and likes

    //        for (int j = 0; j < postCount; j++)
    //        {
    //            string caption = ReadNonEmptyString($"Enter caption for post {j + 1}: ");
    //            int likes = ReadNonNegativeInt("Enter likes: ");
    //            postsByUser[i][j * 2] = caption;       // even index: caption
    //            postsByUser[i][j * 2 + 1] = likes;     // odd index: likes
    //        }
    //    }

    //    Console.WriteLine("\n--- Displaying Instagram Posts ---");

    //    for (int i = 0; i < postsByUser.Length; i++)
    //    {
    //        Console.WriteLine($"User {i + 1}:");
    //        for (int j = 0; j < postsByUser[i].Length; j += 2)
    //        {
    //            string caption = (string)postsByUser[i][j];
    //            int likes = (int)postsByUser[i][j + 1];
    //            Console.WriteLine($"Post {(j / 2) + 1} - Caption: {caption} | Likes: {likes}");
    //        }
    //        Console.WriteLine();
    //    }
    //}
}
