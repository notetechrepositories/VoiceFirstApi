using System.Data;
using System.Security.Claims;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
using VoiceFirstApi.Utilits;
namespace VoiceFirstApi.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _UserRepo;
        private readonly ILocalRepo _LocalRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IRoleRepo _RoleRepo;

        public UserService(IUserRepo UserRepo,
            ILocalRepo LocalRepo,
            IHttpContextAccessor httpContextAccessor,
            IRoleRepo roleRepo)
        {
            _UserRepo = UserRepo;
            _LocalRepo = LocalRepo;
            _HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _RoleRepo = roleRepo;
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
            var userId = "";
            var emailService = new CommunicationUtilities();
            if (UserDtoModel.id_t5_1_m_user_roles!= "FC970C84-E654-4C7F-9893-87D1D2EF03F5")
            {
                 userId = GetCurrentUserId();
            }
            
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
            var password = StringUtilities.GenerateRandomString(6);
            string salt = SecurityUtilities.GenerateSalt();
            string hashPassword = SecurityUtilities.HashPassword(password, salt);
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
                Password = hashPassword,
                SaltKey = salt.Trim(),
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
                EmailModel mailUser = new EmailModel();
                mailUser.from_email_password = "frsj ucpw vaww xzmv";
                mailUser.from_email = "anil.p@notetech.com";
                mailUser.to_email = UserDtoModel.t5_email;
                mailUser.email_html_body = "<html><body><p> Hi " + UserDtoModel.t5_first_name + " " + UserDtoModel.t5_last_name + "</p><p> Your Password is " + password +
                        "<br> Don`t share the Password.</p><p><strong> Thanks & Regards,</strong><br><em> " +
                        " Leadwear Team </em></p><p><em> Powered by Leadwear </em></p></body></html>";
                mailUser.subject = "Yor Passward";

                emailService.SendMail(mailUser);

         
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

            if (UserList != null  && UserList.id_t5_users!= userId)
            {
                return (data, StatusUtilities.EMAIL_OR_MOBILE_ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }

            var parameters = new
            {
                Id = userId,
                FirstName = User.t5_first_name.Trim(),
                LastName = User.t5_last_name.Trim(),
                Address1 = User.t5_address_1.Trim(),
                Address2 = User.t5_address_2.Trim(),
                ZipCode = User.t5_zip_code.Trim(),
                Mobile = User.t5_mobile_no.Trim(),
                Email = User.t5_email.Trim(),
                BirthDate = User.t5_birth_year.Trim(),
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

        public async Task<(Dictionary<string, object>, string, int)> GetAllEmployeeAsync()
        {
            var data = new Dictionary<string, object>();
            var userId = GetCurrentUserId();
            
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                {"inserted_by",userId },
                {"is_delete","0" }
            };
            var list = await _UserRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            Dictionary<string, string> localfilters = new Dictionary<string, string>();
            var list = await _UserRepo.GetByIdAsync(id, filters);
            if (list != null)
            {
                var locationDetail = _LocalRepo.GetByIdAsync(list.id_t2_1_local, localfilters);

                UserProfileModel userProfileModel = new UserProfileModel();
                userProfileModel.id_t5_users = list.id_t5_users;
                userProfileModel.t5_first_name = list.t5_first_name;
                userProfileModel.t5_last_name = list.t5_last_name;
                userProfileModel.t5_address_1 = list.t5_address_1;
                userProfileModel.t5_address_2 = list.t5_address_2;
                userProfileModel.t5_zip_code = list.t5_zip_code;
                userProfileModel.t5_mobile_no = list.t5_mobile_no;
                userProfileModel.t5_email = list.t5_email;
                userProfileModel.t5_birth_year = list.t5_birth_year;
                userProfileModel.t5_sex = list.t5_sex;
                userProfileModel.id_t2_1_local = list.id_t2_1_local;
                userProfileModel.id_t5_1_m_user_roles = list.id_t5_1_m_user_roles;
                if (locationDetail.Result == null)
                {
                    return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
                }
                else
                {
                    userProfileModel.id_t2_1_country = locationDetail.Result.id_t2_1_country;
                    userProfileModel.id_t2_1_div1 = locationDetail.Result.id_t2_1_div1;
                    userProfileModel.id_t2_1_div2 = locationDetail.Result.id_t2_1_div2;
                    userProfileModel.id_t2_1_div3 = locationDetail.Result.id_t2_1_div3;
                    userProfileModel.t2_1_local_name = locationDetail.Result.t2_1_local_name;
                    userProfileModel.t2_1_country_name = locationDetail.Result.t2_1_country_name;
                    userProfileModel.t2_1_div1_name = locationDetail.Result.t2_1_div1_name;
                    userProfileModel.t2_1_div2_name = locationDetail.Result.t2_1_div2_name;
                    userProfileModel.t2_1_div3_name = locationDetail.Result.t2_1_div3_name;
                }


                data["Items"] = userProfileModel;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }

            return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
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