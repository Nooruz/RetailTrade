﻿using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Services
{
    public interface ILabelPriceTagService : IDataService<LabelPriceTag>
    {
        event Action<LabelPriceTag> OnCreated;
        event Action<int> OnDeleted;
        event Action<LabelPriceTag> OnEdited;
    }
}