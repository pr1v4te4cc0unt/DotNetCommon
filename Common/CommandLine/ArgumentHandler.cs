namespace Common.CommandLine
{
    public class ArgumentHandler
    {
        public string[] Args;

        public ArgumentHandler(string[] args)
        {
            Args = args;
        }

        protected bool IsValidIndex(int index)
        {
            return index >= 0 && index < Args.Length;
        }
    }
}
