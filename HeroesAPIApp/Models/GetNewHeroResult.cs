using Newtonsoft.Json;


namespace HeroesAPIApp.Models
{
    
    public class Rootobject
    {
        
        public string response { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public Powerstats powerstats { get; set; }
        public Biography biography { get; set; }
        public Appearance appearance { get; set; }
        public Work work { get; set; }
        public Connections connections { get; set; }
        public Image image { get; set; }
    }

    public class Powerstats
    {
         
        public string intelligence { get; set; }
        public string strength { get; set; }
        public string speed { get; set; }
        public string durability { get; set; }
        public string power { get; set; }
        public string combat { get; set; }
    }

    public class Biography
    {
        
        public string fullname { get; set; }
        public string alteregos { get; set; }
        public string[] aliases { get; set; }
        public string placeofbirth { get; set; }
        public string firstappearance { get; set; }
        public string publisher { get; set; }
        public string alignment { get; set; }
    }

    public class Appearance
    {
        
        public string gender { get; set; }
        public string race { get; set; }
        public string[] height { get; set; }
        public string[] weight { get; set; }
        public string eyecolor { get; set; }
        public string haircolor { get; set; }
    }

    public class Work
    {
        public int Id { get; set; }
        public string occupation { get; set; }
        public string _base { get; set; }
    }

    public class Connections
    {
        
        public string groupaffiliation { get; set; }
        public string relatives { get; set; }
    }

    public class Image
    {
         
        public string url { get; set; }
    }



}
