using Pokedex.Application.Contracts.v1.Responses;


namespace Pokedex.Test.Mocks;
public static class PokemonResponseMock
{
    public static Pokemon ObterPikachuResponseMock()
    {
        var pikachu = new Pokemon();
        var stats = GetStats();

        pikachu.Id = 25;
        pikachu.Name = "pikachu";
        pikachu.Sprites = new Sprites { FrontDefault = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png"};
        pikachu.Weight = 60;
        pikachu.Height = 4;
        pikachu.Types = GetTypeElements();
        pikachu.Stats = stats;

        return pikachu;

    }

    private static List<TypeElement> GetTypeElements()
    {            
        var typeElement = new TypeElement();
        typeElement.Type = new Species { Name = "electric" };

        var types = new List<TypeElement>();
        types.Add(typeElement);
        return types;
    }

    private static List<StatX> GetStats()
    {

        var stats = new List<StatX>();

        var hp = new StatX
        {
            BaseStat = 35,
            Stat = new Species
            {
                Name = "hp"
            }
        };
        var attack = new StatX
        {
            BaseStat = 55,
            Stat = new Species
            {
                Name = "attack"
            }
        };
        var defense = new StatX
        {
            BaseStat = 40,
            Stat = new Species
            {
                Name = "defense"
            }
        };
        var special_attack = new StatX
        {
            BaseStat = 50,
            Stat = new Species
            {
                Name = "special-attack"
            }
        };

        var special_defense = new StatX
        {
            BaseStat = 50,
            Stat = new Species
            {
                Name = "special-defense"
            }
        };
        var speed = new StatX
        {
            BaseStat = 90,
            Stat = new Species
            {
                Name = "speed"
            }
        };

        stats.Add(hp);
        stats.Add(attack);
        stats.Add(defense);
        stats.Add(special_attack);
        stats.Add(special_defense);
        stats.Add(speed);

        return stats;
    }

    public static PokeList ObterTodosPokemonsResponse(int quantidadePokemons)
    {

        var results = new List<Results>();

        for (int i = 0; i < quantidadePokemons; i++)
        {
            var result = new Results
            {
                Name = $"Pikachu{i}",
                Url = new Uri($"https://pokeapi.co/api/v2/pokemon/{i}/")
            };

            results.Add(result);
        };

        var pokeList = new PokeList
        {
            Results = results

        };

        return pokeList;
    }


}


