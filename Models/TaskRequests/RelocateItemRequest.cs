﻿namespace OrderPickingSystem.Models.TaskRequests;

public class RelocateItemRequest
{
    public int Id { get; set; }
    public int ItemId { get; set; } //TODO Maybe Unneded
    public int InitialLocationId { get; set; }
    public int DestinationLocationId { get; set; }
}