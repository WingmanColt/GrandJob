using Dapper;
using HireMe.Core.Helpers;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.StoredProcedures.Enums;
using HireMe.StoredProcedures.Interfaces;
using HireMe.StoredProcedures.Helpers;
using HireMe.StoredProcedures.Models.Jobs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HireMe.StoredProcedures.Services
{
    public class spJobService : IspJobService
    {
        private readonly IConfiguration _config;
        private string ConnectionString { get; set; }
        public spJobService(IConfiguration config)
        {
            _config = config;
           ConnectionString = _config.GetConnectionString("DefaultConnection");
        }

        public IDbConnection Connection { get { return new SqlConnection(ConnectionString); } }

        public async Task<OperationResult> CRUD(object parameters, JobCrudActionEnum action, bool AutoFindParams, string skipAttribute, User user)
        {
            var param = new DynamicParameters();

            if (AutoFindParams)
                param = DapperPropertiesHelper.AutoParameterFind(parameters, skipAttribute);
            else
                param.AddDynamicParams(parameters);

            var model = new { StatementType = action.GetDisplayName() };
            param.AddDynamicParams(model);


            if (action.Equals(JobCrudActionEnum.Create) || action.Equals(JobCrudActionEnum.Update))
            {
                Create jobs = new Create();
                jobs.Update((CreateJobInputModel)parameters, ApproveType.Waiting, user);

                param.Add("@newId", dbType: DbType.Int32, size: 50, direction: ParameterDirection.Output);
            }

            using (IDbConnection connection = Connection)
            {
                try
                {
                    connection.Open();
                    var result = await connection.ExecuteScalarAsync<int>("spJobCRUD", param, commandType: CommandType.StoredProcedure);
                    connection.Close();

                    if (action.Equals(JobCrudActionEnum.Create) || action.Equals(JobCrudActionEnum.Update))
                    {
                        OperationResult.SuccessResult("").Id = param.Get<int?>("@newId"); 
                    }
                    return OperationResult.SuccessResult("");
                }
                catch (Exception ex)
                {
                   return OperationResult.FailureResult(ex.Message);
                }
            }

        }

        
        public async Task<IAsyncEnumerable<T>> GetAll<T>(object parameters, JobGetActionEnum state, bool AutoFindParams, string skipAttribute)
        {
            var param = new DynamicParameters();

            if(AutoFindParams)
                param = DapperPropertiesHelper.AutoParameterFind(parameters, skipAttribute);
            else
                param.AddDynamicParams(parameters);

            var model = new { StatementType = state.GetDisplayName(), Id = 0 };
            param.AddDynamicParams(model);
            
            using (IDbConnection connection = Connection)
            {
                connection.Open();
                var result = await connection.QueryAsync<T>("spJobCRUD", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                connection.Close();

                return result.ToAsyncEnumerable();
            }
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            using (IDbConnection connection = Connection)
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<T>("spJobCRUD", new { Id = id > 0 ? id : 0, StatementType = "Select" }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                connection.Close();

                return result;
            }
        }

        public async Task<int> GetAllCountBy(object parameters)
        {
            var param = new DynamicParameters();
            var model = new { StatementType = "GetAllCountBy", Id = 0 };
            param.AddDynamicParams(model);
            param.AddDynamicParams(parameters);

            using (IDbConnection connection = Connection)
            {
                connection.Open();
                var result = await connection.ExecuteScalarAsync<int>("spJobCRUD", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                connection.Close();

                return result;
            }
        }

        public async Task<bool> AddRatingToJobs(object parameters)
        {
            var param = new DynamicParameters();
            var model = new { StatementType = "AddRatingToJobs" };
            param.AddDynamicParams(model);
            param.AddDynamicParams(parameters);

            using (IDbConnection connection = Connection)
            {
                connection.Open();
                var result = await connection.ExecuteAsync("spJobCRUD", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                connection.Close();

                return result > 0;
            }
        }
    }

}
