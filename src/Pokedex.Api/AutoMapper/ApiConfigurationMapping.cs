using AutoMapper;
using Pokedex.Api.ViewModels;
using Pokedex.Application.Models;

namespace Pokedex.Api.AutoMapper
{
    public class ApiConfigurationMapping : Profile
    {
        public ApiConfigurationMapping()
        {  
           
            CreateMap<PokemonDetailModel, PokemonDetailViewModel>();

            CreateMap<PokemonBaseModel, PokemonBaseViewModel>();

            CreateMap<PokemonListModel, PokemonListViewModel>()
                .ForMember(dest => dest.Pokemons, map =>map.MapFrom(s => s.Pokemons));            

            CreateMap<PokemonListPaginadoModel, PokemonListPaginadoViewModel>();
            
        }

    }
}
