using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{
	public class MockCommandRepo : ICommanderRepo
	{
		public void CreateCommand(Command cmd)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<Command> GetAllCommands()
		{
			var commands = new List<Command>{
				new Command{
					Id=0,
					HowTo="Boil an egg",
					Line="Boil Water",
					Platform="Pan"
				},
				new Command{
					Id=1,
					HowTo="Cut bread",
					Line="get a knife",
					Platform="board"
				},
				new Command{
					Id=2,
					HowTo="Make cup of tea",
					Line="Place teabag in cup",
					Platform="Kettle & cup"
				}
			};
			return commands;
		}

		public Command GetCommandById(int id)
		{
			return new Command
			{
				Id = 0,
				HowTo = "Boil an egg",
				Line = "Boil Water",
				Platform = "Pan"
			};
		}

		public bool SaveChanges()
		{
			throw new System.NotImplementedException();
		}

		public void UpdateCommand(Command cmd)
		{
			throw new System.NotImplementedException();
		}
	}
}


// W I T H O U T   D T O   A N D   P R O F I L E


/*

Fake data - proving a point of what repo should be
eveentually we will get to the point of acctually
implementing prolerly tthe interface
Connection to DB obviosly

*/