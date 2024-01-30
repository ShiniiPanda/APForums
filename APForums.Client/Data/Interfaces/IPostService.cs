using APForums.Client.Data.DTO;
using APForums.Client.Data.DTO.PagesDTO;
using APForums.Client.Data.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Interfaces
{
    public interface IPostService
    {
        Task<BasicHttpResponse> AddCommentImpression(CommentImpression impression);
        Task<BasicHttpResponseWithData<int>> AddPost(Post post);
        Task<BasicHttpResponseWithData<Comment>> AddPostComment(Comment comment);
        Task<BasicHttpResponse> AddPostImpression(PostImpression impression);
        Task<BasicHttpResponseWithData<PaginatedList<Post>>> GetForumPosts(int forumId, int page, int sort = 0, int size = 5, string search = "");
        Task<BasicHttpResponseWithData<SinglePost>> GetSinglePost(int id);
        Task<BasicHttpResponse> RemoveCommentImpression(CommentImpression impression);
        Task<BasicHttpResponse> RemovePostImpression(PostImpression impression);
    }
}
