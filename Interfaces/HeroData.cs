namespace Interfaces
{
    public class HeroData
    {
        public string Name { get; set; }
        public string AlterEgo { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Name: {this.Name}, Alter ego: {this.AlterEgo}";
        }
    }
}
