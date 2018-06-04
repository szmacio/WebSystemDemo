using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JuCheap.Data;
using JuCheap.Data.Entity;
using JuCheap.Interfaces;
using Mehdime.Entity;

namespace JuCheap.Services.AppServices
{
    public class EmailSubscribeService : IEmailSubscribeService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public EmailSubscribeService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="email"></param>
        public async Task<bool> Subscribe(string email)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                if (await db.EmailSubscribes.AnyAsync(x => x.Email == email))
                {
                    return true;
                }
                var entity = new EmailSubscribeEntity
                {
                    Email = email
                };
                entity.Create();
                db.EmailSubscribes.Add(entity);
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="email"></param>
        public async Task<bool> UnSubscribe(string email)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.EmailSubscribes.FirstOrDefaultAsync(x => x.Email == email);
                if (entity == null)
                {
                    return true;
                }
                db.EmailSubscribes.Remove(entity);
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 获取所有的订阅人员
        /// </summary>
        /// <returns></returns>
        public async Task<IList<string>> GetAll()
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                return await db.EmailSubscribes.Select(x => x.Email).ToListAsync();
            }
        }
    }
}
