using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace UI.Web.Controllers
{
    public class MissionController : Controller
    {
        IServiceMission sm;
        IServiceEmployees se;
        public MissionController(IServiceMission sm)
        {

            this.sm = sm;
        }
        // GET: MissionController
        [Authorize(Policy = "EmployeeRead")]

        public ActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var missions = sm.GetMany();

            if (startDate.HasValue && endDate.HasValue)
            {
                missions = missions.Where(m => m.DateDebut >= startDate.Value && m.DateFin <= endDate.Value);
            }

            return View(missions);
        }
        

        // GET: MissionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MissionController/Create
        [Authorize(Policy = "EmployeeCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MissionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Mission collection)
        {
            if (collection.DateDebut < collection.DateFin)
            {
                sm.Add(collection);
                sm.Commit();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Invalid mission data. Please ensure the start date is earlier than the end date.");
                return View();
            }
        }

        // GET: MissionController/Edit/5
        [Authorize(Policy = "EmployeeUpdate")]
        public ActionResult Edit(int id)
        {
            var mission = sm.GetById(id);
            if (mission == null)
            {
                return RedirectToAction(nameof(Index));
            }



            return View(mission);
        }

        // POST: MissionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Mission updatedMission)
        {
            var mission = sm.GetById(id);

            if (mission == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Update properties with the new values
            mission.Description = updatedMission.Description;
            mission.DateDebut = updatedMission.DateDebut;
            mission.DateFin = updatedMission.DateFin;

            if (mission.DateDebut < mission.DateFin)
            {
                sm.Update(mission);
                sm.Commit();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Invalid mission data. Please ensure the start date is earlier than the end date.");
                return View(mission);
            }
        }

        // GET: MissionController/Delete/5
        [Authorize(Policy = "EmployeeDelete")]
        public ActionResult Delete(int id)
        {
            var mission = sm.GetById(id);
            if (mission == null)
            {
                return RedirectToAction("Index");
            }

            return View(mission);
        }

        // POST: MissionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var mission = sm.GetById(id);
                if (mission == null)
                {
                    return RedirectToAction("Index");
                }

                sm.Delete(mission);
                sm.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
