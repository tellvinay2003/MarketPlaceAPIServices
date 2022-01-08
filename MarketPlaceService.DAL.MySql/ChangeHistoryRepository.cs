using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.DAL.Utilities;

namespace MarketPlaceService.DAL
{
    public class ChangeHistoryRepository : BaseRepository, IChangeHistoryRepository
    {
        public ChangeHistoryRepository(MarketplaceDbContext context): base(context)
        {
           
        }
        public async Task<IEnumerable<Entities.ChangeHistory>> GetChangeHistory(int dataType, HistoryOrigin origin, Guid site)
        {            
            var result = _context.ChangeHistory.Where(ch=> ch.Datatypeid==dataType && ch.Origin== (byte)origin && ch.Siteid == site).Select(a=>a).ToList();
            if (result != null && result.Count > 0)
            {
                return result.Select(a=> new Entities.ChangeHistory
                {
                    Details = a.Details,
                    UserId = a.Modifiedby,
                    Who = _context.UserAccount.Where(z => z.Id == a.Modifiedby).Select(y => y.FirstName + ' ' + y.LastName).FirstOrDefault(),
                    When = (DateTime)a.Modifieddate,
                    Action =  (HistoryAction)Enum.Parse(typeof(HistoryAction), a.Action.ToString())
                }).ToList();
            }

            return null;
        }

        public async Task<IEnumerable<Entities.ChangeHistory>> GetChangeHistory(int dataType, HistoryOrigin origin, Guid site, int pagenumber)
        {
            var result = _context.ChangeHistory.Where(ch=> ch.Datatypeid==dataType && ch.Origin== (byte)origin && ch.Siteid == site).Select(a=>
            new Entities.ChangeHistory
            {
                Details = a.Details,
                UserId = a.Modifiedby,
                Who = _context.UserAccount.Where(z => z.Id == a.Modifiedby).Select(y => y.FirstName + ' ' + y.LastName).FirstOrDefault(),
                When = (DateTime)a.Modifieddate,
                Action = (HistoryAction)Enum.Parse(typeof(HistoryAction), a.Action.ToString())
            }).ToList();

            return result;
        }

        public async Task SaveHistory(int dataType, HistoryAction action, HistoryOrigin origin, Guid userId, object sourceObject, object targetObject, Guid site, IChangeHistoryHelper changeHistoryHelper)
        {
            string details = await changeHistoryHelper.GetDetails(action,sourceObject, targetObject);
            Entities.ChangeHistory request = new Entities.ChangeHistory{
                Action = action,
                DataTypeId = dataType,
                UserId = userId,
                When = DateTime.UtcNow,
                Origin = origin,
                Details = details,
                Site = site
            };

            await InsertChangeHistory(request);
        }

        private async Task<Entities.ChangeHistory> InsertChangeHistory(Entities.ChangeHistory request)
        {
            Models.ChangeHistory changeHistory = new Models.ChangeHistory();
            changeHistory.Datatypeid = request.DataTypeId;            
            changeHistory.Action = (byte)request.Action;
            changeHistory.Modifiedby = request.UserId;
            changeHistory.Modifieddate = request.When;
            changeHistory.Origin = (byte)request.Origin;
            changeHistory.Siteid = request.Site;
            changeHistory.Details = request.Details;

            _context.ChangeHistory.Add(changeHistory);
            _context.SaveChanges();
            return request;
        }
    }
}
