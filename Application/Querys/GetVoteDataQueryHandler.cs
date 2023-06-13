using MediatR;
using Persistence.YDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Querys
{
    public class GetVoteDataQueryHandler : IRequestHandler<GetVoteDataQuery, string>
    {
        private readonly YDBContext _ydbContext;
        public GetVoteDataQueryHandler(YDBContext ydbContext) =>
            _ydbContext = ydbContext;

        public async Task<string> Handle(GetVoteDataQuery request, CancellationToken cancellationToken)
        {
            var voteData = await _ydbContext.GetVoteDataById(request.VoteId);

            if (voteData == "")
            {
                return "null";
            }

            return voteData;
        }
    }
}
