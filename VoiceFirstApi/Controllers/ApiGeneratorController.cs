using JWT.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using System.Text;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiGeneratorController : ControllerBase
    {
        [HttpPost("{name}")]
        public IActionResult GenerateFiles(string name)
        {
            var formattedName = char.ToUpper(name[0]) + name.Substring(1);
            var baseDirectory = Directory.GetCurrentDirectory();

            var interfaceRepoFolderPath = Path.Combine(baseDirectory, "IRepository");
            var interfaceServiceFolderPath = Path.Combine(baseDirectory, "IService");
            var repoFolderPath = Path.Combine(baseDirectory, "Repository");
            var serviceFolderPath = Path.Combine(baseDirectory, "Service");
            var controllerFolderPath = Path.Combine(baseDirectory, "Controllers");
            var modelFolderPath = Path.Combine(baseDirectory, "Models");
            var dtoFolderPath = Path.Combine(baseDirectory, "DTOModels");

            // Define file paths inside the respective folders
            var interfaceRepoPath = Path.Combine(interfaceRepoFolderPath, $"I{formattedName}Repo.cs");
            var interfaceServicePath = Path.Combine(interfaceServiceFolderPath, $"I{formattedName}Service.cs");
            var repoPath = Path.Combine(repoFolderPath, $"{formattedName}Repo.cs");
            var servicePath = Path.Combine(serviceFolderPath, $"{formattedName}Service.cs");
            var controllerPath = Path.Combine(controllerFolderPath, $"{formattedName}Controller.cs");
            var modelPath = Path.Combine(modelFolderPath, $"{formattedName}Model.cs");
            var dtoPath = Path.Combine(dtoFolderPath, $"{formattedName}DTOModel.cs");


            var interfaceRepoContent =
                 $"using System.Diagnostics.Metrics;\r\n" +
                 $"using VoiceFirstApi.DtoModels;\r\n" +
                 $"using VoiceFirstApi.Models;\r\n\r\n" +
                 $"namespace VoiceFirstApi.IRepository\r\n" +
                 $"{{\r\n" +
                 $"    public interface I{formattedName}Repo\r\n" +
                            $"    {{\r\n" +
                 $"        Task<IEnumerable<{formattedName}Model>> GetAllAsync(Dictionary<string, string> filters);\r\n" +
                 $"        Task<{formattedName}Model> GetByIdAsync(string id, Dictionary<string, string> filters);\r\n" +
                 $"        Task<int> AddAsync(object parameters);\r\n" +
                 $"        Task<int> UpdateAsync(object parameters);\r\n" +
                 $"        Task<int> DeleteAsync(string id);\r\n" +
                 $"    }}\r\n" +
                 $"}}";

            var interfaceServiceContent =
                $"using VoiceFirstApi.DtoModels;\r\n" +
                $"using VoiceFirstApi.Models;\r\n\r\n" +
                $"namespace VoiceFirstApi.IService\r\n" +
                $"{{\r\n" +
                $"    public interface I{formattedName}Service\r\n" +
                $"    {{\r\n" +
                $"        Task<(Dictionary<string, object>, string)> AddAsync({formattedName}DtoModel {formattedName});\r\n" +
                $"        Task<(Dictionary<string, object>, string)> UpdateAsync(Update{formattedName}DtoModel {formattedName});\r\n" +
                $"        Task<(Dictionary<string, object>, string)> GetAllAsync(Dictionary<string, string> filters);\r\n" +
                $"        Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, string> filters);\r\n" +
                $"        Task<(Dictionary<string, object>, string)> DeleteAsync(string id);\r\n" +
                $"    }}\r\n" +
                $"}}";


            var repoContent =
                $"using Dapper;\r\n" +
                $"using System.Diagnostics.Metrics;\r\n" +
                $"using VoiceFirstApi.Context;\r\n" +
                $"using VoiceFirstApi.DtoModels;\r\n" +
                $"using VoiceFirstApi.IRepository;\r\n" +
                $"using VoiceFirstApi.Models;\r\n" +
                $"using VoiceFirstApi.Utilities;\r\n\r\n" +

                $"namespace VoiceFirstApi.Repository\r\n" +
                            $"{{\r\n" +
                $"    public class {formattedName}Repo : I{formattedName}Repo\r\n" +
                $"    {{\r\n" +
                $"        private readonly DapperContext _dapperContext;\r\n\r\n" +
                $"        public {formattedName}Repo(DapperContext dapperContext)\r\n" +
                $"        {{\r\n" +
                $"            _dapperContext = dapperContext;\r\n" +
                $"        }}\r\n\r\n" +
                $"        public async Task<int> AddAsync(object parameters)\r\n" +
                $"        {{\r\n" +
                $"            var query = @\"\r\n" +
                $"                INSERT INTO {name}(id,name,inserted_by,inserted_date) \r\n" +
                $"                VALUES (@Id,@Name,@InsertedBy,@InsertedDate);\";\r\n" +
                $"            using (var connection = _dapperContext.CreateConnection())\r\n" +
                $"            {{\r\n" +
                $"                return await connection.ExecuteAsync(query, parameters);\r\n" +
                $"            }}\r\n" +
                $"        }}\r\n\r\n" +
                $"        public async Task<int> DeleteAsync(string id)\r\n" +
                $"        {{\r\n" +
                $"            var query = \"DELETE FROM {name} WHERE id = @id\";\r\n" +
                $"            using (var connection = _dapperContext.CreateConnection())\r\n" +
                $"            {{\r\n" +
                $"                return await connection.ExecuteAsync(query, new {{ id = id }});\r\n" +
                $"            }}\r\n" +
                $"        }}\r\n\r\n" +
                $"        public async Task<IEnumerable<{formattedName}Model>> GetAllAsync(Dictionary<string, string> filters)\r\n" +
                $"        {{\r\n" +
                $"          var query = \"SELECT * FROM {name} \";\r\n\r\n" +
                $"          if (filters != null && filters.Any())\r\n" +
                $"          {{\r\n" +
                $"              var keys = new List<string>(filters.Keys);\r\n" +
                $"              var whereClauses = \"\";\r\n" +
                $"              for (int i = 0; i < keys.Count; i++)\r\n" +
                $"              {{\r\n" +
                $"                  string key = keys[i];\r\n" +
                $"                  string value = filters[key];\r\n" +
                $"                  if (i == 0)\r\n" +
                $"                  {{\r\n" +
                $"                      whereClauses = \" \" + key + \"='\" + value + \"'\";\r\n" +
                $"                  }}\r\n" +
                $"                  else\r\n" +
                $"                  {{\r\n" +
                $"                      whereClauses += \" AND \" + key + \"='\" + value + \"'\";\r\n" +
                $"                  }}\r\n" +
                $"              }}\r\n" +
                $"              query += \" WHERE \" + whereClauses + \";\";\r\n" +
                $"          }}\r\n\r\n" +
                $"          using (var connection = _dapperContext.CreateConnection())\r\n" +
                $"          {{\r\n" +
                $"                  return await connection.QueryAsync<{formattedName}Model>(query);\r\n" +
                $"            }}\r\n" +
                $"        }}\r\n\r\n" +
                $"        public async Task<{formattedName}Model> GetByIdAsync(string id, Dictionary<string, string> filters)\r\n" +
                $"        {{\r\n" +
                $"            var query = \"SELECT * FROM {name} WHERE id = @id\";\r\n\r\n" +
                $"            if (filters != null && filters.Any())\r\n" +
                $"            {{\r\n" +
                $"              var keys = new List<string>(filters.Keys);\r\n" +
                $"              var whereClauses = \"\";\r\n" +
                $"              for (int i = 0; i < keys.Count; i++)\r\n" +
                $"              {{\r\n" +
                $"                  string key = keys[i];\r\n" +
                $"                  string value = filters[key];\r\n" +
                $"                  whereClauses += \" AND \" + key + \"='\" + value + \"'\";\r\n" +
                $"              }}\r\n" +
                $"              query +=  whereClauses + \";\";\r\n" +
                $"            }}\r\n\r\n" +
                $"            var parameters = new DynamicParameters();\r\n" +
                $"            parameters.Add(\"id\", id);\r\n\r\n" +
                $"            using (var connection = _dapperContext.CreateConnection())\r\n" +
                $"            {{\r\n" +
                $"                return await connection.QuerySingleOrDefaultAsync<{formattedName}Model>(query, parameters);\r\n" +
                $"            }}\r\n" +
                $"        }}\r\n\r\n" +
                $"        public async Task<int> UpdateAsync(object parameters)\r\n" +
                $"        {{\r\n" +
                $"            var query = @\"\r\n" +
                $"                UPDATE {name}\r\n" +
                $"                SET \r\n" +
                $"                    name = @Name, \r\n" +
                $"                    updated_by = @UpdatedBy, \r\n" +
                $"                    updated_date = @UpdatedDate\r\n" +
                $"                WHERE id = @Id\";\r\n\r\n" +
                $"            using (var connection = _dapperContext.CreateConnection())\r\n" +
                $"            {{\r\n" +
                $"                return await connection.ExecuteAsync(query, parameters);\r\n" +
                $"            }}\r\n" +
                $"        }}\r\n" +
                $"    }}\r\n" +
                $"}}";
            var serviceContent =
                $"using VoiceFirstApi.DtoModels;\r\n" +
                $"using VoiceFirstApi.IRepository;\r\n" +
                $"using VoiceFirstApi.IService;\r\n" +
                $"using VoiceFirstApi.Models;\r\n" +
                $"using VoiceFirstApi.Utilities;\r\n" +
                $"namespace VoiceFirstApi.Service\r\n" +
                $"{{\r\n" +
                $"    public class {formattedName}Service : I{formattedName}Service\r\n" +
                $"    {{\r\n" +
                $"        private readonly I{formattedName}Repo _{formattedName}Repo;\r\n\r\n" +
                $"        public {formattedName}Service(I{formattedName}Repo {formattedName}Repo)\r\n" +
                $"        {{\r\n" +
                $"            _{formattedName}Repo = {formattedName}Repo;\r\n" +
                $"        }}\r\n\r\n" +
                $"        private string GetCurrentUserId()\r\n" +
                $"        {{\r\n" +
                $"            return \"abc1\";\r\n" +
                $"            /*var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);\r\n" +
                $"            if (userIdClaim == null)\r\n" +
                $"            {{\r\n" +
                $"                throw new UnauthorizedAccessException(\"User ID not found in the token.\");\r\n" +
                $"            }}\r\n" +
                $"            return userIdClaim.Value;*/\r\n" +
                $"        }}\r\n\r\n" +
                $"        public async Task<(Dictionary<string, object>, string)> AddAsync({formattedName}DtoModel {formattedName}DtoModel)\r\n" +
                $"        {{\r\n" +
                $"            var userId = GetCurrentUserId();\r\n" +
                $"            var data = new Dictionary<string, object>();\r\n" +
                $"            var generatedId = Guid.NewGuid().ToString();\r\n\r\n" +
                $"            var parameters = new\r\n" +
                $"            {{\r\n" +
                $"                Id = generatedId.Trim(),\r\n" +
                $"                Name = {formattedName}DtoModel.name.Trim(),\r\n" +
                $"                InsertedBy = userId.Trim(),\r\n" +
                $"                InsertedDate = DateTime.UtcNow\r\n" +
                $"            }};\r\n\r\n" +
                $"            var status = await _{formattedName}Repo.AddAsync(parameters);\r\n\r\n" +
                $"            if (status > 0)\r\n" +
                $"            {{\r\n" +
                $"                data[\"Items\"] = parameters;\r\n" +
                $"                return (data, StatusUtilities.SUCCESS);\r\n" +
                $"            }}\r\n" +
                $"            else\r\n" +
                $"            {{\r\n" +
                $"                return (data, StatusUtilities.FAILED);\r\n" +
                $"            }}\r\n" +
                $"        }}\r\n\r\n" +
                $"        public async Task<(Dictionary<string, object>, string)> UpdateAsync(Update{formattedName}DtoModel {formattedName})\r\n" +
                $"        {{\r\n" +
                $"            var userId = GetCurrentUserId();\r\n" +
                $"            var data = new Dictionary<string, object>();\r\n" +
                $"            var parameters = new\r\n" +
                $"            {{\r\n" +
                $"                Id = {formattedName}.id,\r\n" +
                $"                Name = {formattedName}.name,\r\n" +
                $"                UpdatedBy = userId,\r\n" +
                $"                UpdatedDate = DateTime.UtcNow\r\n" +
                $"            }};\r\n\r\n" +
                $"            var status = await _{formattedName}Repo.UpdateAsync(parameters);\r\n\r\n" +
                $"            if (status > 0)\r\n" +
                $"            {{\r\n" +
                $"                data[\"Items\"] = parameters;\r\n" +
                $"                return (data, StatusUtilities.SUCCESS);\r\n" +
                $"            }}\r\n" +
                $"            else\r\n" +
                $"            {{\r\n" +
                $"                return (data, StatusUtilities.FAILED);\r\n" +
                $"            }}\r\n" +
                $"        }}\r\n\r\n" +
                $"        public async Task<(Dictionary<string, object>, string)> GetAllAsync(Dictionary<string, string> filters)\r\n" +
                $"        {{\r\n" +
                $"            var data = new Dictionary<string, object>();\r\n" +
                $"            var list = await _{formattedName}Repo.GetAllAsync(filters);\r\n" +
                $"            data[\"Items\"] = list;\r\n" +
                $"            return (data, StatusUtilities.SUCCESS);\r\n" +
                $"        }}\r\n\r\n" +
                $"        public async Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, string> filters)\r\n" +
                $"        {{\r\n" +
                $"            var data = new Dictionary<string, object>();\r\n" +
                $"            var list = await _{formattedName}Repo.GetByIdAsync(id, filters);\r\n" +
                $"            data[\"Items\"] = list;\r\n" +
                $"            return (data, StatusUtilities.SUCCESS);\r\n" +
                $"        }}\r\n\r\n" +
                $"        public async Task<(Dictionary<string, object>, string)> DeleteAsync(string id)\r\n" +
                $"        {{\r\n" +
                $"            var data = new Dictionary<string, object>();\r\n" +
                $"            var list = await _{formattedName}Repo.DeleteAsync(id);\r\n" +
                $"            if (list > 0)\r\n" +
                $"            {{\r\n" +
                $"                return (data, StatusUtilities.SUCCESS);\r\n" +
                $"            }}\r\n" +
                $"            else\r\n" +
                $"            {{\r\n" +
                $"                return (data, StatusUtilities.FAILED);\r\n" +
                $"            }}\r\n" +
                $"        }}\r\n" +
                $"    }}\r\n" +
                $"}}";
            

           var controllerContent =
                $"using Dapper;\r\n" +
                $"using Microsoft.AspNetCore.Http;\r\n" +
                $"using Microsoft.AspNetCore.Mvc;\r\n" +
                $"using System.Net.NetworkInformation;\r\n" +
                $"using VoiceFirstApi.Context;\r\n" +
                $"using VoiceFirstApi.DtoModels;\r\n" +
                $"using VoiceFirstApi.IService;\r\n" +
                $"using VoiceFirstApi.Service;\r\n" +
                $"using static System.Runtime.InteropServices.JavaScript.JSType;\r\n\r\n" +
                $"namespace VoiceFirstApi.Controllers\r\n" +
                            $"{{\r\n" +
                $"    [Route(\"api/{formattedName}\")]\r\n" +
                $"    [ApiController]\r\n" +
                $"    public class {formattedName}Controller : ControllerBase\r\n" +
                $"    {{\r\n" +
                $"        private readonly I{formattedName}Service _{formattedName}Service;\r\n\r\n" +
                $"        public {formattedName}Controller(I{formattedName}Service {formattedName}Service)\r\n" +
                $"        {{\r\n" +
                $"            _{formattedName}Service = {formattedName}Service;\r\n" +
                $"        }}\r\n\r\n" +
                $"        [HttpPost]\r\n" +
                $"        public async Task<IActionResult> AddAsync([FromBody] {formattedName}DtoModel {formattedName}Dto)\r\n" +
                $"        {{\r\n" +
                $"            var (data, status) = await _{formattedName}Service.AddAsync({formattedName}Dto);\r\n" +
                $"            return Ok(new {{ data = data, message = status }});\r\n" +
                $"        }}\r\n\r\n" +
                $"        [HttpPut]\r\n" +
                $"        public async Task<IActionResult> UpdateAsync([FromBody] Update{formattedName}DtoModel {formattedName}Dto)\r\n" +
                $"        {{\r\n" +
                $"            var (data, status) = await _{formattedName}Service.UpdateAsync({formattedName}Dto);\r\n" +
                $"            return Ok(new {{ data = data, message = status }});\r\n" +
                $"        }}\r\n\r\n" +
                $"        [HttpGet]\r\n" +
                $"        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, string> filters)\r\n" +
                $"        {{\r\n" +
                $"            var (data, status) = await _{formattedName}Service.GetAllAsync(filters);\r\n" +
                $"            return Ok(new {{ data = data, message = status }});\r\n" +
                $"        }}\r\n\r\n" +
                $"        [HttpGet(\"{{id}}\")]\r\n" +
                $"        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, string> filters)\r\n" +
                $"        {{\r\n" +
                $"            var (data, status) = await _{formattedName}Service.GetByIdAsync(id, filters);\r\n" +
                $"            return Ok(new {{ data = data, message = status }});\r\n" +
                $"        }}\r\n\r\n" +
                $"        [HttpDelete(\"{{id}}\")]\r\n" +
                $"        public async Task<IActionResult> DeleteAsync(string id)\r\n" +
                $"        {{\r\n" +
                $"            var (data, status) = await _{formattedName}Service.DeleteAsync(id);\r\n" +
                $"            return Ok(new {{ data = data, message = status }});\r\n" +
                $"        }}\r\n" +
                $"    }}\r\n" +
                $"}}";


            var dtoContent =
                $"using System.ComponentModel.DataAnnotations;\r\n\r\n" +
                $"namespace VoiceFirstApi.DtoModels\r\n" +
                $"{{\r\n" +
                $"    public class {formattedName}DtoModel\r\n" +
                $"    {{\r\n" +
                $"        [Required(ErrorMessage = \"Name is required.\")]\r\n" +
                $"        public string name {{ get; set; }}\r\n" +
                $"    }}\r\n" +
                $"    public class Update{formattedName}DtoModel : {formattedName}DtoModel\r\n" +
                $"    {{\r\n" +
                $"        [Required(ErrorMessage = \"Name is required.\")]\r\n" +
                $"        public string id {{ get; set; }}\r\n\r\n" +
                $"    }}\r\n" +
                $"}}";

            var modelContent =
                $"namespace VoiceFirstApi.Models\r\n" +
                $"{{\r\n" +
                $"    public class {formattedName}Model\r\n" +
                $"    {{\r\n" +
                $"        public string id {{ get; set; }}\r\n" +
                $"        public string name {{ get; set; }}\r\n" +
                $"        public string inserted_by {{ get; set; }}\r\n" +
                $"        public DateTime inserted_date {{ get; set; }}\r\n" +
                $"        public string updated_by {{ get; set; }}\r\n" +
                $"        public DateTime updated_date {{ get; set; }}\r\n" +
                $"    }}\r\n" +
                $"}}";


            // Create the folders if they do not exist
            Directory.CreateDirectory(modelFolderPath);
            Directory.CreateDirectory(dtoFolderPath);
            Directory.CreateDirectory(interfaceRepoFolderPath);
            Directory.CreateDirectory(interfaceServiceFolderPath);
            Directory.CreateDirectory(repoFolderPath);
            Directory.CreateDirectory(serviceFolderPath);
            Directory.CreateDirectory(controllerFolderPath);



            // Create files inside the respective folders
            System.IO.File.WriteAllText(modelPath, modelContent, Encoding.UTF8);
            System.IO.File.WriteAllText(dtoPath, dtoContent, Encoding.UTF8);
            System.IO.File.WriteAllText(interfaceRepoPath, interfaceRepoContent, Encoding.UTF8);
            System.IO.File.WriteAllText(interfaceServicePath, interfaceServiceContent, Encoding.UTF8);
            System.IO.File.WriteAllText(repoPath, repoContent, Encoding.UTF8);
            System.IO.File.WriteAllText(servicePath, serviceContent, Encoding.UTF8);
            System.IO.File.WriteAllText(controllerPath, controllerContent, Encoding.UTF8);

            return Ok(new
            {
                success = true,
                message = $"Repository, Interface, Controller, Model, and DTO for {formattedName} created successfully inside their respective folders!"
            });
        }
    }
}
