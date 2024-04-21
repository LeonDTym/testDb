using SCRepository.Entity.Models.UserModels;
using System;

namespace SCRepository.Entity.Repository.UserRepository
{
    public interface IDataContextRepository : IDisposable
    {
        UserInfoModel GetUserInfoModel(LoginModel model);
        UserEtcData GetEtcData(string model);
    }
}
