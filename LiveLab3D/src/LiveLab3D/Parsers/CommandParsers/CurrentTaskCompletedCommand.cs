
namespace LiveLab3D.Parsers.CommandParsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LiveLab3D.Commands;

	[Parses(CommandNumber = 10011)]
	public class CurrentTaskCompletedCommandParser : ReflectionCommandParser<CurrentTaskCompletedCommand>
	{
        public CurrentTaskCompletedCommandParser()
			: base(cmd => cmd.TaskType,
			       cmd => cmd.X,
			       cmd => cmd.Y,
			       cmd => cmd.Z,
			       cmd => cmd.Heading,
			       cmd => cmd.DontCare,
                   cmd => cmd.DontCare2,
             cmd => cmd.DontCare3)
		{
		}
	}
}
