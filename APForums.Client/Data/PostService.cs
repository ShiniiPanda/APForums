using APForums.Client.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APForums.Client.Data.Structures;
using APForums.Client.Data.DTO.PagesDTO;
using APForums.Client.Data.DTO;

namespace APForums.Client.Data
{
    public class PostService : IPostService
    {
        HttpClient _httpClient;
        private readonly ILoginService _loginService;

        public PostService(ILoginService loginService)
        {
            _httpClient = new HttpClient();
            _loginService = loginService;
        }

        public async Task<BasicHttpResponseWithData<SinglePost>> GetSinglePost(int id)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<SinglePost>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_PAGES}/Post/{id}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<SinglePost>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_PAGES}/Post/{id}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var postResponse = JsonSerializer.Deserialize<SinglePost>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<SinglePost>
                {
                    Data = postResponse,
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new BasicHttpResponseWithData<SinglePost>
                    {
                        Status = HttpStatusCode.Forbidden,
                        Error = "Unable to find post information!"
                    };
                }
                return new BasicHttpResponseWithData<SinglePost>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request, please try again!"
                };
            }
        }

        public async Task<BasicHttpResponseWithData<PaginatedList<Post>>> GetForumPosts(int forumId, int page, int sort = 0,  int size = 5, string search = "")
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<PaginatedList<Post>>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_POSTS}/Forum/{forumId}?sort={sort}&page={page}&size={size}&search={search}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<PaginatedList<Post>>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.GetAsync($"{ServicesApiRoutes.API_POSTS}/Forum/{forumId}?sort={sort}&page={page}&size={size}&search={search}");

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return new BasicHttpResponseWithData<PaginatedList<Post>>
                {
                    Data = JsonSerializer.Deserialize<PaginatedList<Post>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new BasicHttpResponseWithData<PaginatedList<Post>>
                    {
                        Status = HttpStatusCode.Forbidden,
                        Error = "User does not have permission to view those posts!"
                    };
                }
                return new BasicHttpResponseWithData<PaginatedList<Post>>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponse> AddPostImpression(PostImpression impression)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(impression);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}/Impression/Post/Add", content);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}/Impression/Post/Add", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.Conflict,
                        Error = "User has already posted this impression for this post!"
                    };
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "NOT FOUND!!!"
                    };
                }
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Failed to unsubscribe to forum"
                };
            }
        }

        public async Task<BasicHttpResponse> RemovePostImpression(PostImpression impression)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(impression);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}/Impression/Post/Remove", content);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}/Impression/Post/Remove", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.InternalServerError,
                        Error = "Impression does not exist!"
                    };
                }
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Failed to unsubscribe to forum"
                };
            }
        }

        public async Task<BasicHttpResponseWithData<int>> AddPost(Post post)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<int>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(post);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}", content);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<int>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var newPostId = int.Parse(result);
                return new BasicHttpResponseWithData<int>
                {
                    Data = newPostId,
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new BasicHttpResponseWithData<int>
                    {
                        Status = HttpStatusCode.Forbidden,
                        Error = "User does not have permission to make a post in this forum!"
                    };
                }
                return new BasicHttpResponseWithData<int>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponseWithData<Comment>> AddPostComment(Comment comment)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponseWithData<Comment>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}/Comment/Add", content);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponseWithData<Comment>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}/Comment/Add", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var resultComment = JsonSerializer.Deserialize<Comment>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new BasicHttpResponseWithData<Comment>
                {
                    Data = resultComment,
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new BasicHttpResponseWithData<Comment>
                    {
                        Status = HttpStatusCode.Forbidden,
                        Error = "User does not have permission to post a comment in this forum!"
                    };
                }
                return new BasicHttpResponseWithData<Comment>
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponse> AddCommentImpression(CommentImpression impression)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(impression);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}/Impression/Comment/Add", content);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}/Impression/Comment/Add", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.Conflict,
                        Error = "User has already posted this impression for this comment!"
                    };
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "NOT FOUND!!!"
                    };
                }
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

        public async Task<BasicHttpResponse> RemoveCommentImpression(CommentImpression impression)
        {
            if (Settings.authInfo == null)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.Unauthorized,
                    Error = "User is not authenticated"
                };
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
            var json = JsonSerializer.Serialize(impression);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}/Impression/Comment/Remove", content);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuth = await _loginService.Refresh(new LoginResponse
                {
                    AccessToken = Settings.authInfo.AccessToken,
                    RefreshToken = Settings.authInfo.RefreshToken
                });

                if (newAuth.Status != AuthStatus.Success)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Error = "User is not authenticated"
                    };
                }

                await _loginService.SetAuthInfo(newAuth.AccessToken, newAuth.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.authInfo.AccessToken);
                response = await _httpClient.PostAsync($"{ServicesApiRoutes.API_POSTS}/Impression/Comment/Remove", content);

            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.OK,
                };
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new BasicHttpResponse
                    {
                        Status = HttpStatusCode.InternalServerError,
                        Error = "Impression does not exist!"
                    };
                }
                return new BasicHttpResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "Unable to process your request!"
                };
            }
        }

    }
}
