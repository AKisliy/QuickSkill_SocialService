using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialService.Core.Interfaces.Repositories
{
    public interface ILectureRepository
    {
        public Task Add(int id);

        public Task Delete(int id);

        public Task<bool> HasLectureWithId(int id);
    }
}