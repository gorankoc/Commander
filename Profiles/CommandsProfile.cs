using AutoMapper;
using Commander.Dtos;
using Commander.Models;

namespace Commander.Profiles
{
    public class CommandsProfile: Profile
    {
        public CommandsProfile()
        {
            // map from source to dest obj.
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDTO, Command>();
        }
    }
}