using MimeKit.Encodings;

namespace VoiceFirstApi.DtoModels
{
    public class IssueDtoModel
    {
       
        public string id_t2_company_branch { get; set;}
        public string? t11_issue_text { get; set; }
        public IFormFile? t11_issue_voice { get; set; }
        public IFormFile? t11_issue_image1 { get; set; }
        public IFormFile? t11_issue_image2 { get; set; }
        public IFormFile? t11_issue_image3 { get; set; }
        public IFormFile? t11_issue_image4 { get; set; }
        public IFormFile? t11_issue_video1 { get; set; }
        public IFormFile? t11_issue_video2 { get; set; }
        public IFormFile? t11_issue_video3 { get; set; }
        public IFormFile? t11_issue_video4 { get; set; }
        public IFormFile t11_evidence_image1 { get; set; }
        public IFormFile? t11_evidence_image2 { get; set; }


    }
    
}
