﻿using MvcTemplate.Components.Alerts;
using MvcTemplate.Services;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Controllers
{
    public abstract class ServicedController<TService> : BaseController
        where TService : IService
    {
        protected TService Service;
        private Boolean disposed;

        protected ServicedController(TService service)
        {
            Service = service;
            Service.ModelState = Service.ModelState ?? ModelState;
            Service.AlertMessages = Service.AlertMessages ?? new MessagesContainer();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (Session["Messages"] == null) {
                Session["Messages"] = Service.AlertMessages;
                return;
            }

            MessagesContainer current = Session["Messages"] as MessagesContainer;
            if (current != Service.AlertMessages)
                current.Merge(Service.AlertMessages);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposed) return;

            Service.Dispose();
            Service = default(TService);

            disposed = true;

            base.Dispose(disposing);
        }
    }
}
