using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using Results;

namespace Contracts
{
    public interface IOfferManager
    {
        Result<IEnumerable<Offer>> GetList(int page, int size, string search);
        Result<Offer> GetOffer(long id);
        VoidResult UpdateOffer(Offer offer);
        VoidResult DeleteOffer(long id);
        VoidResult AddOffer(Offer offer);
    }
}