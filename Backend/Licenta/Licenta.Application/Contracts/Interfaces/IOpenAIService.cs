using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Contracts.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> GetBookRecommendationsAsync(string userRequest);
    }
}
