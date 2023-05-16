using Domain.Models;
using MediatR;
using Persistence.YDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class RegStudentCommandHandler : IRequestHandler<RegStudentCommand, bool>
    {
        private readonly YDBContext _ydbContext;

        public RegStudentCommandHandler(YDBContext ydbContext) =>
            _ydbContext = ydbContext;

        public async Task<bool> Handle(RegStudentCommand student, CancellationToken cancellationToken)
        {
            var checkStudentInDb = await _ydbContext.CheckStudentByNumber(student.PhoneNumber);
            
            if (checkStudentInDb)
                return false;

            try
            {
                var user = new User
                {
                    GroupsId = student.GroupsId,
                    Surname = student.Surname,
                    Name = student.Name,
                    Patronymic = student.Patronymic,
                    PhoneNumber = student.PhoneNumber
                };

                var regStudent = await _ydbContext.RegStudent(user);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return false;
        }
    }
}
