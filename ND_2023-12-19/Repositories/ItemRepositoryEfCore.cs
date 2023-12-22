﻿using Microsoft.EntityFrameworkCore;
using ND_2023_12_19.Contexts;
using ND_2023_12_19.Entities;
using ND_2023_12_19.Exceptions;
using ND_2023_12_19.Interfaces;

namespace ND_2023_12_19.Repositories;

public class ItemRepositoryEfCore : IItemRepository
{
    private readonly DataContext _context;

    public ItemRepositoryEfCore(DataContext context)
    {
        _context = context;
    }

    public async Task<int?> Add(ItemEntity item)
    {
        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();
        return item.Id;
    }

    public async Task<int> Delete(int id)
    {
        var itemToDelete = await _context.Items.FindAsync(id)
            ?? throw new NotFoundException("Item not found.");

        _context.Items.Remove(itemToDelete);
        await _context.SaveChangesAsync();

        return 1;
    }

    public async Task<List<ItemEntity>> GetAll()
    {
        return await _context.Items.ToListAsync();
    }

    public async Task<ItemEntity?> GetById(int id)
    {
        return await _context.Items.FindAsync(id);
    }

    public async Task<int> GetCountByName(string name)
    {
        int itemCount = await _context.Items
        .Where(item => item.Name == name)
        .CountAsync();

        return itemCount;
    }

    public async Task<int> Update(ItemEntity item)
    {
        var itemToUpdate = await _context.Items.FindAsync(item.Id);

        if (itemToUpdate == null)
            return 0;

        _context.Entry(itemToUpdate).State = EntityState.Detached;
        _context.Items.Update(item);

        await _context.SaveChangesAsync();

        return 1;
    }
}
