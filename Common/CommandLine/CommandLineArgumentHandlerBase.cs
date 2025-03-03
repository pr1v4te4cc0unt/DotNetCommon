namespace Common.CommandLine
{
    public class CommandLineArgumentHandlerBase : ArgumentHandler
    {
        protected readonly List<string> ParameterList;

        public CommandLineArgumentHandlerBase(List<string> parameterList) : base(Environment.GetCommandLineArgs())
        {
            ParameterList = parameterList;
        }

        protected bool IsValidParameter(string parameter)
        {
            return ParameterList.Contains(parameter);
        }

        protected int GetParameterIndex(string parameter)
        {
            if (!IsValidParameter(parameter))
            {
                throw new ArgumentException("Invalid parameter.", parameter);
            }
            return Array.IndexOf(Args, parameter);
        }

        protected string GetParameterArg(string parameter)
        {
            int i = GetParameterIndex(parameter.ToLower());
            if (i >= 0)
            {
                i++;
                if (ParameterList.IndexOf(Args[i]) >= 0)
                {
                    throw new ArgumentException("Commandline parsing error; Parameter/Argument format invalid.", parameter);
                }
                return Args[i];
            }
            return null;
        }

        protected string[] GetParameterArgArray(string parameter)
        {
            int i = GetParameterIndex(parameter.ToLower());
            if (i >= 0)
            {
                i++;
                int j = i;
                while (IsValidIndex(j))
                {
                    if (ParameterList.IndexOf(Args[j]) >= 0)
                    {
                        if (i == j)
                        {
                            throw new ArgumentException("Commandline parsing error; Parameter/Argument format invalid.", parameter);
                        }
                        break;
                    }
                    else
                    {
                        j++;
                    }
                };
                return Args[i..j];
            }
            return null;
        }
    }
}
