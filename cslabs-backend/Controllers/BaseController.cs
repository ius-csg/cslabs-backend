using System;
using System.Linq;
using AutoMapper;
using CSLabsBackend.Models;
using CSLabsBackend.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace CSLabsBackend.Controllers
{
    public class BaseController: ControllerBase
    {
        protected readonly DefaultContext DatabaseContext;
        private readonly IMapper mapper;
        private User user = null;

        protected T Map<T>(object value)
        {
            return mapper.Map<T>(value);
        }
        
        public BaseController(DefaultContext defaultContext, IMapper mapper )
        {
            this.DatabaseContext = defaultContext;
            this.mapper = mapper;
        }

        public User GetUser()
        {
            if (user != null)
                return user;
            try
            {
                user = this.DatabaseContext.Users.SingleOrDefault(u => u.Id == int.Parse(this.User.Identity.Name));
                return user;
            }
            catch (ArgumentNullException) { return null; }
            catch (FormatException) { return null; }
            catch (OverflowException) { return null; }
        }
    }
}