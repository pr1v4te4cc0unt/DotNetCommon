namespace Users.WebApi.Option
{
    public class DatabaseOptions
    {
        //Database
        public static string Key { get; } = "NeuroIQ:Database";
        //

        //Database
        public string Type { get; set; } = $@"";

        public string ConnectionString { get; set; } = $@"";
        //
    }
}
