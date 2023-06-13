using Application.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.YDB;

namespace RegistrationInBot.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class VoteController : ControllerBase
    {
        private readonly YDBContext _ydbContext;
        private readonly IMediator _mediatR;
        public VoteController(IMediator mediatR,
            YDBContext ydbContext) 
        {
            _ydbContext = ydbContext;
            _mediatR = mediatR;
        }

        /// <summary>
        /// Возращает vote Data по voteId
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        [HttpGet("GetVoteData")]
        public async Task<string> GetVoteDataAsync([FromQuery] int voteId)
        {
            var getVoteDataQuery = new GetVoteDataQuery { VoteId = voteId };

            var result = await _mediatR.Send(getVoteDataQuery);
            return result;
        }
    }
}
