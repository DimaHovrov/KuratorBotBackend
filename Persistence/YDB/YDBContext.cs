using System.Text.RegularExpressions;
using Ydb.Sdk;
using Ydb.Sdk.Value;
using Ydb.Sdk.Table;
using Ydb.Table;
using System;
using Domain.Models;

namespace Persistence.YDB
{
    public class YDBContext
    {
        private TableClient Client { get; set; }
        static Driver driver;

        public YDBContext()
        {
            Client = new TableClient(driver, new TableClientConfig());
        }

        public static async Task Run()
        {
            var endpoint = Environment.GetEnvironmentVariable("YDB_ENDPOINT");
            var database = Environment.GetEnvironmentVariable("YDB_DATABASE");
            var credentialsProvider = await AuthUtils.MakeCredentialsFromEnv();

            var config = new DriverConfig(
                endpoint,
                database,
                credentialsProvider
            );

            driver = new Driver(
                config: config
            );

            await driver.Initialize();
        }

        public async Task<string> GetInfoMessages()
        {
            var query = @$"select * from InfoMessages where id = 9";
            var result = await CreateRequest(query);

            return (string)result.Rows[0]["Message"];//сработало
        }

        public async Task CreateLink(TemporalLink link)
        {
            var query = @$"insert into TempRegLinks(id, CreationDate, DestroyDate, GroupId)
                            values ('{link.Id}', DateTime('{link.CreationDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}'), DateTime('{link.DestroyDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}'), {link.GroupId})";
            var result = await CreateRequest(query);
        }

        public async Task<TemporalLink> GetLinkByGroup(UInt64 groupId)
        {
            var query = $@"select id,CreationDate,DestroyDate,GroupId 
                           from TempRegLinks
                           where GroupId = {groupId}";
            var result = await CreateRequest(query);


            if (result.Rows.Count == 0)
                return null;

            var CreationDate = (DateTime)result.Rows[0]["CreationDate"].GetOptionalDatetime();
            var DestroyDate = (DateTime)result.Rows[0]["DestroyDate"].GetOptionalDatetime();
            var GroupId = (UInt64)result.Rows[0]["GroupId"].GetOptionalUint64();

            var link = new TemporalLink
            {
                Id = (string)result.Rows[0]["id"],
                CreationDate = CreationDate,
                DestroyDate = DestroyDate,
                GroupId = GroupId
            };

            return link;
        }


        public async Task<DateTime> GetDateNow()
        {
            var query = $@"select CurrentUtcDatetime() as DateNow";
            
            var result = await CreateRequest(query);

            var dateNow = (DateTime)result.Rows[0]["DateNow"];

            return dateNow;
        }

        public async Task<bool> RegStudent(User user)
        {
            try
            {
                var maxId = await GetMaxIdInUsers() + 1;
                var query = $@"insert into Users
                       (Id, GroupsId, IsStudent,Surname,Name,Patronymic,PhoneNumber)
                       values({maxId}, {user.GroupsId}, true,'{user.Surname}','{user.Name}', 
                             '{user.Patronymic}','{user.PhoneNumber}')";

                var result = await CreateRequest(query);
                return true;
            }
            catch (Exception error)
            { 
                Console.WriteLine(error.Message);
                return false;
            }
        }

        public async Task<bool> CheckStudentByNumber(string number)
        {
            var query = $@"select * from Users where PhoneNumber = '{number}'";
            var result = await CreateRequest(query);

            return result.Rows.Count != 0;
        }

        public async Task<TemporalLink> GetLink(string guid, UInt64 groupId)
        {
            var query = $@"select * 
                           from TempRegLinks 
                           where id = '{guid}' 
                           and GroupId = {groupId}";
            var result = await CreateRequest(query);

            if (result.Rows.Count == 0)
                return null;

            var link = new TemporalLink
            {
                Id = (string)result.Rows[0]["id"],
                CreationDate = (DateTime)result.Rows[0]["CreationDate"].GetOptionalDatetime(),
                DestroyDate = (DateTime)result.Rows[0]["DestroyDate"].GetOptionalDatetime(),
                GroupId = (UInt64)result.Rows[0]["GroupId"].GetOptionalUint64()
            };

            
            return link;
        }

        public async Task<UInt64> GetMaxIdInUsers()
        {
            var query = $@"select max(Id) as Id from Users";
            var result = await CreateRequest(query);

            return (UInt64)result.Rows[0]["Id"].GetOptionalUint64();
        }

        private async Task<ResultSet> CreateRequest(string query)
        {
            var response = await Client.SessionExec(async session =>
            {
                return await session.ExecuteDataQuery(
                    query: query,
                    txControl: TxControl.BeginSerializableRW().Commit()
                );
            });

            response.Status.EnsureSuccess();
            var queryResponse = (Ydb.Sdk.Table.ExecuteDataQueryResponse)response;

            if (queryResponse.Result.ResultSets.Count == 0)
                return null;

            var result = queryResponse.Result.ResultSets[0];

            return result;
        }
    }
}
