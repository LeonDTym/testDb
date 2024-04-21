using Microsoft.AspNetCore.Mvc;
using SCRepository.Entity.Models.UserModels;
using SCRepository.Entity.Repository.UserRepository;

namespace StudentCardsAdmin.Controllers
{
    public class UserBasesController : Controller
    {
        protected IDataContextRepository _dataContextRepository;

        public UserBasesController(IDataContextRepository dataContextRepository)
        {
            _dataContextRepository = dataContextRepository;
        }

        protected UserInfoModel GetUserInfoModel(LoginModel model)
        {
            var result = _dataContextRepository.GetUserInfoModel(model);
            return result;
        }
        protected UserEtcData GetUserData(string model)
        {
            var result = _dataContextRepository.GetEtcData(model);
            return result;
        }

    }
}