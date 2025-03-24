using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenant.API.Base.Repository;
using Tenant.API.Base.Service;
using Tenant.API.Base.Model;

namespace Tenant.Query.Service.User
{
    public class UserService : TnBaseService
    {
        #region Variables

        private Repository.User.UserRepository UserRepository;

        #endregion

        public UserService(Repository.User.UserRepository userRepository, ILoggerFactory loggerFactory, TnAudit xcAudit, TnValidation xcValidation) : base(xcAudit, xcValidation)
        {
            this.UserRepository = userRepository;
            this.UserRepository.Logger = loggerFactory.CreateLogger<Repository.User.UserRepository>();
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="userId">User identifier.</param>
        public Model.User.User GetUser(string tenantId, string userId)
        {
            try
            {
                //Logger
                this.Logger.LogInformation($"Calling GetUser({tenantId}, {userId})");

                //Local variale
                Model.User.User users = new Model.User.User();

                //getting user information
                Task<Model.User.User> task = this.UserRepository.GetById(tenantId, null, userId);

                //this.Logger.LogInformation($"Called GetUser({tenantId}, {userId})");

                //if (task.Result != null)
                //    users.AddItem(task.Result);

                return users;
            }
            catch (System.Exception ex)
            {
                //Error logger
                this.Logger.LogError($"GetUser Error({ex.Message}) : {ex.InnerException}");

                throw ex;
            }
        }


        /// <summary>
        /// Get list of roles
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        internal List<Model.User.Role> GetRoles(string[] roleId)
        {
            try
            {
                //Logger 
                this.Logger.LogInformation($"Calling GetRoles");

                //Local variable
                List<Model.User.Role> roles = new List<Model.User.Role>();

                //Get roles
                roles = this.UserRepository.GetRoles(roleId).Result;

                //Logger
                this.Logger.LogInformation($"Called GetRoles");

                //return 
                return roles;
            }
            catch (System.Exception ex)
            {
                //Error logger
                this.Logger.LogError($"GetUser Error({ex.Message}) : {ex.InnerException}");
                throw;
            }
        }
    }
}
