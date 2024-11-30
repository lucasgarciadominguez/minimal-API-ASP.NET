///example of a brewery with beers
namespace MinimalAPI
{
    public class Repository
    {
        List<Brewery> breweries=new List<Brewery>()
        {
            new Brewery(){ Id=1, Name= "Minerva"},
            new Brewery(){ Id=2, Name= "Belching Beaver"},
            new Brewery(){ Id=3, Name= "Samichlaus"}
        };

        public List<Brewery> GetBreweries() => breweries; //returns the brewery List
        public Brewery? GetBrewery(int id) => breweries.Find(x => x.Id == id);  //gets the brewery who matches the id desired
    }

    public class Brewery
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
