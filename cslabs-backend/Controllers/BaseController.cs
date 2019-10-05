using System;
using System.Linq;
using AutoMapper;
using CSLabsBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSLabsBackend.Controllers
{
    public class BaseController: ControllerBase
    {
        protected readonly DefaultContext DatabaseContext;
        private readonly IMapper mapper;

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
            try { return this.DatabaseContext.Users.SingleOrDefault(u => u.Id == int.Parse(this.User.Identity.Name)); }
            catch (ArgumentNullException) { return null; }
            catch (FormatException) { return null; }
            catch (OverflowException) { return null; }
        }
    }
}