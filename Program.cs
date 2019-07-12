using System;
using System.Diagnostics;

namespace RestAPIClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TypicodeRestClient typicodeRestClient = new TypicodeRestClient();

            TypicodeRestClient.Post singlePost = typicodeRestClient.GetPost(1).Result;
            Console.WriteLine("Post with id 1: {0}", singlePost.body);

            TypicodeRestClient.Post newPost = new TypicodeRestClient.Post()
            {
                userId = 123456,
                title = "NEW TITLE",
                body = "NEW BODY"
            };

            newPost = typicodeRestClient.CreatePost(newPost).Result;

            Debug.Assert(newPost.id != 0);

            Console.WriteLine("New post created with id: {0}", newPost.id);

            TypicodeRestClient.User newUser = new TypicodeRestClient.User()
            {
                username = "TEST USER",
                website = "https://www.google.com/",
                address = new TypicodeRestClient.User.Address()
                {
                    city = "San Francisco",
                    zipcode = "90210"
                }
            };

            newUser = typicodeRestClient.CreateUser(newUser).Result;

            Debug.Assert(newUser.id != 0);

            Console.WriteLine("New user created with id: {0}", newUser.id);

            TypicodeRestClient.User[] users = typicodeRestClient.GetUsers().Result;
            Console.WriteLine("Users found: {0}", users.Length);

            bool firstUser = true;

            foreach (TypicodeRestClient.User user in users)
            {
                TypicodeRestClient.Post[] posts = typicodeRestClient.GetPosts(user.id).Result;

                Console.WriteLine("{0} posts found for user {1}", posts.Length, user.username);

                if (firstUser)
                {
                    foreach (TypicodeRestClient.Post post in posts)
                    {
                        post.body += " - updated";
                        TypicodeRestClient.Post updatedPost = typicodeRestClient.UpdatePost(post).Result;

                        Debug.Assert(updatedPost.body == post.body);

                        Console.WriteLine(">>> Updated post {0} success: {1}", post.id, updatedPost.body == post.body);

                        bool deleteResult = typicodeRestClient.DeletePost(post.id).Result;

                        Console.WriteLine(">>> Delete post {0} success: {1}", post.id, deleteResult);
                    }

                    firstUser = false;
                }
            }
        }
    }
}