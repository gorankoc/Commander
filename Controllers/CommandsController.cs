using System.Collections.Generic;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;
using Commander.Data;

namespace Commander.Controllers
{
	[Route("api/commands")]
	[ApiController]
	public class CommandsController:ControllerBase
	{
		//3. not good way just for mock ... and fast start 
		private readonly MockCommandRepo _repository = new MockCommandRepo();

		[HttpGet]
		public ActionResult <IEnumerable<Command>> GetAllCommands(){
			var commandItems = _repository.GetAllCommands();
			return Ok(commandItems);
		}
		// 4. GET api/commands/{id}
		[HttpGet("{id}")]
		public ActionResult <Command> GetCommandById(int id){
			var commandItem = _repository.GetCommandById(id);
			return Ok(commandItem);			
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

   WHAT EVER ID u put api/commands/100 it allways return id 0
   from MockCommandRepo
			{
   				Id = 0,
				HowTo = "Boil an egg",
				Line = "Boil Water",
				Platform = "Pan"
			}

*/