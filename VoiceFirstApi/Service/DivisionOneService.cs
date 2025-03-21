﻿using System.Diagnostics.Metrics;
using System.Security.Claims;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class DivisionOneService : IDivisionOneService
    {
        private readonly IDivisionOneRepo _DivisionOneRepo;
        private readonly ICountryRepo _CountryRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public DivisionOneService(IDivisionOneRepo DivisionOneRepo, ICountryRepo countryRepo, IHttpContextAccessor httpContextAccessor)
        {
            _DivisionOneRepo = DivisionOneRepo;
            _CountryRepo = countryRepo;
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

        public async Task<(Dictionary<string, object>, string, int)> AddAsync(DivisionOneDtoModel DivisionOneDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var generatedId = Guid.NewGuid().ToString();
            var filter = new Dictionary<string, string>
            {
                    { "id_t2_1_country", DivisionOneDtoModel.id_t2_1_country },
                    { "t2_1_div1_name", DivisionOneDtoModel.t2_1_div1_name }
            };

            var exsitList = _DivisionOneRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (exsitList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = DivisionOneDtoModel.t2_1_div1_name.Trim(),
                CountryId = DivisionOneDtoModel.id_t2_1_country.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _DivisionOneRepo.AddAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateDivisionOneDtoModel DivisionOne)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();

            var filter = new Dictionary<string, string>
            {
                    { "id_t2_1_country", DivisionOne.id_t2_1_country },
                    { "t2_1_div1_name", DivisionOne.t2_1_div1_name }
            };

            var exsitList = _DivisionOneRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (exsitList != null && exsitList.id_t2_1_div1 != DivisionOne.id_t2_1_div1)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }

            var parameters = new
            {
                Id = DivisionOne.id_t2_1_div1,
                Name = DivisionOne.t2_1_div1_name.Trim(),
                CountryId = DivisionOne.id_t2_1_country.Trim(),
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _DivisionOneRepo.UpdateAsync(parameters);

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
            var list = await _DivisionOneRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionOneRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionOneRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> ImportStateByCountry(List<ImportDivisionOneModel> importlist)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var Divisions = new List<DivisionOneModel>();
            foreach (var division in importlist)
            {
                if (division.t2_1_country_name != null)
                {
                    var filter = new Dictionary<string, string>
                    {
                        { "t2_1_country_name", division.t2_1_country_name }
                    };
                    var countryList = _CountryRepo.GetAllAsync(filter).Result.FirstOrDefault();

                    if (countryList == null)
                    {
                        return (data, StatusUtilities.COUNTRY_NOT_EXSISTS, StatusUtilities.NOT_FOUND_CODE);
                    }
                    else
                    {
                        division.t2_1_country_name = countryList.id_t2_1_country;
                    }

                }
                else
                {
                    return (data, StatusUtilities.COUNTRY_NOT_EXSISTS, StatusUtilities.NOT_FOUND_CODE);
                }

                
            }
            foreach(var division in importlist)
            {
                var generatedId = Guid.NewGuid().ToString();
                var filter = new Dictionary<string, string>
                {
                    { "id_t2_1_country", division.t2_1_country_name },
                    { "t2_1_div1_name", division.t2_1_div1_name }
                };

                var exsitList = _DivisionOneRepo.GetAllAsync(filter).Result.FirstOrDefault();

                if (exsitList == null)
                {
                    var parameters = new
                    {
                        Id = generatedId.Trim(),
                        Name = division.t2_1_div1_name.Trim(),
                        CountryId = division.t2_1_country_name.Trim(),
                        InsertedBy = userId.Trim(),
                        InsertedDate = DateTime.UtcNow
                    };

                    var status = await _DivisionOneRepo.AddAsync(parameters);

                    if (status > 0)
                    {
                        var filters = new Dictionary<string, string>
                        {
                            { "id_t2_1_country", division.t2_1_country_name }
                        };
                        var countryList = _CountryRepo.GetAllAsync(filters).Result.FirstOrDefault();
                        DivisionOneModel obj = new DivisionOneModel();
                       // obj.id_t2_1_country = parameters.CountryId;
                        obj.t2_1_country_name = countryList.t2_1_country_name;
                        obj.id_t2_1_div1 = parameters.Id;
                        obj.t2_1_div1_name = parameters.Name;
                        obj.inserted_by = parameters.InsertedBy;
                        obj.inserted_date = parameters.InsertedDate;

                        Divisions.Add(obj);
                    }
                }
                
            }
            data["items"] = Divisions;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionOneRepo.UpdateStatus(updateStatusDtoModel.id, updateStatusDtoModel.status);
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