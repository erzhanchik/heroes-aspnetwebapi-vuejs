﻿using Heroes.BusinessLogicLayer.Contracts;
using System;
using System.Collections.Generic;
using Heroes.DataAccessLayer.UnitOfWork;
using Heroes.DataAccessLayer.Models;
using Heroes.DataAccessLayer.GenericRepository;
using Heroes.Data.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace Heroes.BusinessLogicLayer.Services
{
    public class HeroService : BaseService, IHeroService
    {
        private readonly IGenericRepository<Hero> heroRepository;
        private readonly IGenericRepository<Power> powerRepository;

        public HeroService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            heroRepository = this.unitOfWork.GetRepository<Hero>();
            powerRepository = this.unitOfWork.GetRepository<Power>();
        }

        public HeroDTO AddHero(HeroDTO heroDto)
        {
            var hero = Mapper.Map<Hero>(heroDto);
            var powerIds = heroDto?.PowerIDs ?? new List<int>();
            hero.Powers = powerRepository.Get(x => powerIds.Contains(x.ID)).ToList();
            heroRepository.Insert(hero);
            unitOfWork.Save();
            return Mapper.Map<HeroDTO>(hero);
        }

        public HeroDTO DeleteHero(int id)
        {
            var hero = heroRepository.GetById(id);
            var heroDto = Mapper.Map<HeroDTO>(hero);
            heroRepository.Delete(hero);
            unitOfWork.Save();
            return heroDto;
        }

        public IEnumerable<HeroDTO> GetHero()
        {
            return heroRepository.Get(null).ProjectTo<HeroDTO>();
        }

        public HeroDTO GetHeroById(int id)
        {
            var hero = heroRepository.GetById(id);
            return Mapper.Map<HeroDTO>(hero);
        }

        public bool HeroExists(int id)
        {
            var hero = heroRepository.GetById(id);
            return hero != null;
        }

        public void UpdateHero(HeroDTO heroDto)
        {
            var hero = heroRepository.GetById(heroDto.ID);
            Mapper.Map(heroDto, hero);
            heroRepository.LoadCollection(hero, x => x.Powers);
            hero.Powers.Clear();
            var powerIds = heroDto?.PowerIDs ?? new List<int>();
            var newPowers = powerRepository.Get(x => powerIds.Contains(x.ID));
            foreach (var power in newPowers)
            {
                hero.Powers.Add(power);
            }
            unitOfWork.Save();
        }
    }
}
