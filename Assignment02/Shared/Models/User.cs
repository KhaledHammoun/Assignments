namespace Model
{
    public class User : Adult
    {
        public string Username { set; get; }
        public  string Password { set; get; }
        public string SecurityLevel { set; get; }

        public User()
        {
            
        }

        public User(int id, string firstName, string lastName, string hairColor, string eyeColor, int age, float weight, int height, string sex, Job jobTitle, string username, string password, string securityLevel) : base(id, firstName, lastName, hairColor, eyeColor, age, weight, height, sex, jobTitle)
        {
            Username = username;
            Password = password;
            SecurityLevel = securityLevel;
        }
    }
}