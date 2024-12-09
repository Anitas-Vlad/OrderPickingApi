﻿using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Models.TaskRequests;

namespace OrderPickingSystem.Models.Orders;

public class RelocatingOrder : Order
{
    public Item? Item { get; set; }
    public Queue<RelocateItemRequest> Requests { get; set; }

    public RelocatingOrder()
    {
        RequiredRole = UserRole.Relocator;
    }
}