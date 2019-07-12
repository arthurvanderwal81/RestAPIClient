using System;
using System.Threading.Tasks;

namespace RestAPIClient
{
    /// <summary>
    /// Implementation example of RestClient
    /// </summary>
    public class TypicodeRestClient : RestClient
    {
        #region Typicode specific classes

        public class User
        {
            public class Address
            {
                public class Geo
                {
                    public string lat { get; set; }
                    public string lng { get; set; }
                }

                public string street { get; set; }
                public string suite { get; set; }
                public string city { get; set; }
                public string zipcode { get; set; }
                public Geo geo { get; set; }
            }

            public class Company
            {
                public string name { get; set; }
                public string catchPhrase { get; set; }
                public string bs { get; set; }
            }

            public int id { get; set; }
            public string name { get; set; }
            public string username { get; set; }
            public string email { get; set; }
            public Address address { get; set; }
            public string phone { get; set; }
            public string website { get; set; }
            public Company company { get; set; }
        }

        public class Post
        {
            public int id { get; set; }
            public int userId { get; set; }
            public string title { get; set; }
            public string body { get; set; }
        }

        #endregion

        #region Typicode resource names

        private const string _usersResourceName = "users";
        private const string _postsResourceName = "posts";

        #endregion

        #region Constructor

        public TypicodeRestClient() : base("https://jsonplaceholder.typicode.com/")
        {
        }

        #endregion

        #region Users

        /// <summary>
        /// Gets specific User with 'id'
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns></returns>
        public async Task<User> GetUser(int id)
        {
            return await GetObjectAsync<User>(_usersResourceName, id);
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns></returns>
        public async Task<User[]> GetUsers()
        {
            return await GetObjectsAsync<User>(_usersResourceName);
        }

        /// <summary>
        /// Creates a User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> CreateUser(User user)
        {
            return await CreateObjectAsync<User>(_usersResourceName, user);
        }

        #endregion

        #region Posts

        /// <summary>
        /// Gets post with 'id'
        /// </summary>
        /// <param name="id">Post ID</param>
        /// <returns></returns>
        public async Task<Post> GetPost(int id)
        {
            return await GetObjectAsync<Post>(_postsResourceName, id);
        }

        /// <summary>
        /// Gets all Posts for specific userId (if specified), otherwise returns all Posts
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Post[]> GetPosts(int? userId)
        {
            if (userId != null)
            {
                return await GetObjectsAsync<Post>(_postsResourceName, "userId", userId.Value);
            }

            return await GetObjectsAsync<Post>(_postsResourceName);
        }

        /// <summary>
        /// Creates a Post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<Post> CreatePost(Post post)
        {
            return await CreateObjectAsync<Post>(_postsResourceName, post);
        }

        /// <summary>
        /// Updates a Post
        /// </summary>
        /// <param name="post">The updated post, note that the id needs to be specifed</param>
        /// <returns></returns>
        public async Task<Post> UpdatePost(Post post)
        {
            if (post.id == 0)
            {
                throw new ArgumentException("id of post needs to be specified");
            }

            return await UpdateObjectAsync<Post>(_postsResourceName, post.id, post);
        }

        /// <summary>
        /// Deletes a Post specified by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeletePost(int id)
        {
            return await DeleteObject(_postsResourceName, id);
        }

        #endregion
    }
}