using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Querys
{
    public class GetVoteDataQuery : IRequest<string>
    {
        public int VoteId { get; set; }
    }
}
