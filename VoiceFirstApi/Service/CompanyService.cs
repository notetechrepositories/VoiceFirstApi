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
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepo _CompanyRepo;
        private readonly IBranchRepo _BranchRepo;
        private readonly IUserRepo _UserRepo;
        private readonly ILocalRepo _LocalRepo;
        private readonly IUserCompanyLinkRepo _UserCompanyLinkRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public CompanyService(ICompanyRepo CompanyRepo, IBranchRepo BranchRepo, IUserRepo UserRepo,
            ILocalRepo localRepo, IUserCompanyLinkRepo userCompanyLinkRepo, IHttpContextAccessor httpContextAccessor)
        {
            _CompanyRepo = CompanyRepo;
            _BranchRepo = BranchRepo;
            _UserRepo = UserRepo;
            _LocalRepo = localRepo;
            _UserCompanyLinkRepo = userCompanyLinkRepo;
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

        public async Task<(Dictionary<string, object>, string,int)> AddAsync(CompanyDtoModel CompanyDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t1_company_name", CompanyDtoModel.t1_company_name }
            };
            var companyList= _CompanyRepo.GetAllAsync(filter).Result.FirstOrDefault();
            if (companyList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();

            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = CompanyDtoModel.t1_company_name.Trim(),
                Type= CompanyDtoModel.id_company_type.Trim(),
                Currency=CompanyDtoModel.id_currency.Trim(),
                Date=CompanyDtoModel.is_active_till_date,
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _CompanyRepo.AddAsync(parameters);

            if (status > 0)
            {
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED,StatusUtilities.FAILED_CODE);
            }
        }
        public async Task<(Dictionary<string, object>, string, int)> InsertCompany(InsertCompanyDTOModel Company)
        {
            var userId = GetCurrentUserId();
            var emailService = new CommunicationUtilities();
            var data = new Dictionary<string, object>();
            var repoStatus = 0;
            var filter = new Dictionary<string, string>
            {
                    { "t1_company_name", Company.t1_company_name },
                    { "is_delete", "0" }
            };
            var companyList = _CompanyRepo.GetAllAsync(filter).Result.FirstOrDefault();
            if (companyList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();

            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = Company.t1_company_name.Trim(),
                Type = Company.id_company_type.Trim(),
                Currency = "",
                Date = "",
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };
            var status = await _CompanyRepo.AddAsync(parameters);

            if (status > 0)
            {

                var filterBranch = new Dictionary<string, string>
                {
                        { "t2_email", Company.insertBranchDTOModel.t2_email },
                        { "is_delete", "0" }

                };
                var BranchList = _BranchRepo.GetAllAsync(filterBranch).Result.FirstOrDefault();
                if (BranchList != null)
                {
                    await _CompanyRepo.DeleteAsync(parameters.Id);
                    return (data, StatusUtilities.EMAIL_ALREADY_EXIST_IN_BRANCH, StatusUtilities.ALREADY_EXIST_CODE);
                }
                var generatedBranchId = Guid.NewGuid().ToString();
                var generatedLocalId = Guid.NewGuid().ToString();

                var parametersLocal = new
                {
                    Id = generatedLocalId.Trim(),
                    CountryId = Company.insertBranchDTOModel.id_t2_1_country.Trim(),
                    Division1Id = Company.insertBranchDTOModel.id_t2_1_div1.Trim(),
                    Division2Id = Company.insertBranchDTOModel.id_t2_1_div2.Trim(),
                    Division3Id = Company.insertBranchDTOModel.id_t2_1_div3.Trim(),
                    Name = Company.insertBranchDTOModel.t2_1_local_name.Trim(),
                    InsertedBy = userId.Trim(),
                    InsertedDate = DateTime.UtcNow
                };

                var statusLocal = await _LocalRepo.AddAsync(parametersLocal);

                if (statusLocal == 0)
                {
                    await _CompanyRepo.DeleteAsync(parameters.Id);
                    return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
                }
                var parametersBranch = new
                {
                    Id = generatedBranchId.Trim(),
                    CompanyId = parameters.Id.Trim(),
                    Name = Company.insertBranchDTOModel.t2_company_branch_name.Trim(),
                    BranchType = Company.insertBranchDTOModel.t2_id_branch_type.Trim(),
                    Address1 = Company.insertBranchDTOModel.t2_address_1.Trim(),
                    Address2 = Company.insertBranchDTOModel.t2_address_2.Trim(),
                    ZipCode = Company.insertBranchDTOModel.t2_zip_code.Trim(),
                    Mobile = Company.insertBranchDTOModel.t2_mobile_no.Trim(),
                    PhoneNo = Company.insertBranchDTOModel.t2_phone_no.Trim(),
                    Email = Company.insertBranchDTOModel.t2_email.Trim(),
                    Local = parametersLocal.Id.Trim(),
                    InsertedBy = userId.Trim(),
                    InsertedDate = DateTime.UtcNow
                };

                var branchStatus= await _BranchRepo.AddAsync(parametersBranch);
                if (branchStatus > 0)
                {
                   

                    var generatedUserId = Guid.NewGuid().ToString();
                    var filterUser = new Dictionary<string, string>
                    {
                            { "t5_email",Company.userDtoModel.t5_email },
                            { "t5_mobile_no",Company.userDtoModel.t5_mobile_no },
                            { "t5_mobile_no",Company.userDtoModel.t5_mobile_no }
                    };
                    var UserList = _UserRepo.GetAllAsync(filterUser).Result.FirstOrDefault();

                    if (UserList != null)
                    {
                        await _CompanyRepo.DeleteAsync(parameters.Id);
                        await _BranchRepo.DeleteAsync(parametersBranch.Id);
                        return (data, StatusUtilities.EMAIL_OR_MOBILE_ALREADY_EXIST_IN_USER, StatusUtilities.ALREADY_EXIST_CODE);
                    }
                    var generatedUserLocalId = Guid.NewGuid().ToString();

                    var parametersUserLocal = new
                    {
                        Id = generatedUserLocalId.Trim(),
                        CountryId = Company.insertBranchDTOModel.id_t2_1_country.Trim(),
                        Division1Id = Company.insertBranchDTOModel.id_t2_1_div1.Trim(),
                        Division2Id = Company.insertBranchDTOModel.id_t2_1_div2.Trim(),
                        Division3Id = Company.insertBranchDTOModel.id_t2_1_div3.Trim(),
                        Name = Company.insertBranchDTOModel.t2_1_local_name.Trim(),
                        InsertedBy = userId.Trim(),
                        InsertedDate = DateTime.UtcNow
                    };

                    var statusUserLocal = await _LocalRepo.AddAsync(parametersUserLocal);

                    if (statusUserLocal == 0)
                    {
                        await _CompanyRepo.DeleteAsync(parameters.Id);
                        await _BranchRepo.DeleteAsync(parametersBranch.Id);
                        return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
                    }
                    var password = StringUtilities.GenerateRandomString(6);
                    string salt = SecurityUtilities.GenerateSalt();
                    string hashPassword = SecurityUtilities.HashPassword(password,salt);
                    var parametersUser = new
                    {
                        Id = generatedUserId.Trim(),
                        FirstName = Company.userDtoModel.t5_first_name.Trim(),
                        LastName = Company.userDtoModel.t5_last_name.Trim(),
                        Address1 = Company.userDtoModel.t5_address_1.Trim(),
                        Address2 = Company.userDtoModel.t5_address_2.Trim(),
                        ZipCode = Company.userDtoModel.t5_zip_code.Trim(),
                        Mobile = Company.userDtoModel.t5_mobile_no.Trim(),
                        Email = Company.userDtoModel.t5_email.Trim(),
                        //Password = SecurityUtilities.Encryption(password).Trim(),
                        Password = hashPassword,
                        SaltKey = salt.Trim(),
                        RoleId = "da65e845-e201-4b24-8664-a78a82284212",
                        BirthDate = Company.userDtoModel.t5_birth_year.Trim(),
                        Sex = Company.userDtoModel.t5_sex.Trim(),
                        Local = parametersUserLocal.Id.Trim(),
                        InsertedBy = userId.Trim(),
                        InsertedDate = DateTime.UtcNow
                    };

                    var statusUser = await _UserRepo.AddAsync(parametersUser);

                    if (statusUser > 0)
                    {
                        var UserLinkFilters = new Dictionary<string, string>
                        {
                            { "id_t5_users",parametersUser.Id },
                            { "is_delete", "0" }
                        };
                        var UserCopmanyList = _UserCompanyLinkRepo.GetAllAsync(UserLinkFilters).Result.FirstOrDefault();

                        if (UserCopmanyList != null)
                        {
                            await _CompanyRepo.DeleteAsync(parameters.Id);
                            await _BranchRepo.DeleteAsync(parametersBranch.Id);
                            await _UserRepo.DeleteAsync(parametersUser.Id);
                            return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
                        }
                        var generateUserCompanyLinkId = Guid.NewGuid().ToString();


                        var parametersOfUserCompanyLink = new
                        {
                            Id = generateUserCompanyLinkId.Trim(),
                            UserId = parametersUser.Id,
                            TypeId = parameters.Id,
                            SelectionValueId = "35c0c4e0-1a33-4a7f-9705-636cd5f9403f",
                            InsertedBy = userId.Trim(),
                            InsertedDate = DateTime.UtcNow
                        };

                        var UserLinkStatus = await _UserCompanyLinkRepo.AddAsync(parametersOfUserCompanyLink);
                        if (UserLinkStatus > 0)
                        {
                            EmailModel mailUser = new EmailModel();
                            mailUser.from_email_password = "frsj ucpw vaww xzmv";
                            mailUser.from_email = "anil.p@notetech.com";
                            mailUser.to_email = Company.userDtoModel.t5_email;
                            mailUser.email_html_body = "<html><body><p> Hi " + Company.userDtoModel.t5_first_name + " " + Company.userDtoModel.t5_last_name + "</p><p> Your Password is " + password +
                                    "<br> Don`t share the Password.</p><p><strong> Thanks & Regards,</strong><br><em> " +
                                    " Leadwear Team </em></p><p><em> Powered by Leadwear </em></p></body></html>";
                            mailUser.subject = "Yor Passward";

                            emailService.SendMail(mailUser);

                            EmailModel mail = new EmailModel();
                            mail.from_email_password = "frsj ucpw vaww xzmv";
                            mail.from_email = "anil.p@notetech.com";
                            mail.to_email = Company.insertBranchDTOModel.t2_email;
                            mail.email_html_body = "<html><body><p> Hi " + Company.insertBranchDTOModel.t2_company_branch_name + ",</p><p> Thank you for register your company  " +
                                "<p><strong> Thanks & Regards,</strong><br><em> " +
                                " Notetech Team </em></p><p><em> Powered by Notetech software </em></p></body></html>";
                            mail.subject = "Company successfully register";
                            emailService.SendMail(mail);
                            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
                        }
                        else
                        {
                            await _CompanyRepo.DeleteAsync(parameters.Id);
                            await _BranchRepo.DeleteAsync(parametersBranch.Id);
                            await _UserRepo.DeleteAsync(parametersUser.Id);
                        }
                        


                    }
                    else
                    {
                        await _CompanyRepo.DeleteAsync(parameters.Id);
                        await _BranchRepo.DeleteAsync(parametersBranch.Id);
                    }
                }
                else
                {
                    await _CompanyRepo.DeleteAsync(parameters.Id);
                }

            }
            return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateCompanyDtoModel Company)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var parameters = new
            {
                Id = Company.id_t1_company.Trim(),
                Name = Company.t1_company_name.Trim(),
                Type = Company.id_company_type.Trim(),
                Currency = Company.id_currency.Trim(),
                Date = Company.is_active_till_date,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _CompanyRepo.UpdateAsync(parameters);

            if (status > 0)
            {
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED,StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _CompanyRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _CompanyRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _CompanyRepo.DeleteAsync(id);
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
            var list = await _BranchRepo.UpdateStatus(updateStatusDtoModel.id, updateStatusDtoModel.status);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> GetAllCompanyDetails(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            List<CompanyWithBranchAndUserDetailsModel> companyWithBranchAndUserDetailsModels = new List<CompanyWithBranchAndUserDetailsModel>();
            var companylist = await _CompanyRepo.GetAllAsync(filters);
            if (companylist != null)
            {
                foreach(var company in companylist)
                {
                    CompanyWithBranchAndUserDetailsModel obj = new CompanyWithBranchAndUserDetailsModel();
                    obj.id_company_type = company.id_company_type;
                    obj.company_type = company.company_type;
                    obj.id_t1_company = company.id_t1_company;
                    obj.t1_company_name = company.t1_company_name;
                    obj.currency_name = company.currency_name;
                    obj.id_currency = company.id_currency;
                    obj.is_active_till_date = company.is_active_till_date;
                    obj.is_active = company.is_active;
                    var branchFilter = new Dictionary<string, string>
                    {
                        { "id_t1_company",company.id_t1_company }
                    };
                    var branchList = _BranchRepo.GetAllAsync(branchFilter);
                    obj.branchDetailsModel = branchList.Result.ToList();
                    companyWithBranchAndUserDetailsModels.Add(obj);

                } 
            }
            data["Item"] = companyWithBranchAndUserDetailsModels;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }
    }
}
