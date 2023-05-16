using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydb.Sdk.Auth;
using Ydb.Sdk.Yc;

namespace Persistence.YDB
{
    public class AuthUtils
    {
        public static async Task<ICredentialsProvider> MakeCredentialsFromEnv()
        {
            try
            {
                var rootPathProject = Directory.GetCurrentDirectory();
                var saFileValue = Path.Combine(rootPathProject, Environment.GetEnvironmentVariable("SA_KEY_FILE"));
                await Console.Out.WriteLineAsync(Environment.GetEnvironmentVariable("SA_KEY_FILE"));
                if (!string.IsNullOrEmpty(saFileValue))
                {
                    var saProvider = new ServiceAccountProvider(
                    saFilePath: saFileValue);
                    await saProvider.Initialize();
                    return saProvider;
                }
            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw new Exception("Произошла какая-то ошибка при подключении к удаленной базе данных");
            }
            return null;
        }
    }
}
