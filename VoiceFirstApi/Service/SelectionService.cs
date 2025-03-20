using System.Security.Claims;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class SelectionService : ISelectionService
    {
        private readonly ISelectionRepo _SelectionRepo;

        private readonly IHttpContextAccessor _HttpContextAccessor;

        public SelectionService(ISelectionRepo SelectionRepo,
            IHttpContextAccessor httpContextAccessor

            )
        {
            _SelectionRepo = SelectionRepo;
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

        public async Task<(Dictionary<string, object>, string,int)> AddAsync(SelectionDtoModel SelectionDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t4_selection_name",SelectionDtoModel.t4_selection_name }
            };
            var selectionList = _SelectionRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (selectionList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();

            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = SelectionDtoModel.t4_selection_name.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _SelectionRepo.AddAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string,int)> UpdateAsync(UpdateSelectionDtoModel Selection)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t4_selection_name",Selection.t4_selection_name }
            };
            var selectionList = _SelectionRepo.GetAllAsync(filter).Result.FirstOrDefault();
            if (selectionList != null && selectionList.id_t4_selection!= Selection.id_t4_selection)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var parameters = new
            {
                Id = Selection.id_t4_selection,
                Name = Selection.t4_selection_name,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _SelectionRepo.UpdateAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string,int)> GetAllAsync(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _SelectionRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
        }
        
        public async Task<(Dictionary<string, object>, string,int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _SelectionRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string,int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _SelectionRepo.DeleteAsync(id);
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