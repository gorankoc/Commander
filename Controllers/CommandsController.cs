using System.Collections.Generic;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;
using Commander.Data;
using AutoMapper;
using Commander.Dtos;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;

namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController:ControllerBase
    {
        private readonly IMapper _mapper;
        private ICommanderRepo _repository;

        public CommandsController(ICommanderRepo repository, IMapper mapper){
            _mapper = mapper;
            _repository = repository;			
        }		

        [HttpGet]
        public ActionResult <IEnumerable<Command>> GetAllCommands(){
            var commandItems = _repository.GetAllCommands();            
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }
        
        [HttpGet("{id}", Name="GetCommandById")]
        public ActionResult <CommandReadDto> GetCommandById(int id){

            var commandItem = _repository.GetCommandById(id);
            // if id is invalid resource does not exist ... it will return no content 204 error code
            // we want content not found instead error code
            if(commandItem != null){
                return Ok(_mapper.Map<CommandReadDto>(commandItem));			
            }else{
                return NotFound();
            }
        }
        //POST api/commands/
        [HttpPost]
        public ActionResult <CommandReadDto> CreateCommand(CommandUpdateDto commandCreateDto){
            //validation goes here
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();
            
            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);            
            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id}, commandReadDto);
             //return Ok(commandModel); //does not return URI as REST Specs

            //return Ok(commandReadDto); //does not return URI as REST Specs
            // we want to return back CommandReadDTO not Command model with platform !!!
            // REST requires u should return URI where that resource exists, u can see that in
            // header ... no URI ... and we should be returning 201 response
        }
        //PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto){
            // check if resource exists
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null){
                return NotFound(); // 404
            }
            // update Profile for mapping
            // different way, previously we were mapping from a model that contained data 
            // to a new or empty DTO 
            // var commandItem =  _repository.getCommandById(id);
            // return Ok(_mapper.Map<CommanderReadDto>(commandItem);
            // 2:54:00
            
            // maps the profile!
            _mapper.Map(commandUpdateDto, commandModelFromRepo);
            
            // this mapping has updated this model Command commandModelFromRepo 
            // we dont have to do anything else our SqlCommanderRepo.UpdateCommand is not
            // implemented, we just need to SaveChanges 


            // however in the interest of maintaining seperate Interface from 
            // implementation, good practice is still to call Update method on repo
            // and supply our commandModel from repo, because other implementations 
            // might need that ... so even in this case its useless we still call.
            _repository.UpdateCommand(commandModelFromRepo);

            // will flush our changes made by _mapper.Map step to db
            _repository.SaveChanges(); 

            return NoContent(); // 204 ! 

        }
        //PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null){
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);

            if(!TryValidateModel(commandToPatch)){
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

    }
}

/*
1. apicontroller atrtibut - out of the box behaviours
2. Route - hoe to get ot resources - basic Controller level

other example:

//api/commands WUILDCARD approuch
[Route("api/[commands]")]

by contract: 

[Route("api/commands")] decorates [HttepGet] so when we make that
call it should hit this endpoint.

3. use repository and pull that data and repesent to the user

4. GET api/commands/{id}
   when we request via this endopoint it will be
   resolved via URI ... That comes from somthing called
   Binding Sourdces ( Model Binding for HTTP GET requests )
   from query, ROUTE, form, body, Header.

   WHAT EVER ID u put api/commands/100 it allways return 
                   Id = 0,
                HowTo = "Boil an egg",
                Line = "Boil Water",
                Platform = "Pan"

*/