﻿using System.Diagnostics.Metrics;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface ICountryRepo
    {
        Task<IEnumerable<CountryModel>> GetAllAsync(Dictionary<string, string> filters);
        Task<IEnumerable<CountryModel>> GetAscAll(Dictionary<string, string> filters);
        Task<CountryModel> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> DeleteAsync(string id);
        Task<int> UpdateStatus(string id, int status);
    }
}