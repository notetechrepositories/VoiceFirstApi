using System.Data;
using System.Security.Claims;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _UserRepo;
        private readonly ILocalRepo _LocalRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public UserService(IUserRepo UserRepo, ILocalRepo LocalRepo, IHttpContextAccessor httpContextAccessor)
        {
            _UserRepo = UserRepo;
            _LocalRepo = LocalRepo;
            _HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private string GetCurrentUserId()
        {
            if (_HttpContextAccessor == null)
            {
                throw new InvalidOperationException("HTTP Context Accessor is not initialized.");
            }

            // Validate that the HTTP context and user claims are available
            var userClaims = _HttpContextAccessor.HttpContext?.User;
            if (userClaims == null || !userClaims.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // Find the user_id claim
            var userIdClaim = userClaims.FindFirst("user_id");
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in the token.");
            }
            var decryUserId = SecurityUtilities.Decryption(userIdClaim.Value);
            if (decryUserId == null)
            {
                throw new UnauthorizedAccessException("User ID not found in the token.");
            }
            return decryUserId;
        }

        public async Task<(Dictionary<string, object>, string, int)> AddAsync(UserDtoModel UserDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var generatedId = Guid.NewGuid().ToString();
            var generatedLocalId = Guid.NewGuid().ToString();

            var parametersLocal = new
            {
                Id = generatedLocalId.Trim(),
                CountryId = UserDtoModel.id_t2_1_country.Trim(),
                Division1Id = UserDtoModel.id_t2_1_div1.Trim(),
                Division2Id = UserDtoModel.id_t2_1_div2.Trim(),
                Division3Id = UserDtoModel.id_t2_1_div3.Trim(),
                Name = UserDtoModel.t2_1_local_name.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var statusLocal = await _LocalRepo.AddAsync(parametersLocal);

            if (statusLocal== 0)
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
            var filter = new Dictionary<string, string>
            {
                    { "t5_email",UserDtoModel.t5_email },
                    { "t5_mobile_no",UserDtoModel.t5_mobile_no }
            };
            var UserList = _UserRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (UserList != null )
            {
                return (data, StatusUtilities.EMAIL_OR_MOBILE_ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var password =  StringUtilities.GenerateRandomString(6);
            var parameters = new
            {
                Id = generatedId.Trim(),
                FirstName = UserDtoModel.t5_first_name.Trim(),
                LastName = UserDtoModel.t5_last_name.Trim(),
                Address1 = UserDtoModel.t5_address_1.Trim(),
                Address2 = UserDtoModel.t5_address_2.Trim(),
                ZipCode = UserDtoModel.t5_zip_code.Trim(),
                Mobile = UserDtoModel.t5_mobile_no.Trim(),
                Email = UserDtoModel.t5_email.Trim(),
                Password = SecurityUtilities.Encryption(password).Trim(),
                BirthDate = UserDtoModel.t5_birth_year.Trim(),
                RoleId = UserDtoModel.id_t5_1_m_user_roles.Trim(),
                Sex = UserDtoModel.t5_sex.Trim(),
                Local = parametersLocal.Id.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _UserRepo.AddAsync(parameters);

            if (status > 0)
            {
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateUserDtoModel User)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            if (User.id_t2_1_local == null)
            {
                var generatedLocalId = Guid.NewGuid().ToString();

                var parametersLocal = new
                {
                    Id = generatedLocalId.Trim(),
                    CountryId = User.id_t2_1_country.Trim(),
                    Division1Id = User.id_t2_1_div1.Trim(),
                    Division2Id = User.id_t2_1_div2.Trim(),
                    Division3Id = User.id_t2_1_div3.Trim(),
                    Name = User.t2_1_local_name.Trim(),
                    InsertedBy = userId.Trim(),
                    InsertedDate = DateTime.UtcNow
                };

                var statusLocal = await _LocalRepo.AddAsync(parametersLocal);

                if (statusLocal > 0)
                {
                    User.id_t2_1_local = parametersLocal.Id;
                }
                else
                {
                    return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
                }
            }
            var filter = new Dictionary<string, string>
            {
                    { "t5_email",User.t5_email },
                    { "t5_mobile_no",User.t5_mobile_no }
            };
            var UserList = _UserRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (UserList != null  && UserList.id_t5_users!= User.id_t5_users)
            {
                return (data, StatusUtilities.EMAIL_OR_MOBILE_ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }

            var parameters = new
            {
                Id = User.id_t5_users,
                FirstName = User.t5_first_name.Trim(),
                LastName = User.t5_last_name.Trim(),
                Address1 = User.t5_address_1.Trim(),
                Address2 = User.t5_address_2.Trim(),
                ZipCode = User.t5_zip_code.Trim(),
                Mobile = User.t5_mobile_no.Trim(),
                Email = User.t5_email.Trim(),
                BirthDate = User.t5_birth_year.Trim(),

                RoleId = User.id_t5_1_m_user_roles.Trim(),
                Sex = User.t5_sex.Trim(),
                Local = User.id_t2_1_local.Trim(),
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _UserRepo.UpdateAsync(parameters);

            if (status > 0)
            {
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _UserRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _UserRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _UserRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel)
        {
            var data = new Dictionary<string, object>();
            var list = await _UserRepo.UpdateStatus(updateStatusDtoModel.id, updateStatusDtoModel.status);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }
    }
}