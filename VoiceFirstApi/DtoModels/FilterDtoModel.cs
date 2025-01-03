namespace VoiceFirstApi.DtoModels
{
    public class FilterDtoModel
    {
        public Dictionary<string, string>? filters {  get; set; }
    }
    public class FiltersAndIdDtoModel:FilterDtoModel
    {
        public string id { get; set; }
    }
}
