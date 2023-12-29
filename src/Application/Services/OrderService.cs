using Application.DTOs.Requests;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class OrderService : IOrderService
{
    private readonly IItemRepository _itemRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public OrderService(
        IItemRepository itemRepository,
        IUserService userService,
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _itemRepository = itemRepository;
        _userService = userService;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<int> Create(OrderRequest request)
    {
        var item = await _itemRepository.GetById(request.ItemId)
            ?? throw new NotFoundException("Item not found.");

        try
        {
            await _userService.GetById(request.UserId);
        }
        catch (Exception)
        {
            throw new NotFoundException("User not found. ");
        }

        OrderEntity order = _mapper.Map<OrderEntity>(request);

        return await _orderRepository.Add(order)
            ?? throw new Exception("Failed to create an order.");
    }
}
