using Domain.Models;
using MediatR;
using Persistence.YDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Querys
{
    public class CheckLinkQueryHandler : IRequestHandler<CheckLinkQuery, bool>
    {
        private readonly YDBContext _ydbContext;
        public CheckLinkQueryHandler(YDBContext ydbContext) =>
            _ydbContext = ydbContext;

        public async Task<bool> Handle(CheckLinkQuery request, CancellationToken cancellationToken)
        {
            //false - ссылка отсутствует
            var link = await _ydbContext.GetLink(request.Id, request.GroupId);

            if (link == null)  return false;

            var checkLink = await CheckExpirationDateLink(link);

            return checkLink;
        }

        public async Task<bool> CheckExpirationDateLink(TemporalLink link)
        {
            DateTime dateNow = await _ydbContext.GetDateNow();

            if (dateNow.CompareTo(link.DestroyDate) >= 0)
                return false;
            return true;
        }
    }
}
