using AutoMapper;
using Models.DtoModels;
using Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class AutoMapper
    {
        public IMapper mapper;
        public AutoMapper()
        {
            MapperConfiguration conf = new MapperConfiguration(cfg => {
                cfg.CreateMap<User,UserDTO>();
                cfg.CreateMap<UserDTO, User>();
            });

            mapper = conf.CreateMapper();

        }
         
    }
}
