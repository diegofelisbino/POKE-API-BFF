using Bogus;
using Moq.AutoMock;
using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Models;
using Pokedex.Application.Services;
using Pokedex.Domain.Services;

namespace Pokedex.Test.Mocks;

[CollectionDefinition(nameof(PokemonCollection))]
public class PokemonCollection : ICollectionFixture<PokemonTestsFixture> { }
public class PokemonTestsFixture : IDisposable
{    
    public AutoMocker Mocker;   

    public PokemonService ObterPokemonService()
    {
        Mocker = new AutoMocker();
        var pokemonService = Mocker.CreateInstance<PokemonService>();        

        return pokemonService;
    }

    public Pokemon GerarPokemonResponseValido()
    {
        var spritesFake = new Faker<Sprites>();
        spritesFake.RuleFor(s => s.FrontDefault, f => f.Internet.Url());

        var pokemonFake = new Faker<Pokemon>();
        pokemonFake.RuleFor(p => p.Id, f => Convert.ToInt64(f.Random.Enum<PokemonEnum>()));
        pokemonFake.RuleFor(p => p.Name, f => f.Random.Enum<PokemonEnum>().ToString());
        pokemonFake.RuleFor(p => p.Sprites, f => spritesFake);
        pokemonFake.RuleFor(p => p.Weight, f => f.Random.Int(10, 90));
        pokemonFake.RuleFor(p => p.Height, f => f.Random.Int(1, 10));
        pokemonFake.RuleFor(p => p.Types, f => GetTypeElements());
        pokemonFake.RuleFor(p => p.Stats, f => GetStats());
        pokemonFake.Generate();

        return pokemonFake;
    }
    public Pokemon GerarPokemonResponseSemStats()
    {
        var spritesFake = new Faker<Sprites>();
        spritesFake.RuleFor(s => s.FrontDefault, f => f.Internet.Url());

        var pokemonFake = new Faker<Pokemon>();
        pokemonFake.RuleFor(p => p.Id, f => Convert.ToInt64(f.Random.Enum<PokemonEnum>()));
        pokemonFake.RuleFor(p => p.Name, f => f.Random.Enum<PokemonEnum>().ToString());
        pokemonFake.RuleFor(p => p.Sprites, f => spritesFake);
        pokemonFake.RuleFor(p => p.Weight, f => f.Random.Int(10, 90));
        pokemonFake.RuleFor(p => p.Height, f => f.Random.Int(1, 10));
        pokemonFake.RuleFor(p => p.Types, f => GetTypeElements());
        pokemonFake.RuleFor(p => p.Stats, f => new List<StatX>());
        pokemonFake.Generate();

        return pokemonFake;

    }
    public Pokemon GerarPokemonResponseComUmStats()
    {
        var pokemon = this.GerarPokemonResponseValido();
        var stat = pokemon.Stats.Take(1).FirstOrDefault();
        pokemon.Stats.Clear();
        pokemon.Stats.Add(stat);

        return pokemon;

    }
    public PokemonDetailModel GerarPokemonModelValido()
    {
        var response = GerarPokemonResponseValido();

        return new PokemonDetailModel

        {
            Id = response.Id,
            Nome = response.Name,
            ImagemUri = new Uri(response.Sprites.FrontDefault),
            Peso = response.Weight,
            Altura = response.Height,
            Elementos = response.Types.Select(x => x.Type.Name).ToList()
        };
    }
    public PokeList GerarPokeListValido(int quantidadePokemons)
    {
        var pokeList = new PokeList
        {
            Results = GerarResults(quantidadePokemons)

        };

        return pokeList;
    }
    public PokemonListModel GerarPokemonListModel(PokeList pokeList)
    {
        var pokemons = new List<PokemonBaseModel>();
        long id = 1;
        foreach (var result in pokeList.Results)
        {
            PokemonBaseModel pokemonBase = new PokemonBaseModel();
            pokemonBase.Id = id;
            pokemonBase.Nome = result.Name;
            pokemons.Add(pokemonBase);

            id++;
        }

        var pokemonListModel = new PokemonListModel();
        pokemonListModel.Results = pokeList.Results;
        //pokemonListModel.Pokemons = pokemons;        

        return pokemonListModel;
    }
    private List<TypeElement> GetTypeElements()
    {
        var typeElement = new TypeElement();
        typeElement.Type = new Species { Name = "electric" };

        var types = new List<TypeElement>();
        types.Add(typeElement);
        return types;
    }
    private List<StatX> GetStats()
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
    private List<Results> GerarResults(int quantidadePokemons)
    {

        var resultsFake = new Faker<Results>();
        resultsFake.RuleFor(r => r.Name, f => f.Random.Enum<PokemonEnum>().ToString());
        resultsFake.RuleFor(r => r.Url, f => new Uri($"https://pokeapi.co/api/v2/pokemon/{f.Random.Int(1, 10)}"));

        var results = resultsFake.Generate(quantidadePokemons);

        return results;
    }
    public Dictionary<string, long> GerarNiveisDePoderPorStats(List<StatX> stats)
    {
        var niveisDePoder = new Dictionary<string, long>();

        if (stats.Count > 0)
        {
            foreach (var stat in stats)
            {
                niveisDePoder.Add(stat.Stat.Name, stat.BaseStat);
            }

            string maxStat = "max-stat";
            niveisDePoder.Add(maxStat, stats.Select(s => s.BaseStat).Max());
        }

        return niveisDePoder;
    }
    public void Dispose()
    {

    }
}

public enum PokemonEnum
{
    bulbasaur = 1,
    ivysaur = 2,
    venusaur = 3,
    charmander = 4,
    charmeleon = 5,
    charizard = 6,
    squirtle = 7,
    wartortle = 8,
    blastoise = 9,
    caterpie = 10
}


