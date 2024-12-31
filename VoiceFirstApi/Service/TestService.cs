using System.Security.Claims;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class TestService : ITestService
    {
        private readonly ITestRepo _TestRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public TestService(ITestRepo TestRepo, IHttpContextAccessor httpContextAccessor)
        {
            _TestRepo = TestRepo;
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

        public async Task<(Dictionary<string, object>, string)> AddAsync(TestDtoModel TestDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var generatedId = Guid.NewGuid().ToString();

            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = TestDtoModel.name.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _TestRepo.AddAsync(parameters);

            if (status > 0)
            {
                data["data"] = parameters;
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }

        public async Task<(Dictionary<string, object>, string)> UpdateAsync(TestDtoModel Test)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var parameters = new
            {
                Id = Test.id,
                Name = Test.name,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _TestRepo.UpdateAsync(parameters);

            if (status > 0)
            {
                data["data"] = parameters;
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }

        public async Task<(Dictionary<string, object>, string)> GetAllAsync(Dictionary<string, object> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _TestRepo.GetAllAsync(filters);
            data["data"] = list;
            return (data, StatusUtilities.SUCCESS);
        }

        public async Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, object> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _TestRepo.GetByIdAsync(id, filters);
            data["data"] = list;
            return (data, StatusUtilities.SUCCESS);
        }

        public async Task<(Dictionary<string, object>, string)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _TestRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }
    }
}