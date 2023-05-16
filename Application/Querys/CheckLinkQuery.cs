using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Querys
{
    public class CheckLinkQuery : IRequest<bool>
    {
        public string Id { get; set; }
        public UInt64 GroupId { get; set; }
    }
}
