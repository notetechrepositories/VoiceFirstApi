using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace VoiceFirstApi.Service
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepo _IssueRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IBranchRepo _branchRepo;
        private readonly IUserRepo _userRepo;

        public IssueService(IIssueRepo issueRepo, IHttpContextAccessor httpContextAccessor,IBranchRepo branchRepo,IUserRepo userRepo)
        {
            _IssueRepo = issueRepo;
            _HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _branchRepo = branchRepo;
            _userRepo = userRepo;
            
          
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
        public async Task<(Dictionary<string, object>, string, int)> AddAsync(IssueDtoModel issue)
        {
            var fileMappings = new Dictionary<string, IFormFile?>
            {
                { "IssueVoice", issue.t11_issue_voice },
                { "Image1", issue.t11_issue_image1 },
                { "Image2", issue.t11_issue_image2 },
                { "Image3", issue.t11_issue_image3 },
                { "Image4", issue.t11_issue_image4 },
                { "Video1", issue.t11_issue_video1 },
                { "Video2", issue.t11_issue_video2 },
                { "Video3", issue.t11_issue_video3 },
                { "Video4", issue.t11_issue_video4 },
                { "EvidenceImage1", issue.t11_evidence_image1 },
                { "EvidenceImage2", issue.t11_evidence_image2 }
            };

            // Dictionary to store uploaded file names
            var uploadedFiles = new Dictionary<string, string>();

            // Upload files asynchronously
            var uploadTasks = fileMappings
                .Where(entry => entry.Value != null)
                .Select(async entry =>
                {
                    try
                    {
                        var uploadedFile = await _IssueRepo.AddFile(entry.Value);
                        return new { entry.Key, FileName = uploadedFile ?? "" };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error uploading {entry.Key}: {ex.Message}");
                        return new { entry.Key, FileName = "" };
                    }
                });

            var results = await Task.WhenAll(uploadTasks);
            foreach (var result in results)
            {
                uploadedFiles[result.Key] = result.FileName;
            }

            // Generate necessary IDs
            var generatedId = Guid.NewGuid().ToString();
            var userId = GetCurrentUserId();

            // Create parameters
            var parameters = new
            {
                Id = generatedId.Trim(),
                CompanyBranch = issue.id_t2_company_branch,
                IssueText = issue.t11_issue_text,
                IssueVoice = uploadedFiles.GetValueOrDefault("IssueVoice", ""),
                Image1 = uploadedFiles.GetValueOrDefault("Image1", ""),
                Image2 = uploadedFiles.GetValueOrDefault("Image2", ""),
                Image3 = uploadedFiles.GetValueOrDefault("Image3", ""),
                Image4 = uploadedFiles.GetValueOrDefault("Image4", ""),
                Video1 = uploadedFiles.GetValueOrDefault("Video1", ""),
                Video2 = uploadedFiles.GetValueOrDefault("Video2", ""),
                Video3 = uploadedFiles.GetValueOrDefault("Video3", ""),
                Video4 = uploadedFiles.GetValueOrDefault("Video4", ""),
                SubmittedBy = userId.Trim(),
                EvidenceImage1 = uploadedFiles.GetValueOrDefault("EvidenceImage1", ""),
                EvidenceImage2 = uploadedFiles.GetValueOrDefault("EvidenceImage2", "")
            };

            // Insert into database
            var status = await _IssueRepo.AddAsync(parameters);
            var data = new Dictionary<string, object>();

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

        public async Task<(IssueModel?, string, int)> GetByIdAsync(string issueId)
        {
            
            var issue = await _IssueRepo.GetByIdAsync(issueId);

            if (issue != null)
            {
                return (issue, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (null, StatusUtilities.FAILED, StatusUtilities.NOT_FOUND_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> GetAllAsync()
        {
            var data = new Dictionary<string, object>();
            List<GetAllIssuesWithBranch> getAllIssuesWithBranches = new List<GetAllIssuesWithBranch>();
            var issues = await _IssueRepo.GetAllAsync(); // Fetch all issues
            foreach (var item in issues)
            {
                GetAllIssuesWithBranch obj = new GetAllIssuesWithBranch();
                var filter = new Dictionary<string, string>
                {
                    { "id_t2_company_branch",item.id_t2_company_branch}
                };
                var userDetails=await _userRepo.GetByIdAsync(item.t11_issue_submitted_by,new Dictionary<string, string>());
                if (userDetails != null)
                {
                    var branchDetails = await _branchRepo.GetAllAsync(filter);
                    obj.t11_issue_id = item.t11_issue_id;
                    obj.id_t2_company_branch = item.id_t2_company_branch;
                    obj.t11_issue_text = item.t11_issue_text;
                    obj.t11_issue_voice = item.t11_issue_voice != "" ? StringUtilities.GetUploadIssueAudio() + item.t11_issue_voice : null;
                    obj.t11_issue_image1 = item.t11_issue_image1 != "" ? StringUtilities.GetUploadIssueImage() + item.t11_issue_image1 : null;
                    obj.t11_issue_image2 = item.t11_issue_image2 != "" ? StringUtilities.GetUploadIssueImage() + item.t11_issue_image2 : null;
                    obj.t11_issue_image3 = item.t11_issue_image3 != "" ? StringUtilities.GetUploadIssueImage() + item.t11_issue_image3 : null;
                    obj.t11_issue_image4 = item.t11_issue_image4 != "" ? StringUtilities.GetUploadIssueImage() + item.t11_issue_image4 : null;
                    obj.t11_issue_video1 = item.t11_issue_video1 != "" ? StringUtilities.GetUploadIssueVideo() + item.t11_issue_video1 : null;
                    obj.t11_issue_video2 = item.t11_issue_video2 != "" ? StringUtilities.GetUploadIssueVideo() + item.t11_issue_video2 : null;
                    obj.t11_issue_video3 = item.t11_issue_video3 != "" ? StringUtilities.GetUploadIssueVideo() + item.t11_issue_video3 : null;
                    obj.t11_issue_video4 = item.t11_issue_video4 != "" ? StringUtilities.GetUploadIssueVideo() + item.t11_issue_video4 : null;
                    obj.t11_evidence_image1 = item.t11_evidence_image1 != "" ? StringUtilities.GetUploadIssueImage() + item.t11_evidence_image1 : null;
                    obj.t11_evidence_image2 = item.t11_evidence_image2 != "" ? StringUtilities.GetUploadIssueImage() + item.t11_issue_image2 : null;
                    obj.userModel = userDetails;
                    obj.branchModel = branchDetails.FirstOrDefault();
                    getAllIssuesWithBranches.Add(obj);

                }
                
            }
            
            if (getAllIssuesWithBranches.Count>0) // If there are records
            {
                data["Items"] = getAllIssuesWithBranches;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.NOT_FOUND_CODE);
            }
        

        }
}
}

