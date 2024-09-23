using Business.Models;
using Core.Results;
using Core.Results.Bases;
using DataAccess.Enums;

namespace Business.Services
{
    public interface IAccountService
    {
        Result Login(AccountLoginModel accountLoginModel, UserModel userResultModel);

        Result Register(AccountRegisterModel model);
    }
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;

        public AccountService(IUserService userService)
        {
            _userService = userService;
        }

        public Result Login(AccountLoginModel accountLoginModel, UserModel userResultModel) 
            //Kullanıcı girişi
        {
            
            var user = _userService.Query().SingleOrDefault(u => u.Name == accountLoginModel.Name && u.Password == accountLoginModel.Password);

            if (user == null) //eğer böyle bir kullanıcı bulunamadıysa
            {
                return new ErrorResult("İnvalid user name or password");
            }

            userResultModel.Name = user.Name;
            userResultModel.Role.RoleName = user.Role.RoleName;
            userResultModel.Id = user.Id;

            return new SuccessResult();
        }

        public Result Register(AccountRegisterModel model)
        {
            //büyük küçük harf probelmi için sorgu üzerinden yani Query üzerinden değil koleksiyon üzerinden yapıyoruz.
            // if(_userService.Query().Any(u => u.Name.ToLower() == model.Name.ToLower().Trim())) //bunu sorgu bazında yapıyoruz SQL sorgusu bunu oluşturabildiği için end framework üzerinden bu şekilde yazıyoruz

            //Eğer Query de bir şey yapamıyorsak yukardaki gibi yani yazdığım kodlar üzerinden end framework sorguyu oluşturup işlemi yapamıyorsam her zaman bir sorgu sonucunda bir obje (koleksiyonda olabilir ya da tek bir obje de olabilir) çekip bunun üzerinden yani c# üzerinden çekip istediğimiz her seyi kullanabiliriz
            
            List<UserModel> users = _userService.Query().ToList();//bu şekilde c# 'a geçiş yapmış oluyoruz (bütün kullanıcıları çekmiş oluruz) 


            if (users.Any(u => u.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase))) //c# üzerinden link çalıştırmış oluyoruz
                return new ErrorResult("User with the same user name exists!");

            UserModel userModel = new UserModel()
            {
                Password = model.Password,
                RoleId = (int)Roles.User,
                Name = model.Name,
            };

            return _userService.Add(userModel);
        }
    }
}
