using Application.Common;
using Domain.Models;
using MediatR;
using Persistence.YDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ydb.Monitoring.SelfCheck.Types;

namespace Application.Commands
{
    public class CreateLinkCommandHandler : IRequestHandler<CreateLinkCommand, string>
    {
        private readonly YDBContext _ydbContext;

        public CreateLinkCommandHandler(YDBContext ydbContext) =>
            _ydbContext = ydbContext;

        public async Task<string> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
        {
            ulong groupId = request.GroupId;

            var checkLink = await CheckGroupLink(groupId);

            if (checkLink)
                return General.LinkExist;//временная ссылка для данной группы уже создана

            var dateNow = await _ydbContext.GetDateNow();
            var newLink = new TemporalLink
            {
                Id = Guid.NewGuid().ToString(),
                CreationDate = await _ydbContext.GetDateNow(),
                DestroyDate = dateNow.AddHours(General.LifeTime),
                GroupId = groupId
            };

            await _ydbContext.CreateLink(newLink);
            return newLink.Id;
        }

        public async Task<bool> CheckGroupLink(ulong groupId)
        {
            //Проверяет есть ли существующая ссылка для данной группы
            //false - нет существующей ссылки
            //создать второй раз временную ссылку нельзя
            var link = await _ydbContext.GetLinkByGroup(groupId);

            if (link == null) return false;

            DateTime dateNow = await _ydbContext.GetDateNow();

            if (dateNow.CompareTo(link.DestroyDate) >= 0)
                return false;

            return true;
        }
    }
}
