﻿using _0_Framework.Domain;

namespace AM.Domain.NaceAggregate
{
    public interface INaceRepository : IRepository<long, Nace>
    {

    }
    public interface INaceDataRepository : IRepository<long, NaceData>
    {

    }
}