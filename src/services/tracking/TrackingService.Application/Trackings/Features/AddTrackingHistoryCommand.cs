﻿using FastDelivery.Service.Tracking.Application.Trackings.Dtos;
using FastDelivery.Service.Tracking.Domain;
using MapsterMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Service.Tracking.Application.Trackings.Features;
public record AddTrackingHistoryCommand(string ParcelId, string InvoiceId, string TrackingStatus) : IRequest<Guid>
{ }

public sealed class Handler : IRequestHandler<AddTrackingHistoryCommand, Guid>
{
    private readonly ITrackingRepository _trackingRepository;
    private readonly IMapper _mapper;

    public Handler(ITrackingRepository trackingRepository, IMapper mapper)
    {
        _trackingRepository = trackingRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(AddTrackingHistoryCommand request, CancellationToken cancellationToken)
    {
        var parcelTrackingHistory = new ParcelTrackingHistory()
        {
            InvoiceId = request.InvoiceId,
            ParcelId = request.ParcelId,
            TrackingStatus = request.TrackingStatus,
        };
        await _trackingRepository.AddAsync(parcelTrackingHistory,cancellationToken);

        return parcelTrackingHistory.Id;
    }
}