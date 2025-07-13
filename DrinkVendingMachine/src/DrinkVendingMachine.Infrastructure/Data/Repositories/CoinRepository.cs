using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Interfaces;

namespace DrinkVendingMachine.Infrastructure.Data.Repositories;

public class CoinRepository(ApplicationDbContext context) : BaseRepository<Coin>(context), ICoinRepository;