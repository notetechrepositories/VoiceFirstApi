using Dapper;
using VoiceFirstApi.Context;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.Repository
{
    public class IssueRepo : IIssueRepo
    {
        private readonly DapperContext _dapperContext;
        private readonly IWebHostEnvironment _env;

        public IssueRepo(DapperContext dapperContext, IWebHostEnvironment env)
        {
            _dapperContext = dapperContext;
            _env = env;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t11_issue(t11_issue_id,id_t2_company_branch,t11_issue_text,t11_issue_voice,t11_issue_image1,t11_issue_image2,t11_issue_image3,t11_issue_image4,t11_issue_video1,t11_issue_video2,t11_issue_video3,t11_issue_video4,t11_issue_submitted_by,t11_evidence_image1,t11_evidence_image2) 
                VALUES (@Id,@CompanyBranch,@IssueText,@IssueVoice,@Image1,@Image2,@Image3,@Image4,@Video1,@Video2,@Video3,@Video4,@SubmittedBy,@EvidenceImage1,@EvidenceImage2);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
        public async Task<string> AddFile(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return "Invalid file. Please upload a valid file.";
            }

            // Define base folder within wwwroot
            var baseFolder = Path.Combine(_env.WebRootPath, "Uploads");

            // Get the file extension
            var fileExtension = Path.GetExtension(formFile.FileName).ToLower();

            // Define allowed extensions for different media types
            var imageExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };
            var videoExtensions = new HashSet<string> { ".mp4", ".avi", ".mov", ".mkv" };
            var audioExtensions = new HashSet<string> { ".mp3", ".wav", ".aac", ".ogg" };

            string mediaType;
            string subFolder;

            // Determine the file type and folder
            if (imageExtensions.Contains(fileExtension))
            {
                mediaType = "Image";
                subFolder = "Images";
            }
            else if (videoExtensions.Contains(fileExtension))
            {
                mediaType = "Video";
                subFolder = "Videos";
            }
            else if (audioExtensions.Contains(fileExtension))
            {
                mediaType = "Audio";
                subFolder = "Audio";
            }
            else
            {
                return "Invalid file type. Only images (JPG, PNG, GIF), videos (MP4, AVI, MOV), and audio (MP3, WAV) are allowed.";
            }

            // Create the directory if it doesn't exist
            var folderPath = Path.Combine(baseFolder, subFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Generate a unique file name
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var randomNumber = new Random().Next(1000, 9999);
            var newFileName = $"{mediaType}_{timestamp}_{randomNumber}{fileExtension}";

            // Define the full file path
            var filePath = Path.Combine(folderPath, newFileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            // Return the file path or name (based on how you want to store it in the DB)
            return  newFileName; // Returns relative path: "Images/IMG_20240306123045_1234.jpg"
        }

        public async Task<IEnumerable<IssueModel>> GetAllAsync()
        {
            var Query = @"SELECT * FROM t11_issue ;";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QueryAsync<IssueModel>(Query);
            }
        }

        public async Task<IssueModel> GetByIdAsync(string issueId)
        {
            var query = @" SELECT * FROM t11_issue WHERE t11_issue_id = @IssueId";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<IssueModel>(query, new { IssueId = issueId });
            }
        }
    }
}
