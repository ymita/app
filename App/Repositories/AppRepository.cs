using App.Models;
using App.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;
using System.Security.Claims;
using System.Text;

namespace App.Repositories
{
    public class AppRepository : IAppRepository
    {
        public IAppDbContext _appDbContext { get; set; }
        public IIdentityRepository _identityRepository { get; set; }

        public AppRepository(IAppDbContext appDbContext, IIdentityRepository identityRepository)
        {
            this._appDbContext = appDbContext;
            this._identityRepository = identityRepository;
        }

        public async Task<List<Post>> getPostsByUserAsync(string userName)
        {
            List<Post> posts = null;

            var user = this._identityRepository.getUserByName(userName);
            
            if (user != null)
            {
                posts = await this._appDbContext.Posts.Where(p => p.OwnerId == user.Id).ToListAsync();
            }
            
            return posts.OrderByDescending(x => x.UpdatedDate).ToList();
        }

        public async Task<Post> getPostAsync(int id, string userName = null)
        {
            Post post;
            if(userName == null)
            {
                post = await this._appDbContext.Posts.Where(p => p.Id == id).FirstOrDefaultAsync();
            } else
            {
                var owner = _identityRepository.getUserByName(userName);
                
                if (owner == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                var ownerId = owner.Id;
                post = await this._appDbContext.Posts.Where(p => p.OwnerId == ownerId && p.Id == id).FirstOrDefaultAsync();
            }
            return post;
        }

        public async Task<List<Post>> getAllPostsAsync(bool includesDraft = false)
        {
            List<Post> posts;
            if (includesDraft == true)
            {
                posts = await this._appDbContext.Posts.ToListAsync();
            } else {
                posts = await this._appDbContext.Posts.Where(p => p.IsDraft == includesDraft).ToListAsync();
            }
            return posts.OrderByDescending(x=>x.UpdatedDate).ToList();
        }

        public async Task<List<PostTagCrossReference>> getPostsTagsReferencesAsync(string userId)
        {
            string sql = "SELECT Id, PostId, TagId from dbo.Posts_Tags_XREF WHERE PostId in (SELECT Id from dbo.Posts WHERE OwnerId = '" + userId + "')";
            var visiblePostsTagsCrossReferences = await this._appDbContext.PostsTagsCrossReferences.FromSqlRaw(sql).ToListAsync();
            return visiblePostsTagsCrossReferences.GroupBy(x => x.TagId).Select(g => g.First()).ToList();
        }

        public async Task<List<PostTagCrossReference>> getPostTagsReferencesByPostIdAsync(string userId, int postId)
        {
            var sql = "SELECT Id, PostId, TagId from dbo.Posts_Tags_XREF WHERE PostId in (SELECT Id from dbo.Posts WHERE OwnerId = '" + userId + "') AND PostId = " + postId;
            return await this._appDbContext.PostsTagsCrossReferences.FromSqlRaw(sql).ToListAsync();
        }

        public async Task<List<Tag>> getTagsAsync(int postId)
        {
            var sql = "SELECT * from dbo.Tags WHERE Id IN(SELECT TagId from dbo.Posts_Tags_XREF WHERE PostId = " + postId + ")";
            return await this._appDbContext.Tags.FromSqlRaw(sql).ToListAsync();
        }

        public async Task<List<Post>> getPostsByTagAsync(string tag)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * from dbo.Posts");
            sb.Append(" WHERE");
            sb.Append(" IsDraft = 0 ");
            sb.Append(" AND ");
            sb.Append(" Id IN (");
            sb.Append("	SELECT PostId from dbo.Posts_Tags_XREF, dbo.Posts");
            sb.Append("	WHERE dbo.Posts_Tags_XREF.PostId = Posts.Id AND TagId = ");
            sb.Append("		(");
            sb.Append("			SELECT Id from dbo.Tags WHERE TagName = '" + tag +"'");
            sb.Append("		)");
            sb.Append(");");
            return await this._appDbContext.Posts.FromSqlRaw(sb.ToString()).ToListAsync();
        }
    }
}
