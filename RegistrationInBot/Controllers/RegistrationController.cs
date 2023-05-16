using Application.Commands;
using Application.Common;
using Application.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.YDB;

namespace RegistrationInBot.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class RegistrationController : ControllerBase
    {
        private readonly YDBContext _ydbContext;
        private readonly IMediator _mediatR;
        public RegistrationController(IMediator mediatR, 
            YDBContext ydbContext)
        {
            _ydbContext = ydbContext;
            _mediatR = mediatR;
        }

        [HttpGet("CreateLink")]
        public async Task<string> CreateLinkAsync([FromQuery] UInt64 groupId)
        {
            var createLinkCommand = new CreateLinkCommand { GroupId = groupId };
            var result = await _mediatR.Send(createLinkCommand);//groupId

            if (result != General.LinkExist)
                result = $"{General.RegHost}?Id={result}&GroupId={groupId} ";
            
            return result;
        }

        [HttpPost("RegStudent")]
        public async Task<bool> RegStudentAsync([FromBody] RegStudentCommand regStudentCommand)
        {
            var result = await _mediatR.Send(regStudentCommand);

            return result;
        }

        [HttpGet("CheckListExist")]
        public async Task<bool> CheckListExistAsync([FromQuery] string id, UInt64 groupId)
        {
            var checkLinkQuery = new CheckLinkQuery { Id = id, GroupId = groupId };
            var result = await _mediatR.Send(checkLinkQuery);
            return result;
        }
    }
}
