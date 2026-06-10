using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GroupMeetingASP.NETCoreWebApp.Models;

namespace GroupMeetingASP.NETCoreWebApp.Controllers
{
    public class GroupMeetingController : Controller
    {
        public IActionResult Index()
        {
            return View(GroupMeeting.GetGroupMeetings());
        }

        [HttpGet]
        public IActionResult AddGroupMeeting()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddGroupMeeting([Bind] GroupMeeting groupMeeting)
        {
            if (ModelState.IsValid)
            {
                if (GroupMeeting.AddGroupMeeting(groupMeeting) > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(groupMeeting);
        }

        [HttpGet]
        public IActionResult EditMeeting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GroupMeeting group = GroupMeeting.GetGroupMeetingById(id);
            if (group == null)
                return NotFound();
            return View(group);
        }

        [HttpPost]
        public IActionResult EditMeeting(int id, [Bind] GroupMeeting groupMeeting)
        {
            if (id != groupMeeting.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                GroupMeeting.UpdateGroupMeeting(groupMeeting);
                return RedirectToAction("Index");
            }
            return View(groupMeeting);
        }
        [HttpGet]
        public IActionResult DeleteMeeting(int id)
        {
            GroupMeeting group = GroupMeeting.GetGroupMeetingById(id);
            if (group==null)
            {
                return NotFound();
            }

            return View(group);
        }
        [HttpPost]
        public IActionResult DeleteMeeting(int id,GroupMeeting groupMeeting)
        {
            if (GroupMeeting.DeleteGroupMeeting(id) > 0)
            {
                return RedirectToAction("Index");
            }
            return View(groupMeeting);
        }
    }
}